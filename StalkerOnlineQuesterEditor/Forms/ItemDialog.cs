using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using StalkerOnlineQuesterEditor.Forms;

namespace StalkerOnlineQuesterEditor
{
    public partial class ItemDialog : Form
    {
        const int ITEM_REWARD = 0;
        const int ITEM_QUESTRULES = 1;
        const int ITEM_LOCALIZATION_RULES = 2;
        const int ITEM_LOCALIZATION_REWARD = 3;
        const int ITEM_PENALTY = 4;

        int questID;
        public MainForm parent;
        EditQuestForm parentForm;
        LocaleQuestForm parentForm2;

        //List<string> locale_title;
        //List<string> locale_description;

        Dictionary<int, QuestItemInfo> locale;
        int translate = 0;
        int formType;

        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (formType == ITEM_QUESTRULES)
            {
                if (e.ColumnIndex == 1) // your combo column index 
                {
                    e.Value = "Квестовый";
                }
            }
        }

        public ItemDialog(MainForm parent, EditQuestForm parentForm, LocaleQuestForm parentForm2, int questID, int type)
        {
            InitializeComponent();
            itemGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(CellFormatting);
            //itemGridView.CellBeginEdit += CellFormatting;
            this.Enabled = false;
            this.parent = parent;
            this.parentForm2 = parentForm2;
            this.parentForm = parentForm;
            this.formType = type;
            this.questID = questID;

            locale = new Dictionary<int, QuestItemInfo>();

            foreach (CItem item in parent.itemConst.getAllItems().Values)
                ((DataGridViewComboBoxColumn)itemGridView.Columns[0]).Items.Add(item.getName());
            ((DataGridViewComboBoxColumn)itemGridView.Columns[0]).Sorted = true;
            if (formType == ITEM_LOCALIZATION_REWARD || formType == ITEM_LOCALIZATION_RULES)
            {
                bTranslate.Enabled = true;
                if (formType == ITEM_LOCALIZATION_REWARD)
                {
                    this.locale = parentForm2.cur_locale_quest.QuestInformation.Items;
                }
                else
                {
                    this.locale = parentForm2.cur_locale_quest.QuestInformation.Items;
                }
            }
            fillItemGrid();
        }                
        
        private void fillItemGrid()
        {
            itemGridView.Rows.Clear();
            List<int> EDIT_TYPES = new List<int> { ITEM_REWARD, ITEM_PENALTY, ITEM_QUESTRULES };
            List<int> LOCALIZATE_TYPES = new List<int> { ITEM_LOCALIZATION_REWARD, ITEM_LOCALIZATION_RULES };
            if (EDIT_TYPES.Contains(formType))
            {
                string formTitle = "";
                List<QuestItem> items;
                switch (formType)
                {
                    case ITEM_REWARD:
                        formTitle = " Награда"; items = parentForm.editQuestReward.items; break;
                    case ITEM_PENALTY:
                        formTitle = " Штраф"; items = parentForm.editQuestPenalty.items; break;
                    case ITEM_QUESTRULES:
                        formTitle = " Правила квеста"; items = parentForm.editQuestRules.items; break;
                    default:
                        return;
                }
                this.Text += formTitle;
                foreach (QuestItem item in items)
                {
                    int typeID = item.itemType;
                    int quantity = item.count;
                    string name = parent.itemConst.getItemName(typeID);

                    string attr;
                    switch (item.attribute)
                    {
                        case ItemAttribute.QUEST:
                            attr = "Квестовый";
                            break;
                        case ItemAttribute.USE:
                            attr = "Использовать";
                            break;
                        default:
                            attr = "Обычный";
                            break;
                    }

                    string title = "";
                    string description = "";
                    string activation = "";
                    string content = "";

                    if (parentForm.quest.QuestInformation.Items.Keys.Contains(typeID))
                    {
                        title = parentForm.quest.QuestInformation.Items[typeID].title;
                        description = parentForm.quest.QuestInformation.Items[typeID].description;
                        activation = parentForm.quest.QuestInformation.Items[typeID].activation;
                        content = parentForm.quest.QuestInformation.Items[typeID].content;
                    }
                    object[] row = { name, attr, quantity.ToString(), title, description, content, activation };
                    itemGridView.Rows.Add(row);
                }
            }
            else if (LOCALIZATE_TYPES.Contains(formType))
            {
                string formTitle = "";
                List<QuestItem> items;
                switch (formType)
                {
                    case ITEM_LOCALIZATION_REWARD:
                        formTitle = "Локализация награды"; items = parentForm2.pub_quest.Reward.items; break;
                    case ITEM_LOCALIZATION_RULES:
                        formTitle = "Локализация правил квеста"; items = parentForm2.pub_quest.QuestRules.items; break;
                    default:
                        return;
                }
                this.Text = formTitle;
                foreach (QuestItem item in items)
                {
                    int typeID = item.itemType;
                    int quantity = item.count;
                    string name = parent.itemConst.getItemName(typeID);
                    string attr;
                    switch (item.attribute)
                    {
                        case ItemAttribute.QUEST:
                            attr = "Квестовый";
                            break;
                        case ItemAttribute.USE:
                            attr = "Использовать";
                            break;
                        default:
                            attr = "Обычный";
                            break;
                    }
                    string title = "";
                    string description = "";
                    string content = "";
                    string activation = "";
                    if (parentForm2.pub_quest.QuestInformation.Items.Keys.Contains(typeID))
                    {
                        if (this.translate == 0 || !this.locale.ContainsKey(typeID))
                        {
                            title = parentForm2.pub_quest.QuestInformation.Items[typeID].title;
                            description = parentForm2.pub_quest.QuestInformation.Items[typeID].description;
                            content = parentForm2.pub_quest.QuestInformation.Items[typeID].content;
                            activation = parentForm2.pub_quest.QuestInformation.Items[typeID].activation;
                        }
                        else
                        {
                            title = this.locale[typeID].title;
                            description = this.locale[typeID].description;
                            content = this.locale[typeID].content;
                            activation = this.locale[typeID].activation;
                        }
                    }

                    object[] row = { name, attr, quantity.ToString(), title, description, content, activation };
                    itemGridView.Rows.Add(row);

                    for (int row_index = 0; row_index < itemGridView.Rows.Count; row_index ++ )
                    {
                        if (translate == 1)
                            itemGridView.Rows[row_index].ReadOnly = false;
                        else
                        {
                            if (row_index != 4 && row_index != 5)
                                itemGridView.Rows[row_index].ReadOnly = false;
                            else
                                itemGridView.Rows[row_index].ReadOnly = true;
                        }
                    }
                }
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ItemDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (parentForm != null)
                parentForm.Enabled = true;
            else
                parentForm2.Enabled = true;
        }

        void parseLocale()
        {
            foreach (DataGridViewRow row in itemGridView.Rows)
            {
                string title = row.Cells["itemTitle"].FormattedValue.ToString();
                string description = row.Cells["itemDescription"].FormattedValue.ToString();
                string activation = row.Cells["itemActivation"].FormattedValue.ToString();
                string content = row.Cells["itemContent"].FormattedValue.ToString();
                string typeName = row.Cells["itemType"].FormattedValue.ToString();
                int typeID = parent.itemConst.getIDOnName(typeName);
                if (!this.locale.Keys.Contains(typeID))
                    this.locale.Add(typeID, new QuestItemInfo());
                this.locale[typeID].description = description;
                this.locale[typeID].title = title;
                this.locale[typeID].content = content;
                this.locale[typeID].activation = activation;
            }
        }

        private void bTranslate_Click(object sender, EventArgs e)
        {
            if (this.translate == 0)
            {
                this.translate = 1;
                bTranslate.Text = "Оригинал";
                fillItemGrid();
            }
            else
            {
                this.translate = 0;
                bTranslate.Text = "Перевод";
                parseLocale();
                fillItemGrid();
            }
        }
        //! Нажатие ОК - проверка и сохранение данных, выход в форму редактирования квеста
        private void bOk_Click(object sender, EventArgs e)
        {
            List<QuestItem> items = new List<QuestItem>();
            Dictionary<int, QuestItemInfo> itemsInfo = new Dictionary<int, QuestItemInfo>();

            foreach (DataGridViewRow row in itemGridView.Rows)
            {
                QuestItem item = new QuestItem();
                string typeName = row.Cells["itemType"].FormattedValue.ToString();
                if (!typeName.Equals(""))
                {
                    int quantity = int.Parse(row.Cells["itemQuantity"].FormattedValue.ToString());
                    if (quantity >= 1)
                    {
                        item.itemType = parent.itemConst.getIDOnName(typeName);
                        item.count = quantity;
                        string attrName = row.Cells["itemAttr"].FormattedValue.ToString();
                        string title = row.Cells["itemTitle"].FormattedValue.ToString();
                        string description = row.Cells["itemDescription"].FormattedValue.ToString();
                        string activation = row.Cells["itemActivation"].FormattedValue.ToString();
                        string content = row.Cells["itemContent"].FormattedValue.ToString();
                        switch (attrName)
                        {
                            case "Квестовый":
                                item.attribute = ItemAttribute.QUEST;
                                try
                                {
                                    itemsInfo.Add(item.itemType, new QuestItemInfo(title, description, activation, content));
                                }
                                catch
                                {
                                    MessageBox.Show("Есть идентичные квестовые предметы.", "Ошибка.");
                                    return;
                                }
                                break;
                            case "Использовать": item.attribute = ItemAttribute.USE; break;
                            default: item.attribute = ItemAttribute.NORMAL; break;
                        }
                        items.Add(item);
                    }
                }
            }

            if (formType == ITEM_QUESTRULES)
            {
                parentForm.editQuestRules.items.Clear();
                parentForm.editQuestRules.items = items;
                parentForm.checkQuestRulesIndicates();
                parentForm.items_rules.Clear();
                parentForm.items_rules = itemsInfo;
            }
            else if (formType == ITEM_REWARD)
            {
                parentForm.editQuestReward.items.Clear();
                parentForm.editQuestReward.items = items;
                parentForm.items_reward.Clear();
                parentForm.items_reward = itemsInfo;
            }
            else if (formType == ITEM_PENALTY)
            {
                parentForm.editQuestPenalty.items.Clear();
                parentForm.editQuestPenalty.items = items;
                parentForm.checkRewardIndicates();
                parentForm.items_penalty.Clear();
                parentForm.items_penalty = itemsInfo;
            }
            else if (formType == ITEM_LOCALIZATION_RULES || formType == ITEM_LOCALIZATION_REWARD)
            {
                if (this.translate == 1)
                    parseLocale();
                parentForm2.cur_locale_quest.QuestInformation.Items = this.locale;
            }
            this.Close();
        }
        //! Проверка, что в поле Вероятность ввели корректное число
        float checkFloatValue(string value)
        {            
            float result;
            value = value.Replace(",", ".");
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                MessageBox.Show("Поле вероятность содержит недопустимые символы","Ошибка!");
                return -1.0f;
            }

            if (result > 1)
            {
                MessageBox.Show("Вероятность не может быть больше 1", "Ошибка!");
                return -1.0f;
            }
            if (result < 0)
            {
                MessageBox.Show("Вероятность не может быть меньше 0", "Ошибка!");
                return -1.0f;
            }

            return result;
        }
    }
}