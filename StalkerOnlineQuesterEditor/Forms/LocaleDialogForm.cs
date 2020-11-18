﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor.Forms
{
    //! Форма локализации диалога, вопрос NPC и ответ героя.
    public partial class LocaleDialogForm : Form
    {
        MainForm parent;
        CDialog originalDialog;
        CDialog localeDialog;

        //! Конструктор, заполняет поля переводов
        public LocaleDialogForm(MainForm parent, int selectedDialogID)
        {
            InitializeComponent();
            this.parent = parent;
            parent.Enabled = false;
            CDialog dialog = parent.getDialogOnDialogID(selectedDialogID);
            NPCReactiontextBox.Text = dialog.Text;
            textBoxAnswer.Text = dialog.Title;
            lViewNpcName.Text = dialog.Holder;
            lViewDialogId.Text = dialog.DialogID.ToString();
            this.originalDialog = dialog;
            localeDialog = CDialogs.getLocaleDialog(dialog.DialogID, CSettings.getCurrentLocale(), dialog.Holder, parent.dialogs.locales);

            if (localeDialog != null)
            {
                if (localeDialog.Text != dialog.Text || localeDialog.Title != dialog.Title)
                {
                    textBoxNPCReactionLocale.Text = localeDialog.Text;
                    textBoxAnswerLocale.Text = localeDialog.Title;
                }
            }
        }
        //! Закрытие формы
        private void LocaleDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parent.Enabled = true;
        }
        //! Нажатие Отмена на форме - ничего не сохраняем
        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //! Нажатие ОК - сохраняем все данные
        private void bOk_Click(object sender, EventArgs e)
        {
            CDialog newDialog = new CDialog();
            newDialog.Text = textBoxNPCReactionLocale.Text;
            newDialog.Title = textBoxAnswerLocale.Text;
            newDialog.DialogID = originalDialog.DialogID;
            newDialog.Holder = originalDialog.Holder;
            newDialog.version = originalDialog.version;
            newDialog.Nodes = originalDialog.Nodes;
            newDialog.Precondition = originalDialog.Precondition;
            newDialog.Actions = originalDialog.Actions;
            if (localeDialog != null)
                newDialog.coordinates = localeDialog.coordinates;
            else
                newDialog.coordinates = originalDialog.coordinates;
            if (cbNotFinal.Checked)
                newDialog.version --;
            parent.addLocaleDialog(newDialog);
            this.Close();
        }
    }
}
