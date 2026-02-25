using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace StalkerOnlineQuesterEditor
{
    //! Форма правки диалога и всех его опций
    public partial class EditDialogForm : Form
    {
        public MainForm parent;
        public CDialogPrecondition editPrecondition = new CDialogPrecondition();
        public List<int> editKarmaPK = new List<int>();
        public CDialog curDialog;
        System.Timers.Timer debounceTimer;
        int currentDialogID;
        ListLastDialogsForm listLastDialogsForm;
        bool isAdd;
        //! Максимальная длина ответа ГГ, при превышении которого выводится сообщение
        int MAX_SYMBOL_ANSWER = 48;

        //! Конструктор
        public EditDialogForm(bool isAdd, MainForm parent, int selectedDialogID)
        {
            InitializeComponent();
            currentDialogID = selectedDialogID;
            this.isAdd = isAdd;
            this.parent = parent;
            //this.parent.Enabled = false;
            parent.setDisable();
            this.listLastDialogsForm = new ListLastDialogsForm(parent);
            curDialog = parent.getDialogOnDialogID(currentDialogID);
            lAttention.Text = "";
            teleportComboBox.Items.Clear();
            foreach (string key in parent.tpConst.getKeys())
                teleportComboBox.Items.Add(key);

            foreach (CItem item in parent.itemConst.getAllItems().Values)
            {
                ((DataGridViewComboBoxColumn)GVItems.Columns[0]).Items.Add(item.getName());
                ((DataGridViewComboBoxColumn)GVNonItems.Columns[0]).Items.Add(item.getName());
            }
            ((DataGridViewComboBoxColumn)GVItems.Columns[0]).Sorted = true;
            ((DataGridViewComboBoxColumn)GVNonItems.Columns[0]).Sorted = true;

            foreach (string category in parent.itemCategories.getAllItems().Values)
            {
                cbCategory.Items.Add(category);
                cbNonCategory.Items.Add(category);
            }

            FillActionsComboBox();
            FillTutorialComboBox();
            FillPVPComboBox();
            if (parent.isRoot(currentDialogID) && (!isAdd))
                lReactionNPC.Text = "Приветствие:";
            cbRadioNode.SelectedIndex = 0;
            cbDungeonPhase.SelectedIndex = 0;
            cbSeasonEvent.Items.Clear();
            cbSeasonEvent.Items.Add("");
            cbSeasonEvent.Items.AddRange(SeasonEvents.getListNames());
            cbSeasonEvent.SelectedIndex = 0;
            cbMap.Items.Clear();
            cbMap.Items.Add("-1 Карта игрока");
            foreach (var i in parent.spacesConst.getSpacesNamesWithoutInstances())
            {
                cbMap.Items.Add(i);
            }
            cbMap.SelectedIndex = 0;
            if (!isAdd)
            {
                fillDialogEditForm(currentDialogID);
                this.Text = "Редактирование диалога ID = " + currentDialogID + "";
            }

            this.Text += "   Версия: " + curDialog.version;
        }
        void fillClanOptions(string options)
        {
            List<int> list = new List<int>();
            if ((options == "") || (options == "&") || (options == "|"))
                return;
            switch(options[0])
                
            {
                case '&':
                    radioButtonAND.Checked = true;
                    radioButtonOR.Checked = false;
                    break;
                case '|':
                    radioButtonAND.Checked = false;
                    radioButtonOR.Checked = true;
                    break;
                default:
                    options = "&," + options;
                    break;

            }
            options = options.Substring(2);
            foreach (string node in options.Split(','))
                list.Add(int.Parse(node)); ;
            // какой-то пиздец c кланами и одиночками
                cbSameClanOnly.Checked = list.Contains(1);
                cbNotSameClanOnly.Checked = list.Contains(2);
                cbEnemy.Checked = list.Contains(3);
                cbNotEnemy.Checked = list.Contains(4);
                cbPeaceTime.Checked = list.Contains(5);
                cbWarTime.Checked = list.Contains(6);
                cbAnyClanOnly.Checked = list.Contains(7);
                cbLonerOnly.Checked = list.Contains(8);
                cbSecurExst.Checked = list.Contains(9);
                cbSecurNotExst.Checked = list.Contains(10);
                cbAllyance.Checked = list.Contains(11);
                cbNotAllyance.Checked = list.Contains(12);
        }
        //! Заполняет всю форму данными из CDialog
        void fillDialogEditForm(int dialogID)
        {
            cbAutoNode.Checked = curDialog.isAutoNode;
            autoDefaultNode.Text = curDialog.defaultNode;
            tbNextDialog.Text = curDialog.nextDialog.ToString();
            
            // заполнение текста речевки и ответа ГГ
            tPlayerText.Text = curDialog.Title.Normalize();
            tReactionNPC.Text = curDialog.Text;
            update_errorFiner_btn();
             
            //TextUtils.findTextErrors(tPlayerText);
            //TextUtils.findTextErrors(tReactionNPC);

            foreach (TreeNode active in parent.GetCurrentTree().Nodes.Find("Active",true))
                foreach (TreeNode node in active.Nodes)
                    ToDialogComboBox.Items.Add(node.Text);
            // заполнение текстбокса "Поддиалоги" (Nodes)
            autoDefaultNode.Items.Clear();
            if (curDialog.Nodes.Any())
                foreach (int dialog in curDialog.Nodes)
                {
                    if (tNodes.Text.Equals(""))
                        tNodes.Text += dialog.ToString();
                    else
                        tNodes.Text += ("," + dialog.ToString());
                    autoDefaultNode.Items.Add(dialog.ToString());

                }
            if (curDialog.CheckNodes.Any())
                foreach (int node in curDialog.CheckNodes)
                {
                    if (tCheckNodes.Text.Equals(""))
                        tCheckNodes.Text += node.ToString();
                    else
                        tCheckNodes.Text += ("," + node.ToString());
                }
            this.fillClanOptions(curDialog.Precondition.clanOptions);

            clanLevelFrom.Value = curDialog.Precondition.clanLevel[0];
            clanLevelTo.Value = curDialog.Precondition.clanLevel[1];
            

            if (curDialog.Actions.Any())
            {
                actionsCheckBox.Checked = true;
                ActionsComboBox.SelectedValue = curDialog.Actions.Event.Value;
                cbExit.Checked = curDialog.Actions.Exit;

                if (ActionsComboBox.Text == "Телепорт")
                {
                    string key = parent.tpConst.getName(curDialog.Actions.Data);
                    teleportComboBox.SelectedItem = key;
                }
                List<string> list = new List<string>() { "Перейти в точку", "Бартер (обмен)", "Торговля", "Подземелье. активировать", "Вертолёт", "Зарядка магазинов" };
                if (list.Contains(ActionsComboBox.Text))
                {
                    tbAvatarGoTo.Text = curDialog.Actions.Data;
                }
                if (ActionsComboBox.Text == "Команда НПЦ")
                {
                    string key = parent.cmConst.getName(curDialog.Actions.Data);
                    commandsComboBox.SelectedItem = key;
                }
                if ((ActionsComboBox.Text == "Починка") || (ActionsComboBox.Text == "Комплексная починка"))
                {
                    string key = parent.rpConst.getName(curDialog.Actions.Data);
                    commandsComboBox.SelectedItem = key;
                }
                if (ActionsComboBox.Text == "Телепорт в подземелье")
                {
                    string[] data = curDialog.Actions.Data.Split();
                    int dung_id = Convert.ToInt32(data[0]);
                    int enter_key = Convert.ToInt32(data[1]);
                    commandsComboBox.SelectedItem = parent.dungeonConst.getNameByID(dung_id);
                    nudDungeonEnterKey.Value = enter_key;
                }
                if (ActionsComboBox.Text == "Наложить эффект")
                {
                    string[] data = curDialog.Actions.Data.Split();
                    int effect_id = Convert.ToInt32(data[0]);
                    int stack = Convert.ToInt32(data[1]);
                    commandsComboBox.SelectedItem = parent.effects.getDescriptionOnID(effect_id);
                    nudDungeonEnterKey.Value = stack;
                }
                if (ActionsComboBox.Text == "Запустить станок")
                {
                    string key = parent.workbenchTypes.getName(curDialog.Actions.Data);
                    commandsComboBox.SelectedItem = key;
                }

                if (ActionsComboBox.Text == "Списание репутации")
                {
                    string key = parent.fractions2.getFractionDesctByID(Convert.ToInt32(curDialog.Actions.Data));
                    commandsComboBox.SelectedItem = key;
                }



                if (curDialog.Actions.ToDialog != 0)
                {
                    ActionsComboBox.SelectedValue = 100;
                    int neededDialog = ToDialogComboBox.FindStringExact(curDialog.Actions.ToDialog.ToString());
                    ToDialogComboBox.SelectedIndex = neededDialog;
                }

                if (curDialog.Actions.CompleteQuests.Any())
                {
                    cbCompleteQuests.Checked = true;
                    foreach (int completeQuest in curDialog.Actions.CompleteQuests)
                        Global.addItemToTextBox(completeQuest.ToString(), tbCompleteQuests);
                }

                if (curDialog.Actions.GetQuests.Any())
                {
                    cbGetQuests.Checked = true;
                    foreach (int getQuest in curDialog.Actions.GetQuests)
                        Global.addItemToTextBox(getQuest.ToString(), tbGetQuests);
                }

                if (curDialog.Actions.CancelQuests.Any())
                {
                    cbCancelQuests.Checked = true;
                    foreach (int cancelQuest in curDialog.Actions.CancelQuests)
                        Global.addItemToTextBox(cancelQuest.ToString(), tbCancelQuests);
                }

                if (curDialog.Actions.FailQuests.Any())
                {
                    cbFailQuests.Checked = true;
                    foreach (int failQuest in curDialog.Actions.FailQuests)
                        Global.addItemToTextBox(failQuest.ToString(), tbFailQuests);
                }
                if (curDialog.Actions.GetKnowleges.Any())
                {
                    cbGetKnowleges.Checked = true;
                    foreach (int knowlege in curDialog.Actions.GetKnowleges)
                        Global.addItemToTextBox(knowlege.ToString(), tbGetKnowleges);
                }
                
            }

            // заполнение условий для открытия диалога - список открытых, завершенных, заваленных квестов
            if (curDialog.Precondition.Any())
            {

                foreach(int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfHaveQuests)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveQuests);
                cbShouldntHaveQuests.Checked = curDialog.Precondition.ListOfMustNoQuests.conditionOfQuests == '|';

                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfCompletedQuests)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveCompletedQuests);
                cbShouldntHaveCompletedQuests.Checked = (curDialog.Precondition.ListOfMustNoQuests.conditionOfCompletedQuests == '|');

                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfOpenedQuests)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveOpenQuests);
                cbShouldntHaveOpenQuests.Checked = (curDialog.Precondition.ListOfMustNoQuests.conditionOfOpenedQuests == '|');

                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfFailQuests)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveFailQuests);
                cbShouldntHaveFailQuests.Checked = (curDialog.Precondition.ListOfMustNoQuests.conditionOfFailQuests == '|');
                
                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfOnTestQuests)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveQuestsOnTest);
                cbShouldntHaveQuestsOnTest.Checked = (curDialog.Precondition.ListOfMustNoQuests.conditionOfOnTestQuest == '|');
                
                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfCounters)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveCounters);
                cbShouldntHaveCounters.Checked = (curDialog.Precondition.ListOfMustNoQuests.conditionOfCounterss == '|');
                
                foreach (int quest in curDialog.Precondition.ListOfMustNoQuests.ListOfRepeat)
                    Global.addItemToTextBox(quest.ToString(), tShouldntHaveRepeat);
                
                Global.addItemToTextBox(curDialog.Precondition.ListOfMustNoQuests.ListOfMassQuests, tShouldntHaveMassQuests);
                cbShouldntHaveMassQuests.Checked = curDialog.Precondition.ListOfMustNoQuests.conditionOfMassQuests == '|';
                
                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfHaveQuests)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveQuests);
                cbMustHaveQuests.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfQuests == '|';

                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveCompletedQuests);
                cbMustHaveCompletedQuests.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfCompletedQuests == '|';
               

                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveQuestsOnTest);
                cbMustHaveQuestsOnTest.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfOnTestQuest == '|';
                
                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveOpenQuests);
                cbMustHaveOpenQuests.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfOpenedQuests == '|';
               
                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfFailQuests)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveFailQuests);
                cbMustHaveFailQuests.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfFailQuests == '|';
                
                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfCounters)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveCounters);
                cbMustHaveCounters.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfCounterss == '|';
                
                foreach (int quest in curDialog.Precondition.ListOfNecessaryQuests.ListOfRepeat)
                    Global.addItemToTextBox(quest.ToString(), tMustHaveRepeat);
                cbMustHaveRepeat.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfRepeat == '|';
                
                Global.addItemToTextBox(curDialog.Precondition.ListOfNecessaryQuests.ListOfMassQuests, tMustHaveMassQuests);
                cbMustHaveMassQuests.Checked = curDialog.Precondition.ListOfNecessaryQuests.conditionOfMassQuests == '|';


                foreach (int knowlege in curDialog.Precondition.knowledges.mustKnowledge)
                    Global.addItemToTextBox(knowlege.ToString(), tMustHaveKnow);
                cMustHaveKnow.Checked = curDialog.Precondition.knowledges.conditionMustKnowledge == '|';

                foreach (int knowlege in curDialog.Precondition.knowledges.shouldntKnowledge)
                    Global.addItemToTextBox(knowlege.ToString(), tMustNoHaveKnow);
                cbShouldntHaveKnow.Checked = curDialog.Precondition.knowledges.conditionShouldntKnowledge == '|';

            }
            if (curDialog.DebugData != "") debugTextBox.Text = curDialog.DebugData;
            cbNoLocale.Checked = curDialog.noLocale;


            calcSymbolMaxAnswer();


            if (curDialog.Precondition.Any())
            {
                editPrecondition = curDialog.Precondition;
            }
            if (curDialog.Precondition.KarmaPK.Any())
            {
                editKarmaPK = curDialog.Precondition.KarmaPK;
                checkOtherIndicates();
            }

            if (curDialog.Precondition.playerCoords.Any())
            {
                List<string> tmp = new List<string>();
                foreach (var i in curDialog.Precondition.playerCoords)
                    tmp.Add(i.ToString().Replace(',', '.'));
                tbCoordinates.Text = string.Join(" ", tmp);
            }

            if (curDialog.Precondition.coordsMap != -1)
                cbMap.SelectedItem = parent.spacesConst.getSpaceByID(curDialog.Precondition.coordsMap);

            nupCoordRadius.Value = curDialog.Precondition.coordsRadius;
            if (curDialog.Precondition.Skills.Any())
                editPrecondition.Skills = curDialog.Precondition.Skills;
            if (curDialog.Precondition.Perks.Any())
                editPrecondition.Perks = curDialog.Precondition.Perks;
            if (curDialog.Precondition.noPerks.Any())
                editPrecondition.noPerks = curDialog.Precondition.noPerks;

            if (curDialog.Precondition.Achievements.Any())
                editPrecondition.Achievements = curDialog.Precondition.Achievements;
            if (curDialog.Precondition.noAchievements.Any())
                editPrecondition.noAchievements = curDialog.Precondition.noAchievements;

            cbRadioNode.SelectedIndex = (int)curDialog.Precondition.radioAvailable;
            cbDungeonPhase.SelectedIndex = curDialog.Precondition.dungeonPhase;
            cbSeasonEvent.SelectedItem = curDialog.Precondition.seasonEvent;
            cbDungeonNon.Checked = curDialog.Precondition.dungeonNot;
            cbForDev.Checked = curDialog.Precondition.forDev;
            cbHidden.Checked = curDialog.Precondition.hidden;
            this.initReputationTab(dataReputation, parent.fractions, this.editPrecondition.Reputation, this.editPrecondition.NPCReputation);
            this.initReputationTab(dataReputation2, parent.fractions2, this.editPrecondition.Reputation2, new Dictionary<string, List<double>>());
            fillGroupBonuses();
            this.initKarmaPKTab();
            this.initEffectsTab();
            this.initLevelTab();
            this.initAchievementsTab();
            this.initSkillsTab();
            this.initActionTab();
            this.initItemsTab();
            this.initTransportTab();
            this.initTutorialTab();
            this.initPVPTab();
            this.initWeatherTab();
            checkClanOptionsIndicator();
            checkKnowlegeIndicates();


        }

        //! Антиговнокод-функция, добавление номера квеста в текстбокс


        private void FillTutorialComboBox()
        {
            cbTutorialPhase.Items.Clear();
            cbTutorialPhase.DataSource = parent.tutorialPhases.getAllNames();
            cbTutorialPhase.SelectedItem = null;
        }

        private void FillPVPComboBox()
        {
            cbPVPRank1.Items.Clear();
            cbPVPRank2.Items.Clear();
            cbPVPRank1.DataSource = parent.pvPRanks.getKeys();
            cbPVPRank2.DataSource = parent.pvPRanks.getKeys();
            cbPVPRank1.SelectedItem = null;
            cbPVPRank2.SelectedItem = null;

            cbRatingPVPMode.Items.Clear();
            cbRatingPVPMode.Items.Add("нет");
            cbRatingPVPMode.Items.AddRange(CPVPConstans.getAllDescriptions().ToArray());
        }

        private void FillActionsComboBox()
        {
            ActionsComboBox.Items.Clear();            
            ActionsComboBox.DataSource = parent.dialogEvents.GetFullList();
            ActionsComboBox.DisplayMember = "Display";
            ActionsComboBox.ValueMember = "Value";
        }

        private void checkClanOptionsIndicator()
        {
            if (checkClanOptions())
            {
                pictureClan.Visible = true;
            }
            else pictureClan.Visible = false;
        }
        private bool checkClanOptions()
        {
            if (cbSameClanOnly.Checked || cbAnyClanOnly.Checked || cbLonerOnly.Checked || cbWarTime.Checked ||
                cbEnemy.Checked || cbNotEnemy.Checked || cbPeaceTime.Checked || cbNotSameClanOnly.Checked ||
                cbSecurExst.Checked || cbSecurNotExst.Checked || cbAllyance.Checked || cbNotAllyance.Checked ||
                (clanLevelTo.Value + clanLevelFrom.Value > 0))
            {
                return true;
            }
            else return false;
        }

        //! Нажатие Отмена - выход без сохранения
        private void bEditDialogCancel_Click(object sender, EventArgs e)
        {
            //parent.Enabled = true;
            parent.setEnable();
            this.Close();
        }
        //! Чекбокс действия - возможность добавить действия к узлу диалога
        private void actionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            gbActions.Enabled = actionsCheckBox.Checked;
        }
        //! Блокировка комбобокса при клике на соответствующий чекбокс 
        private void cbGetQuests_CheckedChanged(object sender, EventArgs e)
        {
            tbGetQuests.Enabled = cbGetQuests.Checked;
        }
        private void cbGetKnowleges_CheckedChanged(object sender, EventArgs e)
        {
            tbGetKnowleges.Enabled = cbGetKnowleges.Checked;
        }
        //! Блокировка комбобокса при клике на соответствующий чекбокс 
        private void cbCompleteQuests_CheckedChanged(object sender, EventArgs e)
        {
            tbCompleteQuests.Enabled = cbCompleteQuests.Checked;
        }

        private void cbCancelQuests_CheckedChanged(object sender, EventArgs e)
        {
            tbCancelQuests.Enabled = cbCancelQuests.Checked;
        }

        private void cbFailQuests_CheckedChanged(object sender, EventArgs e)
        {
            tbFailQuests.Enabled = cbFailQuests.Checked;
        }

        //! При наборе текста проверяем число символов в строке
        private void tPlayerText_KeyPress(object sender, KeyPressEventArgs e)
        {
            calcSymbolMaxAnswer();
            if (!CSettings.hasErrorFinder()) return;
            if (debounceTimer != null)
            {
                debounceTimer.Stop();
                debounceTimer.Dispose();
            }

            debounceTimer = new System.Timers.Timer(1000);
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += async (s, ev) =>
            {
                await Task.Run(() => SpellChecker.CheckAndHighlightSpellingErrors(tPlayerText));
            };

            debounceTimer.Start();
        }



        //! Считает число символов в строке ответа ГГ и выводит предупреждение
        private void calcSymbolMaxAnswer()
        {
            lAttention.Text = "";
            int length = tPlayerText.Text.Length;
            if (length > this.MAX_SYMBOL_ANSWER)
                lAttention.Text = "Внимание: Текст более " + MAX_SYMBOL_ANSWER.ToString() + " символов!";
        }

        private void EditDialogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.parent.Enabled = true;
            parent.setEnable();
            listLastDialogsForm.Close();
        }
        //! Задать цвет кнопки репутации, если репутация задана
        public void checkReputationIndicates()
        {
            pictureReputation.Visible = editPrecondition.Reputation.Any() || editPrecondition.NPCReputation.Any();
            pictureReputation2.Visible = editPrecondition.Reputation2.Any() || editPrecondition.fracBonus.Sum() > 0;
        }

        public void checkKnowlegeIndicates()
        {
            pictureKnowlege.Visible = (tMustNoHaveKnow.Text.Any() || tMustHaveKnow.Text.Any());
        }

        private void cbExit_Click(object sender, EventArgs e)
        {
            ActionsComboBox.Enabled = !cbExit.Checked;
        }

        //! Изменение текущего однократного действия
        private void ActionsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            int SelectedValue = -1;
            if (!int.TryParse(ActionsComboBox.SelectedValue.ToString(), out SelectedValue))
                return;


            teleportComboBox.Visible = (SelectedValue == 5);
            ToDialogComboBox.Visible = (SelectedValue == 100);

            List<int> list = new List<int>() { 19, 4, 6, 28, 32, 36, 38 };
            commandsComboBox.Visible = list.Contains(SelectedValue);
    
            list = new List<int>() { 20, 1, 7, 30, 31, 40};
            tbAvatarGoTo.Visible = list.Contains(SelectedValue);

            if (SelectedValue == 19)
            {
                commandsComboBox.Items.Clear();
                foreach (string key in parent.cmConst.getKeys())
                    commandsComboBox.Items.Add(key);
            }
            if ((SelectedValue == 4)|| (SelectedValue == 6))
            {
                commandsComboBox.Items.Clear();
                foreach (string key in parent.rpConst.getKeys())
                    commandsComboBox.Items.Add(key);
            }

            nudDungeonEnterKey.Visible = ((SelectedValue == 28) || (SelectedValue == 38));
            labelEnterKey.Visible = ((SelectedValue == 28) || (SelectedValue == 38));
            if (SelectedValue == 28)
            {
                commandsComboBox.Items.Clear();
                foreach (string key in parent.dungeonConst.getAllSpaceNames())
                    commandsComboBox.Items.Add(key);
                labelEnterKey.Text = "номер входа:";
                nudDungeonEnterKey.Minimum = 0;
            }
            if (SelectedValue == 38)
            {
                commandsComboBox.Items.Clear();
                foreach (string key in parent.effects.getAllDescriptions())
                    commandsComboBox.Items.Add(key);
                labelEnterKey.Text = "кол.стаков:";
                nudDungeonEnterKey.Minimum = -999;
            }
            if (SelectedValue == 32)
            {
                commandsComboBox.Items.Clear();
                foreach (string key in parent.workbenchTypes.getKeys())
                    commandsComboBox.Items.Add(key);
            }
            if (SelectedValue == 36)
            {
                commandsComboBox.Items.Clear();
                foreach (var key in parent.fractions2.getListOfFractions())
                    commandsComboBox.Items.Add(key.Value);
            }
            switch (SelectedValue)
            { 
                case 0:
                    cbExit.Checked = false;
                    cbExit.Enabled = true;
                    break;
                case 35:
                case 37:
                case 100:
                    cbExit.Checked = false;
                    cbExit.Enabled = false;
                    break;
                case 38:
                    cbExit.Enabled = true;
                    break;
                default:
                    cbExit.Checked = true;
                    cbExit.Enabled = false;
                    break;
            }
        }

        bool CheckConditions()
        {
            if (ActionsComboBox.Text == "Переход к диалогу")
            {
                String toDialog = ToDialogComboBox.Text;
                int index = ToDialogComboBox.Items.IndexOf(toDialog);
                //int index = ToDialogComboBox.FindString(toDialog);    // very bad way to do!!!
                if (index == -1)
                {
                    MessageBox.Show("Действие невозможно: такой диалог не существует у этого персонажа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (ActionsComboBox.Text == "Телепорт")
            {
                if (teleportComboBox.Text.Equals(""))
                {
                    MessageBox.Show("Действие невозможно: не задана точка для телепортации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (ActionsComboBox.Text == "Команда охраннику")
            {
                if (commandsComboBox.Text.Equals(""))
                {
                    MessageBox.Show("Действие невозможно: не задано действие охраннику.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        //! Нажатие ОК - сохранение всех опций диалога и выход на главную
        private void bEditDialogOk_Click(object sender, EventArgs e)
        {
            int newID;
            string DebugData = ""; 
            Actions actions = new Actions();
            CDialogPrecondition precondition = new CDialogPrecondition();
            NodeCoordinates coord = new NodeCoordinates();
            List<int> nodes = new List<int>();
            List<int> check_nodes = new List<int>();
            //List<string> holder = new List<string>();
            string holder = parent.GetCurrentHolder();

            if (!tNodes.Text.Equals(""))
                foreach (string node in tNodes.Text.Split(','))
                {
                    int node_id = int.Parse(node);
                    if (!this.parent.getAllDialogsIDonCurrentNPC().Contains(node_id))
                    {
                        MessageBox.Show("Действие невозможно: диалог №"+ node_id.ToString() +" не существует у этого персонажа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (this.parent.getDialogOnDialogID(node_id).Nodes.Contains(this.currentDialogID))
                    {
                        MessageBox.Show("Действие невозможно: диалог №" + node_id.ToString() + " уже сам ссылается на текущий диалог.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (node_id == this.currentDialogID)
                    {
                        MessageBox.Show("Действие невозможно: диалог №" + node_id.ToString() + " ссылается сам на себя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    nodes.Add(int.Parse(node));
                }
                    
            if (!tCheckNodes.Text.Equals(""))
                foreach (string node in tCheckNodes.Text.Split(','))
                    check_nodes.Add(int.Parse(node));

            if (!CheckConditions())
                return;


            if (!this.checkReputation(dataReputation, parent.fractions, this.editPrecondition.Reputation, this.editPrecondition.NPCReputation)) 
                return;
            if (!this.checkReputation(dataReputation2, parent.fractions2, this.editPrecondition.Reputation2, new Dictionary<string, List<double>>()))
                return;
            if (!this.checkSkills())
                return;

            this.checkAchievements();

            if (cbFracBonus.SelectedItem != null)
            {
                int bonusID = CFracBonuses.getIDByName(cbFracBonus.SelectedItem.ToString());
                if (bonusID > 0 && cbFracGroup.SelectedItem != null)
                {
                    int fracID = parent.fractions2.getFractionIDByDescr(cbFracGroup.SelectedItem.ToString());
                    precondition.fracBonus = new int[3] { fracID, bonusID, Convert.ToInt32(!cbFracNoMusthave.Checked) };
                }
            }


            this.checkKarmaPK();
            this.checkEffects();
            // заполняем действия диалога - торговля, бартер, починка, телепорт и т.д.
            if (actionsCheckBox.Checked)
            {
                actions.Exit = cbExit.Checked;
                actions.Event = parent.dialogEvents.GetEventFromID((int) ActionsComboBox.SelectedValue);
                if (actions.Event.Display == "Телепорт")
                    actions.Data = parent.tpConst.getTtID(teleportComboBox.SelectedItem.ToString());
                if (actions.Event.Display == "Переход к диалогу")
                    actions.ToDialog = int.Parse(ToDialogComboBox.Text.ToString());
                if (actions.Event.Display == "Команда НПЦ")
                    actions.Data = parent.cmConst.getTtID(commandsComboBox.SelectedItem.ToString());
                if ((actions.Event.Display == "Починка") || (actions.Event.Display == "Комплексная починка"))
                    actions.Data = parent.rpConst.getTtID(commandsComboBox.SelectedItem.ToString());
                if (actions.Event.Display == "Списание репутации")
                    actions.Data = parent.fractions2.getFractionIDByDescr(commandsComboBox.SelectedItem.ToString()).ToString();
                List<string> list = new List<string>() { "Перейти в точку", "Бартер (обмен)", "Торговля", "Подземелье. активировать", "Вертолёт", "Зарядка магазинов" };
                if (list.Contains(actions.Event.Display))
                    actions.Data = tbAvatarGoTo.Text;
                if ((actions.Event.Display == "Телепорт в подземелье"))
                {
                    actions.Data = parent.dungeonConst.getIDByName(commandsComboBox.SelectedItem.ToString()).ToString() +
                        " " + nudDungeonEnterKey.Value.ToString();
                }
                if (actions.Event.Display == "Наложить эффект") 
                {
                    actions.Data = parent.effects.getIDOnDescription(commandsComboBox.SelectedItem.ToString()).ToString() +
                        " " + nudDungeonEnterKey.Value.ToString();
                }
                if (actions.Event.Display == ("Запустить станок"))
                    actions.Data = parent.workbenchTypes.getTtID(commandsComboBox.SelectedItem.ToString());
                if (cbGetQuests.Checked)
                    foreach (string quest in tbGetQuests.Text.Split(','))
                        actions.GetQuests.Add(int.Parse(quest));
                if (cbCompleteQuests.Checked)
                    foreach (string quest in tbCompleteQuests.Text.Split(','))
                        actions.CompleteQuests.Add(int.Parse(quest));
                if (cbCancelQuests.Checked)
                    foreach (string quest in tbCancelQuests.Text.Split(','))
                        actions.CancelQuests.Add(int.Parse(quest));
                if (cbFailQuests.Checked)
                    foreach (string quest in tbFailQuests.Text.Split(','))
                        actions.FailQuests.Add(int.Parse(quest));
                if (cbGetKnowleges.Checked)
                    foreach (string quest in tbGetKnowleges.Text.Split(','))
                        actions.GetKnowleges.Add(int.Parse(quest));
            }
            else
                actions.Event = parent.dialogEvents.GetEventFromID(0);

            if (cbCamera.Checked && tbCamera.Text.Any())
                actions.actionCameraSmoothly = cbCameraSmoothly.Checked;
                actions.actionCamera = tbCamera.Text;
            if (cbAnimationPlayer.Checked && tbAnimationPlayer.Text.Any())
                actions.actionAnimationPlayer = tbAnimationPlayer.Text;
            if (cbAnimationNPC.Checked && tbAnimationNPC.Text.Any())
                actions.actionAnimationNPC = tbAnimationNPC.Text;
            if (cbActionNPC.Checked && tbActionNPC.Text.Any())
            {
                actions.actionActionNPC = int.Parse(parent.npcActions.getTtID(tbActionNPC.Text));
                actions.actionAdditionalActionNPC = tbAdditionalAction.Text;
            }
               
          
            if (cbAvatarPoint.Checked && tbAvatarPoint.Text.Any())
                actions.actionAvatarPoint = tbAvatarPoint.Text;
            if (cbPlaySonund.Checked && tbPlaySonund.Text.Any())
                actions.actionPlaySound = tbPlaySonund.Text;
            if (nupChangeMoney.Value != 0)
            {
                actions.changeMoney = Convert.ToInt32(nupChangeMoney.Value);
                if (tbChangeMoneyFailNode.Text.Any())
                    int.TryParse(tbChangeMoneyFailNode.Text, out actions.changeMoneyFailNode);
            }


            // заполняем условия появления диалога - открытые и закрытые квесты и т.д.
            if (!tMustHaveQuests.Text.Equals(""))
            {
                if (cbMustHaveQuests.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfQuests = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfQuests = '&';
                foreach (string quest in tMustHaveQuests.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfHaveQuests.Add(int.Parse(quest));
            }
            if (!tMustHaveOpenQuests.Text.Equals(""))
            {
                if (cbMustHaveOpenQuests.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfOpenedQuests = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfOpenedQuests = '&';
                foreach (string quest in tMustHaveOpenQuests.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfOpenedQuests.Add(int.Parse(quest));
            }

            if (!tMustHaveFailQuests.Text.Equals(""))
            {
                if (cbMustHaveFailQuests.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfFailQuests = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfFailQuests = '&';
                foreach (string quest in tMustHaveFailQuests.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfFailQuests.Add(int.Parse(quest));
            }
            if (!tMustHaveQuestsOnTest.Text.Equals(""))
            {
                if (cbMustHaveQuestsOnTest.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfOnTestQuest = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfOnTestQuest = '&';
                foreach (string quest in tMustHaveQuestsOnTest.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfOnTestQuests.Add(int.Parse(quest));
            }
            if (!tMustHaveCompletedQuests.Text.Equals(""))
            {
                if (cbMustHaveCompletedQuests.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfCompletedQuests = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfCompletedQuests = '&';
                foreach (string quest in tMustHaveCompletedQuests.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfCompletedQuests.Add(int.Parse(quest));
            }
            if (!tMustHaveMassQuests.Text.Equals(""))
            {
                if (cbMustHaveMassQuests.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfMassQuests = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfMassQuests = '&';
                precondition.ListOfNecessaryQuests.ListOfMassQuests = tMustHaveMassQuests.Text;
            }
            if (!tMustHaveCounters.Text.Equals(""))
            {
                if (cbMustHaveCounters.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfCounterss = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfCounterss = '&';
                foreach (string quest in tMustHaveCounters.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfCounters.Add(int.Parse(quest));
            }
            if (!tMustHaveRepeat.Text.Equals(""))
            {
                if (cbMustHaveRepeat.Checked)
                    precondition.ListOfNecessaryQuests.conditionOfRepeat = '|';
                else
                    precondition.ListOfNecessaryQuests.conditionOfRepeat = '&';
                foreach (string quest in tMustHaveRepeat.Text.Split(','))
                    precondition.ListOfNecessaryQuests.ListOfRepeat.Add(int.Parse(quest));
            }

            if (!tShouldntHaveQuests.Text.Equals(""))
            {
                if (cbShouldntHaveOpenQuests.Checked)
                    precondition.ListOfMustNoQuests.conditionOfQuests = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfQuests = '&';
                foreach (string quest in tShouldntHaveQuests.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfHaveQuests.Add(int.Parse(quest));
            }

            if (!tShouldntHaveOpenQuests.Text.Equals(""))
            {
                if (cbShouldntHaveOpenQuests.Checked)
                    precondition.ListOfMustNoQuests.conditionOfOpenedQuests = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfOpenedQuests = '&';
                foreach (string quest in tShouldntHaveOpenQuests.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfOpenedQuests.Add(int.Parse(quest));
            }
            if (!tShouldntHaveFailQuests.Text.Equals(""))
            {
                if (cbShouldntHaveFailQuests.Checked)
                    precondition.ListOfMustNoQuests.conditionOfFailQuests = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfFailQuests = '&';
                foreach (string quest in tShouldntHaveFailQuests.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfFailQuests.Add(int.Parse(quest));
            }
            if (!tShouldntHaveQuestsOnTest.Text.Equals(""))
            {
                if (cbShouldntHaveQuestsOnTest.Checked)
                    precondition.ListOfMustNoQuests.conditionOfOnTestQuest = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfOnTestQuest = '&';
                foreach (string quest in tShouldntHaveQuestsOnTest.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfOnTestQuests.Add(int.Parse(quest));
            }
            if (!tShouldntHaveCompletedQuests.Text.Equals(""))
            {
                if (cbShouldntHaveCompletedQuests.Checked)
                    precondition.ListOfMustNoQuests.conditionOfCompletedQuests = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfCompletedQuests = '&';
                foreach (string quest in tShouldntHaveCompletedQuests.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfCompletedQuests.Add(int.Parse(quest));
            }
            if (!tShouldntHaveMassQuests.Text.Equals(""))
            {
                if (cbShouldntHaveMassQuests.Checked)
                    precondition.ListOfMustNoQuests.conditionOfMassQuests = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfMassQuests = '&';
                precondition.ListOfMustNoQuests.ListOfMassQuests = tShouldntHaveMassQuests.Text;
            }
            if (!tShouldntHaveCounters.Text.Equals(""))
            {
                if (cbShouldntHaveCounters.Checked)
                    precondition.ListOfMustNoQuests.conditionOfCounterss = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfCounterss = '&';
                foreach (string quest in tShouldntHaveCounters.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfCounters.Add(int.Parse(quest));
            }
            if (!tShouldntHaveRepeat.Text.Equals(""))
            {
                if (cbShouldntHaveRepeat.Checked)
                    precondition.ListOfMustNoQuests.conditionOfRepeat = '|';
                else
                    precondition.ListOfMustNoQuests.conditionOfRepeat = '&';
                foreach (string quest in tShouldntHaveRepeat.Text.Split(','))
                    precondition.ListOfMustNoQuests.ListOfRepeat.Add(int.Parse(quest));
            }

            if (!tMustHaveKnow.Text.Equals(""))
            {
                if (cMustHaveKnow.Checked)
                    precondition.knowledges.conditionMustKnowledge = '|';
                else
                    precondition.knowledges.conditionMustKnowledge = '&';
                foreach (string knw in tMustHaveKnow.Text.Split(','))
                    precondition.knowledges.mustKnowledge.Add(int.Parse(knw));
            }
            if (!tMustNoHaveKnow.Text.Equals(""))
            {
                if (cbShouldntHaveKnow.Checked)
                    precondition.knowledges.conditionShouldntKnowledge = '|';
                else
                    precondition.knowledges.conditionShouldntKnowledge = '&';
                foreach (string knw in tMustNoHaveKnow.Text.Split(','))
                    precondition.knowledges.shouldntKnowledge.Add(int.Parse(knw));
            }

            precondition.transport.inTransportList = cbTransportInList.Checked;
            precondition.transport.notInTransportList = cbNotInTransportList.Checked;
            precondition.transport.inBoatList = cbInBoatList.Checked;
            precondition.transport.notInBoatList = cbNotInBoatList.Checked;
            precondition.transport.boatInTransit = cbIsBoatInTransit.Checked;
            precondition.transport.boatStopped = cbIsBoatStopped.Checked;

            if (cbIsBoatInTransit.Checked && cbIsBoatStopped.Checked)
            {
                MessageBox.Show("Перевозки. Некорректные правила для лодок");
                 return;
            }

            if (cbIsBoatInTransit.Checked || cbIsBoatStopped.Checked)
            {
                if (!boatName.Text.Any())
                {
                    MessageBox.Show("Перевозки. Не указано имя лодочника");
                    return;
                }
                precondition.transport.boatName = boatName.Text;
            }

            if (cbTutorialPhase.SelectedItem != null)
                precondition.tutorialPhase = this.parent.tutorialPhases.getIDByName(cbTutorialPhase.SelectedItem.ToString());

            if (cbPVPRank1.SelectedIndex != -1)
                precondition.PVPranks[0] = cbPVPRank1.SelectedIndex;
            if (cbPVPRank2.SelectedIndex != -1)
                precondition.PVPranks[1] = cbPVPRank2.SelectedIndex;
            if (cbRatingPVPMode.SelectedIndex > 0)
                precondition.PVPMode = CPVPConstans.getPVPModeIDByName(cbRatingPVPMode.SelectedItem.ToString());
            if (checkClanOptions())
            {
                precondition.clanOptions = "";
              
                if (cbSameClanOnly.Checked)
                    precondition.clanOptions += ",1";
                if (cbNotSameClanOnly.Checked)
                    precondition.clanOptions += ",2";
                if (cbEnemy.Checked)
                    precondition.clanOptions += ",3";
                if (cbNotEnemy.Checked)
                    precondition.clanOptions += ",4";
                if (cbPeaceTime.Checked)
                    precondition.clanOptions += ",5";
                if (cbWarTime.Checked)
                    precondition.clanOptions += ",6";
                if (cbAnyClanOnly.Checked)
                    precondition.clanOptions += ",7";
                if (cbLonerOnly.Checked)
                    precondition.clanOptions += ",8";
                if (cbSecurExst.Checked)
                    precondition.clanOptions += ",9";
                if (cbSecurNotExst.Checked)
                    precondition.clanOptions += ",10";
                if (cbAllyance.Checked) precondition.clanOptions += ",11";
                if (cbNotAllyance.Checked) precondition.clanOptions += ",12";
                if (precondition.clanOptions.Any())
                {
                    if (radioButtonAND.Checked)
                        precondition.clanOptions = "&" + precondition.clanOptions;
                    else if (radioButtonOR.Checked)
                        precondition.clanOptions = "|" + precondition.clanOptions;
                }

            }             
            if (checkLevel())
            {
                precondition.PlayerLevel = mtbPlayerLevelMin.Text + ":" + mtbPlayerLevelMax.Text;
            }
            precondition.clanLevel = new List<int>() { Convert.ToInt32(clanLevelFrom.Value), Convert.ToInt32(clanLevelTo.Value) };
            precondition.Reputation = editPrecondition.Reputation;
            precondition.Reputation2 = editPrecondition.Reputation2;
            precondition.NPCReputation = editPrecondition.NPCReputation;
            precondition.KarmaPK = editKarmaPK;
            precondition.NecessaryEffects = editPrecondition.NecessaryEffects;
            precondition.nec_effects_is_or = editPrecondition.nec_effects_is_or;
            precondition.MustNoEffects = editPrecondition.MustNoEffects;
            precondition.must_no_effects_is_or = editPrecondition.must_no_effects_is_or;
            precondition.Skills = editPrecondition.Skills;
            precondition.Perks = editPrecondition.Perks;
            precondition.noPerks = editPrecondition.noPerks;
            precondition.Achievements = editPrecondition.Achievements;
            precondition.noAchievements = editPrecondition.noAchievements;
            precondition.forDev = cbForDev.Checked;
            precondition.hidden = cbHidden.Checked;
            try
            {
                string s_coords = tbCoordinates.Text.Replace(',', ' ').Replace('(', ' ').Replace(')', ' ');
                foreach (var i in s_coords.Split())
                {
                    if (!i.Trim().Any()) continue;
                    precondition.playerCoords.Add(float.Parse(i, System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка условия \"Другое\"=>Координаты игрока", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            precondition.coordsMap = Convert.ToInt32(cbMap.SelectedItem.ToString().Split(' ')[0]);
            precondition.coordsRadius = Convert.ToInt32(nupCoordRadius.Value);
            if (cbSpaceWeather.SelectedItem != null)
            {
                precondition.weather.space = Convert.ToInt32(cbSpaceWeather.SelectedItem.ToString().Split(' ')[0]);
                precondition.weather.is_or = rbWeatherOr.Checked;
                foreach(var i in lbWeather.CheckedItems)
                    precondition.weather.weathers.Add(i.ToString());
                precondition.weather.timeStart = nupHour1.Value.ToString() + ":" + nupMin1.Value.ToString();
                precondition.weather.timeEnd = nupHour2.Value.ToString() + ":" + nupMin2.Value.ToString();
                precondition.weather.only_no = cbWeatherOnlyNO.Checked;
            }
            precondition.items.is_or = rbItemsOr.Checked;
            precondition.itemsNone.is_or = rbNonItemsOr.Checked;

            if (rbCategory.Checked && (cbCategory.SelectedIndex != -1) || (cbNonCategory.SelectedIndex != -1))
            {
                precondition.items.itemCategory = parent.itemCategories.getID(cbCategory.Text);
                precondition.itemsNone.itemCategory = parent.itemCategories.getID(cbNonCategory.Text);
            }
            else
            {
                precondition.items.itemCategory = -1;
                precondition.itemsNone.itemCategory = -1;
                precondition.items.equipped = cbItemsEquipped.Checked;
                precondition.itemsNone.equipped = cbItemsEquipped.Checked;
                precondition.items.not_equipped = cbItemsNOEquipped.Checked;
                precondition.itemsNone.not_equipped = cbItemsNOEquipped.Checked;
                List<object[]> obj = this.getItemsDataGrid(GVItems);
                foreach (object[] tmp in obj)
                {
                    QuestItem item = new QuestItem();
                    item.itemType = Convert.ToInt32(tmp[0]);
                    item.count = Convert.ToInt32(tmp[1]);
                    item.attribute = Convert.ToInt32(tmp[2]);
                    item.condition = Convert.ToSingle(tmp[3]);

                    precondition.items.items.Add(item);
                }
                obj = this.getItemsDataGrid(GVNonItems);
                foreach (object[] tmp in obj)
                {
                    QuestItem item = new QuestItem();
   
                    item.itemType = Convert.ToInt32(tmp[0]);
                    item.count = Convert.ToInt32(tmp[1]);
                    item.attribute = Convert.ToInt32(tmp[2]);
                    item.condition = Convert.ToSingle(tmp[3]);
                    precondition.itemsNone.items.Add(item);
                }
            }

            precondition.radioAvailable = (RadioAvalible)cbRadioNode.SelectedIndex;
            precondition.dungeonPhase = Math.Max(0, cbDungeonPhase.SelectedIndex);
            if (cbSeasonEvent.SelectedItem != null)
                precondition.seasonEvent = cbSeasonEvent.SelectedItem.ToString();
            precondition.dungeonNot = cbDungeonNon.Checked;

            if (debugTextBox.Text != "")
                DebugData = debugTextBox.Text;

            int nextDialog = 0;
            int.TryParse(tbNextDialog.Text, out nextDialog);

            if (isAdd)
            {
                newID = CDialogs.getDialogsNewID();
                CDialog dialog = new CDialog(holder, tPlayerText.Text, tReactionNPC.Text, precondition, actions, nodes, check_nodes, newID, 1,
                                        coord, DebugData, cbNoLocale.Checked, nextDialog, cbAutoNode.Checked, autoDefaultNode.Text);
                parent.addActiveDialog(newID, dialog, currentDialogID);
            }
            else
            {
                coord.X = curDialog.coordinates.X;
                coord.Y = curDialog.coordinates.Y;
                coord.RootDialog = curDialog.coordinates.RootDialog;
                coord.Active = curDialog.coordinates.Active;
                int version = curDialog.version;
                if (tPlayerText.Text != curDialog.Title || tReactionNPC.Text != curDialog.Text)
                {
                    if (!parent.isEmptyLocaledDialog(currentDialogID))
                    {
                        DialogResult dr = MessageBox.Show("Текст был изменён, нужно переводить?", "Внимание, ньюанс с переводами", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                            version++;
                    }
                    else version++;
                }
                parent.replaceDialog(new CDialog(holder, tPlayerText.Text, tReactionNPC.Text,
                    precondition, actions, nodes, check_nodes, currentDialogID, version, 
                                        coord, DebugData, cbNoLocale.Checked, nextDialog, cbAutoNode.Checked, autoDefaultNode.Text), currentDialogID);
            }
            //parent.Enabled = true;
            parent.setEnable();
            parent.DialogSelected(true);
            parent.startEmulator(currentDialogID);
            this.Close();
        }


        private void setItemsInDataGrid(List<QuestItem> items, DataGridView dg)
        {
            foreach (QuestItem item in items)
            {
                int item_type = item.itemType;
                string item_name = parent.itemConst.getItemName(item_type);
                int item_attr = item.attribute;
                int count = item.count;
                string cond;
                try
                {
                    cond = item.condition.ToString("G6", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    cond = "0";
                }
                object[] row = { item_name, count, item_attr, cond};
                dg.Rows.Add(row);
            }
        }

        private List<object[]> getItemsDataGrid(DataGridView dg)
        {
            List<object[]> result = new List<object[]>();
            foreach (DataGridViewRow row in dg.Rows)
            {
                string typeName = row.Cells[dg.Name + "_itemType"].FormattedValue.ToString();
                if (!typeName.Equals(""))
                {
                    int quantity = 1;
                    float cond = 0;

                    float.TryParse(row.Cells[dg.Name + "_itemCond"].FormattedValue.ToString().Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out cond);

                    if ((int.TryParse(row.Cells[dg.Name + "_itemQuantity"].FormattedValue.ToString(), out quantity)) && (quantity >= 1))
                    {
                        int typeID = parent.itemConst.getIDOnName(typeName);
                        int attr = 0;
                        int.TryParse(row.Cells[dg.Name + "_itemAttr"].FormattedValue.ToString(), out attr);
                        object[] obj = { typeID, quantity, attr, cond };
                        result.Add(obj);
                    }
                }
            }
            return result;
        }

        private void initAchievementsTab()
        {
            foreach (string item in CAchivements.getListNames())
                ((DataGridViewComboBoxColumn)dataAchievements.Columns[1]).Items.Add(item);

            int id = 0;
            foreach (int perkID in editPrecondition.Achievements)
            {
                string name = CAchivements.getNameByID(perkID);
                object[] row = { id, name, "иметься должен " };
                dataAchievements.Rows.Add(row);
                id++;
            }
            foreach (int perkID in editPrecondition.noAchievements)
            {
                string name = CAchivements.getNameByID(perkID);
                object[] row = { id, name, "отсутствовать должен " };
                dataAchievements.Rows.Add(row);
                id++;
            }
            this.checkAchievements();
        }

        private bool checkAchievements()
        {

            this.editPrecondition.Achievements.Clear();
            this.editPrecondition.noAchievements.Clear();
            foreach (DataGridViewRow row in dataAchievements.Rows)
            {
                int perkID = CAchivements.getIDByName(row.Cells[1].FormattedValue.ToString());
                if (perkID == 0) continue;
                if (row.Cells[2].FormattedValue.ToString() == "иметься должен ")
                    this.editPrecondition.Achievements.Add(perkID);
                else
                    this.editPrecondition.noAchievements.Add(perkID);
            }

            this.checkAchIndicates();
            return true;
        }

        private void initSkillsTab()
        {
            SkillConstants skills = this.parent.skills;
            int id = 0;
            foreach (string skill_name in skills.getKeys())
            {
                string a = "";
                string b = "";
                string skill_id = skills.getTtID(skill_name);
                DialogSkill skill = this.editPrecondition.Skills.getSkillValByName(skill_id);
                if (skill != null)
                {
                    a = skill.min;
                    b = skill.max;
                }
                object[] row = { id, skill_name, a, b };
                dataSkill.Rows.Add(row);
                id++;
            }

            foreach (string item in parent.perks.getNames())
                ((DataGridViewComboBoxColumn)dataPerks.Columns[1]).Items.Add(item);

            id = 0;
            foreach (int perkID in editPrecondition.Perks)
            {
                string name = parent.perks.getName(perkID);
                object[] row = { id, name, "иметься должен " };
                dataPerks.Rows.Add(row);
                id++;
            }
            foreach (int perkID in editPrecondition.noPerks)
            {
                string name = parent.perks.getName(perkID);
                object[] row = { id, name, "отсутствовать должен " };
                dataPerks.Rows.Add(row);
                id++;
            }
            this.checkSkills();
        }

        private bool checkSkills()
        {
            this.editPrecondition.Skills.Clear();
            this.editPrecondition.Perks.Clear();
            this.editPrecondition.noPerks.Clear();
            foreach (DataGridViewRow row in dataSkill.Rows)
            {
                if (row.Cells[0].FormattedValue.ToString() != "")
                {
                    string skill_name = parent.skills.getTtID(row.Cells[1].FormattedValue.ToString());
                    string stringA = row.Cells[2].FormattedValue.ToString().Replace('.', ',');
                    string stringB = row.Cells[3].FormattedValue.ToString().Replace('.', ',');

                    int intA;
                    int intB;
                    if ((stringA != "") && (!int.TryParse(stringA, out intA)))
                    {
                        MessageBox.Show("Неправильное условие по навыкам!");
                        return false;
                    }
                    if ((stringB != "") && (!int.TryParse(stringB, out intB)))
                    {
                        MessageBox.Show("Неправильное условие по навыкам!");
                        return false;
                    }


                    if ((stringA == "") && (stringB == ""))
                    {
                        continue;
                    }
                    this.editPrecondition.Skills.Add(skill_name, stringA, stringB);
                }
            }

            foreach (DataGridViewRow row in dataPerks.Rows)
            {
                int perkID = parent.perks.getID(row.Cells[1].FormattedValue.ToString());
                if (perkID == 0) continue;
                if (row.Cells[2].FormattedValue.ToString() == "иметься должен ")
                    this.editPrecondition.Perks.Add(perkID);
                else
                    this.editPrecondition.noPerks.Add(perkID);
            }

            this.checkSkillIndicates();
            return true;
        }

        private void checkSkillIndicates()
        {
            if (editPrecondition.Skills.Any() || editPrecondition.Perks.Any() || editPrecondition.noPerks.Any())
                pictureSkill.Visible = true;
            else
                pictureSkill.Visible = false;
        }

        private void checkAchIndicates()
        {
            pictureAchievements.Visible = editPrecondition.Achievements.Any() || editPrecondition.noAchievements.Any();
        }

        private void fillGroupBonuses()
        {
            cbFracBonus.Items.Clear();
            cbFracBonus.Items.AddRange(CFracBonuses.getListNames());
            
            cbFracGroup.Items.Clear();
            foreach (KeyValuePair<int, string> pair in parent.fractions2.getListOfFractions())
            {
                cbFracGroup.Items.Add(pair.Value);
            }

            int fracID = editPrecondition.fracBonus[0];
            int bonusID = editPrecondition.fracBonus[1];
            cbFracNoMusthave.Checked = !Convert.ToBoolean(editPrecondition.fracBonus[2]);
            cbFracGroup.SelectedItem = parent.fractions2.getFractionDesctByID(fracID);
            cbFracBonus.SelectedItem = CFracBonuses.getNameByID(bonusID);
        }

        private void initReputationTab(DataGridView dataReputation, CFracConstants fractions, 
            Dictionary<int, List<double>> reputation, Dictionary<string, List<double>> npc_reputation)
        {
            foreach (KeyValuePair<int, string> pair in fractions.getListOfFractions())
                ((DataGridViewComboBoxColumn)dataReputation.Columns[1]).Items.Add(pair.Value);

            foreach (KeyValuePair<string, List<double>> pair in this.editPrecondition.NPCReputation)
            {
                ((DataGridViewComboBoxColumn)dataReputation.Columns[1]).Items.Add(pair.Key);
            }

            foreach (KeyValuePair<int, string> pair in fractions.getListOfFractions())
            {
                int id = pair.Key;
                string name = pair.Value;
                string a = "";
                string b = "";
                if (reputation.Keys.Contains(pair.Key))
                {
                    if (reputation[id].Count == 3)         // костыль для старой версии, выжечт огнем позже
                    {
                        double type = reputation[pair.Key][0];
                        if (type == 0 || (type == 1))
                            a = reputation[pair.Key][1].ToString();
                        if (type == 0 || (type == 2))
                            b = reputation[pair.Key][2].ToString();
                    }
                    else if (reputation[id].Count == 2)
                    {
                        if (reputation[id][0] != double.NegativeInfinity)
                            a = reputation[id][0].ToString();
                        if (reputation[id][1] != double.PositiveInfinity)
                            b = reputation[id][1].ToString();
                    }
                }
                if (!(a.Any() || b.Any())) continue;
                object[] row = { id, name, a, b };
                dataReputation.Rows.Add(row);
            }
            foreach(KeyValuePair<string, List<double>> pair in npc_reputation)
            {
                string name = pair.Key;
                string a = "";
                string b = "";
                if (npc_reputation[pair.Key].Count == 3)         // костыль для старой версии, выжечт огнем позже
                {
                    double type = npc_reputation[pair.Key][0];
                    if (type == 0 || (type == 1))
                        a = npc_reputation[pair.Key][1].ToString();
                    if (type == 0 || (type == 2))
                        b = npc_reputation[pair.Key][2].ToString();
                }
                else if (npc_reputation[pair.Key].Count == 2)
                {
                    if (npc_reputation[pair.Key][0] != double.NegativeInfinity)
                        a = npc_reputation[pair.Key][0].ToString();
                    if (npc_reputation[pair.Key][1] != double.PositiveInfinity)
                        b = npc_reputation[pair.Key][1].ToString();
                }
                if (!(a.Any() || b.Any())) continue;
                object[] row = { "", name, a, b };
                dataReputation.Rows.Add(row);
            }
            this.checkReputation(dataReputation, fractions, reputation, npc_reputation);
        }

        private bool checkReputation(DataGridView dataReputation, CFracConstants fractions,
            Dictionary<int, List<double>> reputation, Dictionary<string, List<double>> npc_reputation)
        {

            reputation.Clear();
            npc_reputation.Clear();
            foreach (DataGridViewRow row in dataReputation.Rows)
            {
                    string fractionName = row.Cells[1].FormattedValue.ToString();
                    string stringA = row.Cells[2].FormattedValue.ToString().Replace('.', ',');
                    string stringB = row.Cells[3].FormattedValue.ToString().Replace('.', ',');

                    if ((stringA != "") || (stringB != ""))
                    {
                        double doubleA;
                        double doubleB;
                        if (!double.TryParse(stringA, out doubleA))
                            doubleA = double.NegativeInfinity;
                        if (!double.TryParse(stringB, out doubleB))
                            doubleB = double.PositiveInfinity;
                        if (doubleA >= doubleB)
                        {
                            MessageBox.Show("Неправильное условие по репутации! Значение А должно быть меньше B", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }


                    int fractionID = fractions.getFractionIDByDescr(fractionName);
                        
                        if (fractionID >= 0)
                        {
                        reputation.Add(fractionID, new List<double>() { doubleA, doubleB });
                        }
                        else
                        {
                        npc_reputation.Add(fractionName.Trim(), new List<double>() { doubleA, doubleB });
                        }
                    }
            }
            this.checkReputationIndicates();
            return true;
        }

        private bool checkEffects()
        {
            this.editPrecondition.MustNoEffects = new List<DialogEffect>();
            this.editPrecondition.NecessaryEffects = new List<DialogEffect>();
            foreach (DataGridViewRow row in dataGridNotEffects.Rows)
            {
                if (row.Cells[0].FormattedValue.ToString() != "")
                {
                    string typeName = row.Cells[0].FormattedValue.ToString();
                    int id = parent.effects.getIDOnDescription(typeName);
                    string stack_from = row.Cells[1].FormattedValue.ToString();
                    string stack_before = row.Cells[2].FormattedValue.ToString();
                    this.editPrecondition.MustNoEffects.Add(new DialogEffect(id, stack_from, stack_before));
                }
            }

            foreach (DataGridViewRow row in dataGridEffects.Rows)
            {
                if (row.Cells[0].FormattedValue.ToString() != "")
                {
                    string typeName = row.Cells[0].FormattedValue.ToString();
                    int id = parent.effects.getIDOnDescription(typeName);
                    string stack_from = row.Cells[1].FormattedValue.ToString();
                    string stack_before = row.Cells[2].FormattedValue.ToString();
                    this.editPrecondition.NecessaryEffects.Add(new DialogEffect(id, stack_from, stack_before));
                }
            }
            editPrecondition.nec_effects_is_or = rbEffectsOr.Checked;
            editPrecondition.must_no_effects_is_or = rbNoEffectsOr.Checked;
            this.checkEffectsIndicates();
            return true;
        }

        private void initTransportTab()
        {
            this.cbTransportInList.Checked = this.editPrecondition.transport.inTransportList;
            this.cbNotInTransportList.Checked = this.editPrecondition.transport.notInTransportList;

            this.cbInBoatList.Checked = this.editPrecondition.transport.inBoatList;
            this.cbNotInBoatList.Checked = this.editPrecondition.transport.notInBoatList;
            this.cbIsBoatInTransit.Checked = this.editPrecondition.transport.boatInTransit;
            this.cbIsBoatStopped.Checked = this.editPrecondition.transport.boatStopped;
            this.boatName.Text = this.editPrecondition.transport.boatName;

            this.checkTransportIndicates();
        }

        private void initTutorialTab()
        {
            if (this.curDialog.Precondition.tutorialPhase != -1)
            {
                this.cbTutorialPhase.SelectedItem = this.parent.tutorialPhases.getNameByID(this.curDialog.Precondition.tutorialPhase);
            }
            else { this.cbTutorialPhase.SelectedItem = null; }
            this.checkOtherIndicates();
        }

        private void initPVPTab()
        {
            this.cbPVPRank1.SelectedIndex = curDialog.Precondition.PVPranks[0];
            this.cbPVPRank2.SelectedIndex = Math.Min(curDialog.Precondition.PVPranks[1], parent.pvPRanks.getKeys().Count - 1);
            this.cbRatingPVPMode.SelectedItem = CPVPConstans.getPVPModeNameByID(curDialog.Precondition.PVPMode);
            this.checkOtherIndicates();
        }

        private void initWeatherTab()
        {
            cbSpaceWeather.Items.Clear();
            cbSpaceWeather.Items.Add("-1 Карта игрока");
            foreach (var i in parent.spacesConst.getSpacesNamesWithoutInstances())
            {
                cbSpaceWeather.Items.Add(i);
            }
            string spaceName = parent.spacesConst.getSpaceNameByID(curDialog.Precondition.weather.space);
            if (curDialog.Precondition.weather.space < 0)
                cbSpaceWeather.SelectedIndex = 0;// "-1 Карта игрока";
            else
                cbSpaceWeather.SelectedItem = parent.spacesConst.getSpaceByID(curDialog.Precondition.weather.space);
            string[] a = curDialog.Precondition.weather.timeStart.Split(':');
            nupHour1.Value = Convert.ToDecimal(a[0]);
            nupMin1.Value = Convert.ToDecimal(a[1]);

            a = curDialog.Precondition.weather.timeEnd.Split(':');
            nupHour2.Value = Convert.ToDecimal(a[0]);
            nupMin2.Value = Convert.ToDecimal(a[1]);
            rbWeatherAnd.Checked = !curDialog.Precondition.weather.is_or;
            rbWeatherOr.Checked = curDialog.Precondition.weather.is_or;
            List<int> indexes = new List<int>();

            List<string> weathers;
            if (curDialog.Precondition.weather.space < 0)
                weathers = Weathers.getAllWeathers();
            else
                weathers = Weathers.getWeathers(spaceName);

            foreach (var w in curDialog.Precondition.weather.weathers)
            {
                int idx = weathers.IndexOf(w);
                if (idx >= 0)
                    indexes.Add(idx);
            }
  
            foreach(var idx in indexes)
            {
                lbWeather.SetItemChecked(idx, true);
            }

            cbWeatherOnlyNO.Checked = curDialog.Precondition.weather.only_no;
            checkWeatherIndicates();
        }

        private void initItemsTab()
        {

            if (editPrecondition.items.is_or)
                rbItemsOr.Checked = true;
            if (editPrecondition.itemsNone.is_or)
                rbNonItemsOr.Checked = true;

            if ((this.editPrecondition.items.itemCategory != -1) || (this.editPrecondition.itemsNone.itemCategory != -1))
            {
                this.rbCategory.Checked = true;
                this.cbCategory.SelectedItem = parent.itemCategories.getNameOnID(this.editPrecondition.items.itemCategory);
                this.cbNonCategory.SelectedItem = parent.itemCategories.getNameOnID(this.editPrecondition.itemsNone.itemCategory);
            }
            else if (this.editPrecondition.items.Any() || this.editPrecondition.itemsNone.Any())
            {
                this.rbItems.Checked = true;
                cbItemsEquipped.Checked = editPrecondition.items.equipped;
                cbItemsNOEquipped.Checked = editPrecondition.items.not_equipped;
                setItemsInDataGrid(this.editPrecondition.items.items, GVItems);
                setItemsInDataGrid(this.editPrecondition.itemsNone.items, GVNonItems);
            }
            checkItemsIndicates();

        }
        private void initEffectsTab()
        {

            ((DataGridViewComboBoxColumn)dataGridEffects.Columns[0]).Items.Add("");
            ((DataGridViewComboBoxColumn)dataGridNotEffects.Columns[0]).Items.Add("");

            foreach (string effect_name in parent.effects.getAllDescriptions())
            {
                ((DataGridViewComboBoxColumn)dataGridEffects.Columns[0]).Items.Add(effect_name);
                ((DataGridViewComboBoxColumn)dataGridNotEffects.Columns[0]).Items.Add(effect_name);
            }
            ((DataGridViewComboBoxColumn)dataGridEffects.Columns[0]).Sorted = true;
            ((DataGridViewComboBoxColumn)dataGridNotEffects.Columns[0]).Sorted = true;

            foreach (DialogEffect effect in this.editPrecondition.MustNoEffects)
            {
                string name = parent.effects.getDescriptionOnID(effect.getID());
                string stack_from = effect.getStackFrom().ToString();
                string stack_before = effect.getStackBefore().ToString();
                object[] row = { name, stack_from, stack_before };
                dataGridNotEffects.Rows.Add(row);
            }

            foreach (DialogEffect effect in this.editPrecondition.NecessaryEffects)
            {
                string name = parent.effects.getDescriptionOnID(effect.getID());
                string stack_from = effect.getStackFrom().ToString();
                string stack_before = effect.getStackBefore().ToString();
                object[] row = { name, stack_from, stack_before };
                dataGridEffects.Rows.Add(row);
            }
            this.checkEffectsIndicates();

            if (editPrecondition.nec_effects_is_or)
                rbEffectsOr.Checked = true;
            if (editPrecondition.must_no_effects_is_or) rbNoEffectsOr.Checked = true;

        }

        private void initActionTab()
        {
            if (curDialog.Actions.actionCamera.Any())
            {
                cbCamera.Checked = true;
                tbCamera.Text = curDialog.Actions.actionCamera;
                if (curDialog.Actions.actionCameraSmoothly)
                {
                    cbCameraSmoothly.Checked = curDialog.Actions.actionCameraSmoothly;
                }
            }

            tbAnimationPlayer.Items.Clear();
            tbAnimationNPC.Items.Clear();
            tbActionNPC.Items.Clear();
            
            foreach (string key in parent.avAmin.getKeys())
            {
                tbAnimationPlayer.Items.Add(key);
                tbAnimationNPC.Items.Add(key);
            }
            foreach (string key in parent.npcActions.getKeys())
            {
                tbActionNPC.Items.Add(key);
            }
            if (curDialog.Actions.actionActionNPC != 0)
            {
                tbActionNPC.SelectedItem = parent.npcActions.getName(curDialog.Actions.actionActionNPC.ToString());
                tbAdditionalAction.Text = curDialog.Actions.actionAdditionalActionNPC;
            }

            if (curDialog.Actions.actionAnimationPlayer.Any())
            {
                cbAnimationPlayer.Checked = true;
                tbAnimationPlayer.Text = curDialog.Actions.actionAnimationPlayer;
            }
            if (curDialog.Actions.actionAnimationNPC.Any())
            {
                cbAnimationNPC.Checked = true;
                tbAnimationNPC.Text = curDialog.Actions.actionAnimationNPC;
            }
            cbActionNPC.Checked = curDialog.Actions.actionActionNPC != 0;
            tbAdditionalAction.Text = curDialog.Actions.actionAdditionalActionNPC;
            if (curDialog.Actions.actionAvatarPoint.Any())
            {
                cbAvatarPoint.Checked = true;
                tbAvatarPoint.Text = curDialog.Actions.actionAvatarPoint;
            }
            foreach (string key in parent.listSouds.getKeys())
            {
                tbPlaySonund.Items.Add(key);
            }
            if (curDialog.Actions.actionPlaySound.Any())
            {
                cbPlaySonund.Checked = true;
                tbPlaySonund.Text = curDialog.Actions.actionPlaySound;
            }
            nupChangeMoney.Value = Convert.ToDecimal(curDialog.Actions.changeMoney);
            tbChangeMoneyFailNode.Text = "";
            if (curDialog.Actions.changeMoneyFailNode != 0)
            {
                tbChangeMoneyFailNode.Text = curDialog.Actions.changeMoneyFailNode.ToString();
            }
            this.checkActionIndicates();
        }

        private void initLevelTab()
        {
            
            string[] tmp;
            tmp = curDialog.Precondition.PlayerLevel.Split(':');
            if (tmp.Any())
            {
                if (tmp.Length > 1)
                {
                    mtbPlayerLevelMin.Text = tmp[0];
                    mtbPlayerLevelMax.Text = tmp[1];
                }
                else mtbPlayerLevelMin.Text = curDialog.Precondition.PlayerLevel.ToString();
            }
            this.checkOtherIndicates();

        }
        private bool checkLevel()
        {
            return mtbPlayerLevelMin.Text.Any() || mtbPlayerLevelMax.Text.Any();
        }

        private void initKarmaPKTab()
        {
            labelDescription.Text = "Задаются пороги Кармы A,B такие, что A < Karma < B \n" +
                "Игрок начинает игру с Кармой ПК = 0. \n" +
                "За каждое ПК убийство Карма увеличивается на 100. \n" +
                "После превышения Кармой 500 жизнь осложняется: \n" +
                "Дроп выше, NPC не разговаривают, ник подсвечен красным";

            List<int> karma = this.editKarmaPK;
            if (karma.Any())
            {
                if (karma[0] == 0 || karma[0] == 1)
                {
                    aTextBox.Text = karma[2].ToString();    // костыль, похоже перепутаны значения
                }
                if (karma[0] == 0 || karma[0] == 2)
                {
                    bTextBox.Text = karma[1].ToString();    // костыль, похоже перепутаны значения
                }
            }
        }

        private void checkKarmaPK()
        {
            this.editKarmaPK.Clear();
            int flag = 0;
            int a = 0;
            int b = 0;
            if ((aTextBox.Text != "") || (bTextBox.Text != ""))
            {
                if (aTextBox.Text != "")
                {
                    a = int.Parse(aTextBox.Text);
                    flag = 1;
                }
                if (bTextBox.Text != "")
                {
                    b = int.Parse(bTextBox.Text);
                    if (flag == 1) flag = 0;
                    else flag = 2;
                }

                this.editKarmaPK.Add(flag);
                this.editKarmaPK.Add(b);        // тот же костыль
                this.editKarmaPK.Add(a);        // тот же костыль
            }
            this.checkOtherIndicates();
        }

        private void checkActionIndicates()
        {
            pictureAction.Visible = (cbAnimationNPC.Checked && tbAnimationNPC.Text.Any()) || (cbAnimationPlayer.Checked && tbAnimationPlayer.Text.Any()) ||
                (cbCamera.Checked && tbCamera.Text.Any()) || (cbPlaySonund.Checked && tbPlaySonund.Text.Any()) || (cbAvatarPoint.Checked && tbAvatarPoint.Text.Any()) ||
                (cbActionNPC.Checked && tbActionNPC.Text.Any());   
        }

        private void checkEffectsIndicates()
        {
            pictureEffects.Visible = editPrecondition.MustNoEffects.Any() || editPrecondition.NecessaryEffects.Any();
        }

        private void checkItemsIndicates()
        {
            pictureItems.Visible = (rbCategory.Checked && cbCategory.SelectedIndex != -1) || (rbItems.Checked && GVItems.Rows.Count > 0);
        }


        public void checkOtherIndicates()
        {
            bool result = false;
            result = result || (cbTutorialPhase.SelectedItem != null);
            result = result || (cbPVPRank1.SelectedIndex > 0 || cbPVPRank2.SelectedIndex > 0 || cbRatingPVPMode.SelectedIndex > 0);
            result = result || editKarmaPK.Any();
            result = result || tbCoordinates.Text.Any();
            result = result || (mtbPlayerLevelMax.Text.Any() || mtbPlayerLevelMin.Text.Any());
            result = result || ((cbTutorialPhase.SelectedItem != null) && (Convert.ToInt32(cbMap.SelectedItem.ToString().Split(' ')[0]) != -1));
            result = result || cbSeasonEvent.SelectedIndex > 0;
            pictureOther.Visible = result;
        }

        private void checkWeatherIndicates()
        {
            pictureWeather.Visible = editPrecondition.weather.Any();
        }

        private void checkTransportIndicates()
        {
            pictureTransport.Visible = (cbNotInTransportList.Checked || cbTransportInList.Checked || cbNotInBoatList.Checked || cbInBoatList.Checked ||
                                        cbIsBoatInTransit.Checked || cbIsBoatStopped.Checked);
        }

        private void tabQuestsCircs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkClanOptionsIndicator();
            this.checkReputation(dataReputation, parent.fractions, this.editPrecondition.Reputation, this.editPrecondition.NPCReputation);
            this.checkReputation(dataReputation2, parent.fractions2, this.editPrecondition.Reputation2, new Dictionary<string, List<double>>());
            this.checkEffects();
            this.checkSkills();
            this.checkAchievements();
            this.checkActionIndicates();
            this.checkItemsIndicates();
            this.checkTransportIndicates();
            this.checkOtherIndicates();
            checkKnowlegeIndicates();
            checkWeatherIndicates();
        }

        private void digitTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void cbAutoNode_CheckedChanged(object sender, EventArgs e)
        {
            bool check = !cbAutoNode.Checked;
            //tPlayerText.Enabled = check;
            //tReactionNPC.Enabled = check;
            /*
            tNodes.Enabled = check;
            gbPrecondition.Enabled = check;
            gbActions.Enabled = check;
            actionsCheckBox.Enabled = check;
            cbForDev.Enabled = check;
            */
            //tPlayerText.Visible = check;
            //tReactionNPC.Visible = check;
            /*
            tNodes.Visible = check;
            gbPrecondition.Visible = check;
            gbActions.Visible = check;
            actionsCheckBox.Visible = check;
            cbForDev.Visible = check;
            */
            autoPanel.Enabled = !check;
            autoPanel.Visible = !check;
        }

        private void rbItems_CheckedChanged(object sender, EventArgs e)
        {
            panelItems.Visible = rbItems.Checked;
            panel1.Visible = rbItems.Checked;
            panel3.Visible = rbItems.Checked;
            cbItemsEquipped.Visible = rbItems.Checked;
            cbItemsNOEquipped.Visible = rbItems.Checked;
            //GVNonItems.Visible = rbItems.Checked;
        }

        private void rbCategory_CheckedChanged(object sender, EventArgs e)
        {
            cbCategory.Visible = rbCategory.Checked;
            cbNonCategory.Visible = rbCategory.Checked;
            //cbItemsEquipped.Visible = rbItems.Checked;
           // cbItemsNOEquipped.Visible = rbItems.Checked;
        }

        private void tabItems_SizeChanged(object sender, EventArgs e)
        {
            GVItems.Width = (sender as Panel).Width / 2;
            GVNonItems.Width = (sender as Panel).Width / 2;
            GVNonItems.Location = new Point((sender as Panel).Width / 2, GVNonItems.Location.Y);
        }

        private void rbOr_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbAnd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbRadioNode_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void btnTODO_Click(object sender, EventArgs e)
        {

            string text = parent.dialogs.getDialogToDoToolTip(currentDialogID);
            Forms.InputBox input = new Forms.InputBox("TODO:", text);
            DialogResult result =  input.ShowDialog();
            if (result != DialogResult.OK) return;
            parent.dialogs.setDialogToDoToolTip(currentDialogID, input.getResult());
        }

        private void btnLastNPCPhrase_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();
            if (isAdd)
            {
                CDialog dialog = parent.getAnyDialogOnID(currentDialogID);
                data.Add(dialog.DialogID.ToString() + " :" + dialog.Text);
            }
            else
                foreach (CDialog dialog in parent.getDialogsWithDialogIDInNodes(currentDialogID))
                    data.Add(dialog.DialogID.ToString() + " :" + dialog.Text);

            if (listLastDialogsForm.IsDisposed)
                listLastDialogsForm = new ListLastDialogsForm(parent);
            listLastDialogsForm.setListData(data);
            listLastDialogsForm.Location = new Point(this.Location.X + this.Width + 10, this.Location.Y);
            listLastDialogsForm.Show();
        }

        private void cbSpaceWeather_SelectedIndexChanged(object sender, EventArgs e)
        {
            string spaceName = cbSpaceWeather.SelectedItem.ToString();
            int space_id = Convert.ToInt32(spaceName.Split(' ')[0]);
            
            lbWeather.Items.Clear();
            if (space_id < 0)
                foreach (var i in Weathers.getAllWeathers())
                {
                    lbWeather.Items.Add(i);
                }
            else
            {
                spaceName = parent.spacesConst.getSpaceNameByID(space_id);
                foreach (var i in Weathers.getWeathers(spaceName))
                {
                    lbWeather.Items.Add(i);
                }
            }
        }

        private void tReactionNPC_TextChanged(object sender, EventArgs e)
        {
            if (!CSettings.hasErrorFinder()) return;
            if (debounceTimer != null)
            {
                debounceTimer.Stop();
                debounceTimer.Dispose();
            }

            debounceTimer = new System.Timers.Timer(1000);
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += async (s, ev) =>
            {
                await Task.Run(() => SpellChecker.CheckAndHighlightSpellingErrors(tReactionNPC));
            };

            debounceTimer.Start();

        }

        private void update_errorFiner_btn()
        {
            btnFindError.FlatStyle = (CSettings.hasErrorFinder()) ? FlatStyle.Flat : FlatStyle.Standard;
            btnFindError.Text = (CSettings.hasErrorFinder()) ? "Отключить поиск" : "Поиск ошибок";
        }

        private void btnFindError_Click(object sender, EventArgs e)
        {
            CSettings.setErrorFinder(!CSettings.hasErrorFinder());
            update_errorFiner_btn();

            SpellChecker.CheckAndHighlightSpellingErrors(tPlayerText);
            SpellChecker.CheckAndHighlightSpellingErrors(tReactionNPC);
        }

        private void cbItemsEquipped_CheckedChanged(object sender, EventArgs e)
        {
            if (cbItemsEquipped.Checked)
            {
                cbItemsNOEquipped.Checked = false;
            }
        }

        private void cbItemsNOEquipped_CheckedChanged(object sender, EventArgs e)
        {
            if (cbItemsNOEquipped.Checked)
            {
                cbItemsEquipped.Checked = false;
            }
        }
    }
}
