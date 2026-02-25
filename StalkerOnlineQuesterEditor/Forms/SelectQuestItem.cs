using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor
{
    public partial class SelectQuestItem : Form
    {
        EditQuestForm itemParent;

        public SelectQuestItem(EditQuestForm itemParent, int QuestID)
        {
            this.itemParent = itemParent;
            InitializeComponent();
            CQuest quest = new CQuest();
            quest = itemParent.getQuest();
            List<CQuest> wayIDs = new List<CQuest>();
            
            if (QuestItem.hasQuestItem(quest.Reward.items) || QuestItem.hasQuestItem(quest.QuestRules.items))
            {
                /*
                не надо убирать самого себя
                foreach (CQuest q in wayIDs)
                    if (q.QuestID == QuestID)
                    {
                        wayIDs.Remove(q);
                        break;
                    }
                */
                wayIDs.Add(quest);
            }

            List<CQuest> getted = new List<CQuest>();
            if (quest.Additional.IsSubQuest.Equals(0))
                getted = itemParent.parent.getTreesItemIDs(quest.QuestID);
            else
                getted = itemParent.parent.getTreesItemIDs(quest.Additional.IsSubQuest);
            CQuest rem = null;

            foreach (CQuest q in getted)
                if (q.QuestID == QuestID)
                    rem = q;
            if (rem !=null)
                getted.Remove(rem);

            
            wayIDs.AddRange(getted);
            bool flag = false;
            foreach (CQuest q in wayIDs)
                if (q.QuestID == QuestID)
                {
                    flag = true;
                    break;
                }
            if (!flag) wayIDs.Add(quest);
            if (!wayIDs.Any())
            {
                System.Console.WriteLine("Ways empty");
                this.Close();

            }
            else
            {
                this.Enabled = true;
                this.Visible = true;
            }
            foreach (CQuest q in wayIDs)
            {
                CheckBox box = new CheckBox();
                box.Text = q.QuestID.ToString();
                box.Dock = DockStyle.Top;
                if (quest.Target.AObjectAttrs.Contains(q.QuestID))
                    box.Checked = true;
                box.CheckedChanged += new EventHandler(box_CheckedChanged);
                splitContainer1.Panel1.Controls.Add(box);
            }
            updateLabel();
        }

        void box_CheckedChanged(object sender, EventArgs e)
        {
            updateLabel();
        }

        void updateLabel()
        {
            lItems.Text = "";
            foreach (CheckBox obj in splitContainer1.Panel1.Controls)
                if (obj.Checked)
                {

                    int qID = int.Parse(obj.Text);
                    CQuest q = itemParent.parent.getQuestOnQuestID(qID);
                    if (q != null)
                    {
                        lItems.Text += "Квест ID:" + q.QuestID.ToString() + "\n";
                        if (QuestItem.hasQuestItem(q.Reward.items))
                        {
                            lItems.Text += "\tНаграда:\n";
                            foreach (QuestItem item in q.Reward.items) 
                            {
                                if ((ItemAttribute)item.attribute == ItemAttribute.QUEST)
                                {
                                    lItems.Text += item.count.ToString() + " X " + itemParent.parent.itemConst.getItemName(item.itemType) + "\n";
                                }

                            }
                        }
                        if (QuestItem.hasQuestItem(q.QuestRules.items))
                        {
                            lItems.Text += "\tПравила квеста:\n";
                            foreach (QuestItem item in q.QuestRules.items)
                            {
                                if ((ItemAttribute)item.attribute == ItemAttribute.QUEST)
                                {
                                    lItems.Text += item.count.ToString() + " X " + itemParent.parent.itemConst.getItemName(item.itemType) + "\n";
                                }
                            }
                        }
                    }
                }
        }

        private void SelectQuestItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            itemParent.Enabled = true;
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            List<int> ret = new List<int>();
            foreach (CheckBox obj in splitContainer1.Panel1.Controls)
                if (obj.Checked)
                    ret.Add(int.Parse(obj.Text));

            //itemParent.quest.Target.AObjectAttrs = ret;
            itemParent.editTarget.AObjectAttrs = ret;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
