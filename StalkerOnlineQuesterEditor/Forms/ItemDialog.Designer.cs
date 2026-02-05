namespace StalkerOnlineQuesterEditor
{
    partial class ItemDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.itemGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bTranslate = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.itemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.itemAttr = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.itemQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemActivation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemBlock = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.itemGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemGridView
            // 
            this.itemGridView.AllowUserToOrderColumns = true;
            this.itemGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemType,
            this.itemAttr,
            this.itemQuantity,
            this.itemTitle,
            this.itemDescription,
            this.itemContent,
            this.itemActivation,
            this.itemBlock});
            this.itemGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemGridView.Location = new System.Drawing.Point(0, 0);
            this.itemGridView.Name = "itemGridView";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.itemGridView.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.itemGridView.Size = new System.Drawing.Size(895, 258);
            this.itemGridView.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bTranslate);
            this.panel1.Controls.Add(this.bCancel);
            this.panel1.Controls.Add(this.bOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 258);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(895, 49);
            this.panel1.TabIndex = 1;
            // 
            // bTranslate
            // 
            this.bTranslate.Enabled = false;
            this.bTranslate.Location = new System.Drawing.Point(12, 6);
            this.bTranslate.Name = "bTranslate";
            this.bTranslate.Size = new System.Drawing.Size(75, 23);
            this.bTranslate.TabIndex = 2;
            this.bTranslate.Text = "Перевод";
            this.bTranslate.UseVisualStyleBackColor = true;
            this.bTranslate.Click += new System.EventHandler(this.bTranslate_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(626, 6);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(545, 6);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 0;
            this.bOk.Text = "Ок";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // itemType
            // 
            this.itemType.HeaderText = "Тип";
            this.itemType.Name = "itemType";
            this.itemType.Width = 150;
            // 
            // itemAttr
            // 
            this.itemAttr.HeaderText = "Аттрибут";
            this.itemAttr.Items.AddRange(new object[] {
            "Обычный",
            "Квестовый",
            "Использовать"});
            this.itemAttr.Name = "itemAttr";
            // 
            // itemQuantity
            // 
            this.itemQuantity.HeaderText = "Количество";
            this.itemQuantity.Name = "itemQuantity";
            this.itemQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // itemTitle
            // 
            this.itemTitle.HeaderText = "Название";
            this.itemTitle.Name = "itemTitle";
            // 
            // itemDescription
            // 
            this.itemDescription.HeaderText = "Описание";
            this.itemDescription.Name = "itemDescription";
            // 
            // itemContent
            // 
            this.itemContent.HeaderText = "Содержание";
            this.itemContent.Name = "itemContent";
            // 
            // itemActivation
            // 
            this.itemActivation.HeaderText = "Имя действия";
            this.itemActivation.Name = "itemActivation";
            // 
            // itemBlock
            // 
            this.itemBlock.HeaderText = "Запрет использовать";
            this.itemBlock.Name = "itemBlock";
            this.itemBlock.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemBlock.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ItemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 307);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.itemGridView);
            this.Name = "ItemDialog";
            this.Text = "Предметы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ItemDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.itemGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView itemGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bTranslate;
        private System.Windows.Forms.DataGridViewComboBoxColumn itemType;
        private System.Windows.Forms.DataGridViewComboBoxColumn itemAttr;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemActivation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn itemBlock;
    }
}