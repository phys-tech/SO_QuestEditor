using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor.Forms
{
    //! Форма локализации квеста, заголовок, описание и т.д.
    public partial class LocaleQuestForm : Form
    {
        //int ITEM_REWARD = 0;
        //int ITEM_QUESTRULES = 1;
        int ITEM_LOCALIZATION_RULES = 2;
        int ITEM_LOCALIZATION_REWARD = 3;

        MainForm parent;
        public CQuest cur_locale_quest;
        public CQuest pub_quest;
        int pub_version;

        //! Конструктор, заполняем поля формы русскими текстами и локализированными
        public LocaleQuestForm(MainForm parent, int questID)
        {
            InitializeComponent();
            this.parent = parent;
            parent.Enabled = false;

            CQuest locale_quest = parent.getLocaleQuest(questID);
            CQuest quest = parent.getQuestOnQuestID(questID);
            pub_version = quest.Version;
            lViewNpcName.Text = quest.Additional.Holder;
            lViewQuestID.Text = quest.QuestID.ToString();

            if (locale_quest == null)
            {
                // если нет локализации совсем - берем за основу русский квест и обнуляем данные с полями текста
                locale_quest = (CQuest)quest.Clone();
                locale_quest.QuestInformation.Description = "";
                locale_quest.QuestInformation.DescriptionClosed = "";
                locale_quest.QuestInformation.DescriptionOnTest = "";
                locale_quest.QuestInformation.Title = "";
                foreach (var key in locale_quest.QuestInformation.Items.Keys)
                {
                    locale_quest.QuestInformation.Items[key].description = "";
                    locale_quest.QuestInformation.Items[key].title = "";
                    locale_quest.QuestInformation.Items[key].activation = "";
                }
            }
            
            cur_locale_quest = (CQuest)locale_quest.Clone();
            pub_quest = (CQuest)quest.Clone();

            titleTextBox.Text = quest.QuestInformation.Title;
            descriptionTextBox.Text = quest.QuestInformation.Description;
            descriptionOnTestTextBox.Text = quest.QuestInformation.DescriptionOnTest;
            descriptionClosedTextBox.Text = quest.QuestInformation.DescriptionClosed;

            for (int i=0; i< quest.QuestInformation.messages.Count; i++ )
            {
                object[] row = { quest.QuestInformation.messages[i].message, locale_quest.QuestInformation.messages[i].message };
                dgQuestMessages.Rows.Add(row);
            }


            // если заголовки и описание совпадают, это означает, что в локализации просто копия русских
            // текстов (еще не готова и тупо скопирована) - значит ее не выводим на форму
           // if (locale_quest.QuestInformation.Description != quest.QuestInformation.Description ||
           //     locale_quest.QuestInformation.Title != quest.QuestInformation.Title)
            {
                localeLitleTextBox.Text = locale_quest.QuestInformation.Title;
                localeDescriptionTextBox.Text = locale_quest.QuestInformation.Description;
                localeDescriptionOnTestTextBox.Text = locale_quest.QuestInformation.DescriptionOnTest;
                localeDescriptionClosedTextBox.Text = locale_quest.QuestInformation.DescriptionClosed;
            }

            ComponentResourceManager resources = new ComponentResourceManager(this.GetType());
            ApplyResourceToControl(resources, this);
        }
    

    private static void ApplyResourceToControl(ComponentResourceManager res, Control control)
    {
        foreach (Control c in control.Controls)
            ApplyResourceToControl(res, c);

        var text = res.GetString(String.Format("{0}.Text", control.Name));
        control.Text = text ?? control.Text;
    }

    //! Закрытие формы
    private void LocaleQuestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
        }

        private void bItemQuestRules_Click(object sender, EventArgs e)
        {
            ItemDialog dialog = new ItemDialog(this.parent, null, this, cur_locale_quest.QuestID, ITEM_LOCALIZATION_RULES);
            dialog.Enabled = true;
            dialog.Visible = true;
            this.Enabled = false;
        }

        private void bItemReward_Click(object sender, EventArgs e)
        {
            ItemDialog dialog = new ItemDialog(this.parent, null, this, cur_locale_quest.QuestID, ITEM_LOCALIZATION_REWARD);
            dialog.Enabled = true;
            dialog.Visible = true;
            this.Enabled = false;

        }
        //! Нажатие ОК - сохраняем данные о переводе
        private void bOK_Click(object sender, EventArgs e)
        {
            cur_locale_quest.QuestInformation.Title = localeLitleTextBox.Text;
            cur_locale_quest.QuestInformation.Description = localeDescriptionTextBox.Text;
            cur_locale_quest.QuestInformation.DescriptionOnTest = localeDescriptionOnTestTextBox.Text;
            cur_locale_quest.QuestInformation.DescriptionClosed = localeDescriptionClosedTextBox.Text;


            foreach (DataGridViewRow row in dgQuestMessages.Rows)
            {
                string text_loc = row.Cells["text_loc"].FormattedValue.ToString();
                cur_locale_quest.QuestInformation.messages[row.Index].message = text_loc;
            }

            cur_locale_quest.Version = pub_version;
            // возможно здесь придется копировать данные из quest в cur_locale_quest
            parent.addLocaleQuest(cur_locale_quest);
            this.Close();
        }
        //! Нажатие Отмена - выходим без сохранения данных перевода
        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
