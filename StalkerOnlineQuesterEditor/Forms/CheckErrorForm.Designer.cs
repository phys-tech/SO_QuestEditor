namespace StalkerOnlineQuesterEditor.Forms
{
    partial class CheckErrorForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckErrorForm));
            this.btnStart = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lCurrentCheck = new System.Windows.Forms.Label();
            this.bpNoErrors = new System.Windows.Forms.PictureBox();
            this.cbErrorItem = new System.Windows.Forms.CheckBox();
            this.cbErrorQuest = new System.Windows.Forms.CheckBox();
            this.cbErrQuest5 = new System.Windows.Forms.CheckBox();
            this.cbErrOther = new System.Windows.Forms.CheckBox();
            this.cbErrNoRoot = new System.Windows.Forms.CheckBox();
            this.cbErrDialogs = new System.Windows.Forms.CheckBox();
            this.сохранитьВсёВФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bpNoErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 7);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(85, 29);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Начать";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.ContextMenuStrip = this.contextMenuStrip1;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(3, 68);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(623, 472);
            this.lbLog.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.удалитьToolStripMenuItem,
            this.сохранитьВсёВФайлToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 114);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem2.Text = "Перейти к квесту";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem1.Text = "Копировать QuestID";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(94, 26);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(531, 10);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Visible = false;
            // 
            // lCurrentCheck
            // 
            this.lCurrentCheck.AutoSize = true;
            this.lCurrentCheck.Location = new System.Drawing.Point(94, 7);
            this.lCurrentCheck.Name = "lCurrentCheck";
            this.lCurrentCheck.Size = new System.Drawing.Size(219, 13);
            this.lCurrentCheck.TabIndex = 3;
            this.lCurrentCheck.Text = "Перед проверкой нужно расспарсить всё";
            // 
            // bpNoErrors
            // 
            this.bpNoErrors.Image = ((System.Drawing.Image)(resources.GetObject("bpNoErrors.Image")));
            this.bpNoErrors.Location = new System.Drawing.Point(174, 101);
            this.bpNoErrors.Name = "bpNoErrors";
            this.bpNoErrors.Size = new System.Drawing.Size(452, 434);
            this.bpNoErrors.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bpNoErrors.TabIndex = 5;
            this.bpNoErrors.TabStop = false;
            this.bpNoErrors.Visible = false;
            // 
            // cbErrorItem
            // 
            this.cbErrorItem.AutoSize = true;
            this.cbErrorItem.Checked = true;
            this.cbErrorItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrorItem.Location = new System.Drawing.Point(3, 42);
            this.cbErrorItem.Name = "cbErrorItem";
            this.cbErrorItem.Size = new System.Drawing.Size(96, 17);
            this.cbErrorItem.TabIndex = 6;
            this.cbErrorItem.Text = "Err. предметы";
            this.cbErrorItem.UseVisualStyleBackColor = true;
            this.cbErrorItem.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbErrorQuest
            // 
            this.cbErrorQuest.AutoSize = true;
            this.cbErrorQuest.Checked = true;
            this.cbErrorQuest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrorQuest.Location = new System.Drawing.Point(97, 42);
            this.cbErrorQuest.Name = "cbErrorQuest";
            this.cbErrorQuest.Size = new System.Drawing.Size(74, 17);
            this.cbErrorQuest.TabIndex = 7;
            this.cbErrorQuest.Text = "Err. квест";
            this.cbErrorQuest.UseVisualStyleBackColor = true;
            this.cbErrorQuest.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbErrQuest5
            // 
            this.cbErrQuest5.AutoSize = true;
            this.cbErrQuest5.Location = new System.Drawing.Point(527, 42);
            this.cbErrQuest5.Name = "cbErrQuest5";
            this.cbErrQuest5.Size = new System.Drawing.Size(98, 17);
            this.cbErrQuest5.TabIndex = 8;
            this.cbErrQuest5.Text = "Для разрабов";
            this.cbErrQuest5.UseVisualStyleBackColor = true;
            this.cbErrQuest5.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbErrOther
            // 
            this.cbErrOther.AutoSize = true;
            this.cbErrOther.Checked = true;
            this.cbErrOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrOther.Location = new System.Drawing.Point(378, 42);
            this.cbErrOther.Name = "cbErrOther";
            this.cbErrOther.Size = new System.Drawing.Size(79, 17);
            this.cbErrOther.TabIndex = 9;
            this.cbErrOther.Text = "Err. другие";
            this.cbErrOther.UseVisualStyleBackColor = true;
            this.cbErrOther.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbErrNoRoot
            // 
            this.cbErrNoRoot.AutoSize = true;
            this.cbErrNoRoot.Checked = true;
            this.cbErrNoRoot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrNoRoot.Location = new System.Drawing.Point(260, 42);
            this.cbErrNoRoot.Name = "cbErrNoRoot";
            this.cbErrNoRoot.Size = new System.Drawing.Size(112, 17);
            this.cbErrNoRoot.TabIndex = 10;
            this.cbErrNoRoot.Text = "Err. нет родителя";
            this.cbErrNoRoot.UseVisualStyleBackColor = true;
            this.cbErrNoRoot.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbErrDialogs
            // 
            this.cbErrDialogs.AutoSize = true;
            this.cbErrDialogs.Checked = true;
            this.cbErrDialogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErrDialogs.Location = new System.Drawing.Point(174, 42);
            this.cbErrDialogs.Name = "cbErrDialogs";
            this.cbErrDialogs.Size = new System.Drawing.Size(80, 17);
            this.cbErrDialogs.TabIndex = 11;
            this.cbErrDialogs.Text = "Err. диалог";
            this.cbErrDialogs.UseVisualStyleBackColor = true;
            // 
            // сохранитьВсёВФайлToolStripMenuItem
            // 
            this.сохранитьВсёВФайлToolStripMenuItem.Name = "сохранитьВсёВФайлToolStripMenuItem";
            this.сохранитьВсёВФайлToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.сохранитьВсёВФайлToolStripMenuItem.Text = "Сохранить всё в файл";
            this.сохранитьВсёВФайлToolStripMenuItem.Click += new System.EventHandler(this.сохранитьВсёВФайлToolStripMenuItem_Click);
            // 
            // CheckErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 548);
            this.Controls.Add(this.cbErrDialogs);
            this.Controls.Add(this.cbErrNoRoot);
            this.Controls.Add(this.cbErrOther);
            this.Controls.Add(this.cbErrQuest5);
            this.Controls.Add(this.cbErrorQuest);
            this.Controls.Add(this.cbErrorItem);
            this.Controls.Add(this.bpNoErrors);
            this.Controls.Add(this.lCurrentCheck);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btnStart);
            this.Name = "CheckErrorForm";
            this.Text = "CheckErrorForm";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bpNoErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lCurrentCheck;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.PictureBox bpNoErrors;
        private System.Windows.Forms.CheckBox cbErrorItem;
        private System.Windows.Forms.CheckBox cbErrorQuest;
        private System.Windows.Forms.CheckBox cbErrQuest5;
        private System.Windows.Forms.CheckBox cbErrOther;
        private System.Windows.Forms.CheckBox cbErrNoRoot;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.CheckBox cbErrDialogs;
        private System.Windows.Forms.ToolStripMenuItem сохранитьВсёВФайлToolStripMenuItem;
    }
}