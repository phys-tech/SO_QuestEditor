namespace StalkerOnlineQuesterEditor.Forms
{
    partial class LocaleQuestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.questInformationBox = new System.Windows.Forms.GroupBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.localeDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.localeDescriptionOnTestTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.localeDescriptionClosedTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOpen = new System.Windows.Forms.TabPage();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.tabOnTest = new System.Windows.Forms.TabPage();
            this.descriptionOnTestTextBox = new System.Windows.Forms.TextBox();
            this.tabClosed = new System.Windows.Forms.TabPage();
            this.descriptionClosedTextBox = new System.Windows.Forms.TextBox();
            this.lViewQuestID = new System.Windows.Forms.Label();
            this.labelQuestID = new System.Windows.Forms.Label();
            this.lViewNpcName = new System.Windows.Forms.Label();
            this.labelNpcName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.localeLitleTextBox = new System.Windows.Forms.MaskedTextBox();
            this.lWin = new System.Windows.Forms.Label();
            this.lDescription = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.MaskedTextBox();
            this.rewardGroupBox = new System.Windows.Forms.GroupBox();
            this.bItemReward = new System.Windows.Forms.Button();
            this.lQuestRules = new System.Windows.Forms.GroupBox();
            this.bItemQuestRules = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.dgQuestMessages = new System.Windows.Forms.DataGridView();
            this.text_rus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.text_loc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.questInformationBox.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOpen.SuspendLayout();
            this.tabOnTest.SuspendLayout();
            this.tabClosed.SuspendLayout();
            this.rewardGroupBox.SuspendLayout();
            this.lQuestRules.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgQuestMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // questInformationBox
            // 
            this.questInformationBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.questInformationBox.Controls.Add(this.dgQuestMessages);
            this.questInformationBox.Controls.Add(this.tabControl2);
            this.questInformationBox.Controls.Add(this.tabControl1);
            this.questInformationBox.Controls.Add(this.lViewQuestID);
            this.questInformationBox.Controls.Add(this.labelQuestID);
            this.questInformationBox.Controls.Add(this.lViewNpcName);
            this.questInformationBox.Controls.Add(this.labelNpcName);
            this.questInformationBox.Controls.Add(this.label3);
            this.questInformationBox.Controls.Add(this.label2);
            this.questInformationBox.Controls.Add(this.label1);
            this.questInformationBox.Controls.Add(this.localeLitleTextBox);
            this.questInformationBox.Controls.Add(this.lWin);
            this.questInformationBox.Controls.Add(this.lDescription);
            this.questInformationBox.Controls.Add(this.titleTextBox);
            this.questInformationBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.questInformationBox.Location = new System.Drawing.Point(0, 0);
            this.questInformationBox.Name = "questInformationBox";
            this.questInformationBox.Size = new System.Drawing.Size(911, 326);
            this.questInformationBox.TabIndex = 1;
            this.questInformationBox.TabStop = false;
            this.questInformationBox.Text = "Информация";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(496, 97);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(410, 100);
            this.tabControl2.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.localeDescriptionTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(402, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "open";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // localeDescriptionTextBox
            // 
            this.localeDescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localeDescriptionTextBox.Location = new System.Drawing.Point(3, 3);
            this.localeDescriptionTextBox.Multiline = true;
            this.localeDescriptionTextBox.Name = "localeDescriptionTextBox";
            this.localeDescriptionTextBox.Size = new System.Drawing.Size(396, 68);
            this.localeDescriptionTextBox.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.localeDescriptionOnTestTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "onTest";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // localeDescriptionOnTestTextBox
            // 
            this.localeDescriptionOnTestTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localeDescriptionOnTestTextBox.Location = new System.Drawing.Point(3, 3);
            this.localeDescriptionOnTestTextBox.Multiline = true;
            this.localeDescriptionOnTestTextBox.Name = "localeDescriptionOnTestTextBox";
            this.localeDescriptionOnTestTextBox.Size = new System.Drawing.Size(396, 68);
            this.localeDescriptionOnTestTextBox.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.localeDescriptionClosedTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(402, 74);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Closed";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // localeDescriptionClosedTextBox
            // 
            this.localeDescriptionClosedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localeDescriptionClosedTextBox.Location = new System.Drawing.Point(3, 3);
            this.localeDescriptionClosedTextBox.Multiline = true;
            this.localeDescriptionClosedTextBox.Name = "localeDescriptionClosedTextBox";
            this.localeDescriptionClosedTextBox.Size = new System.Drawing.Size(396, 68);
            this.localeDescriptionClosedTextBox.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabOpen);
            this.tabControl1.Controls.Add(this.tabOnTest);
            this.tabControl1.Controls.Add(this.tabClosed);
            this.tabControl1.Location = new System.Drawing.Point(83, 97);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(407, 100);
            this.tabControl1.TabIndex = 9;
            // 
            // tabOpen
            // 
            this.tabOpen.Controls.Add(this.descriptionTextBox);
            this.tabOpen.Location = new System.Drawing.Point(4, 22);
            this.tabOpen.Name = "tabOpen";
            this.tabOpen.Padding = new System.Windows.Forms.Padding(3);
            this.tabOpen.Size = new System.Drawing.Size(399, 74);
            this.tabOpen.TabIndex = 0;
            this.tabOpen.Text = "open";
            this.tabOpen.UseVisualStyleBackColor = true;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionTextBox.Location = new System.Drawing.Point(3, 3);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(393, 68);
            this.descriptionTextBox.TabIndex = 4;
            // 
            // tabOnTest
            // 
            this.tabOnTest.Controls.Add(this.descriptionOnTestTextBox);
            this.tabOnTest.Location = new System.Drawing.Point(4, 22);
            this.tabOnTest.Name = "tabOnTest";
            this.tabOnTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnTest.Size = new System.Drawing.Size(345, 74);
            this.tabOnTest.TabIndex = 1;
            this.tabOnTest.Text = "onTest";
            this.tabOnTest.UseVisualStyleBackColor = true;
            // 
            // descriptionOnTestTextBox
            // 
            this.descriptionOnTestTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionOnTestTextBox.Location = new System.Drawing.Point(3, 3);
            this.descriptionOnTestTextBox.Multiline = true;
            this.descriptionOnTestTextBox.Name = "descriptionOnTestTextBox";
            this.descriptionOnTestTextBox.Size = new System.Drawing.Size(339, 68);
            this.descriptionOnTestTextBox.TabIndex = 5;
            // 
            // tabClosed
            // 
            this.tabClosed.Controls.Add(this.descriptionClosedTextBox);
            this.tabClosed.Location = new System.Drawing.Point(4, 22);
            this.tabClosed.Name = "tabClosed";
            this.tabClosed.Padding = new System.Windows.Forms.Padding(3);
            this.tabClosed.Size = new System.Drawing.Size(345, 74);
            this.tabClosed.TabIndex = 2;
            this.tabClosed.Text = "Closed";
            this.tabClosed.UseVisualStyleBackColor = true;
            // 
            // descriptionClosedTextBox
            // 
            this.descriptionClosedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionClosedTextBox.Location = new System.Drawing.Point(3, 3);
            this.descriptionClosedTextBox.Multiline = true;
            this.descriptionClosedTextBox.Name = "descriptionClosedTextBox";
            this.descriptionClosedTextBox.Size = new System.Drawing.Size(339, 68);
            this.descriptionClosedTextBox.TabIndex = 5;
            // 
            // lViewQuestID
            // 
            this.lViewQuestID.AutoSize = true;
            this.lViewQuestID.Location = new System.Drawing.Point(694, 20);
            this.lViewQuestID.Name = "lViewQuestID";
            this.lViewQuestID.Size = new System.Drawing.Size(21, 13);
            this.lViewQuestID.TabIndex = 21;
            this.lViewQuestID.Text = "lab";
            // 
            // labelQuestID
            // 
            this.labelQuestID.AutoSize = true;
            this.labelQuestID.Location = new System.Drawing.Point(633, 19);
            this.labelQuestID.Name = "labelQuestID";
            this.labelQuestID.Size = new System.Drawing.Size(52, 13);
            this.labelQuestID.TabIndex = 20;
            this.labelQuestID.Text = "Quest ID:";
            // 
            // lViewNpcName
            // 
            this.lViewNpcName.AutoSize = true;
            this.lViewNpcName.Location = new System.Drawing.Point(83, 20);
            this.lViewNpcName.Name = "lViewNpcName";
            this.lViewNpcName.Size = new System.Drawing.Size(21, 13);
            this.lViewNpcName.TabIndex = 19;
            this.lViewNpcName.Text = "lab";
            // 
            // labelNpcName
            // 
            this.labelNpcName.AutoSize = true;
            this.labelNpcName.Location = new System.Drawing.Point(19, 20);
            this.labelNpcName.Name = "labelNpcName";
            this.labelNpcName.Size = new System.Drawing.Size(57, 13);
            this.labelNpcName.TabIndex = 18;
            this.labelNpcName.Text = "Имя NPC:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(650, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Локализация";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Русский язык";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Заголовок:";
            // 
            // localeLitleTextBox
            // 
            this.localeLitleTextBox.Location = new System.Drawing.Point(496, 76);
            this.localeLitleTextBox.Name = "localeLitleTextBox";
            this.localeLitleTextBox.Size = new System.Drawing.Size(407, 20);
            this.localeLitleTextBox.TabIndex = 12;
            // 
            // lWin
            // 
            this.lWin.AutoSize = true;
            this.lWin.Location = new System.Drawing.Point(16, 203);
            this.lWin.Name = "lWin";
            this.lWin.Size = new System.Drawing.Size(55, 13);
            this.lWin.TabIndex = 9;
            this.lWin.Text = "Выигрыш";
            // 
            // lDescription
            // 
            this.lDescription.AutoSize = true;
            this.lDescription.Location = new System.Drawing.Point(14, 127);
            this.lDescription.Name = "lDescription";
            this.lDescription.Size = new System.Drawing.Size(60, 13);
            this.lDescription.TabIndex = 2;
            this.lDescription.Text = "Описание:";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(83, 76);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(407, 20);
            this.titleTextBox.TabIndex = 1;
            // 
            // rewardGroupBox
            // 
            this.rewardGroupBox.AutoSize = true;
            this.rewardGroupBox.Controls.Add(this.bItemReward);
            this.rewardGroupBox.Location = new System.Drawing.Point(0, 390);
            this.rewardGroupBox.Name = "rewardGroupBox";
            this.rewardGroupBox.Size = new System.Drawing.Size(884, 61);
            this.rewardGroupBox.TabIndex = 6;
            this.rewardGroupBox.TabStop = false;
            this.rewardGroupBox.Text = "Награда";
            // 
            // bItemReward
            // 
            this.bItemReward.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bItemReward.Location = new System.Drawing.Point(8, 19);
            this.bItemReward.Name = "bItemReward";
            this.bItemReward.Size = new System.Drawing.Size(104, 23);
            this.bItemReward.TabIndex = 32;
            this.bItemReward.Text = "Предметы";
            this.bItemReward.UseVisualStyleBackColor = true;
            this.bItemReward.Click += new System.EventHandler(this.bItemReward_Click);
            // 
            // lQuestRules
            // 
            this.lQuestRules.Controls.Add(this.bItemQuestRules);
            this.lQuestRules.Location = new System.Drawing.Point(0, 332);
            this.lQuestRules.Name = "lQuestRules";
            this.lQuestRules.Size = new System.Drawing.Size(884, 52);
            this.lQuestRules.TabIndex = 5;
            this.lQuestRules.TabStop = false;
            this.lQuestRules.Text = "Правила квеста";
            // 
            // bItemQuestRules
            // 
            this.bItemQuestRules.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bItemQuestRules.ImageKey = "(none)";
            this.bItemQuestRules.Location = new System.Drawing.Point(10, 19);
            this.bItemQuestRules.Name = "bItemQuestRules";
            this.bItemQuestRules.Size = new System.Drawing.Size(102, 23);
            this.bItemQuestRules.TabIndex = 11;
            this.bItemQuestRules.Text = "Предметы";
            this.bItemQuestRules.UseVisualStyleBackColor = true;
            this.bItemQuestRules.Click += new System.EventHandler(this.bItemQuestRules_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.bCancel);
            this.groupBox1.Controls.Add(this.bOK);
            this.groupBox1.Location = new System.Drawing.Point(0, 457);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(884, 61);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(797, 19);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(694, 19);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // dgQuestMessages
            // 
            this.dgQuestMessages.AllowUserToAddRows = false;
            this.dgQuestMessages.AllowUserToDeleteRows = false;
            this.dgQuestMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgQuestMessages.ColumnHeadersVisible = false;
            this.dgQuestMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.text_rus,
            this.text_loc});
            this.dgQuestMessages.Location = new System.Drawing.Point(87, 199);
            this.dgQuestMessages.Name = "dgQuestMessages";
            this.dgQuestMessages.RowHeadersVisible = false;
            this.dgQuestMessages.Size = new System.Drawing.Size(812, 121);
            this.dgQuestMessages.TabIndex = 26;
            // 
            // text_rus
            // 
            this.text_rus.HeaderText = "Русский";
            this.text_rus.Name = "text_rus";
            this.text_rus.Width = 403;
            // 
            // text_loc
            // 
            this.text_loc.HeaderText = "Локализация";
            this.text_loc.Name = "text_loc";
            this.text_loc.Width = 405;
            // 
            // LocaleQuestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(911, 517);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rewardGroupBox);
            this.Controls.Add(this.lQuestRules);
            this.Controls.Add(this.questInformationBox);
            this.Name = "LocaleQuestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Локализация события";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LocaleQuestForm_FormClosing);
            this.questInformationBox.ResumeLayout(false);
            this.questInformationBox.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabOpen.ResumeLayout(false);
            this.tabOpen.PerformLayout();
            this.tabOnTest.ResumeLayout(false);
            this.tabOnTest.PerformLayout();
            this.tabClosed.ResumeLayout(false);
            this.tabClosed.PerformLayout();
            this.rewardGroupBox.ResumeLayout(false);
            this.lQuestRules.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgQuestMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox questInformationBox;
        private System.Windows.Forms.Label lWin;
        private System.Windows.Forms.Label lDescription;
        private System.Windows.Forms.MaskedTextBox titleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox localeLitleTextBox;
        private System.Windows.Forms.GroupBox rewardGroupBox;
        private System.Windows.Forms.Button bItemReward;
        private System.Windows.Forms.GroupBox lQuestRules;
        private System.Windows.Forms.Button bItemQuestRules;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lViewQuestID;
        private System.Windows.Forms.Label labelQuestID;
        private System.Windows.Forms.Label lViewNpcName;
        private System.Windows.Forms.Label labelNpcName;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox localeDescriptionOnTestTextBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox localeDescriptionClosedTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOpen;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TabPage tabOnTest;
        private System.Windows.Forms.TextBox descriptionOnTestTextBox;
        private System.Windows.Forms.TabPage tabClosed;
        private System.Windows.Forms.TextBox descriptionClosedTextBox;
        private System.Windows.Forms.TextBox localeDescriptionTextBox;
        private System.Windows.Forms.DataGridView dgQuestMessages;
        private System.Windows.Forms.DataGridViewTextBoxColumn text_rus;
        private System.Windows.Forms.DataGridViewTextBoxColumn text_loc;
    }
}