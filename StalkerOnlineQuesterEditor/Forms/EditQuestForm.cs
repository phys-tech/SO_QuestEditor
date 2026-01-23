using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StalkerOnlineQuesterEditor
{
    //! Форма для редактирования условий квеста
    public partial class EditQuestForm : Form
    {
        int SHOW_MESSAGE_TAKE = 1;
        int SHOW_MESSAGE_CLOSE = 2;
        int SHOW_MESSAGE_PROGRESS = 4;
        int SHOW_JOURNAL = 8;
        int SHOW_ONWIN = 16;
        int SHOW_ONFAILED = 32;
        int NOT_SHOW_AVAILABILITY = 64;
        int SHOW_ONGET = 128;
        int SHOW_ONOPEN = 256;
        int SHOW_ONTEST = 512;

        const int ADD_NEW = 1;
        const int EDIT = 2;
        const int EDIT_SUB = 3;
        const int ADD_SUB = 4;

        int ITEM_REWARD = 0;
        int ITEM_QUESTRULES = 1;
        //int ITEM_LOCALIZATION = 2;
        int ITEM_PENALTY = 4;

        int QuestType;

        int iInformationHeight = 0;
        int iTargetHeight = 0;
        int iRulesHeight = 0;
        int iRewardHeight = 0;
        int QuestID;

        Dictionary<int, string> scenaryTypes = new Dictionary<int, string>() {{0, "None"}, {2, "2 ATTACK_ON_PLAYER"}, {4, "4 GO_TO_WAYPOINT"}};

        Dictionary<int, string> dReputation = new Dictionary<int, string>();

        public CQuestTarget editTarget = new CQuestTarget();
        public CQuestRules editQuestRules = new CQuestRules();
        public CQuestReward editQuestReward = new CQuestReward();
        public CQuestReward editQuestPenalty = new CQuestReward();
        public CQuestInformation editInformation = new CQuestInformation();
        public Dictionary<int, QuestItemInfo> items_rules = new Dictionary<int, QuestItemInfo>();
        public Dictionary<int, QuestItemInfo> items_reward = new Dictionary<int, QuestItemInfo>();
        public Dictionary<int, QuestItemInfo> items_penalty = new Dictionary<int, QuestItemInfo>();
        public MainForm parent;
        public CQuest quest;
        int iState;
        System.Timers.Timer debounceTimer;
        //! Конструктор, заполянет форму данными
        public EditQuestForm(MainForm parent, int currentQuest, int iState, int subID = 0)
        {
            InitializeComponent();
            this.parent = parent;
            this.iState = iState;
            if (iState == ADD_SUB)
                this.QuestID = subID;
            else
                this.QuestID = currentQuest;

            if (iState != ADD_NEW)
                this.quest = parent.getQuestOnQuestID(currentQuest);
            else
                this.quest = new CQuest();

            if (iState == ADD_SUB || iState == EDIT_SUB)
            {
                titleTextBox.Enabled = false;
                showCloseCheckBox.Enabled = false;
                showCloseCheckBox.Checked = false;
                showTakeCheckBox.Enabled = false;
                showTakeCheckBox.Checked = false;           
            }

            if (iState == ADD_SUB)
            {
                CQuest parent_quest = parent.getQuestOnQuestID(quest.Additional.IsSubQuest);
            }

            if (iState == EDIT || iState == EDIT_SUB)
            {
                editQuestRules = quest.QuestRules;
                editQuestReward = quest.Reward;
                editQuestPenalty = quest.QuestPenalty;
                editTarget = quest.Target;
                editInformation = quest.QuestInformation;
                foreach (var item in editQuestRules.items)
                {
                    if (editInformation.Items.ContainsKey(item.itemType))
                        {
                        items_rules.Add(item.itemType, editInformation.Items[item.itemType]);
                    }
                }
                foreach (var item in editQuestReward.items)
                {
                    if (editInformation.Items.ContainsKey(item.itemType))
                    {
                        items_reward.Add(item.itemType, editInformation.Items[item.itemType]);
                    }
                }
                foreach (var item in editQuestPenalty.items)
                {
                    if (editInformation.Items.ContainsKey(item.itemType))
                    {
                        items_penalty.Add(item.itemType, editInformation.Items[item.itemType]);
                    }
                }
            }
            this.Text += " " + this.QuestID.ToString();
            this.Text += (": " + this.QuestID.ToString() + "   Версия: " + this.quest.Version.ToString());
            fillForm();
        }

        void clearTargetContent()
        {
            targetComboBox.Items.Clear();
            resultComboBox.Items.Clear();
            //targetComboBox.SelectedText = "";
//          repeatComboBox.Items.Clear();
        }
        //! Заполняет форму данными о квесте
        void fillForm()
        {
            cbHidden.Checked = quest.hidden;
            
            //bItemQuestRules.ImageKey = "";
            //bItemQuestRules.ImageKey = "but_indicate";
            resultComboBox.Items.Clear();
            foreach (СQuestType eventDescription in parent.questConst.getListQuests())
                eventComboBox.Items.Add(eventDescription.getDescription());
            cbPriority.Items.Clear();
            cbPriority.Items.AddRange(QuestPriorities.getListNames());
            cbPriority.SelectedItem = QuestPriorities.getNameByID(quest.Priority);
            nudLevel.Value = quest.Level;
            showProgressCheckBox.Checked = true;
            showCloseCheckBox.Checked = true;
            showTakeCheckBox.Checked = true;
            showJournalCheckBox.Checked = true;
            nudLevel.Visible = (quest.Additional.IsSubQuest == 0);
            labelLevel.Visible = (quest.Additional.IsSubQuest == 0);
            lbQuestLink.Visible = (quest.Additional.IsSubQuest == 0);
           
            //CQuest parentQuest =  new CQuest();

            CQuest parentQuest =  new CQuest();
            //if (iState == EDIT_SUB)
            //{
            //    parentQuest = parent.getQuestOnQuestID(quest.Additional.IsSubQuest);
            //}

            availabilityCheckBox.Checked = (quest.Additional.ShowProgress & this.NOT_SHOW_AVAILABILITY) == 0; 
            fillQuestLinks();
            fillPreconditiomForm();
            fillConditionsTab();
            
            initCreateNPCPanel();
            initCreateMobPanel();
            if (iState != ADD_NEW)
            {
                fillTargetForm(quest.Target.QuestType);
                fillChangedQuests();
            }
            fillQuestRulesForm();
            cbOldQuest.Checked = quest.isOld;

            cbFraction2Bonus.Items.Clear();
            cbFraction2Bonus.Items.Add("нет");
            foreach (var i in parent.fractions2.getListOfFractions())
            {
                cbFraction2Bonus.Items.Add(i.Value);
            }
            cbFraction2Bonus.SelectedItem = parent.fractions2.getFractionDesctByID(quest.Additional.isFractionBonus);

            if (iState == EDIT || iState == EDIT_SUB)
            {
                if (quest.Target.onFin == 1)
                    winRButton.Checked = true;
                else
                    loseRButton.Checked = true;

                eventComboBox.SelectedItem = parent.questConst.getDescription(quest.Target.QuestType);
                titleTextBox.Text = quest.QuestInformation.Title;
                update_errorFiner_btn();
                SpellChecker.CheckAndHighlightSpellingErrors(titleTextBox);
                descriptionTextBox.Text = quest.QuestInformation.Description;
                SpellChecker.CheckAndHighlightSpellingErrors (descriptionTextBox);
                descriptionOnTestTextBox.Text = quest.QuestInformation.DescriptionOnTest;
                SpellChecker.CheckAndHighlightSpellingErrors(descriptionOnTestTextBox);
                descriptionClosedTextBox.Text = quest.QuestInformation.DescriptionClosed;
                SpellChecker.CheckAndHighlightSpellingErrors(descriptionClosedTextBox);

                messageGridView.Rows.Clear();
                foreach (var i in quest.QuestInformation.messages)
                {
                    object[] row = { QuestMessageType.getName(i.state1), QuestMessageType.getName(i.state2), i.message, i.isHighMessage };
                    messageGridView.Rows.Add(row);
                }
                cantCancelCheckBox.Checked = quest.Additional.CantCancel;
                cantFailCheckBox.Checked = quest.Additional.CantFail;

                showCloseCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_CLOSE) > 0);
                showTakeCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_TAKE) > 0);
                showProgressCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_MESSAGE_PROGRESS) > 0);
                showJournalCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_JOURNAL) > 0);
                //showGetCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_ONGET) > 0);
                //showWinCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_ONWIN) > 0);
                //showFailedCheckBox.Checked = ((quest.Additional.ShowProgress & this.SHOW_ONFAILED) > 0);

                if (quest.Additional.DebugData != "")
                {
                    this.debugTextBox.Text = quest.Additional.DebugData;
                }
                
                fillTarget();
                fillPrecondition();
                fillQuestRules();
                fillReward();
            }
            else
            {
                winRButton.Checked = true;
            }

        }
        void fillQuestLinks()
        {
            cbQuestLink.Visible = false;
            cbQuestLinkType.Visible = (quest.Additional.IsSubQuest == 0);
            cbQuestLinkType.SelectedIndex = quest.questLinkType;

            foreach(int questid in parent.questChapters)
            {
                string questName = questid.ToString() + " " + parent.getQuestOnQuestID(questid).QuestInformation.Title;
                cbQuestLink.Items.Add(questName);
            }

            if (cbQuestLinkType.SelectedIndex > 0 && quest.questLink > 0)
            {
                cbQuestLink.SelectedIndex = parent.questChapters.IndexOf(quest.questLink);
            }
        }

        //! Заполняет форму условиями квеста (повторное взятие)
        void fillPreconditiomForm()
        {
            repeatComboBox.Items.Add("Повторное взятие квеста после его закрытия.");
            repeatComboBox.Items.Add("Нет повтора.");
            repeatComboBox.SelectedIndex = 1;
            if (iState == ADD_SUB || iState == EDIT_SUB)
                takenPeriodTextBox.Enabled = false;
      
        }
        //! Заполняет данные об условиях повторного взятия квеста
        void fillPrecondition()
        {
            if (quest.Precondition.Repeat == 1)
                repeatComboBox.SelectedIndex = 0;
            else
                repeatComboBox.SelectedIndex = 1;
            IsCounterCheckBox.Checked = quest.Precondition.omniCounter;
            takenPeriodTextBox.Text = quest.Precondition.TakenPeriod.ToString();
            IsGroupCheckBox.Checked = quest.Precondition.isGroup;
        }


        void fillChangedQuests()
        {
            labelGiveQuestsOpened.Text = "";
            labelGiveQuestsClosed.Text = "";
            labelGiveQuestsFailed.Text = "";
            labelGiveQuestsCanceled.Text = "";
            if (CQuests.QuestParentList.ContainsKey(this.QuestID))
            {
                if (CQuests.QuestParentList[QuestID].ContainsKey(0))
                    labelGiveQuestsOpened.Text = Global.GetListAsString(CQuests.QuestParentList[this.QuestID][0]);
                if (CQuests.QuestParentList[QuestID].ContainsKey(1))
                    labelGiveQuestsClosed.Text = Global.GetListAsString(CQuests.QuestParentList[this.QuestID][1]);
                if (CQuests.QuestParentList[QuestID].ContainsKey(2))
                    labelGiveQuestsFailed.Text = Global.GetListAsString(CQuests.QuestParentList[this.QuestID][2]);
                if (CQuests.QuestParentList[QuestID].ContainsKey(3))
                    labelGiveQuestsCanceled.Text = Global.GetListAsString(CQuests.QuestParentList[this.QuestID][3]);
            }
        }



        //! Настраивает форму на определенный тип квеста
        void fillTargetForm(int QuestType)
        {
            this.QuestType = QuestType;        
            dynamicCheckBox.Enabled = false;
            lTargetAttr1.Enabled = false;
            targetAttributeComboBox2.Enabled = false;
            lTargetAttr1.Text = "Атрибут2";
            ltargetResult.Text = "Результат";
            cbState.Text = "Учитывать состояние";
            lState.Text = "Состояние";

            panelCreateNPC.Visible = QuestType == CQuestConstants.TYPE_CREATE_NPC;
            panelCreateNPC.Enabled = QuestType == CQuestConstants.TYPE_CREATE_NPC;
            panelPVPQuests.Visible = parent.questConst.isPVPQuest(QuestType);
            panelPVPQuests.Enabled = parent.questConst.isPVPQuest(QuestType);

            IsGroupCheckBox.Enabled = (QuestType == CQuestConstants.TYPE_KILLMOBS) || (QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST) || (QuestType == CQuestConstants.TYPE_TRIGGER_ACTION);
            cbPVPtarget2.Visible = false;
            cbPVPOnlyAlly.Visible = false;
            cbPVPOnlyEnemy.Visible = false;
            panelCreateMob.Visible = QuestType == 52;
            panelCreateMob.Enabled = QuestType == 52;


            if (parent.questConst.isPVPQuest(QuestType))
            {
                if (!tabControl.TabPages.Contains(tabConditions))
                    tabControl.TabPages.Add(tabConditions);

                cbConditionPVPTeam.SelectedIndex = quest.Conditions.bePvpWinner;
                cbConditionPVPTeamWin.SelectedIndex = quest.Conditions.pvpWinTeam;
                nupConditionDead.Value = quest.Conditions.notDieCount;
                string result = parent.npcItems.secondaryyWeapons.getNameByID(quest.Conditions.useWeaponType);
                if (result.Any())
                    cbConditionWeapon.SelectedItem = result;
                else
                    cbConditionWeapon.SelectedItem = parent.npcItems.primaryWeapons.getNameByID(quest.Conditions.useWeaponType);
                nupb2ctime.Value = quest.Conditions.duration;

                if ((QuestType == CQuestConstants.TYPE_B2C_FLAG) || (QuestType == CQuestConstants.TYPE_B2C_KILL) || (QuestType == CQuestConstants.TYPE_B2C_REVIVE))
                {
                    labelPVPPoints.Visible = false; labelPVPTeam.Visible = false; labelPVPWeapon.Visible = false;
                    cbConditionPVPTeam.Visible = false; cbConditionWeapon.Visible = false; cbConditionPVPTeamWin.Visible = false;
                }
            }
            else
            {
                if (tabControl.TabPages.Contains(tabConditions))
                    tabControl.TabPages.Remove(tabConditions);
            }



            if (parent.questConst.isSimple(QuestType) || parent.questConst.isPVPQuest(QuestType))
            {
                targetComboBox.Enabled = true;
                lNameObject.Enabled = true;
                lQuantity.Enabled = true;
                quantityUpDown.Enabled = true;
                cbState.Enabled = false;
                cbReputationLow.Enabled = false;
                cbReputationLow.Visible = false;
                btnChangeQuestZones.Visible = false;
                quantityUpDown.Minimum = 0;

                cbPVPMode.Visible = false;
                cbPVPtarget.Visible = false;
                cbPVPtarget2.Visible = false;
                cbPVPtarget3.Visible = false;
                lbPVPtarget.Visible = false;
                cbPVPAdditional.Visible = false;
                labelPVPAdditional.Visible = false;
                //quantityUpDown.Maximum = 32000;

                targetComboBox.SelectedItem = null;
                targetComboBox.SelectedText = "";
                targetComboBox.Items.Clear();

                if (QuestType == CQuestConstants.TYPE_TIMER)
                {
                    targetComboBox.Enabled = false;
                    quantityUpDown.Enabled = false;
                    resultComboBox.Enabled = true;
                    ltargetResult.Text = "Время (сек):";
                }

                if (QuestType == CQuestConstants.TYPE_QUEST_COUNTER)
                {
                    targetComboBox.Enabled = false;
                    quantityUpDown.Enabled = true;
                    resultComboBox.Enabled = true;
                    ltargetResult.Text = "Квест:";
                }

                if ((QuestType == CQuestConstants.TYPE_FARM) || (QuestType == CQuestConstants.TYPE_FARM_AUTO))
                {
                    lNameObject.Text = "Тип предмета:";
                    foreach (CItem item in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(item.getName());
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Add("Обычный.");
                    targetAttributeComboBox.Items.Add("Квестовый.");
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox2.Enabled = true;
                    targetAttributeComboBox2.Items.Clear();
                    targetAttributeComboBox2.Items.Add("Не забирать.");

                    labelTargetAttr.Enabled = true;
                    ltargetResult.Enabled = true;
                    dynamicCheckBox.Enabled = true;
                    cbState.Enabled = true;
                }
                else if ((QuestType == CQuestConstants.TYPE_CRAFT_ITEM) || (QuestType == CQuestConstants.TYPE_CRAFT_ITEM_AUTO))
                {
                    lNameObject.Text = "Рецепт:";
                    foreach (CItem item in parent.receptConst.getAllItems().Values)
                        targetComboBox.Items.Add(item.getName());
                    quantityUpDown.Enabled = true;
                    lQuantity.Enabled = true;
                    targetAttributeComboBox.Items.Clear();
                    labelTargetAttr.Enabled = true;
                    ltargetResult.Enabled = true;
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox2.Enabled = false;
                    labelTargetAttr.Enabled = false;
                    ltargetResult.Enabled = false;
                    dynamicCheckBox.Enabled = false;
                    cbState.Enabled = false;
                }
                else if ((QuestType == CQuestConstants.TYPE_COOK_ITEM) || (QuestType == CQuestConstants.TYPE_COOK_ITEM_AUTO))
                {
                    ltargetResult.Text = "Ингредиент 1";
                    lNameObject.Text = "Ингредиент 2";
                    labelTargetAttr.Text = "Ингредиент 3";

                    resultComboBox.Items.Clear();
                    targetComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Clear();

                    foreach (CItem item in parent.itemConst.getCookItems().Values)
                    {
                        targetComboBox.Items.Add(item.getName());
                        resultComboBox.Items.Add(item.getName());
                        targetAttributeComboBox.Items.Add(item.getName());
                    }
                    quantityUpDown.Enabled = true;
                    lQuantity.Enabled = true;
                    lNameObject.Enabled = true;
                    ltargetResult.Enabled = true;
                    labelTargetAttr.Enabled = true;
                    targetComboBox.Enabled = true;
                    targetAttributeComboBox.Enabled = true;
                    resultComboBox.Enabled = true;

                    targetAttributeComboBox2.Enabled = false;
                    dynamicCheckBox.Enabled = false;
                    cbState.Enabled = false;
                }
                else if (QuestType == CQuestConstants.TYPE_ITEM_CATEGORY || QuestType == CQuestConstants.TYPE_ITEM_CATEGORY_AUTO)
                {
                    lNameObject.Text = "Категория:";
                    foreach (string description in parent.itemCategories.getAllItems().Values)
                        targetComboBox.Items.Add(description);
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Enabled = false;
                }
                else if (QuestType == CQuestConstants.TYPE_QITEM_USE)
                {
                    lNameObject.Text = "Тип предмета:";

                    foreach (CItem item in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(item.getName());
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Add("Обычный.");
                    targetAttributeComboBox.Items.Add("Квестовый.");
                    targetAttributeComboBox.SelectedItem = "Квестовый.";
                    quantityUpDown.Value = 1;
                    quantityUpDown.Enabled = false;
                    bTargetAddDynamic.Enabled = false;
                    bTargetClearDynamic.Enabled = false;

                    lTargetAttr1.Text = "В зоне:";
                    targetAttributeComboBox2.Items.Clear();
                    foreach (var name in parent.zoneConst.getAllZones().Keys)
                        targetAttributeComboBox2.Items.Add(name);
                    lTargetAttr1.Enabled = true;
                    targetAttributeComboBox2.Enabled = true;

                }
                else if (QuestType == CQuestConstants.TYPE_TALK)
                {
                    lNameObject.Text = "Имя NPC:";
                    foreach (CNPCDescription description in parent.npcConst.getAllNPCsDescription().Values)
                        targetComboBox.Items.Add(description.getName());
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;

                }
                else if ((QuestType == CQuestConstants.TYPE_ENTITY_SEEN) || (QuestType == CQuestConstants.TYPE_ENTITY_SEEN_AUTO))
                {
                    ltargetResult.Text = "Сущность:";
                    ltargetResult.Enabled = true;
                    resultComboBox.Items.Clear();
                    resultComboBox.Enabled = true;

                    List<string> entities = new List<string>() { "Creature", "QuestEntity", "TimeEntity" };

                    foreach (var name in entities)
                        resultComboBox.Items.Add(name);

                    resultComboBox.SelectedIndex = 1;
                }
                else if ((QuestType == CQuestConstants.TYPE_ANOMALY) || (QuestType == CQuestConstants.TYPE_ANOMALY_AUTO))
                {
                    lNameObject.Text = "Артефакт:" ;
                    ltargetResult.Text = "Тип аномалии:";
                    ltargetResult.Enabled = true;
                    lTargetAttr1.Enabled = true;
                    foreach (var name in AnomalyTypes.getListNames())
                        resultComboBox.Items.Add(name);
                    resultComboBox.SelectedIndex = 0;
                    targetComboBox.Items.Clear();
                    targetAttributeComboBox2.Items.Clear();
                    targetAttributeComboBox2.Enabled = true;
                    lTargetAttr1.Text = "В зоне:";
                    foreach (var name in parent.zoneConst.getAllZones().Keys)
                        targetAttributeComboBox2.Items.Add(name);
                    targetComboBox.Enabled = true;
                    resultComboBox.Enabled = true;
                    targetAttributeComboBox2.Enabled = true;
                    lQuantity.Enabled = false;
                }
                else if ((QuestType == CQuestConstants.TYPE_KILLNPC) || (QuestType == CQuestConstants.TYPE_KILLNPC_WITH_ONTEST))
                {
                    lNameObject.Text = "Имя NPC:";
                    ltargetResult.Text = "Карта:";
                    foreach (CNPCDescription description in parent.npcConst.getAllNPCsDescription().Values)
                        targetComboBox.Items.Add(description.getName());
                    foreach (var name in parent.spacesConst.getSpacesNames())
                        resultComboBox.Items.Add(name);
                    resultComboBox.Enabled = true;
                    lQuantity.Enabled = true;
                    ltargetResult.Enabled = true;


                    lQuantity.Text = "Количество:";
                    quantityUpDown.Enabled = true;

                    cbState.Text = "Учитывать урон";
                    cbState.Enabled = true;
                    lState.Text = "Процент урона";

                }
                else if ((QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST) || (QuestType == CQuestConstants.TYPE_KILLMOBS))
                {
                    lNameObject.Text = "Тип моба:";
                    ltargetResult.Text = "Подтип моба";
                    ltargetResult.Enabled = true;
                    lTargetAttr1.Enabled = true;
                    btnChangeQuestZones.Visible = true;
                    resultComboBox.Enabled = true;
                    targetComboBox.Items.Clear();
                    foreach (CMobDescription description in parent.mobConst.getAllDescriptions().Values)
                        targetComboBox.Items.Add(description.getName());

                    resultComboBox.Items.Clear();

                    cbState.Text = "Учитывать урон";
                    cbState.Enabled = true;
                    lState.Text = "Процент урона";
                    lQuantity.Text = "Количество:";
                    lTargetAttr1.Text = "Зона";
                    labelTargetAttr.Text = "mob_uniq_ID:";
                    labelTargetAttr.Enabled = true;
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox.Items.Add("");
                    targetAttributeComboBox2.Items.Clear();
                    targetAttributeComboBox2.Enabled = true;
                    targetAttributeComboBox2.Items.Add("");

                    foreach (CZoneDescription description in parent.zoneMobConst.getAllZones().Values)
                        targetAttributeComboBox.Items.Add(description.getName());

                    foreach (CZoneDescription description in parent.zoneConst.getAllZones().Values)
                        targetAttributeComboBox2.Items.Add(description.getName());

                    targetAttributeComboBox.SelectedIndex = 0;
                    targetAttributeComboBox2.SelectedIndex = 0;
                    dynamicCheckBox.Enabled = true;
                }
                else if ((QuestType == CQuestConstants.TYPE_AREA_DISCOVER) || (QuestType == CQuestConstants.TYPE_AREA_LEAVE) ||
                         (QuestType == CQuestConstants.TYPE_IN_AREA))
                {
                    lNameObject.Text = "Имя зоны:";
                    targetComboBox.Items.Clear();
                    foreach (CZoneDescription description in parent.zoneConst.getAllZones().Values)
                        targetComboBox.Items.Add(description.getName());
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                    dynamicCheckBox.Enabled = true;
                }
                else if (QuestType == CQuestConstants.TYPE_MONEYBACK)
                {
                    lNameObject.Text = "Деньги:";
                    targetComboBox.SelectedText = "";
                    targetComboBox.Enabled = false;
                    lQuantity.Enabled = true;
                    quantityUpDown.Enabled = true;
                    targetAttributeComboBox.Enabled = false;
                    labelTargetAttr.Enabled = false;
                }
                else if (QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
                {
                    lNameObject.Text = "Триггер:";
                    targetComboBox.Items.Clear();
                    foreach (string item in parent.triggerConst.getTriggersDescription())
                        targetComboBox.Items.Add(item);
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                    targetAttributeComboBox.Enabled = false;
                    labelTargetAttr.Enabled = false;
                    dynamicCheckBox.Enabled = true;
                }
                else if (QuestType == CQuestConstants.TYPE_DUNGEON_EVENT)
                {
                    ltargetResult.Text = "DungeonID";
                    lNameObject.Text = "ID события";
                    targetComboBox.Items.Clear();
                    resultComboBox.Items.Clear();
                    lQuantity.Enabled = false;
                    quantityUpDown.Enabled = false;
                    targetAttributeComboBox.Enabled = false;
                    resultComboBox.Enabled = true;
                    ltargetResult.Enabled = true;
                    labelTargetAttr.Enabled = false;
                    dynamicCheckBox.Enabled = false;
                    foreach (string item in parent.dungeonConst.getAllSpaceNames())
                        resultComboBox.Items.Add(item);
                }
                else if (QuestType == CQuestConstants.TYPE_DUNGEON_BOX_COUNTER)
                {
                    ltargetResult.Text = "DungeonID";
                    lNameObject.Text = "";
                    targetComboBox.Items.Clear();
                    resultComboBox.Items.Clear();
                    lQuantity.Enabled = true;
                    quantityUpDown.Enabled = true;
                    targetAttributeComboBox.Enabled = false;
                    resultComboBox.Enabled = true;
                    ltargetResult.Enabled = true;
                    labelTargetAttr.Enabled = false;
                    dynamicCheckBox.Enabled = false;
                    foreach (string item in parent.dungeonConst.getAllSpaceNames())
                        resultComboBox.Items.Add(item);
                }
                else if (QuestType == 201 || QuestType == 202 || QuestType == 203 || QuestType == 204)
                {

                }
                else if ((QuestType == CQuestConstants.TYPE_LEAVE_MAP) || (QuestType == CQuestConstants.TYPE_ENTER_MAP))
                {
                    lNameObject.Text = "Карта:";
                    targetComboBox.Items.Clear();
                    foreach (var map in parent.spacesConst.getSpacesNames())
                        targetComboBox.Items.Add(map);
                    quantityUpDown.Enabled = false;
                    lQuantity.Enabled = false;
                    targetAttributeComboBox.Enabled = false;
                    labelTargetAttr.Enabled = false;
                    dynamicCheckBox.Enabled = false;
                }
                else if (QuestType == CQuestConstants.TYPE_ITEM_EQIP)
                {
                    lNameObject.Text = "Тип предмета:";
                    targetComboBox.Items.Clear();
                    foreach (CItem item in parent.itemConst.getAllItems().Values)
                        targetComboBox.Items.Add(item.getName());
                    quantityUpDown.Enabled = false;
                    lQuantity.Enabled = false;
                }
                else if (QuestType == 206)
                {
                    targetComboBox.Items.Clear();
                    foreach (string descr in parent.gui.guiIDs.Keys)
                        targetComboBox.Items.Add(descr);
                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox.Items.Add("Показать.");
                    targetAttributeComboBox.Items.Add("Скрыть.");
                    targetAttributeComboBox.SelectedIndex = 0;
                }
                else if ((QuestType == CQuestConstants.TYPE_GIVE_EFFECT) || (QuestType == CQuestConstants.TYPE_HAVE_EFFECT))
                {
                    lNameObject.Text = "Эффект";
                    targetComboBox.Items.Clear();
                    foreach (string description in parent.effects.getAllDescriptions())
                        targetComboBox.Items.Add(description);
                    quantityUpDown.Enabled = true;
                    targetComboBox.Enabled = true;
                    lNameObject.Enabled = true;
                    lQuantity.Enabled = false;
                }
                else if (QuestType == CQuestConstants.TYPE_KILL)
                {
                    lNameObject.Text = "Тип убийства:";
                    targetComboBox.Items.Clear();
                    targetComboBox.Items.Add("0 Кого-либо");
                    targetComboBox.Items.Add("1 Creature");
                    targetComboBox.Items.Add("2 Avatar");
                    targetComboBox.Items.Add("3 NPC");

                    targetAttributeComboBox.Enabled = true;
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Items.Add("0 не важно");
                    targetAttributeComboBox.Items.Add("1 без трансформации");
                    targetAttributeComboBox.Items.Add("2 только трансформ");
                }
                else if (QuestType == 53 || QuestType == 54)
                {
                    lNameObject.Text = "Тип NPC:";
                    targetComboBox.Enabled = false;
                    ltargetResult.Text = "Уровень NPC";
                    ltargetResult.Enabled = false;
                    lTargetAttr1.Enabled = false;

                    labelTargetAttr.Text = "Квестовый ID:";
                    labelTargetAttr.Enabled = true;
                    cbState.Text = "Засчитывать всем";
                    cbState.Enabled = true;

                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Enabled = true;
                }
                else if (QuestType == CQuestConstants.TYPE_PVP_MAP_KILL)
                {
                    cbPVPMode.Visible = true;
                    
                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.AddRange(CPVPConstans.getAllDescriptions().ToArray());
                    cbPVPMode.SelectedIndex = 0;

                }
                else if (QuestType == CQuestConstants.TYPE_PVP_MAP_CAPTURE_FLAG)
                {
                    cbPVPMode.Visible = true;
                    cbPVPtarget.Visible = true;

                    cbPVPtarget.Items.Clear();
                    cbPVPtarget.Items.AddRange(QuestPVPConstance.captureTheFlagTypes.getListNames().ToArray());
                    cbPVPtarget.SelectedIndex = 0;

                    cbPVPtarget2.Items.Clear();
                    cbPVPtarget2.Items.AddRange(QuestPVPConstance.bestTypes.getListNames().ToArray());
                    cbPVPtarget2.SelectedIndex = 0;

                    cbPVPtarget3.Items.Clear();
                    cbPVPtarget3.Items.AddRange(QuestPVPConstance.targetCountTypes.getListNames().ToArray());
                    cbPVPtarget3.SelectedIndex = 0;

                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.AddRange(CPVPConstans.getAllDescriptions().ToArray());
                    cbPVPMode.SelectedIndex = 2;
                }
                else if (QuestType == CQuestConstants.TYPE_PVP_MAP_SCORE)
                {
                    cbPVPMode.Visible = true;
                    cbPVPtarget.Visible = true;
                    cbPVPtarget2.Visible = true;
                    lbPVPtarget.Visible = true;

                    cbPVPtarget.Items.Clear();
                    cbPVPtarget.Items.AddRange(QuestPVPConstance.targetCountTypes.getListNames().ToArray());
                    cbPVPtarget.SelectedIndex = 0;

                    cbPVPtarget2.Items.Clear();
                    cbPVPtarget2.Items.AddRange(QuestPVPConstance.bestTypes.getListNames().ToArray());
                    cbPVPtarget2.SelectedIndex = 0;

                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.AddRange(CPVPConstans.getAllDescriptions().ToArray());
                    cbPVPMode.SelectedIndex = 0;
                }
                else if (QuestType == CQuestConstants.TYPE_B2C_KILL)
                {
                    cbPVPMode.Visible = true;
                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.Add("2 Защита");
                    cbPVPMode.Items.Add("3 Атака");
                    
                    cbPVPMode.SelectedIndex = 0;
                    
                    cbPVPtarget.Visible = true;
                    cbPVPtarget.Items.Clear();
                    cbPVPtarget.Items.Add("2 Игрок");
                    cbPVPtarget.Items.Add("3 Охранник");
                    cbPVPtarget.SelectedIndex = 0;
                    nupPVPCount.Visible = true;

                    cbPVPAdditional.Visible = true;
                    cbPVPAdditional.Items.Clear();
                    cbPVPAdditional.Items.Add("Нет");
                    cbPVPAdditional.Items.Add("Да");
                    cbPVPAdditional.SelectedIndex = 0;

                    labelPVPAdditional.Visible = true;
                    labelPVPAdditional.Text = "Добить";
                }
                else if ((QuestType == CQuestConstants.TYPE_B2C_FLAG))
                {
                    cbPVPMode.Visible = false;
                    cbPVPtarget.Visible = false;
                    cbPVPtarget2.Visible = false;
                    lbPVPtarget.Visible = false;
                    cbPVPtarget3.Visible = false;

                    cbPVPMode.Visible = true;
                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.Add("2 Защита");
                    cbPVPMode.Items.Add("3 Атака");
                    cbPVPMode.SelectedIndex = 0;
                    nupPVPCount.Visible = true;
                }
                else if ((QuestType == CQuestConstants.TYPE_B2C_REVIVE))
                {
                    cbPVPMode.Visible = false;
                    cbPVPtarget.Visible = false;
                    cbPVPtarget2.Visible = false;
                    lbPVPtarget.Visible = false;
                    cbPVPtarget3.Visible = false;
                    cbPVPOnlyAlly.Visible = true;
                    cbPVPOnlyEnemy.Visible = true;
                    cbPVPMode.Visible = true;
                    cbPVPMode.Items.Clear();
                    cbPVPMode.Items.Add("2 Защита");
                    cbPVPMode.Items.Add("3 Атака");
                    cbPVPMode.SelectedIndex = 0;
                    nupPVPCount.Visible = true;
                }

            }
            else
            {
                targetComboBox.Enabled = false;
                lNameObject.Enabled = false;
                lQuantity.Enabled = false;
                quantityUpDown.Enabled = false;
            }

            if (iState != ADD_NEW)
            {
                isClanCheckBox.Checked = quest.Target.IsClan;
                //IsGroupCheckBox.Checked = quest.Target.IsGroup;
            }

            if (iState == ADD_SUB || iState == EDIT_SUB)
            {
                isClanCheckBox.Enabled = false;
                takenPeriodTextBox.Enabled = false;
                lDaily.Enabled = false; 
                lH.Enabled = false;
                //IsGroupCheckBox.Enabled = false;
            }
            if (targetComboBox.Items.Count > 0)
                targetComboBox.DropDownWidth = this.DropDownWidth(targetComboBox) + 10;
        }
        //! Заполняет данные о целях квеста (о боги какой говнокод)
        void fillTarget()
        {
            if ((quest.Target.QuestType == CQuestConstants.TYPE_FARM) || (quest.Target.QuestType == CQuestConstants.TYPE_FARM_AUTO) || (quest.Target.QuestType == CQuestConstants.TYPE_QITEM_USE))
            {
                targetComboBox.SelectedItem = parent.itemConst.getItemName(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
                targetAttributeComboBox.SelectedIndex = quest.Target.ObjectAttr;
                if ((quest.Target.QuestType == CQuestConstants.TYPE_FARM || quest.Target.QuestType == CQuestConstants.TYPE_FARM_AUTO) && quest.Target.additional.Any())
                    targetAttributeComboBox2.SelectedIndex = 0;
                if (quest.Target.QuestType == CQuestConstants.TYPE_QITEM_USE)
                    targetAttributeComboBox2.SelectedItem = quest.Target.AreaName;
                if (quest.Target.usePercent)
                {
                    cbState.Checked = true;
                    udState.Value = Convert.ToDecimal(quest.Target.percent * 100);
                }
                else cbState.Checked = false;

            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_CRAFT_ITEM) || (quest.Target.QuestType == CQuestConstants.TYPE_CRAFT_ITEM_AUTO))
            {
                targetComboBox.SelectedItem = parent.receptConst.getItemName(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_COOK_ITEM) || (quest.Target.QuestType == CQuestConstants.TYPE_COOK_ITEM_AUTO))
            {
                List<int> result = new List<int>();
                ComboBox[] list = { resultComboBox, targetComboBox, targetAttributeComboBox };
                for (int i = 0; i < quest.Target.AObjectAttrs.Count; i++)
                {
                    int itemType = quest.Target.AObjectAttrs[i];
                    list[i].SelectedItem = parent.itemConst.getItemName(itemType);
                }
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY || quest.Target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY_AUTO)
            {
                targetComboBox.SelectedItem = parent.itemCategories.getNameOnID(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_TALK)
            {
                targetComboBox.SelectedItem = parent.npcConst.getDescriptionOnKey(quest.Target.ObjectName).getName();
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_DUNGEON_EVENT)
            {
                resultComboBox.SelectedItem = parent.dungeonConst.getNameByID(quest.Target.ObjectType);
                targetComboBox.SelectedItem = parent.dungeonConst.getBossName(quest.Target.ObjectType, quest.Target.ObjectAttr);
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_DUNGEON_BOX_COUNTER)
            {
                resultComboBox.SelectedItem = parent.dungeonConst.getNameByID(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_ANOMALY) || (quest.Target.QuestType == CQuestConstants.TYPE_ANOMALY_AUTO))
            {
                resultComboBox.SelectedItem  = AnomalyTypes.getNameByID(quest.Target.ObjectType.ToString());
                targetComboBox.SelectedItem = parent.itemConst.getItemName(quest.Target.ObjectAttr);
                targetAttributeComboBox2.SelectedItem = quest.Target.AreaName;

            }
            if (quest.Target.QuestType == CQuestConstants.TYPE_ENTITY_SEEN || quest.Target.QuestType == CQuestConstants.TYPE_ENTITY_SEEN_AUTO)
            {
                resultComboBox.SelectedItem = quest.Target.ObjectName;
                switch(quest.Target.ObjectName)
                {
                    case "QuestEntity":
                        targetAttributeComboBox.Text = quest.Target.str_param;
                        break;
                    case "Creature":
                        targetComboBox.SelectedItem = parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getName();
                        targetAttributeComboBox.SelectedItem = quest.Target.str_param;
                        break;
                    case "TimeEntity":
                        targetComboBox.SelectedItem = quest.Target.AreaName;
                        targetAttributeComboBox.Text = quest.Target.str_param;
                        break;
                }
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_KILLNPC) || (quest.Target.QuestType == CQuestConstants.TYPE_KILLNPC_WITH_ONTEST))
            {
                targetComboBox.SelectedItem = parent.npcConst.getDescriptionOnKey(quest.Target.ObjectName).getName();
                quantityUpDown.Value = quest.Target.NumOfObjects;
                resultComboBox.SelectedItem = parent.spacesConst.getSpaceByID(quest.Target.ObjectType);
                if (quest.Target.usePercent)
                {
                    cbState.Checked = true;
                    udState.Value = Convert.ToDecimal(quest.Target.percent * 100);
                }
                else cbState.Checked = false;

            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST) || (quest.Target.QuestType == CQuestConstants.TYPE_KILLMOBS))
            {
                targetComboBox.SelectedItem = parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getName();
                if (!quest.Target.str_param2.Equals(""))
                {
                    targetAttributeComboBox.Items.Clear();
                    targetAttributeComboBox.Text = quest.Target.str_param2;
                    // иначе чёрт знает как засунуть текст, который не в Items
                    foreach (CZoneDescription description in parent.zoneMobConst.getAllZones().Values)
                        targetAttributeComboBox.Items.Add(description.getName());

                    string area = parent.zoneMobConst.getDescriptionOnKey(quest.Target.str_param2).getName();
                    if (area.Any())
                        targetAttributeComboBox.SelectedItem = area;
                }
                if (!quest.Target.AreaName.Equals(""))
                {
                    targetAttributeComboBox2.Items.Clear();
                    targetAttributeComboBox2.Text = quest.Target.AreaName;
                    // иначе чёрт знает как засунуть текст, который не в Items
                    foreach (CZoneDescription description in parent.zoneConst.getAllZones().Values)
                        targetAttributeComboBox2.Items.Add(description.getName());
                    string area = quest.Target.AreaName;
                    //string area = parent.zoneConst.getDescriptionOnKey(quest.Target.AreaName).getName();
                    if (area.Any())
                        targetAttributeComboBox2.Text = area;
                }

                //if (quest.Target.ObjectAttr < 0)
                //    targetAttributeComboBox2.SelectedIndex = 0;
                //else
                //    targetAttributeComboBox2.SelectedIndex = parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getIndexOnLevel(quest.Target.ObjectAttr);
                if (quest.Target.ObjectAttr != 0)
                    quest.Target.str_param = quest.Target.ObjectAttr.ToString();
                if (quest.Target.str_param.Any())
                    resultComboBox.SelectedItem = quest.Target.str_param.ToString();
                else
                {
                    resultComboBox.SelectedItem = quest.Target.ObjectName;
                }
                if (quest.Target.usePercent)
                {
                    cbState.Checked = true;
                    udState.Value = Convert.ToDecimal(quest.Target.percent * 100);
                }
                else cbState.Checked = false;

                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultComboBox.Text = quest.Target.ObjectName;
                }
                else
                {
                    quantityUpDown.Value = quest.Target.NumOfObjects;
                }

            }

            else if ((quest.Target.QuestType == CQuestConstants.TYPE_AREA_DISCOVER) ||
                    (quest.Target.QuestType == CQuestConstants.TYPE_AREA_LEAVE) ||
                    (quest.Target.QuestType == CQuestConstants.TYPE_IN_AREA))
            {

                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultComboBox.Text = quest.Target.AreaName;
                }
                else
                {
                    targetComboBox.SelectedItem = parent.zoneConst.getDescriptionOnKey(quest.Target.AreaName).getName();
                }
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_MONEYBACK)
            {
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
            {

                if (quest.Target.ObjectName.Contains(','))
                {
                    dynamicCheckBox.Checked = true;
                    resultComboBox.Text = quest.Target.ObjectName;
                }
                else
                {
                    targetComboBox.SelectedItem = parent.triggerConst.getDescriptionOnId(quest.Target.ObjectType);
                }
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_TIMER)
            {
                resultComboBox.Text = resultComboBox.Text.Replace('.', ',');
                resultComboBox.Text = quest.Target.Time.ToString();
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_QUEST_COUNTER)
            {
                resultComboBox.Text = quest.Target.ObjectName;
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_LEAVE_MAP) || (QuestType == CQuestConstants.TYPE_ENTER_MAP))
            {
                targetComboBox.SelectedItem = parent.spacesConst.getSpaceByID(quest.Target.ObjectType);
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_ITEM_EQIP)
            {
                //System.Console.WriteLine("getQuest: type" + quest.Target.QuestType.ToString());
                targetComboBox.SelectedItem = parent.itemConst.getItemName(quest.Target.ObjectType);
            }
            else if (quest.Target.QuestType == 206)
            {
                targetComboBox.SelectedItem = parent.gui.getDescriptionOnID(quest.Target.ObjectType);
                targetAttributeComboBox.SelectedIndex = quest.Target.ObjectAttr;
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_GIVE_EFFECT) || (quest.Target.QuestType == CQuestConstants.TYPE_HAVE_EFFECT))
            {
                targetComboBox.SelectedItem = parent.effects.getDescriptionOnID(quest.Target.ObjectType);
                quantityUpDown.Value = quest.Target.NumOfObjects;
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_KILL)
            {
                targetComboBox.SelectedIndex = quest.Target.ObjectType;
                targetAttributeComboBox.SelectedIndex = quest.Target.ObjectAttr;
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY || quest.Target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY_AUTO)
            {

            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_CREATE_NPC)
            {
                fillCreateNPCPanel();
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_CREATE_MOB)
            {
                fillCreateMobPanel();
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_PVP_MAP_KILL)
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;
                cbPVPMode.SelectedItem = CPVPConstans.getPVPModeNameByID(Convert.ToInt32(quest.Target.additional));
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_PVP_MAP_CAPTURE_FLAG)
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;

                cbPVPtarget.SelectedItem = QuestPVPConstance.captureTheFlagTypes.getNameByID(quest.Target.ObjectType);
                cbPVPtarget2.SelectedItem = QuestPVPConstance.bestTypes.getNameByID(quest.Target.ObjectAttr);
                cbPVPtarget3.SelectedItem = QuestPVPConstance.targetCountTypes.getNameByID(Convert.ToInt32(quest.Target.ObjectName));
                cbPVPMode.SelectedItem = CPVPConstans.getPVPModeNameByID(Convert.ToInt32(quest.Target.additional));
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_PVP_MAP_SCORE)
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;
                cbPVPtarget.SelectedItem = QuestPVPConstance.targetCountTypes.getNameByID(quest.Target.ObjectType);
                cbPVPtarget2.SelectedItem = QuestPVPConstance.bestTypes.getNameByID(quest.Target.ObjectAttr);
                cbPVPMode.SelectedItem = CPVPConstans.getPVPModeNameByID(Convert.ToInt32(quest.Target.additional));
            }
            else if (quest.Target.QuestType == CQuestConstants.TYPE_B2C_KILL)
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;
                cbPVPMode.SelectedIndex = Math.Max(0, Convert.ToInt32(quest.Target.additional) - 2);
                cbPVPtarget.SelectedIndex = Math.Max(0, quest.Target.ObjectType - 2);
                cbPVPAdditional.SelectedIndex = quest.Target.ObjectAttr;
            }
            else if ((quest.Target.QuestType == CQuestConstants.TYPE_B2C_FLAG))
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;
                cbPVPMode.SelectedIndex = Math.Max(0, Convert.ToInt32(quest.Target.additional) - 2); ;
            }
            else if ((QuestType == CQuestConstants.TYPE_B2C_REVIVE))
            {
                nupPVPCount.Value = quest.Target.NumOfObjects;
                cbPVPMode.SelectedIndex = Math.Max(0, Convert.ToInt32(quest.Target.additional) - 2);
                cbPVPOnlyAlly.Checked = Convert.ToBoolean(quest.Target.ObjectAttr & 1);
                cbPVPOnlyEnemy.Checked = Convert.ToBoolean(quest.Target.ObjectAttr & 1 << 1); 
            }
            else if ((quest.Target.QuestType == 53) || (quest.Target.QuestType == 54))
            {
                if (!quest.Target.AreaName.Equals(""))
                    targetAttributeComboBox.Text = quest.Target.AreaName;

                if (quest.Target.ObjectAttr != 0)
                {
                    cbState.Checked = true;
                }
                else cbState.Checked = false;
            }
        }

        CQuestRules.NPC getCreateNPC()
        {
            CQuestRules.NPC npc = new CQuestRules.NPC();

            npc.name = tbNpcName.Text;
            npc.displayName = tbDisplayName.Text;
            int.TryParse(tbReputation.Text, out npc.reputation);
            npc.way = tbWay.Text;
            npc.animation = tbNPCAnim.Text;

            npc.fraction = cbFractionID.SelectedItem == null ? 0 : parent.fractions2.getFractionIDByDescr(cbFractionID.SelectedItem.ToString());
            npc.npcWeapon = cbWeapon.SelectedIndex == -1 ? 0 : cbWeapon.SelectedIndex;
            npc.npcWeaponPrimary = cbPrimaryWeapon.SelectedItem == null ? 0 : parent.npcItems.primaryWeapons.getIDByName(cbPrimaryWeapon.SelectedItem.ToString());

            npc.npcWeaponPrimary2 = cbPrimary2Weapon.SelectedItem == null ? 0 : parent.npcItems.primaryWeapons.getIDByName(cbPrimary2Weapon.SelectedItem.ToString());

            npc.npcWeaponSecondary = cbSecondaryWeapon.SelectedItem == null ? 0 : parent.npcItems.secondaryyWeapons.getIDByName(cbSecondaryWeapon.SelectedItem.ToString());

            npc.NPCEquippedArmor = tbCloth.Text;
            npc.uniq = cbUniqNPC.Checked;
            npc.invulnerable = cbNPCInvul.Checked;
            npc.mobNoAggr = cbNPCMobNoAggr.Checked;
            npc.ignoreWAR = Convert.ToInt16(cbNPCIgnoreWAR.Checked);

            npc.shootRange = Convert.ToSingle(nupNPCShootRange.Value);
            npc.shootRangeOnCreature = Convert.ToSingle(nupNPCShootRangeOnCreature.Value);
            npc.walkSpeed = Convert.ToSingle(nupNPCSpeed.Value);
            return npc;
        }

        CQuestRules.Mob getCreateMob()
        {
            CQuestRules.Mob mob = new CQuestRules.Mob();

            if (cbMobType.SelectedItem != null)
                mob.mob_type = parent.mobConst.getTypeOnDescription(cbMobType.SelectedItem.ToString());

            mob.level = cbMobLevel.Text;
            if (cbScenaryType.Text.Any())
                int.TryParse(cbScenaryType.Text[0].ToString(), out mob.scen_type);
            mob.way = tbMobWay.Text;
            mob.count = Convert.ToInt32(nupMobCount.Value);
            mob.questID = tbUniqMobID.Text;
            mob.uniq = cbMobUniq.Checked;
            mob.invulnerable = cbMobInvul.Checked;

            return mob;
        }

        void fillCreateMobPanel()
        {
            if (!quest.QuestRules.mobs.Any())
                return;
            cbMobType.SelectedItem = parent.mobConst.getDescriptionOnType(quest.QuestRules.mobs.mob_type).getName();
            cbMobLevel.SelectedItem = quest.QuestRules.mobs.level.ToString();
            cbScenaryType.SelectedItem = scenaryTypes[quest.QuestRules.mobs.scen_type];
            tbMobWay.Text = quest.QuestRules.mobs.way;
            nupMobCount.Value = quest.QuestRules.mobs.count;
            tbUniqMobID.Text = quest.QuestRules.mobs.questID;
            cbMobUniq.Checked = quest.QuestRules.mobs.uniq;
            cbMobInvul.Checked = quest.QuestRules.mobs.invulnerable;
        }

        void fillCreateNPCPanel()
        {
            tbNpcName.Text = quest.QuestRules.npc.name;
            tbDisplayName.Text = quest.QuestRules.npc.displayName;
            tbReputation.Text = quest.QuestRules.npc.reputation.ToString();

            tbWay.Text = quest.QuestRules.npc.way;
            tbNPCAnim.Text = quest.QuestRules.npc.animation;
            cbFractionID.SelectedItem = parent.fractions2.getFractionDesctByID(quest.QuestRules.npc.fraction);
            
            cbWeapon.SelectedIndex = quest.QuestRules.npc.npcWeapon;
            cbPrimaryWeapon.SelectedItem = parent.npcItems.primaryWeapons.getNameByID(quest.QuestRules.npc.npcWeaponPrimary);
            cbPrimary2Weapon.SelectedItem = parent.npcItems.primaryWeapons.getNameByID(quest.QuestRules.npc.npcWeaponPrimary2);
            cbSecondaryWeapon.SelectedItem = parent.npcItems.secondaryyWeapons.getNameByID(quest.QuestRules.npc.npcWeaponSecondary);

            tbCloth.Text = quest.QuestRules.npc.NPCEquippedArmor;
            cbUniqNPC.Checked = quest.QuestRules.npc.uniq;
            cbNPCInvul.Checked = quest.QuestRules.npc.invulnerable;
            cbNPCMobNoAggr.Checked = quest.QuestRules.npc.mobNoAggr;
            cbNPCIgnoreWAR.Checked = quest.QuestRules.npc.ignoreWAR > 0;
            nupNPCSpeed.Value = Convert.ToDecimal(quest.QuestRules.npc.walkSpeed);
            nupNPCShootRange.Value = Convert.ToDecimal(quest.QuestRules.npc.shootRange);
            nupNPCShootRangeOnCreature.Value = Convert.ToDecimal(quest.QuestRules.npc.shootRangeOnCreature);
        }
        void initCreateMobPanel()
        {
            cbMobType.Items.Clear();
            foreach (CMobDescription mob in parent.mobConst.getAllDescriptions().Values)
                cbMobType.Items.Add(mob.getName());

            cbMobLevel.Items.Clear();
            cbScenaryType.Items.Clear();
            cbScenaryType.Items.Add("2 ATTACK_ON_PLAYER");
            cbScenaryType.Items.Add("4 GO_TO_WAYPOINT");

        }

        void initCreateNPCPanel()
        {
            cbFractionID.Items.Clear();
            cbFractionID.Items.AddRange(parent.fractions2.getListOfFractions().Values.ToArray());

            cbWeapon.Items.Clear();
            cbWeapon.Items.Add("пусто"); cbWeapon.Items.Add("Перв. оруж"); cbWeapon.Items.Add("Перв2. оруж"); cbWeapon.Items.Add("Втор. оруж");

            cbPrimaryWeapon.Items.Clear();
            cbPrimaryWeapon.Items.AddRange(parent.npcItems.primaryWeapons.getNames().ToArray());

            cbPrimary2Weapon.Items.Clear();
            cbPrimary2Weapon.Items.AddRange(parent.npcItems.primaryWeapons.getNames().ToArray());

            cbSecondaryWeapon.Items.Clear();
            cbSecondaryWeapon.Items.AddRange(parent.npcItems.secondaryyWeapons.getNames().ToArray());


            tbCloth.Text = "";
        }

        void fillConditionsTab()
        {
            cbConditionWeapon.Items.Clear();
            cbConditionWeapon.Items.AddRange(parent.npcItems.secondaryyWeapons.getNames().ToArray());
            cbConditionWeapon.Items.AddRange(parent.npcItems.primaryWeapons.getNames().ToArray());
            cbConditionWeapon.SelectedItem = "нет";

            cbConditionPVPTeam.Items.Clear();
            cbConditionPVPTeam.Items.AddRange(new string[] { "Нет", "Красные", "Синие", "Любая"});
            cbConditionPVPTeam.SelectedIndex = 0;

            cbConditionPVPTeamWin.Items.Clear();
            cbConditionPVPTeamWin.Items.Add("нет");
            cbConditionPVPTeamWin.Items.AddRange(QuestPVPConstance.bestTypes.getListNames());
            cbConditionPVPTeamWin.SelectedIndex = 0;
        }

        void fillQuestRulesForm()
        {
            nBaseToCapturePercent.Value = Convert.ToDecimal(quest.QuestRules.basePercent * 100);
            cbTakeItems.Checked = !quest.QuestRules.dontTakeItems;
            bool a = parent.questConst.isOnTest(quest.Target.QuestType);
            cbMarkOnTest.Visible = a;
            cbMarkOnTest.Checked = a && quest.QuestRules.dontHideMarks;
            foreach (var i in parent.spacesConst.getSpacesNames())
                ((DataGridViewComboBoxColumn)dataGridMapMark.Columns[2]).Items.Add(i);

            foreach (var mark in quest.QuestRules.mapMarks)
            {
                string space = parent.spacesConst.getLocalName(mark.space);
                space = parent.spacesConst.getSpaceID(space).ToString() + " " + space;
                object[] row = { mark.coords, mark.radius, space };
                dataGridMapMark.Rows.Add(row);
            }
        }
        //! Заполняет раздел Правила на форме - уровень игрока, сценарий... (???)
        void fillQuestRules()
        {
            foreach (int item in quest.QuestRules.Scenarios)
            {
                if (scenariosTextBox.Text == "")
                    scenariosTextBox.Text += item.ToString();
                else
                    scenariosTextBox.Text += ("," + item.ToString());
            }
            foreach (int item in quest.QuestRules.MassQuests)
            {
                if (massQuestsTextBox.Text == "")
                    massQuestsTextBox.Text += item.ToString();
                else
                    massQuestsTextBox.Text += ("," + item.ToString());
            }
            checkQuestRulesIndicates();
        }
        //! Создает индикацию зеленым светом в графе Награда, если выдаются предметы, эффекты, или статус
        public void checkRewardIndicates()
        {
            if (editQuestReward.items.Any())
                bRewardItem.Image = Properties.Resources.but_indicate;
            else
                bRewardItem.Image = null;

            if (editQuestPenalty.items.Any())
                bPenaltyItem.Image = Properties.Resources.but_indicate;
            else
                bPenaltyItem.Image = null;

            if (editQuestReward.ReputationNotEmpty() || editQuestReward.ReputationNotEmpty(true))
                bRewardReputation.Image = Properties.Resources.but_indicate;
            else
                bRewardReputation.Image = null;

            if (editQuestReward.Reputation2NotEmpty())
                bRewardReputation2.Image = Properties.Resources.but_indicate;
            else
                bRewardReputation2.Image = null;

            if (editQuestPenalty.ReputationNotEmpty() || editQuestReward.ReputationNotEmpty(true))
                bPenaltyReputation.Image = Properties.Resources.but_indicate;
            else
                bPenaltyReputation.Image = null;

            if (editQuestPenalty.Reputation2NotEmpty())
                bPenaltyReputation2.Image = Properties.Resources.but_indicate;
            else
                bPenaltyReputation2.Image = null;

            if (editQuestReward.Effects.Any())
                bRewardEffects.Image = Properties.Resources.but_indicate;
            else
                bRewardEffects.Image = null;

            if (editQuestReward.blackBoxes.Any())
                bRewardBlackBox.Image = Properties.Resources.but_indicate;
            else
                bRewardBlackBox.Image = null;

            if (editQuestPenalty.Effects.Any())
                bPenaltyEffects.Image = Properties.Resources.but_indicate;
            else
                bPenaltyEffects.Image = null;

            if (editQuestReward.ChangeQuests.Any())
                bRewardQuests.Image = Properties.Resources.but_indicate;
            else
                bRewardQuests.Image = null;

            if (editQuestPenalty.ChangeQuests.Any())
                bPenaltyQuests.Image = Properties.Resources.but_indicate;
            else
                bPenaltyQuests.Image = null;

            


        }
        //! Создает индиикацию кнопки Предметы в графе Правила квеста
        public void checkQuestRulesIndicates()
        {
            if (editQuestRules.items.Any())
                bItemQuestRules.Image = Properties.Resources.but_indicate;

            if (editQuestRules.space != 0)
                btnSpace.Image = Properties.Resources.but_indicate;
            else
                btnSpace.Image = null;
        }
        //! Заполняет данные о награде - опыт, деньги, карму
        void fillReward()
        {
            checkRewardIndicates();

            foreach (var i in parent.tpConst.getKeys())
                cbRewardTeleport.Items.Add(i);
            foreach (var i in parent.fractions2.getListOfFractions())
                cbRewardOT.Items.Add(i.Value);
            tbRewardOTvalue.Text = quest.Reward.OTvalue.ToString();
            clanPointsValue.Value = quest.Reward.clanPoints;
            cbRewardOT.SelectedItem = parent.fractions2.getFractionDesctByID(quest.Reward.OTfraction);
            tExperience.Text = quest.Reward.Experience.ToString();
            tbPenaltyExperience.Text = quest.QuestPenalty.Experience.ToString();
            if (quest.Reward.teleportTo.Any())
                cbRewardTeleport.SelectedItem = quest.Reward.teleportTo;
            if (quest.Reward.dungeonTarget.Any())
            {
                tbDungTarget.Text = quest.Reward.dungeonTarget;
            }
            cbTargetMode.SelectedIndex = quest.Reward.dungeonTargetMode;
            cbRewardWindow.Checked = quest.Reward.RewardWindow;
            creditsTextBox.Text = quest.Reward.Credits.ToString();
            tbPenaltyCredits.Text = quest.QuestPenalty.Credits.ToString();
            textBoxKarmaPK.Text = quest.Reward.KarmaPK.ToString();
            tbPenaltyKarmaPK.Text = quest.QuestPenalty.KarmaPK.ToString();
            foreach (int knowlege in quest.Reward.GetKnowleges)
                Global.addItemToTextBox(knowlege.ToString(), tbGetKnowleges);
            foreach (int knowlege in quest.QuestPenalty.GetKnowleges)
                Global.addItemToTextBox(knowlege.ToString(), tbGetKnowlegesPenalty);

        }
        //! Собирает данные с формы, и возвращает экземпляр CQuest с этими данными
        public CQuest getQuest()
        {
            Action<List<string>, string> loggingPaths = (List<string> x, string a) =>
            {
                foreach (string i in x) Console.WriteLine(i, a);
            };

            loggingPaths(new List<string>(), "");


            CQuestAdditional additional = new CQuestAdditional();
            CQuestInformation information = new CQuestInformation();
            CQuestPrecondition precondition = new CQuestPrecondition();
            CQuestReward reward = new CQuestReward();
            CQuestRules rules = new CQuestRules();
            CQuestTarget target = new CQuestTarget();
            CQuestReward penalty = new CQuestReward();
            CQuestAdditionalConditions conditions = new CQuestAdditionalConditions();
           
            information.Description = descriptionTextBox.Text;
            information.DescriptionClosed = descriptionClosedTextBox.Text;
            information.DescriptionOnTest = descriptionOnTestTextBox.Text;

            information.Title = titleTextBox.Text;
            additional.Holder = parent.GetCurrentNPC();
            information.messages = new List<QuestMessages>();
            foreach (DataGridViewRow row in messageGridView.Rows)
            {
                QuestMessages item = new QuestMessages();
                item.message = row.Cells["message"].FormattedValue.ToString();
                if (!item.message.Equals(""))
                {
                    item.state1 = QuestMessageType.getType(row.Cells["state1"].Value.ToString());
                    item.state2 = QuestMessageType.getType(row.Cells["state2"].Value.ToString());
                    item.isHighMessage = Convert.ToBoolean(row.Cells["show"].Value);
                    information.messages.Add(item);
                }
             }
                /*
                information.onWin = onWonTextBox.Text;
                information.onGet = onGotTextBox.Text;
                information.onFailed = onFailedTextBox.Text;
                information.onOpen = onOpenTextBox.Text;
                information.onTest = onTestTextBox.Text;
                */
                rules.basePercent = Convert.ToSingle(nBaseToCapturePercent.Value) / 100;
            rules.dontTakeItems = !cbTakeItems.Checked;
            rules.dontHideMarks = cbMarkOnTest.Checked;
            target.QuestType = parent.questConst.getQuestTypeOnDescription(eventComboBox.SelectedItem.ToString());
            if (loseRButton.Checked)
                target.onFin = 0;
            else
                target.onFin = 1;

            precondition.isGroup = IsGroupCheckBox.Enabled && IsGroupCheckBox.Checked;
            if ((target.QuestType == CQuestConstants.TYPE_FARM) || (target.QuestType == CQuestConstants.TYPE_FARM_AUTO) || (target.QuestType == CQuestConstants.TYPE_QITEM_USE))
            {
                try
                {
                    target.ObjectType = parent.itemConst.getIDOnName(targetComboBox.SelectedItem.ToString());
                }
                catch
                {
                    MessageBox.Show("Не удалось предмет найти", "Ошибка");
                    return null;
                }
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
                
                if (target.NumOfObjects < 1)
                {
                    MessageBox.Show("Цель->Количество - некорректное значение", "Ошибка");
                    return null;
                }
                target.ObjectAttr = targetAttributeComboBox.SelectedIndex;
                target.additional = (targetAttributeComboBox2.SelectedIndex == -1) ? "" : "1";
                if (cbState.Checked && cbState.Enabled)
                {
                    target.usePercent = true;
                    target.percent = Convert.ToSingle(udState.Value) / 100;
                }
                else target.usePercent = false;

                if (target.QuestType == CQuestConstants.TYPE_QITEM_USE && targetAttributeComboBox2.SelectedItem != null)
                {
                    target.AreaName = targetAttributeComboBox2.SelectedItem.ToString();
                }
            }
            else if ((target.QuestType == CQuestConstants.TYPE_CRAFT_ITEM) || (target.QuestType == CQuestConstants.TYPE_CRAFT_ITEM_AUTO))
            {
                target.ObjectType = parent.receptConst.getIDOnName(targetComboBox.SelectedItem.ToString());
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
            }
            else if ((target.QuestType == CQuestConstants.TYPE_COOK_ITEM) || (target.QuestType == CQuestConstants.TYPE_COOK_ITEM_AUTO))
            {
                List<int> result = new List<int>();
                ComboBox[] list = { resultComboBox, targetComboBox, targetAttributeComboBox };
                foreach(var i in list)
                {
                    if (i.SelectedItem == null) continue;
                    int tmp = parent.itemConst.getIDOnName(i.SelectedItem.ToString());
                    if (tmp > 0)
                        result.Add(tmp);
                }
                editTarget.AObjectAttrs = result;
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());

            }
            else if (target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY || target.QuestType == CQuestConstants.TYPE_ITEM_CATEGORY_AUTO)
            {
                target.ObjectType = parent.itemCategories.getID(targetComboBox.SelectedItem.ToString());
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
            }
            else if (target.QuestType == CQuestConstants.TYPE_TALK)
            {
                string key = parent.npcConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
                if (key.Equals(""))
                    target.ObjectName = targetComboBox.SelectedItem.ToString();
                else
                    target.ObjectName = key;
            }
            else if ((target.QuestType == CQuestConstants.TYPE_ANOMALY) || (target.QuestType == CQuestConstants.TYPE_ANOMALY_AUTO))
            {
                string key = AnomalyTypes.getIDByName(resultComboBox.SelectedItem.ToString());
                target.ObjectType = Convert.ToInt32(key);
                if (targetComboBox.SelectedItem == null || !targetComboBox.SelectedItem.ToString().Any())
                {
                    DialogResult dr = MessageBox.Show("Не выбран артефакт", "Внимание");
                    //if (dr != DialogResult.Yes)
                        return null;
                   
                }
                target.ObjectAttr = parent.itemConst.getIDOnName(targetComboBox.SelectedItem.ToString());
                if (targetAttributeComboBox2.SelectedItem != null) target.AreaName = targetAttributeComboBox2.SelectedItem.ToString();
            }
            else if (target.QuestType == CQuestConstants.TYPE_ENTITY_SEEN || target.QuestType == CQuestConstants.TYPE_ENTITY_SEEN_AUTO)
            {
                target.ObjectName = resultComboBox.SelectedItem.ToString();
                switch (target.ObjectName)
                {
                    case "QuestEntity":
                        target.str_param = targetAttributeComboBox.Text;
                        break;
                    case "Creature":
                        target.ObjectType = parent.mobConst.getTypeOnDescription(targetComboBox.SelectedItem.ToString());
                        target.str_param = targetAttributeComboBox.SelectedItem.ToString();
                        break;
                    case "TimeEntity":
                        target.AreaName = targetComboBox.SelectedItem.ToString();
                        target.str_param = targetAttributeComboBox.Text;
                        break;
                }
            }
            else if ((target.QuestType == CQuestConstants.TYPE_KILLNPC) || (target.QuestType == CQuestConstants.TYPE_KILLNPC_WITH_ONTEST))
            {
                string key = parent.npcConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
                if (key.Equals(""))
                    target.ObjectName = targetComboBox.SelectedItem.ToString();
                else
                    target.ObjectName = key;
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
                if (resultComboBox.SelectedItem == null || !resultComboBox.SelectedItem.ToString().Any())
                {
                    DialogResult dr = MessageBox.Show("Не выбран спейс.", "Внимание");
                    //if (dr != DialogResult.Yes)
                    return null;

                }
                target.ObjectType = int.Parse(resultComboBox.SelectedItem.ToString().Split(' ')[0]);
                if (cbState.Checked && cbState.Enabled)
                {
                    target.usePercent = true;
                    target.percent = Convert.ToSingle(udState.Value) / 100;
                }
                else target.usePercent = false;
            }
            else if (target.QuestType == CQuestConstants.TYPE_DUNGEON_EVENT)
            {
                target.ObjectType = parent.dungeonConst.getIDByName(resultComboBox.SelectedItem.ToString());
                target.ObjectAttr = parent.dungeonConst.getBossID(target.ObjectType, targetComboBox.SelectedItem.ToString());
            }
            else if (target.QuestType == CQuestConstants.TYPE_DUNGEON_BOX_COUNTER)
            {
                target.ObjectType = parent.dungeonConst.getIDByName(resultComboBox.SelectedItem.ToString());
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
            }
            else if ((target.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST) || (target.QuestType == CQuestConstants.TYPE_KILLMOBS))
            {
                target.ObjectType = parent.mobConst.getTypeOnDescription(targetComboBox.SelectedItem.ToString());
                
                if (dynamicCheckBox.Checked)
                    target.ObjectName = resultComboBox.Text;
                else
                {
                    if (resultComboBox.SelectedItem == null || !resultComboBox.SelectedItem.ToString().Any())
                    {
                        DialogResult dr = MessageBox.Show("Не выбран подтип моба. Вы уверены что хотите продолжить?", "Внимание", MessageBoxButtons.YesNo);
                        if (dr != DialogResult.Yes)
                            return null;
                        target.str_param = "";
                    }
                    else
                        target.str_param = resultComboBox.SelectedItem.ToString();

                    target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
                    if ((target.NumOfObjects > 32000) || (target.NumOfObjects < 1))
                    {
                        MessageBox.Show("Цель->Количество - некорректное значение(нужно 0 < x < 32000)", "Ошибка");
                        return null;
                    }
                }

                if (cbState.Checked && cbState.Enabled)
                {
                    target.usePercent = true;
                    target.percent = Convert.ToSingle(udState.Value) / 100;
                }
                else target.usePercent = false;

                if (targetAttributeComboBox.SelectedItem == null)
                    target.str_param2 = targetAttributeComboBox.Text;
                else if (targetAttributeComboBox.SelectedItem.ToString().Equals(""))
                    target.str_param2 = "";
                else
                {
                    string area = parent.zoneMobConst.getKeyOnDescription(targetAttributeComboBox.SelectedItem.ToString());
                    target.str_param2 = area.Any() ? area : targetAttributeComboBox.Text;
                }

                if (targetAttributeComboBox2.Text.Any())
                    target.AreaName = targetAttributeComboBox2.Text;
                else if (targetAttributeComboBox2.SelectedItem == null|| targetAttributeComboBox2.SelectedItem.ToString().Equals(""))
                    target.AreaName = "";
                else
                {
                    string area = parent.zoneConst.getKeyOnDescription(targetAttributeComboBox2.SelectedItem.ToString());
                    target.AreaName = area.Any() ? area : targetAttributeComboBox2.Text;
                }
            }
            else if ((target.QuestType == CQuestConstants.TYPE_AREA_DISCOVER) || 
                    (target.QuestType == CQuestConstants.TYPE_AREA_LEAVE) ||
                    (target.QuestType == CQuestConstants.TYPE_IN_AREA))
            {
                if (dynamicCheckBox.Checked)
                    target.AreaName = resultComboBox.Text;
                else
                {
                    string zone = parent.zoneConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
                    if (zone.Equals(""))
                        target.AreaName = targetComboBox.SelectedItem.ToString();
                    else
                        target.AreaName = zone;
                }
            }
            else if (target.QuestType == CQuestConstants.TYPE_MONEYBACK)
            {
                target.NumOfObjects = (int)quantityUpDown.Value;
                if (target.NumOfObjects < 1)
                {
                    MessageBox.Show("Цель->Количество - некорректное значение", "Ошибка");
                    return null;
                }
            }
            else if (target.QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
            {
                if (dynamicCheckBox.Checked)
                    target.ObjectName = resultComboBox.Text;
                else
                    target.ObjectType = parent.triggerConst.getIdOnKey(targetComboBox.SelectedItem.ToString());
            }

            else if (target.QuestType == CQuestConstants.TYPE_TIMER)
            {
                try
                {
                    target.Time = float.Parse(resultComboBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }
            else if (target.QuestType == CQuestConstants.TYPE_QUEST_COUNTER)
            {
                target.ObjectName = resultComboBox.Text;
                target.NumOfObjects = Convert.ToInt32(quantityUpDown.Value);
            }
            else if ((target.QuestType == CQuestConstants.TYPE_LEAVE_MAP) || (QuestType == CQuestConstants.TYPE_ENTER_MAP))
            {
                target.ObjectType = int.Parse(targetComboBox.SelectedItem.ToString().Split(' ')[0]);
            }
            else if ((target.QuestType == CQuestConstants.TYPE_ITEM_EQIP))
            {
                target.ObjectType = parent.itemConst.getIDOnName(targetComboBox.SelectedItem.ToString());
            }
            else if (target.QuestType == 201 ||
                     target.QuestType == 202 ||
                    target.QuestType == 203 ||
                    target.QuestType == 204)
            {
            }
            else if (target.QuestType == 206)
            {
                target.ObjectType = parent.gui.getIDOnDescription(targetComboBox.SelectedItem.ToString());
                target.ObjectAttr = targetAttributeComboBox.SelectedIndex;
            }
            else if ((target.QuestType == CQuestConstants.TYPE_GIVE_EFFECT) || (target.QuestType == CQuestConstants.TYPE_HAVE_EFFECT))
            {
                target.ObjectType = parent.effects.getIDOnDescription(targetComboBox.SelectedItem.ToString());
                target.NumOfObjects = int.Parse(quantityUpDown.Value.ToString());
                if (target.NumOfObjects < 1)
                {
                    MessageBox.Show("Цель->Количество - некорректное значение", "Ошибка");
                    return null;
                }
            }
            else if (target.QuestType == CQuestConstants.TYPE_KILL)
            {
                target.ObjectType = targetComboBox.SelectedIndex;
                target.ObjectAttr = targetAttributeComboBox.SelectedIndex;
            }
            else if (target.QuestType == CQuestConstants.TYPE_CREATE_NPC)
            {
                rules.npc = getCreateNPC();
            }
            else if (target.QuestType == CQuestConstants.TYPE_CREATE_MOB)
            {
                rules.mobs = getCreateMob();
            }
            else if (target.QuestType == CQuestConstants.TYPE_PVP_MAP_KILL)
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.additional = CPVPConstans.getPVPModeIDByName(cbPVPMode.SelectedItem.ToString()).ToString();
            }
            else if (target.QuestType == CQuestConstants.TYPE_PVP_MAP_CAPTURE_FLAG)
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.ObjectType = QuestPVPConstance.captureTheFlagTypes.getIDByName(cbPVPtarget.SelectedItem.ToString());
                target.ObjectAttr = QuestPVPConstance.bestTypes.getIDByName(cbPVPtarget2.SelectedItem.ToString());
                target.ObjectName = QuestPVPConstance.targetCountTypes.getIDByName(cbPVPtarget3.SelectedItem.ToString()).ToString();
                target.additional = CPVPConstans.getPVPModeIDByName(cbPVPMode.SelectedItem.ToString()).ToString();
            }
            else if (target.QuestType == CQuestConstants.TYPE_PVP_MAP_SCORE)
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.ObjectType = QuestPVPConstance.targetCountTypes.getIDByName(cbPVPtarget.SelectedItem.ToString());
                target.ObjectAttr = QuestPVPConstance.bestTypes.getIDByName(cbPVPtarget2.SelectedItem.ToString());
                target.additional = CPVPConstans.getPVPModeIDByName(cbPVPMode.SelectedItem.ToString()).ToString();
            }
            else if (target.QuestType == CQuestConstants.TYPE_B2C_KILL)
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.additional = (cbPVPMode.SelectedIndex + 2).ToString();
                target.ObjectType = cbPVPtarget.SelectedIndex + 2;
                target.ObjectAttr = cbPVPAdditional.SelectedIndex;
            }
            else if ((target.QuestType == CQuestConstants.TYPE_B2C_FLAG))
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.additional = (cbPVPMode.SelectedIndex + 2).ToString();
            }
            else if ((QuestType == CQuestConstants.TYPE_B2C_REVIVE))
            {
                target.NumOfObjects = Convert.ToInt32(nupPVPCount.Value);
                target.additional = (cbPVPMode.SelectedIndex + 2).ToString();
                int value = 0;
                if (cbPVPOnlyAlly.Checked)
                    value |= 1;
                if (cbPVPOnlyEnemy.Checked)
                    value |= 1 << 1;
                target.ObjectAttr = value;
            }
            else if ((target.QuestType == 53) || (target.QuestType == 54))
            {
                if (cbState.Checked && cbState.Enabled)
                {
                    target.ObjectAttr = 1;
                }
                else target.ObjectAttr = 0;
                target.AreaName = targetAttributeComboBox.Text;
            }

            if (iState == EDIT_SUB || iState == EDIT)
                additional = quest.Additional;

            //target.IsGroup = IsGroupCheckBox.Checked;
            target.IsClan = isClanCheckBox.Checked;

            if (iState == EDIT)
            {
                parent.setQuestISClan(quest.QuestID, target.IsClan);
                //parent.setQuestISGroup(quest.QuestID, target.IsGroup);
            }

            if (repeatComboBox.SelectedItem.ToString().Equals("Повторное взятие квеста после его закрытия."))
                precondition.Repeat = 1;

            if (takenPeriodTextBox.Enabled && !takenPeriodTextBox.Text.Equals(""))
                precondition.TakenPeriod = double.Parse(takenPeriodTextBox.Text, System.Globalization.CultureInfo.InvariantCulture);

            precondition.omniCounter = IsCounterCheckBox.Checked;
            foreach (string item in scenariosTextBox.Text.Split(','))
                if (!item.Equals(""))
                    rules.Scenarios.Add(int.Parse(item));
            foreach (string item in massQuestsTextBox.Text.Split(','))
                if (!item.Equals(""))
                    rules.MassQuests.Add(int.Parse(item));

            reward.Experience = ParseIntIfNotEmpty(tExperience.Text);
            if (cbRewardOT.SelectedItem != null)
            {
                reward.OTfraction = parent.fractions2.getFractionIDByDescr(cbRewardOT.SelectedItem.ToString());
                reward.OTvalue = ParseIntIfNotEmpty(tbRewardOTvalue.Text);
            }
            reward.clanPoints = Convert.ToInt32(clanPointsValue.Value);
            reward.Credits = ParseIntIfNotEmpty(creditsTextBox.Text);
            reward.KarmaPK = ParseIntIfNotEmpty(textBoxKarmaPK.Text);
            foreach (string knowlege in tbGetKnowleges.Text.Split(','))
                if (knowlege.Trim().Any())
                    reward.GetKnowleges.Add(int.Parse(knowlege));
            reward.RewardWindow = cbRewardWindow.Checked;
            if (cbRewardTeleport.SelectedItem != null)
                reward.teleportTo = cbRewardTeleport.SelectedItem.ToString();
            if (tbDungTarget.Text.Trim().Any())
            {
                reward.dungeonTarget = tbDungTarget.Text.Trim();
                reward.dungeonTargetMode = cbTargetMode.SelectedIndex;
            }

            penalty.Experience = ParseIntIfNotEmpty(tbPenaltyExperience.Text);
            penalty.Credits = ParseIntIfNotEmpty(tbPenaltyCredits.Text);
            penalty.KarmaPK = ParseIntIfNotEmpty(tbPenaltyKarmaPK.Text);
            foreach (string knowlege in tbGetKnowlegesPenalty.Text.Split(','))
                if (knowlege.Trim().Any())
                    penalty.GetKnowleges.Add(int.Parse(knowlege));

            int iProgressResult = 0;
            if (showCloseCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_CLOSE;
            if (showTakeCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_TAKE;
            if (showJournalCheckBox.Checked)
                iProgressResult |= this.SHOW_JOURNAL;
            if (showProgressCheckBox.Checked)
                iProgressResult |= this.SHOW_MESSAGE_PROGRESS;
            /*
            if (onWonTextBox.Text.Any())
                iProgressResult |= this.SHOW_ONWIN;
            if (onFailedTextBox.Text.Any())
                iProgressResult |= this.SHOW_ONFAILED;
            if (onGotTextBox.Text.Any())
                iProgressResult |= this.SHOW_ONGET;
            if (onOpenTextBox.Text.Any())
                iProgressResult |= this.SHOW_ONOPEN;
            if (onTestTextBox.Text.Any())
                iProgressResult |= this.SHOW_ONTEST;
            */
            if (!availabilityCheckBox.Checked)
            {
                iProgressResult |= this.NOT_SHOW_AVAILABILITY;
            }
            /*
            additional.screenMessageOnWin = cbWonScreenMsg.Checked;
            additional.screenMessageOnFailed = cbFailScreenMsg.Checked;
            additional.screenMessageOnGet = cbGetScreenMsg.Checked;
            additional.screenMessageOnOpen = cbOpenScreenMsg.Checked;
            additional.screenMessageOnTest = cbTestScreenMsg.Checked;
            */
            additional.ShowProgress = iProgressResult;
            additional.CantCancel = cantCancelCheckBox.Checked;
            additional.CantFail = cantFailCheckBox.Checked;
            if (cbFraction2Bonus.SelectedItem != null)
                additional.isFractionBonus = parent.fractions2.getFractionIDByDescr(cbFraction2Bonus.SelectedItem.ToString());
            CQuest retQuest;

            reward.items = new List<QuestItem>(editQuestReward.items);
            reward.Reputation = editQuestReward.Reputation;
            reward.Reputation2 = editQuestReward.Reputation2;
            reward.NPCReputation = editQuestReward.NPCReputation;
            reward.Effects = editQuestReward.Effects;
            reward.ChangeQuests = editQuestReward.ChangeQuests;
            reward.randomQuest = editQuestReward.randomQuest;
            reward.blackBoxes = editQuestReward.blackBoxes;

            penalty.items = new List<QuestItem>(editQuestPenalty.items);
            penalty.Reputation = editQuestPenalty.Reputation;
            penalty.Reputation2 = editQuestPenalty.Reputation2;
            penalty.NPCReputation = editQuestPenalty.NPCReputation;
            penalty.Effects = editQuestPenalty.Effects;
            penalty.ChangeQuests = editQuestPenalty.ChangeQuests;
            penalty.randomQuest = editQuestPenalty.randomQuest;

            editQuestRules.mapMarks = getMapMarks();
            if (editQuestRules.mapMarks == null)
            {
                MessageBox.Show("Неверно указаны координаты/радиус в метках для карты", "Ошибка");
                return null;
            }
            rules.items = new List<QuestItem>(editQuestRules.items);
            rules.dontTakeItems = editQuestRules.dontTakeItems;
            rules.dontHideMarks = editQuestRules.dontHideMarks;
            rules.space = editQuestRules.space;
            rules.mapMarks = new List<MapMark>(editQuestRules.mapMarks);

            editInformation.Items.Clear();
            foreach(var i in items_penalty)
            {
                editInformation.Items.Add(i.Key, i.Value);
            }
            foreach (var i in items_reward)
            {
                if (editInformation.Items.ContainsKey(i.Key))
                {
                    MessageBox.Show("Штраф и награды имеют дублирующий предмет", "Ошибка");
                    return null;
                }
                editInformation.Items.Add(i.Key, i.Value);
            }
            foreach (var i in items_rules)
            {
                if (editInformation.Items.ContainsKey(i.Key))
                {
                    MessageBox.Show("Штраф/награды и правила имеют дублирующий предмет", "Ошибка");
                    return null;
                }
                editInformation.Items.Add(i.Key, i.Value);
            }
            information.Items = editInformation.Items;

            target.AObjectAttrs = editTarget.AObjectAttrs;
            rules.basePercent = editQuestRules.basePercent;

            if (parent.questConst.isPVPQuest(target.QuestType))
            {
                conditions.bePvpWinner = cbConditionPVPTeam.SelectedIndex;
                conditions.pvpWinTeam = cbConditionPVPTeamWin.SelectedIndex;
                conditions.notDieCount = Convert.ToInt32(nupConditionDead.Value);
                conditions.duration = Convert.ToInt32(nupb2ctime.Value);

                int weapon_type = parent.npcItems.secondaryyWeapons.getIDByName(cbConditionWeapon.SelectedItem.ToString());
                if (weapon_type == 0)
                    weapon_type = parent.npcItems.primaryWeapons.getIDByName(cbConditionWeapon.SelectedItem.ToString());
                conditions.useWeaponType = weapon_type;
            }

            int priority = 0;
            if (cbPriority.SelectedItem != null) priority = QuestPriorities.getIDByName(cbPriority.SelectedItem.ToString());

            int level = Convert.ToInt32(nudLevel.Value);
            int qLinkType = 0;
            int qLink = 0;
            if (priority == 5 && !cantCancelCheckBox.Checked && additional.IsSubQuest == 0)
            {
                MessageBox.Show("Нельзя, чтоб рутовый сюжетный квест можно было отменить", "Ошибка");
                return null;
            }

            if (additional.IsSubQuest == 0)
            {
                qLinkType = cbQuestLinkType.SelectedIndex;
                if (cbQuestLinkType.SelectedIndex == 2)
                {
                    qLink = Convert.ToInt32(cbQuestLink.SelectedItem.ToString().Split()[0]);
                }
            }
            if (iState == ADD_NEW || iState == ADD_SUB)
            {
                if (iState == ADD_SUB)
                    additional.IsSubQuest = quest.QuestID;
                retQuest = new CQuest(this.QuestID, 1, priority, level, qLinkType, qLink, information, precondition, rules, reward, additional, target, penalty, conditions, cbHidden.Checked, cbOldQuest.Checked);
                parent.incQuestNewID();
            }
            else
            {
                int version = quest.Version;
                if (quest.QuestInformation.Title != information.Title || quest.QuestInformation.Description != information.Description
                    || quest.QuestInformation.DescriptionOnTest != information.DescriptionOnTest || quest.QuestInformation.DescriptionClosed != information.DescriptionClosed
                    ) //TODO QuestMessages
                        
                    if (parent.isLocaledQuest(quest.QuestID))
                    {
                        DialogResult dr = MessageBox.Show("Текст был изменён, нужно переводить?", "Внимание, ньюанс с переводами", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                            version++;
                    }
                    else version++;
                 
                retQuest = new CQuest(quest.QuestID, version, priority, level, qLinkType, qLink, information, precondition, rules, reward, additional, target, penalty, conditions, cbHidden.Checked, cbOldQuest.Checked);
            }
            return retQuest;
        }

        private List<MapMark> getMapMarks()
        {
            List<MapMark> result = new List<MapMark>();

            foreach (DataGridViewRow row in dataGridMapMark.Rows)
            {
                if (row.IsNewRow) continue;
                string s_coord = row.Cells[0].FormattedValue.ToString().Replace(',', ' ');
                string s_radius = row.Cells[1].FormattedValue.ToString();
                string[] tmp = row.Cells[2].FormattedValue.ToString().Split(' ');
                string space = parent.spacesConst.getSpaceNameByID(int.Parse(tmp[0]));
                float radius = 0;
                //try
                {
                    radius = Convert.ToSingle(s_radius);
                    foreach(var i in s_coord.Split(' '))
                    {
                        if (i.Any())
                            float.Parse(i.Trim(), System.Globalization.CultureInfo.InvariantCulture);
                            //Convert.ToDouble(i);
                    }
                }
                //catch
                {
                //    return null;
                }
                result.Add(new MapMark(s_coord, radius, space));
            }
            return result;
        }

        private int ParseIntIfNotEmpty(string text)
        {
            if (!text.Trim().Equals(""))
                return int.Parse(text);
            return 0;
        }
        //! Действия при смене типа кевеста
        private void eventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedQuestType = parent.questConst.getQuestTypeOnDescription(eventComboBox.SelectedItem.ToString());
            //if (selectedQuestType != 0)
            //{
                clearTargetContent();
                fillTargetForm(selectedQuestType);
            //}
            cbMarkOnTest.Visible = parent.questConst.isOnTest(selectedQuestType);
        }
        //! Закрытие окна без сохранения
        private void cancelButton_Click(object sender, EventArgs e)
        {
            parent.Enabled = true;
            this.Close();
        }
        //! Нажатие OK - сохранение квеста или создание нового
        private void okButton_Click(object sender, EventArgs e)
        {
            CQuest result = getQuest();
            if (result != null)
            {
                if (iState == ADD_NEW)
                    parent.createNewQuest(result);
                else if (iState == ADD_SUB)
                    parent.addQuest(result, quest.QuestID);
                else
                    parent.replaceQuest(result);

                if (result.Additional.IsSubQuest == 0)
                    parent.setTutorial(result.QuestID, (((result.Additional.ShowProgress & this.NOT_SHOW_AVAILABILITY) > 0)));
                if (debugTextBox.Text != "") { result.Additional.DebugData = debugTextBox.Text; }
                parent.Enabled = true;
                this.Close();
            }
        }

        private void dynamicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (dynamicCheckBox.Checked)
            {
                bTargetAddDynamic.Enabled = true;
                bTargetClearDynamic.Enabled = true;
                resultComboBox.Enabled = true;

                if (this.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST || this.QuestType == CQuestConstants.TYPE_KILLMOBS)
                    ltargetResult.Text = "Мин,Макс:";
                else if ((this.QuestType == CQuestConstants.TYPE_AREA_DISCOVER) || (QuestType == CQuestConstants.TYPE_IN_AREA))
                    ltargetResult.Text = "Зоны:";
                else if (this.QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
                    ltargetResult.Text = "Триггеры:";
            }
            else
            {
                if (this.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST || this.QuestType == CQuestConstants.TYPE_KILLMOBS)
                {
                    ltargetResult.Text = "Подтип моба";
                    resultComboBox.Enabled = true;
                }
                else
                {
                    ltargetResult.Text = "Результат:";
                    resultComboBox.Enabled = false;
                }
                bTargetAddDynamic.Enabled = false;
                bTargetClearDynamic.Enabled = false;
            }
        }

        private void bTargetClearDynamic_Click(object sender, EventArgs e)
        {
            resultComboBox.Items.Clear();
            resultComboBox.Text = "";
        }

        private void bTargetAddDynamic_Click(object sender, EventArgs e)
        {
            string str = "";

            if (this.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST || this.QuestType == CQuestConstants.TYPE_KILLMOBS)
            {
                str += quantityUpDown.Value.ToString();
            }
            else if ((this.QuestType == CQuestConstants.TYPE_AREA_DISCOVER) || (QuestType == CQuestConstants.TYPE_IN_AREA))
            {
                str += parent.zoneConst.getKeyOnDescription(targetComboBox.SelectedItem.ToString());
            }
            else if (this.QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
            {
                str += parent.triggerConst.getIdOnKey(targetComboBox.SelectedItem.ToString());
            }

            if (resultComboBox.Text.Equals(""))
                resultComboBox.Text += str;
            else
                resultComboBox.Text += ("," + str);
        }

        private void targetAttributeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (targetAttributeComboBox.SelectedItem.ToString().Equals("Квестовый."))
                bItemQID.Enabled = true;
            else
                bItemQID.Enabled = false;
        }

        private void targetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST || this.QuestType == CQuestConstants.TYPE_KILLMOBS)
            {
                resultComboBox.Items.Clear();
                List<string> mob_subtypes = parent.mobConst.getLevelsOnDescription(targetComboBox.SelectedItem.ToString());
                resultComboBox.Items.Add("");
                foreach (string level in mob_subtypes)
                    resultComboBox.Items.Add(level);
            }
            if (this.QuestType == CQuestConstants.TYPE_ENTITY_SEEN || this.QuestType == CQuestConstants.TYPE_ENTITY_SEEN_AUTO)
            {
                switch (resultComboBox.SelectedItem.ToString())
                {
                    case "QuestEntity":
                        break;
                    case "Creature":
                        targetAttributeComboBox.Items.Clear();
                        List<string> mob_subtypes = parent.mobConst.getLevelsOnDescription(targetComboBox.SelectedItem.ToString());
                        targetAttributeComboBox.Items.Add("");
                        foreach (string level in mob_subtypes)
                            targetAttributeComboBox.Items.Add(level);
                        break;
                    case "TimeEntity":
                        string cfgName = targetComboBox.SelectedItem.ToString();

                        foreach(var i in TimeEntityModels.getModelsByConfig(cfgName))
                        {
                            targetAttributeComboBox.Items.Add(i);
                        }
                        targetAttributeComboBox.SelectedIndex = 0;
                        break;
                }
            }
        }
                   
        private void QuestEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parent.Enabled = true;
        }

        private void bItemQID_Click(object sender, EventArgs e)
        {
            SelectQuestItem selectQItem = new SelectQuestItem(this, this.QuestID);
            this.Enabled = false;
        }
        //! Нажатие Репутация в наградах квеста - открывает окно редактирования репутаций
        private void bRewardReputation_Click(object sender, EventArgs e)
        {
            RewardFractions formFractions = new RewardFractions(this, parent.fractions, ref this.editQuestReward.Reputation, ref this.editQuestReward.NPCReputation);
            formFractions.ShowDialog();
            this.editQuestReward.Reputation = formFractions.reputations;
            this.editQuestReward.NPCReputation = formFractions.npc_reputations;
        }

        private void bRewardReputation2_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> fakeNpcRep = new Dictionary<string, int>();
            RewardFractions formFractions = new RewardFractions(this, parent.fractions2, ref this.editQuestReward.Reputation2, ref fakeNpcRep);
            formFractions.ShowDialog();
            this.editQuestReward.Reputation2 = formFractions.reputations;
        }
        //! Изменение состояния квеста - Туториал или нет
        private void availabilityCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        //! Открывает окно редактирования эффектов (бафов)
        private void bRewardEffects_Click(object sender, EventArgs e)
        {
            EditDialogEffect edit_effect = new EditDialogEffect(this.parent, this, this.QuestID, ref this.editQuestReward.Effects);
            edit_effect.Visible = true;
            this.Enabled = false;
        }

        //! Открывает окно редактирования  квестов
        private void bRewardQuests_Click(object sender, EventArgs e)
        {
            RewardQuestsDialog edit_quest = new RewardQuestsDialog(this, ref this.editQuestReward);
            this.Enabled = false;
            edit_quest.ShowDialog();
            this.Enabled = true;

        }

        private void bRewardBlackBox_Click(object sender, EventArgs e)
        {
            RewardBlackBoxDialog edit_black_box = new RewardBlackBoxDialog(this.parent, this, 0, 0);
            this.Enabled = false;
            edit_black_box.ShowDialog();
            this.Enabled = true;
            checkRewardIndicates();

        }

        //! Нажатие Предметы в правилах квеста - открывает форму с редактором предметов
        private void bItemQuestRules_Click(object sender, EventArgs e)
        {
            ItemDialog itemDialog = new ItemDialog(this.parent, this, null, quest.QuestID, this.ITEM_QUESTRULES);
            itemDialog.Enabled = true;
            itemDialog.Visible = true;
            this.Enabled = false;
        }
        //! Нажатие Предметы в наградах квеста - открывает форму с редактором предметов
        private void bRewardItem_Click(object sender, EventArgs e)
        {
            ItemDialog itemDialog = new ItemDialog(this.parent, this, null, quest.QuestID, this.ITEM_REWARD);
            itemDialog.Enabled = true;
            itemDialog.Visible = true;
            this.Enabled = false;
        }

        private void nBaseToCapturePercent_ValueChanged(object sender, EventArgs e)
        {
            editQuestRules.basePercent = Convert.ToSingle(nBaseToCapturePercent.Value) / 100;
        }

        private void cbState_CheckedChanged(object sender, EventArgs e)
        {
                editTarget.usePercent = cbState.Checked;
                lState.Enabled = cbState.Checked;
                udState.Enabled = cbState.Checked;

        }

        private void cbMobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> levels = parent.mobConst.getLevelsOnDescription(cbMobType.SelectedItem.ToString());
            cbMobLevel.Items.Clear();
            foreach (string item in levels)
            {
                cbMobLevel.Items.Add(item.ToString());
            }  
        }

        private void cbMobUniq_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMobUniq.Checked)
            {
                nupMobCount.Value = 1;
                nupMobCount.Enabled = false;
            }
            else nupMobCount.Enabled = true;
        }

        private void bPenaltyItem_Click(object sender, EventArgs e)
        {
            ItemDialog itemDialog = new ItemDialog(this.parent, this, null, quest.QuestID, this.ITEM_PENALTY);
            itemDialog.Enabled = true;
            itemDialog.Visible = true;
            this.Enabled = false;
        }

        private void bPenaltyEffects_Click(object sender, EventArgs e)
        {
            EditDialogEffect edit_effect = new EditDialogEffect(this.parent, this, this.QuestID, ref this.editQuestPenalty.Effects);
            edit_effect.Visible = true;
            this.Enabled = false;
        }

        private void bPenaltyReputation_Click(object sender, EventArgs e)
        {
            RewardFractions formFractions = new RewardFractions(this, parent.fractions, ref this.editQuestPenalty.Reputation, ref this.editQuestPenalty.NPCReputation);
            formFractions.ShowDialog();
            this.editQuestPenalty.Reputation = formFractions.reputations;
            this.editQuestPenalty.NPCReputation = formFractions.npc_reputations;
        }

        private void bPenaltyReputation2_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> fakeNPCRep = new Dictionary<string, int>();
            RewardFractions formFractions = new RewardFractions(this, parent.fractions2, ref this.editQuestPenalty.Reputation2, ref fakeNPCRep);
            formFractions.ShowDialog();
            this.editQuestPenalty.Reputation2 = formFractions.reputations;
        }

        private void bPenaltyQuests_Click(object sender, EventArgs e)
        {
            RewardQuestsDialog edit_quest = new RewardQuestsDialog(this, ref this.editQuestPenalty);
            this.Enabled = false;
            edit_quest.ShowDialog();
            this.Enabled = true;
        }

        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        private void EditQuestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.parent.QuestBox_SelectedIndexChanged(sender, e);
        }

        private void cbDontTakeItems_CheckedChanged(object sender, EventArgs e)
        {
            editQuestRules.dontTakeItems = !cbTakeItems.Checked;
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            Dictionary<string, bool> spaces = parent.spacesConst.getIntToSpaces(this.editQuestRules.space);
            FilterNPCForm form = new FilterNPCForm( ref spaces, this.parent);
            form.ShowDialog();
            this.editQuestRules.space = parent.spacesConst.getSpacesToInt(spaces);
            checkQuestRulesIndicates();
        }

        private void resultComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QuestType == CQuestConstants.TYPE_DUNGEON_EVENT)
            {
                targetComboBox.Items.Clear();
                int dungID = parent.dungeonConst.getIDByName(resultComboBox.SelectedItem.ToString());
                
                foreach (string boss_name in parent.dungeonConst.getBossesByDungID(dungID))
                    targetComboBox.Items.Add(boss_name);
                if (targetComboBox.Items.Count > 0)
                    targetComboBox.SelectedIndex = 0;
            }
            if (this.QuestType == CQuestConstants.TYPE_ANOMALY || this.QuestType == CQuestConstants.TYPE_ANOMALY_AUTO)
            {
                targetComboBox.Items.Clear();
                List<int> items = AnomalyTypes.getAnomalyLoot(resultComboBox.SelectedItem.ToString());

                //targetComboBox.Items.Add("");
                foreach (int item_type in items)
                    targetComboBox.Items.Add(parent.itemConst.getItemName(item_type));
            }

            if (this.QuestType == CQuestConstants.TYPE_ENTITY_SEEN || this.QuestType == CQuestConstants.TYPE_ENTITY_SEEN_AUTO)
            {
                switch(resultComboBox.SelectedItem.ToString())
                {
                    case "QuestEntity":
                        lNameObject.Text = "";
                        targetAttributeComboBox.Enabled = true;
                        targetAttributeComboBox.Items.Clear();
                        labelTargetAttr.Enabled = true;
                        labelTargetAttr.Text = "Модель:";
                        lTargetAttr1.Text = "";
                        targetComboBox.Enabled = false;
                        targetAttributeComboBox2.Enabled = false;
                        break;
                    case "Creature":
                        lNameObject.Enabled = true;
                        lNameObject.Text = "Тип моба:";
                        targetComboBox.Enabled = true;
                        targetComboBox.Items.Clear();
                        foreach (CMobDescription description in parent.mobConst.getAllDescriptions().Values)
                            targetComboBox.Items.Add(description.getName());
                        
                        targetComboBox.SelectedIndex = 0;
                        labelTargetAttr.Enabled = true;
                        labelTargetAttr.Text = "Подтип:";
                        targetAttributeComboBox.Enabled = true;
                        targetAttributeComboBox.SelectedIndex = 0;

                        lTargetAttr1.Text = "";
                        targetAttributeComboBox2.Enabled = false;
                        break;
                    case "TimeEntity": 
                        lNameObject.Enabled = true;
                        lNameObject.Text = "Имя конфига:";

                        targetComboBox.Enabled = true;
                        targetComboBox.Items.Clear();
                        foreach (var cfgName in TimeEntityModels.getConfigNames())
                            targetComboBox.Items.Add(cfgName);

                        targetComboBox.SelectedIndex = 0;

                        labelTargetAttr.Enabled = true;
                        labelTargetAttr.Text = "Модель:";
                        targetAttributeComboBox.Enabled = true;
                        //targetAttributeComboBox.Items.Clear();
                        targetComboBox.SelectedIndex = 0;

                        lTargetAttr1.Text = "";
                        targetAttributeComboBox2.Enabled = false;
                        break;
                }

            }
        }

        private void cbPVPtarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QuestType == CQuestConstants.TYPE_PVP_MAP_SCORE)
                cbPVPtarget2.Visible = cbPVPtarget.SelectedItem.Equals("больше всех");
        }

        private void cbPVPtarget3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QuestType == CQuestConstants.TYPE_PVP_MAP_CAPTURE_FLAG)
                cbPVPtarget2.Visible = cbPVPtarget3.SelectedItem.Equals("больше всех");
        }

        private void cbQuestLinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbQuestLink.Visible = (cbQuestLinkType.SelectedIndex == 2);
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
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
                await Task.Run(() => SpellChecker.CheckAndHighlightSpellingErrors(sender as RichTextBox));
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
            if (CSettings.hasErrorFinder())
            {
                SpellChecker.CheckAndHighlightSpellingErrors(titleTextBox);
                SpellChecker.CheckAndHighlightSpellingErrors(descriptionTextBox);
                SpellChecker.CheckAndHighlightSpellingErrors(descriptionOnTestTextBox);
                SpellChecker.CheckAndHighlightSpellingErrors(descriptionClosedTextBox);
            }
            /*
            TextUtils.findTextErrors(onWonTextBox);
            TextUtils.findTextErrors(onFailedTextBox);
            TextUtils.findTextErrors(onGotTextBox);
            TextUtils.findTextErrors(onOpenTextBox);
            TextUtils.findTextErrors(onTestTextBox);
            */
        }

        private void btnChangeQuestZones_Click(object sender, EventArgs e)
        {
            Forms.ChangeZones form = new Forms.ChangeZones(parent, targetAttributeComboBox2.Text);
            form.ShowDialog();
            targetAttributeComboBox2.Text = form.getZones();
        }

        private void cbMarkOnTest_CheckedChanged(object sender, EventArgs e)
        {
            editQuestRules.dontHideMarks = cbMarkOnTest.Checked;
        }
    }
}
