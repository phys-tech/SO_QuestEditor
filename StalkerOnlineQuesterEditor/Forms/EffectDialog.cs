﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor
{
    public partial class EditDialogEffect : Form
    {
        public MainForm parent;
        public EditQuestForm parentForm;
        public List<CEffect> effects;

        public EditDialogEffect(MainForm parent, EditQuestForm parentForm, int questID, ref List<CEffect> effects)
        {
            InitializeComponent();
            this.parent = parent;
            this.parentForm = parentForm;
            this.effects = effects;

            foreach (string effect_name in parent.effects.getAllDescriptions())
                ((DataGridViewComboBoxColumn)dataGridEffects.Columns[0]).Items.Add(effect_name);
            ((DataGridViewComboBoxColumn)dataGridEffects.Columns[0]).Sorted = true;

            foreach (CEffect effect in effects)
            {
                string name = parent.effects.getDescriptionOnID(effect.getID());
                string stack = effect.getStack().ToString();
                
                object[] row = { name, stack};
                dataGridEffects.Rows.Add(row);
            }
        }

        private void EffectDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Enabled = true;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            effects.Clear();
            foreach (DataGridViewRow row in dataGridEffects.Rows)
            {
                if (row.Cells[1].FormattedValue.ToString() != "" && row.Cells[0].FormattedValue.ToString()!= "")
                {
                    string typeName = row.Cells[0].FormattedValue.ToString();
                    int id = parent.effects.getIDOnDescription(typeName);
                    int stack = int.Parse(row.Cells[1].FormattedValue.ToString());

                    effects.Add(new CEffect(id, stack));
                }
            }
            parentForm.checkRewardIndicates();
            this.Close();
        }
    }
}
