using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor.Forms
{
    public partial class QuestDialogFinderForm : Form
    {
        private MainForm parent;
        private int mode;


        Dictionary<int, List<int>> quest_to_stories = new Dictionary<int, List<int>>();
        string PATH = "../../../res/scripts/common/data/Stories.pyson";

        public QuestDialogFinderForm(MainForm parent, int mode)
        {
            InitializeComponent();
            this.parent = parent;
            this.mode = mode;
            if ((mode == 0) || (mode == 2))
                label1.Text = "Введите QuestID:";
            else if (mode == 1)
                label1.Text = "Введите ЗнаниеID:";
            if (mode == 2)
                parseStories();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (mode == 0)
                findQuests();
            else if (mode == 1)
                findKnowleges();
            else if (mode == 2)
                findQuestGet();
        }


        private void findKnowleges()
        {
            int knowlegeID = 0;
            List<int> checking = new List<int>();
            List<int> opening = new List<int>();
            if (!int.TryParse(textBox1.Text, out knowlegeID) || knowlegeID == 0)
            {
                return;
            }
            foreach (var npc in parent.dialogs.dialogs)
            {
                foreach (var dialog in npc.Value)
                {
                    if (dialog.Value.Precondition.knowledges.mustKnowledge.Contains(knowlegeID) || dialog.Value.Precondition.knowledges.shouldntKnowledge.Contains(knowlegeID))
                        checking.Add(dialog.Key);
                    if (dialog.Value.Actions.GetKnowleges.Contains(knowlegeID))
                        opening.Add(dialog.Key);
                }
            }
            onFoundKnowleges(checking, opening);
        }

        private void findQuests()
        {
            int questID = 0;
            if (!int.TryParse(textBox1.Text, out questID) || questID == 0)
            {
                onNotFoundQuest(questID);
                return;
            }
            CQuest quest = this.parent.getQuestOnQuestID(questID);
            if (quest == null)
            {
                //onNotFoundQuest(questID);
                //return;
            }
            List<int> checking = new List<int>();
            List<int> opening = new List<int>();
            List<int> closing = new List<int>();
            foreach(var npc in parent.dialogs.dialogs)
            {
                foreach (var dialog in npc.Value)
                {
                    if (dialog.Value.Precondition.ListOfMustNoQuests.hasQuest(questID) || dialog.Value.Precondition.ListOfNecessaryQuests.hasQuest(questID))
                        checking.Add(dialog.Key);
                    if (dialog.Value.Actions.CancelQuests.Contains(questID) || dialog.Value.Actions.FailQuests.Contains(questID) ||
                        dialog.Value.Actions.CompleteQuests.Contains(questID))
                        closing.Add(dialog.Key);
                    if (dialog.Value.Actions.GetQuests.Contains(questID))
                        opening.Add(dialog.Key);
                }
            }

            if(!checking.Any() && !opening.Any() && !closing.Any())
            {
                onNotFoundDialogs();
                return;
            }

            onFound(checking, opening, closing);
        }

        private void findQuestGet()
        {
            List<int> quests_r = new List<int>();
            List<int> quests_p = new List<int>();
            List<int> dialogs = new List<int>();
            int questID = 0;
            if (!int.TryParse(textBox1.Text, out questID) || questID == 0)
            {
                onNotFoundQuest(questID);
                return;
            }

            foreach (var npc in parent.dialogs.dialogs)
            {
                foreach (var dialog in npc.Value)
                {
                    if (dialog.Value.Actions.GetQuests.Contains(questID))
                        dialogs.Add(dialog.Key);
                }
            }

            foreach (KeyValuePair<int, CQuest> quest in parent.quests.quest)
            {
               foreach (var c_quest in quest.Value.Reward.ChangeQuests)
                {
                    if ((c_quest.Key == questID) && (c_quest.Value == 0)) quests_r.Add(quest.Key);
                }
                foreach (var c_quest in quest.Value.QuestPenalty.ChangeQuests)
                {
                    if ((c_quest.Key == questID) && (c_quest.Value == 0)) quests_p.Add(quest.Key);
                }
            }

            List<string> zones = parent.zoneConst.getAreasGiveQuestByID(questID);

            onFoundRecieving(questID, dialogs, quests_r, quests_p, zones);
        }

        private void onNotFoundQuest(int questID)
        {
            label2.Visible = true;
            label2.Text = "Квест ID:" + questID.ToString() + " не найден";
        }

        private void onNotFoundDialogs()
        {
            label2.Visible = true;
            label2.Text = "Диалоги не найдены";
        }

        private void onFoundKnowleges(List<int> checking, List<int> opening)
        {
            label2.Visible = false;
            treeView1.Nodes.Clear();
            if (checking.Any())
            {
                TreeNode node = new TreeNode("Проверяется в диалогах:");
                foreach (int questID in checking)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
            if (opening.Any())
            {
                TreeNode node = new TreeNode("Выдаётся в диалогах:");
                foreach (int questID in opening)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
        }

        private void onFoundRecieving(int quest, List<int> dialogs, List<int> quests_r, List<int> quests_p, List<string> zones)
        {
            label2.Visible = false;
            treeView1.Nodes.Clear();
            if (dialogs.Any())
            {
                TreeNode node = new TreeNode("Выдаётся в диалогах:");
                foreach (int questID in dialogs)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
            if (quests_r.Any())
            {
                TreeNode node = new TreeNode("Выдаётся в квестах как награда:");
                foreach (int questID in quests_r)
                {
                    node.Nodes.Add("quest", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
            if (quests_p.Any())
            {
                TreeNode node = new TreeNode("Выдаётся в квестах как штраф:");
                foreach (int questID in quests_p)
                {
                    node.Nodes.Add("quest", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
            if (zones.Any())
            {
                TreeNode node = new TreeNode("Выдаётся в зонах на карте:");
                foreach (string zone in zones)
                {
                    node.Nodes.Add("space", zone);
                }
                treeView1.Nodes.Add(node);
            }

            if (quest_to_stories.ContainsKey(quest))
            {
                TreeNode node = new TreeNode("Выдаётся в записках:");
                foreach (int storiesID in quest_to_stories[quest])
                {
                    node.Nodes.Add("story", storiesID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
        }

        private void onFound(List<int> checking, List<int> opening, List<int> closing)
        {
            label2.Visible = false;
            treeView1.Nodes.Clear();
            if (checking.Any())
            {
                TreeNode node = new TreeNode("Проверяется в диалогах:");
                foreach(int questID in checking)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }
            if (opening.Any())
            {
                TreeNode node = new TreeNode("Открывается в диалогах:");
                foreach (int questID in opening)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }

            if (closing.Any())
            {
                TreeNode node = new TreeNode("Закрывается в диалогах:");
                foreach (int questID in closing)
                {
                    node.Nodes.Add("dialog", questID.ToString());
                }
                treeView1.Nodes.Add(node);
            }

        }

        private void addStories(int questID, int storiesID)
        {
            if (!quest_to_stories.ContainsKey(questID))
                quest_to_stories.Add(questID, new List<int>());
  
            quest_to_stories[questID].Add(storiesID);
        }

        private void parseStories()
        {
            if (!File.Exists(PATH))
                return;

            string line;
            StreamReader reader = new StreamReader(PATH);
            int id = 0;
            int quest_id = 0;
            int story_id = 0;


            while ((line = reader.ReadLine()) != null)
            {

                if (!line.Any()) continue;
                string tmp_line = line.Trim();
                tmp_line = tmp_line.Replace(":", "");
                if (!tmp_line.Any()) continue;

                if (int.TryParse(tmp_line, out id))
                {
                    story_id = id;
                    continue;
                }
                if (line.Contains("OpenQuest"))
                    {
                        tmp_line = line.Split(':').Last().Replace("\"", "");
                        if (int.TryParse(tmp_line.Trim(), out quest_id))
                        {
                            addStories(quest_id, story_id);
                        }
                    }
            }
            reader.Close();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int dialogID;
            int.TryParse(e.Node.Text, out dialogID);
            if (e.Node.Name == "dialog")
                parent.findDialogByID(dialogID);
            else if (e.Node.Name == "quest")
                parent.selectQuestByID(dialogID);
            else if (e.Node.Name == "space")
            {
                string space = e.Node.Text.Split(' ')[0];
                Clipboard.SetText(e.Node.Text.Remove(0, space.Length + 1));
                MessageBox.Show("Координаты скопированы в буффер обмена");
            };
        }
    }
}
