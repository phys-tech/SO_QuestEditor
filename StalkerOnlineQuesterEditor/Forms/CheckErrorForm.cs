using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace StalkerOnlineQuesterEditor.Forms
{
    public partial class CheckErrorForm : Form
    {
        public class QuestError
        {
            public int error_type { get; set; }
            public string text { get; set; }
            public int quest_id { get; set; }

            public QuestError(int err_type, string err_text, int quest_id = 0)
            {
                this.error_type = error_type;
                this.text = err_text;
                this.quest_id = quest_id;
            }

            public override string ToString()
            {
                return text;
            }
        }
    
        protected CDialogs dialogs;
        protected CQuests quests;
        private MainForm parent;
        private List<int> deleted_quests_ids = new List<int>();

        protected string targetNPCName = "";

        List<QuestError> currentDisplayErrors = new List<QuestError>();

        string JSON_PATH = "settings/ignore_ids.json";
        string DELETED_PATH = "../../../res/scripts/common/QuestsToRemove.py";
        private Dictionary<string, List<int>> ignores_id = new Dictionary<string, List<int>>();
        private Dictionary<int, List<KeyValuePair<int, string>>> errors = new Dictionary<int, List<KeyValuePair<int, string>>>();

        const int ERROR_ITEM = 0;
        const int ERROR_QUEST = 1;
        const int ERROR_QUEST_TYPE5 = 2;
        const int ERROR_OTHER = 3;
        const int ERROR_NO_ROOT = 4;
        const int ERROR_DIALOG = 5;

        public CheckErrorForm(CDialogs dialogs, CQuests quests, MainForm parent, string targetNPCName = "")
        {
            InitializeComponent();
            this.dialogs = dialogs;
            this.quests = quests;
            this.parent = parent;
            this.targetNPCName = targetNPCName;
            readIgnoresIDs();
            readDeletedQuests();
            if (targetNPCName.Any())
            {
                this.Text += " NPC:" + targetNPCName;
                btnStart_Click(null, null);

            }
            //lbLog.DataSource = lbLog.Items;
            //lbLog.DisplayMember = "text";
        }


        private void readDeletedQuests()
        {
             if (!File.Exists(DELETED_PATH))
                return;

            string line;
            StreamReader reader = new StreamReader(DELETED_PATH);
            while((line = reader.ReadLine()) != null)
            {

                if((!line.Any()) || (line[0] == '#')) continue;
                string[] lines;
                lines = line.Split(',');
                foreach (string _num in lines)
                {
                    int i = 0;
                    int.TryParse(_num, out i);
                    if (i != 0) deleted_quests_ids.Add(i);
                }
            }
            reader.Close();
        }

        private void readIgnoresIDs()
        {
            JsonTextReader reader;
            if (!File.Exists(JSON_PATH))
                return;

            bool dialogs = false;
 
            reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            while (reader.Read())
            {
                if ((reader.TokenType == JsonToken.PropertyName) && (reader.Value.ToString() != "quests"))
                    dialogs = true;
                else if (reader.TokenType == JsonToken.Integer)
                {
                    if (dialogs)
                    {
                        if (!ignores_id.ContainsKey("dialogs"))
                            ignores_id.Add("dialogs", new List<int>());
                        ignores_id["dialogs"].Add(Convert.ToInt32(reader.ReadAsInt32()));
                    }
                    else
                    {
                        if (!ignores_id.ContainsKey("quests"))
                            ignores_id.Add("quests", new List<int>());
                        ignores_id["quests"].Add(Convert.ToInt32(reader.Value));
                    }
                }

            }
            reader.Close();
        }

        private void saveIgnoresIDs()
        {
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(JSON_PATH)))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                foreach(KeyValuePair<string, List<int>> val in ignores_id)
                {
                    writer.WritePropertyName(val.Key);
                    writer.WriteStartArray();
                    foreach (int id in val.Value)
                    {
                        writer.WriteValue(id);
                    }
                    writer.WriteEnd();
                }
                writer.WriteEndObject();
            }
        }

        private bool wasIgnoredQuestID(int quest_id)
        {
            if (!ignores_id.ContainsKey("quests"))
                return false;

            return ignores_id["quests"].Contains(quest_id);
        }

        private bool checkQuestIDOnGet(int quest_id, string npc_name, string dialog_id)
        {
            CQuest quest = quests.getQuest(quest_id);
            bool debug = quest_id == 12359;
            if (quest == null)
            {
                string line = "NPC:" + npc_name + "\t\tДиалогID: " + dialog_id + "\t\tКвест №" + quest_id.ToString() + " не существует, а проверяется";
                this.writeToLog(ERROR_OTHER, line);
                return true;
            }
            if (quest.Additional.IsSubQuest != 0)
            {
                return checkQuestIDOnGet(quest.Additional.IsSubQuest, npc_name, dialog_id);
            }
            if (quest.questLinkType == 1)
            {
                return true;
            }
            foreach (KeyValuePair<string, Dictionary<int, CDialog>> npc in dialogs.dialogs)
            {
                foreach (CDialog dia in npc.Value.Values)
                {
                    if (dia.Actions.GetQuests.Contains(quest_id))
                    {
                        return true;
                    }
                }
            }
            foreach (KeyValuePair<int, CQuest> item in quests.quest)
            {
                foreach (KeyValuePair<int, int> change_quest in item.Value.QuestPenalty.ChangeQuests)
                {
                    if (change_quest.Value != 0) continue;
                    if (change_quest.Key == quest_id) return true;
                }
                foreach (KeyValuePair<int, int> change_quest in item.Value.Reward.ChangeQuests)
                {
                    if (change_quest.Value != 0) continue;
                    if (change_quest.Key == quest_id) return true;
                }
            }
            

            if ((parent != null) && parent.zoneConst.checkAreaGiveQuestByID(quest_id))
                return true;
            if ((parent != null) && parent.massQuestRewards.getKeys().Contains(quest_id))
                return true;
            if ((parent != null) && parent.billboardQuests.getKeys().Contains(quest_id))
                return true;
            return false; 
        }

        private bool find_quest_space(CQuest quest)
        {
            if (quest == null) return false;
            if (quest.QuestRules.space != 0) return true;
            if (quest.Additional.IsSubQuest == 0)
                return false;
            CQuest q = parent.getQuestOnQuestID(quest.Additional.IsSubQuest);
            return find_quest_space(q);
        }

        private bool find_quest_type100(int quest_id)
        {
            if (quest_id == 0) return false;
            CQuest q = parent.getQuestOnQuestID(quest_id);
            if (q.Target.QuestType == 100)
                return true;
            return find_quest_type100(q.Additional.IsSubQuest);
        }

        private int get_parent_quest(int quest_id)
        {
            CQuest q = parent.getQuestOnQuestID(quest_id);
            if (q.Additional.IsSubQuest == 0) return quest_id;
            return get_parent_quest(q.Additional.IsSubQuest);
        }



        delegate void WriteToLogDelegate(int error_type, string message, int quest_id = 0);

        private void writeToLog(int error_type, string text, int quest_id = 0)
        {
            if (quest_id > 0 && error_type == ERROR_QUEST)
            {
                CQuest q = parent.getQuestOnQuestID(quest_id);
                if (q != null)
                {
                    CQuest q_p = parent.getQuestOnQuestID(get_parent_quest(quest_id));
                    if (q_p != null && q_p.isOld)
                        error_type = ERROR_QUEST_TYPE5;
                }
            }
            if (lbLog.InvokeRequired)
            {
                var _writeToLog = new WriteToLogDelegate(writeToLog);
                lbLog.Invoke(_writeToLog, error_type, text, quest_id);
            }
            else
            {
                if (!errors.ContainsKey(error_type)) errors.Add(error_type, new List<KeyValuePair<int, string>>());
                errors[error_type].Add(new KeyValuePair<int, string>(quest_id, text));
                if (selectedTypes().Contains(error_type))
                    lbLog.Items.Add(new QuestError(error_type, text, quest_id));   
            }
        }

        delegate void doProgressDelegate(int value);
        private void doProgress(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                var _writeToLog = new doProgressDelegate(doProgress);
                progressBar1.Invoke(_writeToLog, value);
            }
            else
            {
                progressBar1.Value = value; ;
            }
        }

        delegate void setLabelDelegate(string value);
        private void setLabel(string value)
        {
            if (lCurrentCheck.InvokeRequired)
            {
                var _writeToLog = new setLabelDelegate(setLabel);
                lCurrentCheck.Invoke(_writeToLog, value);
            }
            else
            {
                lCurrentCheck.Text = value;
            }
        }

        private void checkErrors()
        {
            int[] quest_types = new int[] { 0, 13, 14, 22, 2, 5 };
            int count = dialogs.dialogs.Count;
            int i = 0;
            errors.Clear();
            doProgress(25);
            setLabel("Проверяем квесты в диалогах");

            Dictionary<string, int> triggers = new Dictionary<string, int>();
            Dictionary<string, int> zone_areas = new Dictionary<string, int>();
            List<int> change_money_dialogs = new List<int>();
            List<int> on_test_list = new List<int>();
            foreach (KeyValuePair<string, Dictionary<int, CDialog>> npc in dialogs.dialogs)
            {
                i++;
                doProgress(50 * Convert.ToInt32(Convert.ToDouble(i) / count));
                if (targetNPCName.Any() && targetNPCName != npc.Key) continue;
                foreach (KeyValuePair<int, CDialog> dia in npc.Value)
                {
                    List<int> check_quest_list = new List<int>();
                    List<int> check_dialog_list = new List<int>();

                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfHaveQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfCompletedQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfOpenedQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfFailQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfOnTestQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfMustNoQuests.ListOfCompletedQuests);

                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfHaveQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfFailQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests);
                    check_quest_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests);

                    check_dialog_list.AddRange(dia.Value.CheckNodes);

                    on_test_list.AddRange(dia.Value.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests);

                    if (dia.Value.Actions.changeMoney != 0)
                    {
                        change_money_dialogs.Add(dia.Key);
                    }
                    
                    foreach(int dialog_id in check_dialog_list)
                    {
                        bool dialog_exist = false;
                        foreach (var _npc in dialogs.dialogs)
                        {
                            if (!_npc.Value.ContainsKey(dialog_id)) continue;
                            dialog_exist = true;
                            break;
                        }
                        if (!dialog_exist)
                        {
                            string line = "Диалог №:" + dialog_id.ToString() + "\tсодержит предпроверку от диалога №" + dia.Key.ToString();
                            this.writeToLog(ERROR_DIALOG, line); this.writeToLog(ERROR_DIALOG, line, dialog_id);
                        }
                    }
                    //dialogs.locales

                    foreach (int quest_id in check_quest_list)
                    {
                        if (parent.getQuestOnQuestID(quest_id) == null)
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + quest_id.ToString() + " не существует";
                            this.writeToLog(ERROR_QUEST, line, quest_id);
                            continue;
                        };
                        if (wasIgnoredQuestID(quest_id))
                            continue;
                        if (deleted_quests_ids.Contains(quest_id))
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + quest_id.ToString() + " находится в списке автозавершения";
                            this.writeToLog(ERROR_QUEST, line, quest_id);
                        }
                        else if(deleted_quests_ids.Contains(get_parent_quest(quest_id)))
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + quest_id.ToString() + " его родитель находится в списке автозавершения";
                            this.writeToLog(ERROR_QUEST, line, quest_id);
                        }

                        if (!checkQuestIDOnGet(quest_id, npc.Key, dia.Key.ToString()) && !checkQuestIDOnGet(get_parent_quest(quest_id), npc.Key, dia.Key.ToString()))
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + quest_id.ToString() + " не выдаётся";
                            //Thread.Sleep(100);
                            this.writeToLog(ERROR_QUEST, line, quest_id);
                        }
                    }
                    foreach (int complete_quest_id in dia.Value.Actions.CompleteQuests)
                    {
                        CQuest complete_quest = parent.getQuestOnQuestID(complete_quest_id);
                        if (complete_quest == null)
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + complete_quest_id + " выдаётся и не существует";
                            this.writeToLog(ERROR_QUEST, line, complete_quest_id);
                            continue;
                        }
                        if (complete_quest.Reward.Any())
                        {
                            if (dia.Value.Actions.GetQuests.Contains(complete_quest_id) && !dia.Value.Actions.Exit)
                            {
                                string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + complete_quest_id + " даётся и завершается в одном ноде диалога";
                                this.writeToLog(ERROR_QUEST, line, complete_quest_id);
                            }

                            if ((complete_quest.Additional.IsSubQuest != 0) && (dia.Value.Actions.GetQuests.Contains(complete_quest.Additional.IsSubQuest)))
                            {
                                int parent_id = complete_quest.Additional.IsSubQuest;
                                CQuest parent_complete_quest = parent.getQuestOnQuestID(parent_id);
                                if (parent_complete_quest.Additional.ListOfSubQuest.Count == 1)
                                {
                                    string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tКвест №" + parent_id + " даётся, а его единственное событие " + complete_quest_id + " завершается";
                                    this.writeToLog(ERROR_QUEST, line, complete_quest_id);
                                }
                            }
                        }
                    }

                    
                    List<DialogEffect> check_eff_list = new List<DialogEffect>();
                    check_eff_list.AddRange(dia.Value.Precondition.NecessaryEffects);
                    check_eff_list.AddRange(dia.Value.Precondition.MustNoEffects);

                    foreach (DialogEffect effect in check_eff_list)
                    {
                        if (!this.parent.effects.hasEffectById(effect.getID()))
                        {
                            string line = "NPC:" + npc.Key + "\t\tДиалогID: " + dia.Key.ToString() + "\t\tЭффекта №" + effect.getID() + " нет, а проверяется";
                            //Thread.Sleep(100);
                            this.writeToLog(ERROR_OTHER, line);
                        }
                    }

                }

            }


            doProgress(40);
            setLabel("Проверяем локализацию диалогов");
            foreach (string local in dialogs.locales.Keys)
                foreach (string _npc_name in dialogs.locales[local].Keys)
                {
                    if (targetNPCName.Any() && targetNPCName != _npc_name) continue;
                    foreach (KeyValuePair<int, CDialog> _dialog in dialogs.locales[local][_npc_name])
                    {
                        string dialog_text = _dialog.Value.Text;
                        if (dialog_text.Contains(":MONEY_COST%"))
                        {
                            Regex regex = new Regex("%D:(.*?):MONEY_COST%");
                            Match match = regex.Match(dialog_text);
                            while (match.Success)
                            {
                                int cm_dialog_id = int.Parse(match.Groups[1].Value);
                                if (!change_money_dialogs.Contains(cm_dialog_id))
                                {
                                    string line = "Диалог №:" + cm_dialog_id.ToString() + "\tне изменяет денег, при наличии указателя в тексте диалога №" + _dialog.Key.ToString();
                                    this.writeToLog(ERROR_DIALOG, line);
                                }
                                match = match.NextMatch();
                            }
                        }
                    }
                }

            count = quests.quest.Count;
            i = 0;
            doProgress(50);
            setLabel("Проверяем квесты");
            Dictionary<int, CItem> items = this.parent.itemConst.getAllItems();
            foreach (KeyValuePair<int, CQuest> quest in quests.quest)
            {
                i++;
                doProgress(50 + 40 * Convert.ToInt32(Convert.ToDouble(i) / count));
                if (wasIgnoredQuestID(quest.Key))
                    continue;
                if (targetNPCName.Any() && targetNPCName != quest.Value.Additional.Holder) continue;
                int parent_quest_id = quest.Value.Additional.IsSubQuest;
                //проверяет, знает ли родительский квест о текущем
                if (parent_quest_id > 0)
                {
                    if (!quests.quest.ContainsKey(parent_quest_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tявляется подквестом несуществущего квеста №" + parent_quest_id.ToString();
                        this.writeToLog(ERROR_NO_ROOT, line, quest.Key);
                    }
                    else
                    {
                        CQuest parent_quest = quests.quest[parent_quest_id];
                        bool is_realy_subquest = false;
                        foreach (int child_quest_id in parent_quest.Additional.ListOfSubQuest)
                        {
                            if (child_quest_id == quest.Key)
                            {
                                is_realy_subquest = true;
                                break;
                            }
                        }
                        if (!is_realy_subquest)
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tявляется подквестом квеста №" + parent_quest_id.ToString() + "\tно у квеста нет такого подквеста";
                            this.writeToLog(ERROR_NO_ROOT, line, quest.Key);
                        }
                    }
                }
                //порверка обратная той, что выше
                if (quest.Value.Additional.ListOfSubQuest.Any())
                {
                    foreach(int child_quest_id in quest.Value.Additional.ListOfSubQuest)
                    {
                        if (!quests.quest.ContainsKey(child_quest_id))
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет несуществущий подквест №" + child_quest_id.ToString();
                            this.writeToLog(ERROR_NO_ROOT, line, quest.Key);
                            continue;
                        }
                        CQuest child_quest = quests.quest[child_quest_id];
                        if (child_quest.Additional.IsSubQuest != quest.Key)
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет подквест №" + parent_quest_id.ToString() + "\tно у подквеста родитель №"+ child_quest.Additional.IsSubQuest.ToString();
                            this.writeToLog(ERROR_NO_ROOT, line, quest.Key);
                            continue;
                        }
                    }
                }

                if ((quest.Value.Priority > 0) && ((quest.Value.Additional.ShowProgress & 4) > 0))
                {
                    if(!find_quest_space(quest.Value))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tне настроена локация";
                        this.writeToLog(ERROR_NO_ROOT, line, quest.Key);
                    }

                }
                
                if (quest.Value.Precondition.omniCounter)
                {
                    if (!QuestsOmnicounter.isOmni(quest.Key))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tявляется omnicounter, но его нет в файле omnicounter.json";
                        this.writeToLog(ERROR_QUEST_TYPE5, line, quest.Key);
                    }
                }
                if (quest_types.Contains(quest.Value.Target.QuestType))
                {
                    if (!on_test_list.Contains(quest.Key) && (!deleted_quests_ids.Contains(quest.Key) || !deleted_quests_ids.Contains(get_parent_quest(quest.Key))) && 
                        !find_quest_type100(quest.Value.Additional.IsSubQuest))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tимеет тип: \"" + this.parent.questConst.getDescription(quest.Value.Target.QuestType) + "\" и нигде не проверяется";
                        if (quest.Value.Target.QuestType == CQuestConstants.TYPE_MONEYBACK)
                        {
                            this.writeToLog(ERROR_QUEST_TYPE5, line, quest.Key);
                        }
                        else if (quest.Value.Target.QuestType == CQuestConstants.TYPE_FARM)
                        {
                            if (quest.Value.Target.additional != "1")
                                this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                        else
                        {
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                    }
                }
                if ((quest.Value.Target.QuestType == CQuestConstants.TYPE_FARM) || (quest.Value.Target.QuestType == CQuestConstants.TYPE_FARM_AUTO) || (quest.Value.Target.QuestType == CQuestConstants.TYPE_QITEM_USE))
                {
                    int item_id = quest.Value.Target.ObjectType;
                    if ((item_id != 0) && (!items.ContainsKey(item_id)))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\t\t\tпредмета цели type:" + item_id + " не существует";
                        this.writeToLog(ERROR_QUEST ,line, quest.Key);
                        continue;
                    }
                }
                else if ((quest.Value.Target.QuestType == CQuestConstants.TYPE_AREA_DISCOVER) || 
                            (quest.Value.Target.QuestType == CQuestConstants.TYPE_AREA_LEAVE) ||
                            (quest.Value.Target.QuestType == CQuestConstants.TYPE_IN_AREA))
                {
                    string zone = quest.Value.Target.ObjectName;
                    if (zone_areas.ContainsKey(zone))
                    {
                        string line = "Квесты №:" + quest.Key.ToString() + " и " + zone_areas[zone] + "\tимеет в целях одинаковую зону: \"" + zone + "\"";
                        this.writeToLog(ERROR_QUEST_TYPE5, line, quest.Key);
                    }
                    else
                    {
                        zone_areas.Add(zone, quest.Key);
                    }
                        
                    if (zone.Any())
                        if (!this.parent.zoneConst.checkHaveArea(zone))
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет в целях зону: \"" + quest.Value.Target.ObjectName + "\", которой нигде нет";
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                }
                else if (quest.Value.Target.QuestType == CQuestConstants.TYPE_TRIGGER_ACTION)
                {
                    string[] _triggers;
                    if (quest.Value.Target.ObjectType > 0)
                        _triggers = new string[] { quest.Value.Target.ObjectType.ToString().Trim() };
                    else
                        _triggers = quest.Value.Target.ObjectName.Split(',');

                    foreach (var trigger_name in _triggers)
                    {
                        if (triggers.ContainsKey(trigger_name))
                        {
                            string line = "Квесты №:" + quest.Key.ToString() + " и " + triggers[trigger_name] + "\tимеет в целях одинаковый триггер: \"" + trigger_name + "\"";
                            this.writeToLog(ERROR_QUEST_TYPE5, line, quest.Key);

                        }
                        else
                            triggers.Add(trigger_name, quest.Key);
                        if (!trigger_name.Any())
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tНЕ имеет в целях триггер";
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                        else if (parent.triggerConst.getDescriptionOnId(Convert.ToInt32(trigger_name)) == "")
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет в целях триггер: \"" + trigger_name + "\", которого нет";
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                    }
                   
                }
                else if ((quest.Value.Target.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST) || (quest.Value.Target.QuestType == CQuestConstants.TYPE_KILLMOBS))
                {
                    if (quest.Value.Target.AreaName.Any())
                        if (!this.parent.zoneConst.checkHaveArea(quest.Value.Target.AreaName))
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет зону: \"" + quest.Value.Target.AreaName + "\", которой нигде нет";
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                    if (quest.Value.Target.str_param2.Any())
                        if (!this.parent.zoneMobConst.checkHaveArea(quest.Value.Target.str_param2))
                        {
                            string line = "Квест №:" + quest.Key.ToString() + "\tимеет зону: \"" + quest.Value.Target.str_param2 + "\", которой нигде нет";
                            this.writeToLog(ERROR_QUEST, line, quest.Key);
                        }
                }

                foreach (QuestItem item in quest.Value.QuestRules.items)
                {
                    int item_id = item.itemType;
                    if (item_id == 0) continue;
                    if (!items.ContainsKey(item_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета условия type:" + item_id + " не существует";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }

                    if (items[item_id].deleted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмет условия type:" + item_id + " помечен на удаление";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                    }

                    if (items[item_id].converted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета условия type:" + item_id + " помечен на замену";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                    }
                }
                foreach (int quest_id in quest.Value.Reward.ChangeQuests.Keys)
                {
                    if (!quests.quest.Keys.Contains(quest_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tв награде имеет несуществующий квест №" + quest_id.ToString();
                        this.writeToLog(ERROR_QUEST, line, quest.Key);
                        continue;
                    }
                }
                foreach (int quest_id in quest.Value.QuestPenalty.ChangeQuests.Keys)
                {
                    if (!quests.quest.Keys.Contains(quest_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tв штрафах имеет несуществующий квест №" + quest_id.ToString();
                        this.writeToLog(ERROR_QUEST, line, quest.Key);
                        continue;
                    }
                }
                foreach (QuestItem item in quest.Value.QuestPenalty.items)
                {
                    if (item.itemType == 0) continue;
                    int item_id = item.itemType;
                    if (!items.ContainsKey(item_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета штрафа type:" + item_id + " не существует";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }

                    if (items[item_id].deleted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмет штрафа type:" + item_id + " помечен на удаление";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }

                    if (items[item_id].converted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета штрафа type:" + item_id + " помечен на замену";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }
                }
                foreach (QuestItem item in quest.Value.Reward.items)
                {
                    if (item.itemType == 0) continue;
                    int item_id = item.itemType;
                    if (!items.ContainsKey(item_id))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета награды type:" + item_id + " не существует";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }

                    if (items[item_id].deleted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмет награды type:" + item_id + " помечен на удаление";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }

                    if (items[item_id].converted)
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tпредмета награды type:" + item_id + " помечен на замену";
                        this.writeToLog(ERROR_ITEM, line, quest.Key);
                        continue;
                    }
                }
                foreach (CEffect effect in quest.Value.Reward.Effects)
                {
                    if (!this.parent.effects.hasEffectById(effect.getID()))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tЭффекта №" + effect.getID() + " нет, а выдаётся как награда";
                        this.writeToLog(ERROR_QUEST, line, quest.Key);
                    }
                }
                foreach (CEffect effect in quest.Value.QuestPenalty.Effects)
                {
                    if (!this.parent.effects.hasEffectById(effect.getID()))
                    {
                        string line = "Квест №:" + quest.Key.ToString() + "\tЭффекта №" + effect.getID() + " нет, а выдаётся как штраф";
                        this.writeToLog(ERROR_QUEST, line, quest.Key);
                    }
                }
            }
            doProgress(95);
            setLabel("Записываю в файл");
            saveLogToFile();
            setLabel("Готово");
            doProgress(100);

        }

        delegate void saveLogDelegate();
        private void saveLogToFile()
        {
            if (lbLog.InvokeRequired)
            {
                var _writeToLog = new saveLogDelegate(saveLogToFile);
                lbLog.Invoke(_writeToLog);
            }
            else
            {
                if (errors.Count == 0)
                {
                    bpNoErrors.Visible = true;
                    MessageBox.Show("Ты молодец, ошибок нет, Стас");  
                }
                StreamWriter writer = new StreamWriter("CheckErrorLog.txt");
                foreach (QuestError err in lbLog.Items)
                {
                    writer.WriteLine(err.text);
                }
                writer.Close();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //step 1:
            bpNoErrors.Visible = false;
            progressBar1.Visible = true;
            lbLog.Items.Clear();
            var thread = new Thread(checkErrors);
            thread.IsBackground = true;
            thread.Start();
           // MessageBox.Show(DateTime.Now.ToString() + " " + lbLog.Items.Count.ToString());
           // progressBar1.Visible = false;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbLog.SelectedItem == null)
            {
                MessageBox.Show("Для удаления выбери элемент, Стас", "Ты допустил ошибку, не надо больше так делать");
                return;
            }
            
            int index = (lbLog.SelectedItem as QuestError).quest_id;
            if (index > 0)
            {
                if (!ignores_id.ContainsKey("quests"))
                    ignores_id.Add("quests", new List<int>());
                ignores_id["quests"].Add(index);
                this.saveIgnoresIDs();
            }
            lbLog.Items.Remove((lbLog.SelectedItem as QuestError));
        }

        private List<int> selectedTypes()
        {
            List<int> result = new List<int>();
            if (cbErrorItem.Checked) result.Add(ERROR_ITEM);
            if (cbErrorQuest.Checked) result.Add(ERROR_QUEST);
            if (cbErrQuest5.Checked) result.Add(ERROR_QUEST_TYPE5);
            if (cbErrDialogs.Checked) result.Add(ERROR_DIALOG);
            if (cbErrOther.Checked) result.Add(ERROR_OTHER);
            if (cbErrNoRoot.Checked) result.Add(ERROR_NO_ROOT);
            
            return result;
        }

        private void updateErrorLog()
        {
            lbLog.Items.Clear();
            List<int> types = selectedTypes();
            foreach(int key in types)
            {
                if (!errors.ContainsKey(key)) continue;
                List<KeyValuePair<int, string>> err_list = errors[key];
                foreach(KeyValuePair<int, string> line in err_list)
                {
                    lbLog.Items.Add(new QuestError(key, line.Value, line.Key));
                }
            }
        }

        private void cbError_CheckedChanged(object sender, EventArgs e)
        {
            updateErrorLog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lbLog.SelectedItem == null)
            {
                MessageBox.Show("Для копирования выбери элемент, Стас", "Ты допустил ошибку, не надо больше так делать");
                return;
            }

            int index = (lbLog.SelectedItem as QuestError).quest_id;
            Clipboard.SetText(index.ToString());
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (lbLog.SelectedItem == null)
            {
                MessageBox.Show("Перехода выбери элемент, Стас", "Ты допустил ошибку, не надо больше так делать");
                return;
            }
            int index = (lbLog.SelectedItem as QuestError).quest_id;
            if (index == 0)
            {
                MessageBox.Show("Это не квест, Стас. Это работает только на ошибках, где есть квест", "Ты допустил ошибку, не надо больше так делать");
                return;
            }
            parent.selectQuestByID(index);
        }
    }
}
