using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Net;

namespace StalkerOnlineQuesterEditor
{
    //! Словарь <QuestID, CQuest>
    using NPCQuestDict = Dictionary<int, CQuest>;
    //! Словарь локализаций квестов <LocaleName, <QuestID, CQuest>>
    using QuestLocales = Dictionary<string, Dictionary<int, CQuest>>;
    //! Словарь из разностей версий диалогов русской версии и локализации
    using DifferenceDict = Dictionary<string, Dictionary<int, CDifference>>;
    //! Словарь изменённых квестов (0 - открыто, 1 - закрыто, 2 - провалено, 3 - отменено)
    using ChangedQuests = Dictionary<int, List<int>>;


    //! Класс обработки квестов
    public class CQuests
    {
        public int SHOW_MESSAGE_TAKE = 1;
        public int SHOW_MESSAGE_CLOSE = 2;
        public int SHOW_MESSAGE_PROGRESS = 4;
        public int SHOW_JOURNAL = 8;

        public int last_quest_id = 0;

        public static Dictionary<int, ChangedQuests> QuestParentList = new Dictionary<int, ChangedQuests>();
        //public Dictionary<string, NPCQuestDict> quest;
        //! Словарь <QuestID, CQuest> - основной, содержащий все квесты
        public NPCQuestDict quest;
        public QuestLocales locales = new QuestLocales();
        public NPCQuestDict m_Buffer = new NPCQuestDict();
        public Dictionary<string, NPCQuestDict> m_LocBuffer = new Dictionary<string, NPCQuestDict>();
        public List<int> deletedQuests = new List<int>();
        XDocument doc = new XDocument();
        MainForm parent;

        public int bufferTop = 0;
        public bool CutQuests = false;

        List<FileStream> fs_list = new List<FileStream>();
        List<string> lock_paths = new List<string>();

        //! Конструктор, заполняет словарь quest, парсит файлы
        public CQuests(MainForm form)
        {
            parent = form;
            this.quest = new NPCQuestDict();
            ParseLastQuestID(CSettings.GetLastQuestIDPath());
            ParseQuestsData(CSettings.GetQuestDataPath(), quest);
            ParseQuestsTexts(CSettings.GetQuestTextPath(), quest);
            ParseDeletedQuest(CSettings.GetDeletedQuestsPath(), deletedQuests);

            foreach (var locale in CSettings.getListLocales())
            {
                if (!locales.Keys.Contains(locale))
                {
                    locales.Add(locale, new NPCQuestDict(quest.ToDictionary(entry => entry.Key, entry => (CQuest)entry.Value.Clone())));
                }

                //ParseQuestsData(CSettings.GetQuestDataPath(), this.locales[locale]);
                ParseQuestsTexts(CSettings.GetQuestLocaleTextPath(locale), this.locales[locale]);
            }
            lock_files();
        }

        void ParseDeletedQuest(string sPath, List<int> list_target)
        {
            if (!File.Exists(sPath))
                return;
            System.IO.StreamReader fileReader = new StreamReader(sPath);
            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Any())
                    continue;
                list_target.Add(Convert.ToInt32(line));
            }
            fileReader.Close();
        }

        public void addDeletedQuests(int questID)
        {
            deletedQuests.Add(questID);
            string path = CSettings.GetDeletedQuestsPath();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true);
            writer.WriteLine(questID.ToString());
            writer.Close();
        }

        void ParseLastQuestID(string sPath)
        {
            if (!File.Exists(sPath))
                return;

            System.IO.StreamReader fileReader = new StreamReader(sPath);
            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Any())
                    continue;
                last_quest_id = Convert.ToInt32(line);
            }
            fileReader.Close();
        }

        void ParseQuestsData(string sPath, NPCQuestDict dict_target)
        {
            doc = XDocument.Load(sPath);
            lock_paths.Add(sPath);

            foreach (XElement item in doc.Root.Elements())
            {
                int QuestID = int.Parse(item.Element("ID").Value);
                if (QuestID >= last_quest_id)
                    last_quest_id = QuestID + 1;
                CQuestInformation information = new CQuestInformation();
                CQuestTarget target = new CQuestTarget();
                CQuestPrecondition precondition = new CQuestPrecondition();
                CQuestRules questRules = new CQuestRules();
                CQuestReward reward = new CQuestReward();
                CQuestAdditional additional = new CQuestAdditional();
                CQuestReward penalty = new CQuestReward();
                CQuestAdditionalConditions conditions = new CQuestAdditionalConditions();
                bool hidden = false;
                if (item.Element("hidden") != null)
                    hidden = true;

                int priority = 0;
                if (item.Element("Priority") != null)
                {
                    priority = int.Parse(item.Element("Priority").Value);
                }
                int level = 0;
                bool isOld = false;
                if (item.Element("Level") != null)
                {
                    level = int.Parse(item.Element("Level").Value);
                }

                if (item.Element("isOld") != null)
                {
                    isOld = true;
                }

                int questLinkType = 0;
                int questLink = 0;

                if (item.Element("questLinkType") != null)
                {
                    questLinkType = int.Parse(item.Element("questLinkType").Value);
                }
                if (item.Element("questLink") != null)
                {
                    questLink = int.Parse(item.Element("questLink").Value);
                }

                additional.Holder = item.Element("Additional").Element("Holder").Value.Trim();

                if (!parent.dialogs.dialogs.ContainsKey(additional.Holder))
                {
                    Console.WriteLine("ЛИШНИЕ NPC " + additional.Holder);
                    continue;
                }


                if (item.Element("Target") != null)
                {
                    if (item.Element("Target").Element("QuestType") != null)
                        target.QuestType = int.Parse(item.Element("Target").Element("QuestType").Value);
                    if ((item.Element("Target").Element("ObjectType") != null) && (!item.Element("Target").Element("ObjectType").Value.Equals("")))
                        target.ObjectType = int.Parse(item.Element("Target").Element("ObjectType").Value);
                    if ((item.Element("Target").Element("NumOfObjects") != null) && (!item.Element("Target").Element("NumOfObjects").Value.Equals("")))
                        target.NumOfObjects = int.Parse(item.Element("Target").Element("NumOfObjects").Value);
                    if ((item.Element("Target").Element("ObjectAttr") != null) && (!item.Element("Target").Element("ObjectAttr").Value.Equals("")))
                    {
                        int Target_ObjectAttr = int.Parse(item.Element("Target").Element("ObjectAttr").Value);
                        if (Target_ObjectAttr < 0)
                            Target_ObjectAttr = 0;
                        target.ObjectAttr = Target_ObjectAttr;
                        if (target.QuestType == CQuestConstants.TYPE_KILLMOBS || target.QuestType == CQuestConstants.TYPE_KILLMOBS_WITH_ONTEST)
                        {
                            target.str_param = target.ObjectAttr.ToString();
                        }
                    }

                    if ((item.Element("Target").Element("str_param") != null) && (!item.Element("Target").Element("str_param").Value.Equals("")))
                        target.str_param = item.Element("Target").Element("str_param").Value;
                    if ((item.Element("Target").Element("str_param2") != null) && (!item.Element("Target").Element("str_param2").Value.Equals("")))
                        target.str_param2 = item.Element("Target").Element("str_param2").Value;

                    if (item.Element("Target").Element("AObjectAttrs") != null)
                        foreach (string at in item.Element("Target").Element("AObjectAttrs").Value.ToString().Split(','))
                            if (!at.Equals(""))
                                target.AObjectAttrs.Add(int.Parse(at));
                    if (item.Element("Target").Element("onFin") != null)
                    {
                        if (!item.Element("Target").Element("onFin").Value.Equals(""))
                            target.onFin = int.Parse(item.Element("Target").Element("onFin").Value);
                        else
                            target.onFin = 1;
                    }

                    if ((item.Element("Target").Element("AreaName") != null) && (!item.Element("Target").Element("AreaName").Value.Equals("")))
                        target.AreaName = item.Element("Target").Element("AreaName").Value;

                    if ((item.Element("Target").Element("ObjectName") != null) && (!item.Element("Target").Element("ObjectName").Value.Equals("")))
                    {
                        target.ObjectName = item.Element("Target").Element("ObjectName").Value;
                    }

                    ParseIntIfNotEmpty(item, "Target", "IsGroup", out target.IsGroup, 0);
                    if (item.Element("Target").Element("IsClan") != null)
                        target.IsClan = item.Element("Target").Element("IsClan").Value.Trim().Equals("1");

                    if (item.Element("Target").Element("percent") != null)
                    {
                        target.usePercent = true;
                        string str = item.Element("Target").Element("percent").Value;
                        target.percent = float.Parse(item.Element("Target").Element("percent").Value, CultureInfo.InvariantCulture);
                    }
                    if (item.Element("Target").Element("additional") != null)
                    {
                        target.additional = item.Element("Target").Element("additional").Value;
                    }
                    if ((item.Element("Target").Element("Time") != null) && (!item.Element("Target").Element("Time").Value.Equals("")))
                    {
                        float Time = float.Parse(item.Element("Target").Element("Time").Value);
                        target.Time = Time;
                    }
                    else
                        target.Time = 0.0f;

                }

                if (item.Element("Precondition") != null)
                {
                    if ((item.Element("Precondition").Element("Repeat") != null) && (!item.Element("Precondition").Element("Repeat").Value.Equals("")))
                        precondition.Repeat = int.Parse(item.Element("Precondition").Element("Repeat").Value);

                    if ((item.Element("Precondition").Element("TakenPeriod") != null) && (!item.Element("Precondition").Element("TakenPeriod").Value.Equals("")))
                        precondition.TakenPeriod = double.Parse(item.Element("Precondition").Element("TakenPeriod").Value, CultureInfo.InvariantCulture);

                    if (item.Element("Precondition").Element("OmniCounter") != null)
                        precondition.omniCounter = item.Element("Precondition").Element("OmniCounter").Value.Trim().Equals("1");
                    if (item.Element("Precondition").Element("isGroup") != null)
                        precondition.isGroup = item.Element("Precondition").Element("isGroup").Value.Trim().Equals("1");
                    //if (precondition.isGroup) Console.WriteLine(QuestID.ToString());
                }
                if (item.Element("QuestRules") != null)
                {

                    if (item.Element("QuestRules").Element("baseToCapturePercent") != null)
                        questRules.basePercent = float.Parse(item.Element("QuestRules").Element("baseToCapturePercent").Value, CultureInfo.InvariantCulture);
                    if (item.Element("QuestRules").Element("dontTakeItems") != null)
                        questRules.dontTakeItems = true;
                    if (item.Element("QuestRules").Element("dontHideMarks") != null)
                        questRules.dontHideMarks = true;
                    if (item.Element("QuestRules").Element("Items") != null)
                        CQuests.parceItems(item.Element("QuestRules").Element("Items"), questRules.items);
                    if (item.Element("QuestRules").Element("Spaces") != null)
                    {
                        questRules.space = long.Parse(item.Element("QuestRules").Element("Spaces").Value);
                    }
                        
                    CQuests.AddDataToList(item, "QuestRules", "Scenarios", questRules.Scenarios);
                    CQuests.AddDataToList(item, "QuestRules", "MassQuests", questRules.MassQuests);

                    ParseIntIfNotEmpty(item, "QuestRules", "MaxGroup", out questRules.MaxGroup, 0);
                    ParseIntIfNotEmpty(item, "QuestRules", "MinGroup", out questRules.MinGroup, 0);
                    ParseIntIfNotEmpty(item, "QuestRules", "MaxMember", out questRules.MaxMember, 0);
                    ParseIntIfNotEmpty(item, "QuestRules", "MinMember", out questRules.MinMember, 0);
                    if (item.Element("QuestRules").Element("TeleportTo") != null)
                        questRules.TeleportTo = item.Element("QuestRules").Element("TeleportTo").Value.ToString();

                    if (item.Element("QuestRules").Element("MapMarks") != null)
                    {
                        foreach (var e in item.Element("QuestRules").Element("MapMarks").Elements())
                        {
                            string coords, space; float radius = 0;

                            coords = e.Element("coordinates").Value.ToString();
                            space = e.Element("space").Value.ToString();
                            if (e.Element("radius") != null)
                                radius = Convert.ToSingle(e.Element("radius").Value.ToString());
                            questRules.mapMarks.Add(new MapMark(coords, radius, space));
                        }
                    }

                    if (item.Element("QuestRules").Element("npc") != null)
                    {
                        questRules.npc.setXML(item.Element("QuestRules").Element("npc"));
                    }
                    if (item.Element("QuestRules").Element("mobs") != null)
                    {
                        questRules.mobs.setXML(item.Element("QuestRules").Element("mobs"));
                    }
                }

                if (item.Element("Reward") != null)
                {
                    CQuests.AddDataToInt(item, "Reward", "Experience", out reward.Experience);
                    ParseIntIfNotEmpty(item, "Reward", "Exp", out reward.Experience, reward.Experience);

                    if (item.Element("Reward").Element("Items") != null)
                        CQuests.parceItems(item.Element("Reward").Element("Items"), reward.items);

                    if ((item.Element("Reward").Element("Credits") != null) && (!item.Element("Reward").Element("Credits").Value.Equals("")))
                        reward.Credits = float.Parse(item.Element("Reward").Element("Credits").Value, System.Globalization.CultureInfo.InvariantCulture);

                    ParseIntIfNotEmpty(item, "Reward", "KarmaPK", out reward.KarmaPK, 0);
                    AddDataToList(item, "Reward", "GetKnowleges", reward.GetKnowleges);

                    if (item.Element("Reward").Element("Teleport") != null)
                        reward.teleportTo = item.Element("Reward").Element("Teleport").Value;

                    if (item.Element("Reward").Element("dungeonTarget") != null)
                        reward.dungeonTarget = item.Element("Reward").Element("dungeonTarget").Value;
                    if (item.Element("Reward").Element("dungeonTargetMode") != null)
                        reward.dungeonTargetMode = int.Parse(item.Element("Reward").Element("dungeonTargetMode").Value);

                    if (item.Element("Reward").Element("Reputation") != null)
                        foreach (string fraction in item.Element("Reward").Element("Reputation").Value.Split(';'))
                            if (!fraction.Equals(""))
                                reward.Reputation.Add(int.Parse(fraction.Split(':')[0]), int.Parse(fraction.Split(':')[1]));
                    if (item.Element("Reward").Element("Reputation2") != null)
                        foreach (string fraction in item.Element("Reward").Element("Reputation2").Value.Split(';'))
                            if (!fraction.Equals(""))
                            {
                               int fractionID = int.Parse(fraction.Split(':')[0]);
                               reward.Reputation2.Add(fractionID, int.Parse(fraction.Split(':')[1]));
                            }
                    if (item.Element("Reward").Element("NPCReputation") != null)
                        foreach (string fraction in item.Element("Reward").Element("NPCReputation").Value.Split(';'))
                            if (!fraction.Equals(""))
                                reward.NPCReputation.Add(fraction.Split(':')[0], int.Parse(fraction.Split(':')[1]));
                    if (item.Element("Reward").Element("ChangeQuests") != null)
                    {
                        if (item.Element("Reward").Element("randomQuest") != null)
                            reward.randomQuest = true;
                        foreach (string quest in item.Element("Reward").Element("ChangeQuests").Value.Split(';'))
                            if (!quest.Equals(""))
                            {
                                int change_quest_id = int.Parse(quest.Split(':')[0]);
                                int change_type = int.Parse(quest.Split(':')[1]);
                                reward.ChangeQuests.Add(change_quest_id, change_type);

                                if (!QuestParentList.ContainsKey(change_quest_id))
                                    QuestParentList.Add(change_quest_id, new ChangedQuests());

                                if (!QuestParentList[change_quest_id].ContainsKey(change_type))
                                    QuestParentList[change_quest_id].Add(change_type, new List<int>());
                                if (QuestParentList[change_quest_id][change_type].Contains(QuestID))
                                {
                                    //По какой-то причине квесты парсятся несколько раз
                                    continue;
                                }
                                QuestParentList[change_quest_id][change_type].Add(QuestID);
                            }
                    }
                    if (item.Element("Reward").Element("blackBoxes") != null)
                    {
                        string black_boxes = item.Element("Reward").Element("blackBoxes").Value;
                        foreach (string bb_name in black_boxes.Split(','))
                        {
                            reward.blackBoxes.Add(bb_name.Trim());
                        }
                    }
                    if (item.Element("Reward").Element("Effects") != null)
                        foreach (XElement effect in item.Element("Reward").Element("Effects").Elements())
                        {
                            reward.Effects.Add(new CEffect(int.Parse(effect.Element("id").Value.ToString()),
                            int.Parse(effect.Element("stack").Value.ToString())));
                        }

                    if (item.Element("Reward").Element("RewardWindow") != null)
                        reward.RewardWindow = item.Element("Reward").Element("RewardWindow").Value.Trim().Equals("1");
                    if (item.Element("Reward").Element("tradingPoints") != null)
                    {
                        string[] result = item.Element("Reward").Element("tradingPoints").Value.Split(':');
                        reward.OTfraction = int.Parse(result[0]);
                        reward.OTvalue = int.Parse(result[1]);
                    }
                    if (item.Element("Reward").Element("clanPoints") != null)
                        reward.clanPoints = int.Parse(item.Element("Reward").Element("clanPoints").Value);

                }


                if (item.Element("Penalty") != null)
                {
                    CQuests.AddDataToInt(item, "Penalty", "Experience", out penalty.Experience);
                    ParseIntIfNotEmpty(item, "Penalty", "Exp", out penalty.Experience, penalty.Experience);

                    if (item.Element("Penalty").Element("Items") != null)
                        CQuests.parceItems(item.Element("Penalty").Element("Items"), penalty.items);

                    if ((item.Element("Penalty").Element("Credits") != null) && (!item.Element("Penalty").Element("Credits").Value.Equals("")))
                        penalty.Credits = float.Parse(item.Element("Penalty").Element("Credits").Value, System.Globalization.CultureInfo.InvariantCulture);

                    ParseIntIfNotEmpty(item, "Penalty", "KarmaPK", out penalty.KarmaPK, 0);
                    AddDataToList(item, "Penalty", "GetKnowleges", penalty.GetKnowleges);
                    if (item.Element("Penalty").Element("Reputation") != null)
                        foreach (string fraction in item.Element("Penalty").Element("Reputation").Value.Split(';'))
                            if (!fraction.Equals(""))
                                penalty.Reputation.Add(int.Parse(fraction.Split(':')[0]), int.Parse(fraction.Split(':')[1]));
                    if (item.Element("Penalty").Element("Reputation2") != null)
                        foreach (string fraction in item.Element("Penalty").Element("Reputation2").Value.Split(';'))
                            if (!fraction.Equals(""))
                                penalty.Reputation2.Add(int.Parse(fraction.Split(':')[0]), int.Parse(fraction.Split(':')[1]));
                    if (item.Element("Penalty").Element("NPCReputation") != null)
                        foreach (string fraction in item.Element("Penalty").Element("NPCReputation").Value.Split(';'))
                            if (!fraction.Equals(""))
                                penalty.NPCReputation.Add(fraction.Split(':')[0], int.Parse(fraction.Split(':')[1]));
                    if (item.Element("Penalty").Element("ChangeQuests") != null)
                    {
                        if (item.Element("Penalty").Element("randomQuest") != null)
                            reward.randomQuest = true;
                        foreach (string quest in item.Element("Penalty").Element("ChangeQuests").Value.Split(';'))
                            if (!quest.Equals(""))
                                penalty.ChangeQuests.Add(int.Parse(quest.Split(':')[0]), int.Parse(quest.Split(':')[1]));
                    }
                    if (item.Element("Penalty").Element("Effects") != null)
                        foreach (XElement effect in item.Element("Penalty").Element("Effects").Elements())
                        {
                            penalty.Effects.Add(new CEffect(int.Parse(effect.Element("id").Value.ToString()),
                            int.Parse(effect.Element("stack").Value.ToString())));
                        }
                }

                if (item.Element("Conditions") != null)
                {
                    if (item.Element("Conditions").Element("useWeaponType") != null)
                        conditions.useWeaponType = int.Parse(item.Element("Conditions").Element("useWeaponType").Value);
                    if (item.Element("Conditions").Element("notDieCount") != null)
                        conditions.notDieCount = int.Parse(item.Element("Conditions").Element("notDieCount").Value);
                    if (item.Element("Conditions").Element("pvpWinTeam") != null)
                        conditions.pvpWinTeam = int.Parse(item.Element("Conditions").Element("pvpWinTeam").Value);
                    if (item.Element("Conditions").Element("bePvpWinner") != null)
                        conditions.bePvpWinner = int.Parse(item.Element("Conditions").Element("bePvpWinner").Value);
                    if (item.Element("Conditions").Element("duration") != null)
                        conditions.duration = int.Parse(item.Element("Conditions").Element("duration").Value);

                }

                if (item.Element("Additional") != null)
                {
                    if (item.Element("Additional").Element("ShowProgress") != null)
                    {
                        if (!item.Element("Additional").Element("ShowProgress").Value.ToString().Equals(""))
                            additional.ShowProgress = int.Parse(item.Element("Additional").Element("ShowProgress").Value.ToString());
                        else
                            additional.ShowProgress = this.SHOW_JOURNAL | this.SHOW_MESSAGE_CLOSE | this.SHOW_MESSAGE_TAKE | this.SHOW_MESSAGE_PROGRESS;
                    }

                    ParseIntIfNotEmpty(item, "Additional", "IsSubQuest", out additional.IsSubQuest, 0);
                    CQuests.AddDataToList(item, "Additional", "ListOfSubQuest", additional.ListOfSubQuest);

                    if (item.Element("Additional").Descendants().Any(u => u.Name == "CantCancel"))
                        additional.CantCancel = item.Element("Additional").Element("CantCancel").Value.Trim().Equals("1");
                    if (item.Element("Additional").Descendants().Any(u => u.Name == "CantFail"))
                        additional.CantFail = item.Element("Additional").Element("CantFail").Value.Trim().Equals("1");
                    if (item.Element("Additional").Element("DebugData") != null)
                    {
                        additional.DebugData = item.Element("Additional").Element("DebugData").Value.ToString();
                    }

                    ParseIntIfNotEmpty(item, "Additional", "isFractionBonus", out additional.isFractionBonus, 0);
                    if (item.Element("Additional").Element("messages") != null)
                    {
                        foreach (XElement message in item.Element("Additional").Element("messages").Elements())
                        {
                            QuestMessages msg = new QuestMessages();

                            if (message.Element("stateFrom") != null)
                                msg.state1 = int.Parse(message.Element("stateFrom").Value);
                            if (message.Element("stateTo") != null)
                                msg.state2 = int.Parse(message.Element("stateTo").Value);
                            if (message.Element("notification") != null)
                                msg.isHighMessage = bool.Parse(message.Element("notification").Value); 
                            information.messages.Add(msg);

                        }
                    }

                }

                if (!dict_target.ContainsKey(QuestID))
                    dict_target.Add(QuestID, new CQuest(QuestID, 0, priority, level, questLinkType, questLink, information, precondition, questRules, reward, additional, target, penalty, conditions, hidden, isOld));
            }
        }

        void ParseQuestsTexts(string sPath, NPCQuestDict target)
        {
            try
            {
                doc = XDocument.Load(sPath);
                lock_paths.Add(sPath);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось загрузить файл:" + sPath + "\n" + e.Message, "Ошибка чтения");
                return;
            }

            foreach (XElement quest in doc.Root.Elements())
            {
                int QuestID = int.Parse(quest.Element("ID").Value);
                String npc_name = quest.Element("NPC").Value;
                if (!target.ContainsKey(QuestID))
                {
                    MessageBox.Show("Ошибка, квест с ID:" + QuestID.ToString() + " был удалён, по причине, что есть текст, но нет квеста", "Удалён квест");
                    continue;
                }

                if (quest.Element("Title") != null)
                    target[QuestID].QuestInformation.Title = quest.Element("Title").Value.ToString();
                if (quest.Element("Description") != null)
                    target[QuestID].QuestInformation.Description = quest.Element("Description").Value.ToString();
                if (quest.Element("DescriptionOnTest") != null)
                    target[QuestID].QuestInformation.DescriptionOnTest = quest.Element("DescriptionOnTest").Value.ToString();
                if (quest.Element("DescriptionClosed") != null)
                    target[QuestID].QuestInformation.DescriptionClosed = quest.Element("DescriptionClosed").Value.ToString();
                
                if (quest.Element("messages") != null)
                {
                    int index = 0;
                    foreach (XElement message in quest.Element("messages").Elements())
                    {
                        if (target[QuestID].QuestInformation.messages.Count <= index) continue;
                        target[QuestID].QuestInformation.messages[index].message = message.Value;

                        index++;
                    }
                }
                int Version = 0;
                if (!quest.Element("Version").Value.Equals(""))
                    Version = int.Parse(quest.Element("Version").Value);
                target[QuestID].Version = Version;
                if (quest.Element("Items") != null)
                    foreach (XElement qitem in quest.Element("Items").Elements())
                    {
                        string title = "", description = "", activation = "", content = "";
                        if (qitem.Element("title") != null)
                            title = qitem.Element("title").Value;
                        if (qitem.Element("description") != null)
                            description = qitem.Element("description").Value;
                        int itemID = int.Parse(qitem.Element("itemID").Value);

                        if (qitem.Element("activation") != null) activation = qitem.Element("activation").Value;
                        if (qitem.Element("content") != null) content = qitem.Element("content").Value;
                        if (target[QuestID].QuestInformation.Items.ContainsKey(itemID))
                        {
                            target[QuestID].QuestInformation.Items[itemID].title = title;
                            target[QuestID].QuestInformation.Items[itemID].description = description;
                            target[QuestID].QuestInformation.Items[itemID].activation = activation;
                            target[QuestID].QuestInformation.Items[itemID].content = content;
                        }
                        else
                            target[QuestID].QuestInformation.Items.Add(itemID, new QuestItemInfo(title, description, activation, content));
                    }

                /*
                if (target[QuestID].Reward.clanPoints != 0)
                {
                    string line = QuestID.ToString() + "\t" + target[QuestID].QuestInformation.Title + "\t";
                    string items = "";
                    foreach (var i in target[QuestID].Reward.items)
                        items += " " + i.itemType.ToString() + ":" + i.count.ToString() + ",";
                    line += items + "\t";
                    line += target[QuestID].Reward.Credits.ToString() + "\t";
                    line += target[QuestID].Reward.Experience.ToString() + "\t";
                    line += target[QuestID].Reward.KarmaPK.ToString() + "\t";
                    string rep = "";
                    foreach (var i in target[QuestID].Reward.Reputation2)
                        rep += " " + i.Key.ToString() + ":" + i.Value.ToString() + ",";
                    line += rep + "\t";
                    string ef = "";
                    foreach (var i in target[QuestID].Reward.Effects)
                        ef += " " + i.getID().ToString() + ":" + i.getStack().ToString() + ",";
                    line += ef + "\t";
                    string qq = "";
                    foreach (var i in target[QuestID].Reward.ChangeQuests)
                        qq += " " + i.Key.ToString() + ":" + i.Value.ToString() + ",";
                    line += qq + "\t";
                    string bb = "";
                    foreach (var i in target[QuestID].Reward.blackBoxes)
                        bb += i + ", ";
                    line += bb + "\t";
                    line += target[QuestID].Reward.teleportTo + "\t";

                    if (target[QuestID].Reward.OTvalue != 0)
                        line += target[QuestID].Reward.OTfraction.ToString() + ":" + target[QuestID].Reward.OTvalue.ToString() + "\t";
                    else
                        line += " " + "\t";

                    string kn = "";
                    foreach (var i in target[QuestID].Reward.GetKnowleges)
                    {
                        kn += i + ", ";
                    }
                    line += kn + "\t";

                    line += target[QuestID].Reward.clanPoints.ToString();

                    //Console.WriteLine(line);

                }
                */
            }
        }

        public static XElement getItemsNode(List<QuestItem> list)
        {
            if (!list.Any()) return null;

            XElement items;
            items = new XElement("Items");
            foreach (QuestItem item in list)
            {
                XElement item_node = new XElement("item");
                if (item.itemType == 0) continue;
                item_node.Add(new XElement("itemType", item.itemType));
                if (item.attribute != ItemAttribute.NORMAL)
                    item_node.Add(new XElement("attribute", (int)item.attribute));
                if (item.count < 1) continue;
                item_node.Add(new XElement("count", item.count));
                if (item.condition > 0) item_node.Add(new XElement("condition", item.condition));
                items.Add(item_node);
            }
            return items;
        }

        public static void parceItems(XElement element, List<QuestItem> list)
        {
            if (element == null) return;
            foreach (XElement itemNode in element.Elements())
            {
                QuestItem item = new QuestItem();
                if (itemNode.Element("itemType") == null) continue;
                item.itemType = int.Parse(itemNode.Element("itemType").Value);
                if (itemNode.Element("attribute") != null)
                    item.attribute = (ItemAttribute)int.Parse(itemNode.Element("attribute").Value);
                if (itemNode.Element("count") != null)
                    item.count = int.Parse(itemNode.Element("count").Value);
                if (itemNode.Element("condition") != null)
                    item.condition = float.Parse(itemNode.Element("condition").Value, System.Globalization.CultureInfo.InvariantCulture);
                list.Add(item);
            }
        }

        public static void AddDataToList(XElement Element, String Name1, String Name2, List<int> list)
        {

            if ((Element == null) || (Element.Element(Name1).Element(Name2) == null))
                return;
            if (Element.Element(Name1).Element(Name2).Value != "")
                foreach (string quest in Element.Element(Name1).Element(Name2).Value.Split(','))
                    list.Add(int.Parse(quest));
        }

        public static void AddDataToInt(XElement Element, String Name1, String Name2, out int result)
        {
            result = 0;
            if ((Element == null) || (Element.Element(Name1).Element(Name2) == null))
                return;
            if (Element.Element(Name1).Element(Name2).Value != "")
                foreach (string quest in Element.Element(Name1).Element(Name2).Value.Split(','))
                    result += int.Parse(quest);
        }

        public static void AddDataToList(XElement Element, String Name1, List<int> list)
        {
            if ((Element == null) || (Element.Element(Name1) == null))
                return;
            if (Element.Element(Name1).Value != "")
                foreach (string quest in Element.Element(Name1).Value.Split(','))
                    list.Add(int.Parse(quest));
        }

        private void ParseIntIfNotEmpty(XElement Element, String Name1, String Name2, out int value, int defaultValue)
        {
            if ((Element == null) || (Element.Element(Name1).Element(Name2) == null))
            {
                value = defaultValue;
                return;
            }
            if (!Element.Element(Name1).Element(Name2).Value.Equals(""))
                value = int.Parse(Element.Element(Name1).Element(Name2).Value);
            else
                value = defaultValue;
        }

        //! Возращает список квестов (без вложенных Subquest) для конкретного NPC
        public List<CQuest> getQuestAndTitleOnNPCName(string NPCName)
        {
            List<CQuest> retQuests = new List<CQuest>();
            foreach (CQuest quest in this.quest.Values)
            {
                //System.Console.WriteLine(NPCName + "vs" + quest.QuestInformation.NameOfHolder);
                if (quest.Additional.Holder.Equals(NPCName) && (quest.Additional.IsSubQuest == 0))
                    retQuests.Add(quest);
            }
            return retQuests;
        }
        //! Возвращает число квестов у заданного NPC 
        public int getCountOfQuests(string NPCName)
        {
            List<CQuest> quests = getQuestAndTitleOnNPCName(NPCName);
            return (quests.Count);
        }
        //! Возвращает число квестов верхнего уровня (без субквестов)
        public int getCountTopLevelQuests()
        {
            int total = 0;
            foreach (int questID in quest.Keys)
                if (quest[questID].Additional.IsSubQuest == 0)
                    total++;
            return total;
        }

        //! Возвращает список ID квестов (для комбобокса)
        public string[] getQuestsIDasString()
        {
            int count = quest.Count + getCountTopLevelQuests();
            int i = 0;
            string[] array = new string[count];
            foreach (int questID in quest.Keys)
            {
                array[i++] = questID.ToString();
                if (quest[questID].Additional.IsSubQuest == 0)
                {
                    if (quest[questID].QuestInformation.Title != "")
                        array[i++] = quest[questID].QuestInformation.Title;
                    else
                        array[i++] = "fuck";
                }
            }
            return array;
        }

        //! Сохраняет данные по квестам в xml файл
        public void SaveQuests()
        {
            string path = CSettings.GetLastQuestIDPath();
            FileStream fcreate = File.Open(path, FileMode.Create);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fcreate);
            writer.WriteLine(last_quest_id.ToString());
            writer.Close();

            SaveQuestsTexts(CSettings.GetQuestTextPath(), this.quest);
            SaveQuestsData(CSettings.GetQuestDataPath(), this.quest);
        }

        //! Сохраняет текущую локализацию квестов в файл
        public void SaveLocales()
        {
            foreach (var locale in CSettings.getListLocales())
            {
                string filepath = CSettings.GetQuestLocaleTextPath(locale);
                NPCQuestDict target = this.locales[locale];
                SaveQuestsTexts(filepath, target);
            }
        }

        private void SaveQuestsTexts(string fileName, NPCQuestDict target)
        {
            XDocument resultDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("root"));
            XElement element;

            foreach (CQuest questValue in target.Values)
            {

                element = new XElement("Quest",
                   new XElement("ID", questValue.QuestID),
                   new XElement("NPC", questValue.Additional.Holder),
                   new XElement("Version", questValue.Version));
                if (questValue.QuestInformation.Title != "")
                    element.Add(new XElement("Title", questValue.QuestInformation.Title));
                if (questValue.QuestInformation.Description != "")
                    element.Add(new XElement("Description", questValue.QuestInformation.Description));
                if (questValue.QuestInformation.DescriptionOnTest != "")
                    element.Add(new XElement("DescriptionOnTest", questValue.QuestInformation.DescriptionOnTest));
                if (questValue.QuestInformation.DescriptionClosed != "")
                    element.Add(new XElement("DescriptionClosed", questValue.QuestInformation.DescriptionClosed));

                XElement subElement = new XElement("messages");
                bool flag_messages = false;
                foreach (var message in questValue.QuestInformation.messages)
                {
                    if (!message.message.Any())
                        continue;
                    subElement.Add(new XElement("message", message.message));
                    flag_messages = true;

                }
                if (flag_messages) element.Add(subElement);
                List<XElement> ItemsXE = getItemElements(questValue.QuestInformation.Items);
                if (ItemsXE.Any())
                    element.Add(new XElement("Items", ItemsXE));
                resultDoc.Root.Add(element);
            }

            System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
            using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(fileName, settings))
            {
                resultDoc.Save(w);
            }
        }

        private void SaveQuestsData(string fileName, NPCQuestDict target)
        {
            XDocument resultDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("root"));
            XElement element;

            foreach (CQuest questValue in target.Values)
            {
                element = new XElement("Quest",
                   new XElement("ID", questValue.QuestID));
                if (questValue.hidden)
                    element.Add(new XElement("hidden", "1"));
                if (questValue.isOld)
                    element.Add(new XElement("isOld", "1"));
                if (questValue.questLinkType > 0)
                {
                    element.Add(new XElement("questLinkType", questValue.questLinkType));
                    if (questValue.questLink > 0)
                        element.Add(new XElement("questLink", questValue.questLink));
                }
                if (questValue.Priority > 0)
                    element.Add(new XElement("Priority", questValue.Priority.ToString()));
                if (questValue.Level > 0)
                    element.Add(new XElement("Level", questValue.Level.ToString()));
                if (questValue.Target.Any())
                {
                    element.Add(new XElement("Target"));
                    if (questValue.Target.onFin != 0)
                        element.Element("Target").Add(new XElement("onFin", questValue.Target.onFin));
                    if (questValue.Target.QuestType != 0)
                        element.Element("Target").Add(new XElement("QuestType", questValue.Target.QuestType));
                    if (questValue.Target.ObjectType != 0)
                        element.Element("Target").Add(new XElement("ObjectType", Global.GetIntAsString(questValue.Target.ObjectType)));
                    if (questValue.Target.ObjectAttr != 0)
                        element.Element("Target").Add(new XElement("ObjectAttr", questValue.Target.ObjectAttr));
                    if (questValue.Target.str_param.Any())
                        element.Element("Target").Add(new XElement("str_param", questValue.Target.str_param));
                    if (questValue.Target.str_param2.Any())
                        element.Element("Target").Add(new XElement("str_param2", questValue.Target.str_param2));
                    if (questValue.Target.NumOfObjects != 0)
                        element.Element("Target").Add(new XElement("NumOfObjects", Global.GetIntAsString(questValue.Target.NumOfObjects)));
                    if (questValue.Target.ObjectName != "")
                        element.Element("Target").Add(new XElement("ObjectName", questValue.Target.ObjectName));
                    if (questValue.Target.AObjectAttrs.Any())
                        element.Element("Target").Add(new XElement("AObjectAttrs", Global.GetListAsString(questValue.Target.AObjectAttrs)));
                    if (questValue.Target.AreaName != "")
                        element.Element("Target").Add(new XElement("AreaName", questValue.Target.AreaName));
                    if (questValue.Target.IsGroup != 0)
                        element.Element("Target").Add(new XElement("IsGroup", questValue.Target.IsGroup.ToString()));
                    if (questValue.Target.IsClan)
                        element.Element("Target").Add(new XElement("IsClan", questValue.Target.IsClan));
                    if (questValue.Target.Time != 0)
                        element.Element("Target").Add(new XElement("Time", questValue.Target.Time.ToString()));
                    if (questValue.Target.usePercent)
                        element.Element("Target").Add(new XElement("percent", questValue.Target.percent.ToString("G6", CultureInfo.InvariantCulture)));
                    if (questValue.Target.additional.Any())
                        element.Element("Target").Add(new XElement("additional", questValue.Target.additional));
                }

                if (questValue.Precondition.Any())
                {
                    element.Add(new XElement("Precondition"));
                    if (questValue.Precondition.TakenPeriod != 0)
                        element.Element("Precondition").Add(new XElement("TakenPeriod", questValue.Precondition.TakenPeriod.ToString("G6", CultureInfo.InvariantCulture)));
                    if (questValue.Precondition.Repeat != 0)
                        element.Element("Precondition").Add(new XElement("Repeat", questValue.Precondition.Repeat));
                    if (questValue.Precondition.omniCounter)
                        element.Element("Precondition").Add(new XElement("OmniCounter", Global.GetBoolAsString(questValue.Precondition.omniCounter)));
                    if (questValue.Precondition.isGroup)
                        element.Element("Precondition").Add(new XElement("isGroup", Global.GetBoolAsString(questValue.Precondition.isGroup)));
                }

                if (questValue.QuestRules.Any())
                {
                    element.Add(new XElement("QuestRules"));
                    if (questValue.QuestRules.Scenarios.Any())
                        element.Element("QuestRules").Add(new XElement("Scenarios", Global.GetListAsString(questValue.QuestRules.Scenarios)));
                    if (questValue.QuestRules.TeleportTo != "")
                        element.Element("QuestRules").Add(new XElement("TeleportTo", questValue.QuestRules.TeleportTo));
                    if (questValue.QuestRules.items.Any())
                        element.Element("QuestRules").Add(CQuests.getItemsNode(questValue.QuestRules.items));
                    if (questValue.QuestRules.MinGroup != 0)
                        element.Element("QuestRules").Add(new XElement("MinGroup", Global.GetIntAsString(questValue.QuestRules.MinGroup)));
                    if (questValue.QuestRules.MaxGroup != 0)
                        element.Element("QuestRules").Add(new XElement("MaxGroup", Global.GetIntAsString(questValue.QuestRules.MaxGroup)));
                    if (questValue.QuestRules.MaxMember != 0)
                        element.Element("QuestRules").Add(new XElement("MaxMember", Global.GetIntAsString(questValue.QuestRules.MaxMember)));
                    if (questValue.QuestRules.MassQuests.Any())
                        element.Element("QuestRules").Add(new XElement("MassQuests", Global.GetListAsString(questValue.QuestRules.MassQuests)));
                    if (questValue.QuestRules.basePercent != 0)
                        element.Element("QuestRules").Add(new XElement("baseToCapturePercent", questValue.QuestRules.basePercent.ToString("G6", CultureInfo.InvariantCulture)));
                    if (questValue.QuestRules.dontTakeItems)
                        element.Element("QuestRules").Add(new XElement("dontTakeItems", "1"));
                    if (questValue.QuestRules.dontHideMarks)
                        element.Element("QuestRules").Add(new XElement("dontHideMarks", "1"));
                    
                    if (questValue.QuestRules.npc.Any())
                        element.Element("QuestRules").Add(questValue.QuestRules.npc.getXML());
                    if (questValue.QuestRules.mobs.Any())
                        element.Element("QuestRules").Add(questValue.QuestRules.mobs.getXML());
                    if (questValue.QuestRules.space != 0)
                        element.Element("QuestRules").Add(new XElement("Spaces", questValue.QuestRules.space.ToString()));
                    if (questValue.QuestRules.mapMarks.Any())
                    {
                        XElement node_marks = new XElement("MapMarks");
                        foreach (var i in questValue.QuestRules.mapMarks)
                        {
                            XElement node = new XElement("item");
                            node.Add(new XElement("coordinates", i.coords));
                            node.Add(new XElement("space", i.space));
                            if (i.radius > 0)
                                node.Add(new XElement("radius", i.radius.ToString()));
                            node_marks.Add(node);
                        }
                        element.Element("QuestRules").Add(node_marks);
                    }

                }


                if (questValue.Reward.Any())
                {
                    element.Add(new XElement("Reward"));
                    if (questValue.Reward.Experience != 0)
                        element.Element("Reward").Add(new XElement("Exp", Global.GetIntAsString(questValue.Reward.Experience)));
                    if (questValue.Reward.items.Any())
                        element.Element("Reward").Add(CQuests.getItemsNode(questValue.Reward.items));
                    if (questValue.Reward.Credits != 0)
                        element.Element("Reward").Add(new XElement("Credits", questValue.Reward.Credits));
                    if (questValue.Reward.ReputationNotEmpty())
                        element.Element("Reward").Add(new XElement("Reputation", questValue.Reward.getReputation()));
                    if (questValue.Reward.Reputation2NotEmpty())
                        element.Element("Reward").Add(new XElement("Reputation2", questValue.Reward.getReputation2()));
                    if (questValue.Reward.ReputationNotEmpty(true))
                        element.Element("Reward").Add(new XElement("NPCReputation", questValue.Reward.getReputation(true)));
                    if (questValue.Reward.ChangeQuests.Any())
                        element.Element("Reward").Add(new XElement("ChangeQuests", questValue.Reward.getChangeQuests()));
                    if (questValue.Reward.randomQuest)
                        element.Element("Reward").Add(new XElement("randomQuest", "1"));
                    if (questValue.Reward.KarmaPK != 0)
                        element.Element("Reward").Add(new XElement("KarmaPK", questValue.Reward.KarmaPK.ToString()));
                    if (questValue.Reward.GetKnowleges.Any())
                        element.Element("Reward").Add(new XElement("GetKnowleges", Global.GetListAsString(questValue.Reward.GetKnowleges)));
                    if (questValue.Reward.teleportTo.Any())
                        element.Element("Reward").Add(new XElement("Teleport", questValue.Reward.teleportTo));
                    if (questValue.Reward.dungeonTarget.Any())
                    {
                        element.Element("Reward").Add(new XElement("dungeonTarget", questValue.Reward.dungeonTarget));
                        element.Element("Reward").Add(new XElement("dungeonTargetMode", questValue.Reward.dungeonTargetMode.ToString()));
                    }
                    if (questValue.Reward.RewardWindow)
                        element.Element("Reward").Add(new XElement("RewardWindow", Global.GetBoolAsString(questValue.Reward.RewardWindow)));
                    if (questValue.Reward.OTvalue > 0)
                    {
                        element.Element("Reward").Add(new XElement("tradingPoints", questValue.Reward.OTfraction.ToString() + ":" + questValue.Reward.OTvalue.ToString()));
                    }
                    if (questValue.Reward.clanPoints > 0)
                        element.Element("Reward").Add(new XElement("clanPoints", questValue.Reward.clanPoints.ToString()));
                    List<XElement> EffectsXE = getEffectElements(questValue.Reward.Effects);
                    if (EffectsXE.Any())
                        element.Element("Reward").Add(new XElement("Effects", EffectsXE));
                    if (questValue.Reward.blackBoxes.Any())
                        element.Element("Reward").Add(new XElement("blackBoxes", Global.GetListAsString(questValue.Reward.blackBoxes)));
                }

                if (questValue.Conditions.Any())
                {
                    element.Add(new XElement("Conditions"));
                    if (questValue.Conditions.useWeaponType != 0)
                        element.Element("Conditions").Add(new XElement("useWeaponType", questValue.Conditions.useWeaponType.ToString()));
                    if (questValue.Conditions.notDieCount != 0)
                        element.Element("Conditions").Add(new XElement("notDieCount", questValue.Conditions.notDieCount.ToString()));
                    if (questValue.Conditions.bePvpWinner != 0)
                        element.Element("Conditions").Add(new XElement("bePvpWinner", questValue.Conditions.bePvpWinner.ToString()));
                    if (questValue.Conditions.pvpWinTeam != 0)
                        element.Element("Conditions").Add(new XElement("pvpWinTeam", questValue.Conditions.pvpWinTeam.ToString()));
                    if (questValue.Conditions.duration != 0)
                        element.Element("Conditions").Add(new XElement("duration", questValue.Conditions.duration.ToString()));
                    
                }

                if (questValue.QuestPenalty.Any())
                {
                    element.Add(new XElement("Penalty"));
                    if (questValue.QuestPenalty.Experience != 0)
                        element.Element("Penalty").Add(new XElement("Exp", Global.GetIntAsString(questValue.QuestPenalty.Experience)));
                    if (questValue.QuestPenalty.items.Any())
                        element.Element("Penalty").Add(CQuests.getItemsNode(questValue.QuestPenalty.items));
                    if (questValue.QuestPenalty.Credits != 0)
                        element.Element("Penalty").Add(new XElement("Credits", questValue.QuestPenalty.Credits.ToString("G6", CultureInfo.InvariantCulture)));
                    if (questValue.QuestPenalty.ReputationNotEmpty())
                        element.Element("Penalty").Add(new XElement("Reputation", questValue.QuestPenalty.getReputation()));
                    if (questValue.QuestPenalty.Reputation2NotEmpty())
                        element.Element("Penalty").Add(new XElement("Reputation2", questValue.QuestPenalty.getReputation2()));
                    if (questValue.QuestPenalty.ReputationNotEmpty(true))
                        element.Element("Penalty").Add(new XElement("NPCReputation", questValue.QuestPenalty.getReputation(true)));
                    if (questValue.QuestPenalty.ChangeQuests.Any())
                        element.Element("Penalty").Add(new XElement("ChangeQuests", questValue.QuestPenalty.getChangeQuests()));
                    if (questValue.QuestPenalty.randomQuest)
                        element.Element("Penalty").Add(new XElement("randomQuest", "1"));
                    if (questValue.QuestPenalty.KarmaPK != 0)
                        element.Element("Penalty").Add(new XElement("KarmaPK", questValue.QuestPenalty.KarmaPK.ToString()));
                    if (questValue.QuestPenalty.GetKnowleges.Any())
                        element.Element("Penalty").Add(new XElement("GetKnowleges", Global.GetListAsString(questValue.QuestPenalty.GetKnowleges)));
                    List<XElement> EffectsXE = getEffectElements(questValue.QuestPenalty.Effects);
                    if (EffectsXE.Any())
                        element.Element("Penalty").Add(new XElement("Effects", EffectsXE));
                }

                if (questValue.Additional.Any() || questValue.QuestInformation.messages.Any())
                {
                    element.Add(new XElement("Additional"));
                    if (questValue.Additional.IsSubQuest != 0)
                        element.Element("Additional").Add(new XElement("IsSubQuest", Global.GetIntAsString(questValue.Additional.IsSubQuest)));
                    if (questValue.Additional.ListOfSubQuest.Any())
                        element.Element("Additional").Add(new XElement("ListOfSubQuest", Global.GetListAsString(questValue.Additional.ListOfSubQuest)));
                    /*
                    if ((questValue.Additional.ShowProgress & 8) > 0)
                    {
                        CQuestInformation qi = this.locales[CSettings.getCurrentLocale()][questValue.QuestID].QuestInformation;
                        bool flag = qi.Title.Any() || qi.Description.Any() || qi.DescriptionClosed.Any() || qi.DescriptionOnTest.Any();
                        if (!flag)
                            questValue.Additional.ShowProgress &= ~8;
                    }*/

                    if (questValue.Additional.ShowProgress != 0)
                        element.Element("Additional").Add(new XElement("ShowProgress", questValue.Additional.ShowProgress.ToString()));
                    if (questValue.Additional.CantCancel)
                        element.Element("Additional").Add(new XElement("CantCancel", Global.GetBoolAsString(questValue.Additional.CantCancel)));
                    if (questValue.Additional.CantFail)
                        element.Element("Additional").Add(new XElement("CantFail", Global.GetBoolAsString(questValue.Additional.CantFail)));
                    if (questValue.Additional.Holder != "")
                        element.Element("Additional").Add(new XElement("Holder", questValue.Additional.Holder));
                    if (questValue.Additional.DebugData != "")
                        element.Element("Additional").Add(new XElement("DebugData", questValue.Additional.DebugData));
                    if (!questValue.QuestInformation.Title.Any() && !questValue.QuestInformation.Description.Any())
                        element.Element("Additional").Add(new XElement("isEmpty", "1")); ;
                    if (questValue.Additional.isFractionBonus != 0)
                        element.Element("Additional").Add(new XElement("isFractionBonus", Global.GetIntAsString(questValue.Additional.isFractionBonus)));
                    if (questValue.QuestInformation.messages.Any())
                    {
                        element.Element("Additional").Add(new XElement("messages"));
                        foreach (var message in questValue.QuestInformation.messages)
                        {
                            element.Element("Additional").Element("messages").Add(new XElement("message", new XElement("stateFrom", message.state1),
                                                                                                          new XElement("stateTo", message.state2),
                                                                                                          new XElement("notification", message.isHighMessage)
                            ));
                        }
                    }

                }

                resultDoc.Root.Add(element);
            }

            System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
            using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(fileName, settings))
            {
                resultDoc.Save(w);
            }
        }

        List<XElement> getEffectElements(List<CEffect> effects)
        {
            List<XElement> ret = new List<XElement>();
            foreach (CEffect effect in effects)
            {
                ret.Add(new XElement("effect",
                    new XElement("id", effect.getID()),
                    new XElement("stack", effect.getStack())));
            }
            return ret;
        }
        List<XElement> getItemElements(Dictionary<int, QuestItemInfo> items)
        {
            List<XElement> ret = new List<XElement>();
            foreach (int key in items.Keys)
            {
                XElement item = new XElement("item");
                item.Add(new XElement("itemID", key.ToString()));
                if (items[key].title.Any())
                    item.Add(new XElement("title", items[key].title));
                if (items[key].description.Any())
                    item.Add(new XElement("description", items[key].description));
                if (items[key].activation.Any())
                    item.Add(new XElement("activation", items[key].activation));
                if (items[key].content.Any())
                    item.Add(new XElement("content", items[key].content));
                ret.Add(item);
            }
            return ret;
        }

        //! Возвращает список как строку аргументов через запятую
        string getListAsString(List<float> list)
        {
            string str = "";
            foreach (float element in list)
            {
                if (str.Equals(""))
                    str += element.ToString("0.00000", CultureInfo.InvariantCulture);
                else
                    str += ";" + element.ToString("0.00000", CultureInfo.InvariantCulture);
            }
            return str;
        }

        //! Возвращает экземпляр класса CQuest с указанным ID
        public CQuest getQuest(int questID)
        {
            if (quest.Keys.Contains(questID))
                return quest[questID];
            else if (m_Buffer.Keys.Contains(questID))
                return m_Buffer[questID];
            else return null;
        }

        //! Возвращает экземпляр класса CQuest с указанным ID
        public CQuest getQuestFromLocale(int questID, String locale)
        {
            if (locales.ContainsKey(locale) && locales[locale].ContainsKey(questID))
                return locales[locale][questID];
            else 
                if (m_LocBuffer.Keys.Contains(locale) && (m_LocBuffer[locale].Keys.Contains(questID)))
                    return m_LocBuffer[locale][questID];
            else return null;
        }

        //! Возвращает экземпляр CQuest  в зависимости от языка перевода
        public CQuest getQuestLocalized(int questID)
        {
            if (CSettings.getMode() == CSettings.MODE_EDITOR)
                return getQuest(questID);
            else
            {
                string locale = CSettings.getCurrentLocale();
                if (locales[locale].ContainsKey(questID))
                    return locales[locale][questID];
                else
                {
                    CQuest quest = getQuest(questID);
                    quest.Version = 0;
                    return quest;
                }
            }
        }

        //! Возвращает строку с описанием цели квеста в удобочитаемом виде
        public string getTargetString(CQuest quest)
        {
            string result;
            switch (quest.Target.QuestType)
            {
                case 1:
                    result = quest.Target.ObjectName;
                    break;
                case 2: case 3:
                    result = "Тип моба " + parent.mobConst.getDescriptionOnType(quest.Target.ObjectType).getName();
                    result += ", " + quest.Target.NumOfObjects + " штук";
                    break;
                case 4: case 8:
                    result = "Зона " + parent.zoneConst.getDescriptionOnKey(quest.Target.ObjectName).getName();
                    break;
                case 5:
                    result = quest.Target.NumOfObjects + " рублей";
                    break;
                case 6:
                    result = "Триггер " + parent.triggerConst.getDescriptionOnId(quest.Target.ObjectType);
                    break;
                case 7: case 19: case 20:
                    result = "Предмет " + parent.itemConst.getItemName(quest.Target.ObjectType);
                    break;
                case 0: case 16:
                    result = "Предмет " + parent.itemConst.getItemName(quest.Target.ObjectType);
                    result += ", " + quest.Target.NumOfObjects + " штук";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public void setClan(int questID, bool IsClan)
        {
            CQuest curQuest = getQuest(questID);
            curQuest.Target.IsClan = IsClan;

            if (curQuest.Additional.ListOfSubQuest.Any())
                foreach (int subQuestID in curQuest.Additional.ListOfSubQuest)
                    setClan(subQuestID, IsClan);
        }

        public void replaceQuest(CQuest iQuest)
        {
            var questID = iQuest.QuestID;
            if (quest.Keys.Contains(questID))
                quest[questID] = iQuest;
            else if (m_Buffer.Keys.Contains(questID))
                m_Buffer[questID] = iQuest;
        }

        private void addQuest(CQuest iQuest)
        {
            quest.Add(iQuest.QuestID, iQuest);
        }

        //! Удаляет квест и  все его подквесты
        private void removeQuestsWithLocals(int questID, bool recursiveCall)
        {
            List<int> temp = getQuest(questID).Additional.ListOfSubQuest;
            foreach (int subquest in temp)
                removeQuestsWithLocals(subquest, true);

            if (!recursiveCall)
                if (getQuest(questID).Additional.IsSubQuest != 0)
                    quest[getQuest(questID).Additional.IsSubQuest].Additional.ListOfSubQuest.Remove(questID);

            // удаляем квест из локализаций
            List<string> languages = CSettings.getListLocales();
            foreach (var lang in languages)
            {
                if (lang == "Russian") continue;
                CQuest english = locales[lang][questID];
                foreach (int subquest in english.Additional.ListOfSubQuest)
                    removeQuestsWithLocals(subquest, true);
                if (english.Additional.IsSubQuest != 0)
                    locales[lang][english.Additional.IsSubQuest].Additional.ListOfSubQuest.Remove(questID);
                locales[lang].Remove(questID);
            }
            quest.Remove(questID);
        }

        public int getRoot(int questID)
        {
            CQuest quest = getQuest(questID);
            if (quest.Additional.IsSubQuest == 0)
                return questID;
            else
                return getRoot(quest.Additional.IsSubQuest);
        }

        public void setTutorial(int questID, bool isTutor)
        {
            var quest = getQuest(questID);
            if (quest != null)
            {
                if (isTutor)
                    quest.Additional.ShowProgress |= 64;
                //else я не знаю зачем это было сделано
                //    quest.Additional.ShowProgress &= 63;

                foreach (int qID in quest.Additional.ListOfSubQuest)
                    setTutorial(qID, isTutor);
            }
        }

        public CQuest getLocaleQuest(int questID, string locale)
        {
            if (this.locales.Keys.Contains(locale) && this.locales[locale].Keys.Contains(questID))
                return this.locales[locale][questID];
            return null;
        }

        public void addLocaleQuest(CQuest quest, string locale)
        {
            if (!this.locales.Keys.Contains(locale))
            {
                this.locales.Add(locale, new NPCQuestDict());
            }
            if (this.locales[locale].Keys.Contains(quest.QuestID))
                this.locales[locale].Remove(quest.QuestID);
            this.locales[locale].Add(quest.QuestID, quest);
        }

        //! Возвращает словарь из квестов для локализации (устаревшие, актуальные или все)
        public DifferenceDict getQuestDifference(string locale, FindType findType)
        {
            //System.Console.WriteLine("CQuests::getQuestDifference");
            QuestLocales sorted_locale = new QuestLocales();
            if (this.locales.Keys.Contains(locale))
            {
                foreach (var locale_quest in this.locales[locale].Values)
                {
                    if (!sorted_locale.Keys.Contains(locale_quest.Additional.Holder))
                        sorted_locale.Add(locale_quest.Additional.Holder, new NPCQuestDict());
                    sorted_locale[locale_quest.Additional.Holder].Add(locale_quest.QuestID, locale_quest);
                }
            }
            DifferenceDict ret = new DifferenceDict();
            foreach (var cur_quest in quest.Values)
            {
                if (!sorted_locale.Keys.Contains(cur_quest.Additional.Holder))
                {
                    NPCQuestDict dict = new NPCQuestDict();
                    dict.Add(cur_quest.QuestID, cur_quest);
                    sorted_locale.Add(cur_quest.Additional.Holder, dict);
                }
                CQuest locale_quest = new CQuest();
                locale_quest.Version = 0;

                if (sorted_locale[cur_quest.Additional.Holder].Keys.Contains(cur_quest.QuestID))
                    locale_quest.Version = sorted_locale[cur_quest.Additional.Holder][cur_quest.QuestID].Version;

                if (!ret.Keys.Contains(cur_quest.Additional.Holder))
                    ret.Add(cur_quest.Additional.Holder, new Dictionary<int, CDifference>());
                switch (findType)
                {
                    case FindType.all:
                        ret[cur_quest.Additional.Holder].Add(cur_quest.QuestID, new CDifference(cur_quest.Version, locale_quest.Version));
                        break;
                    case FindType.outdatedOnly:
                        if (cur_quest.Version != locale_quest.Version)
                            ret[cur_quest.Additional.Holder].Add(cur_quest.QuestID, new CDifference(cur_quest.Version, locale_quest.Version));
                        break;
                    case FindType.actualOnly:
                        if (cur_quest.Version == locale_quest.Version)
                            ret[cur_quest.Additional.Holder].Add(cur_quest.QuestID, new CDifference(cur_quest.Version, locale_quest.Version));
                        break;
                }
            }
            return ret;
        }

        public void PutQuestsToBuffer(int questID, bool cutQuests)
        {
            List<CQuest> result = GetQuestWithSubs(questID);
            List<CQuest> engResult = GetLocalizedQuestWithSubs(questID);
            CutQuests = cutQuests;
            if (cutQuests)
                removeQuestsWithLocals(questID, false);

            result[0].Additional.IsSubQuest = 0;
            engResult[0].Additional.IsSubQuest = 0;
            foreach (CQuest quest in result)
                quest.Additional.Holder = "";
            foreach (CQuest quest in engResult)
                quest.Additional.Holder = "";

            if (!cutQuests)
                ChangeQuestsIDs(result, engResult);

            setBuffer(result, m_Buffer);
            foreach(string lang in CSettings.getListLocales())
                if (m_LocBuffer.Keys.Contains(lang))
                    setBuffer(engResult, m_LocBuffer[lang]);
            bufferTop = result[0].QuestID;
        }
        //! Возвращает список всех подквестов для квеста questID
        private List<CQuest> GetQuestWithSubs(int questID)
        {
            List<CQuest> result = new List<CQuest>();
            CQuest quest = getQuest(questID);
            if (quest != null)
            {
                result.Add((CQuest)quest.Clone());
                if (quest.Additional.ListOfSubQuest.Any())
                    foreach (int subquestID in quest.Additional.ListOfSubQuest)
                        result.AddRange(GetQuestWithSubs(subquestID));
            }
            return result;
        }

        //! Возвращает список всех переведенных подквестов для квеста questID
        private List<CQuest> GetLocalizedQuestWithSubs(int questID)
        {
            List<CQuest> result = new List<CQuest>();
            CQuest quest = locales["English"][questID];
            if (quest != null)
            {
                result.Add((CQuest)quest.Clone());
                if (quest.Additional.ListOfSubQuest.Any())
                    foreach (int subquestID in quest.Additional.ListOfSubQuest)
                        result.AddRange(GetLocalizedQuestWithSubs(subquestID));
            }
            return result;
        }

        //! Заменяет ID квестов в буфере на новые
        private void ChangeQuestsIDs(List<CQuest> quests, List<CQuest> engQuests)
        {
            Dictionary<int, int> replace = new Dictionary<int, int>();
            int new_quest_id = getQuestNewID(quests.Count);
            foreach (CQuest quest in quests)
            {
                List<int> excepts = replace.Values.ToList();
                excepts.AddRange(replace.Keys.ToList());

                replace.Add(quest.QuestID, new_quest_id);
                new_quest_id++;
                quest.QuestID = replace[quest.QuestID];
            }
            foreach (CQuest quest in quests)
            {
                if (replace.Keys.Contains(quest.Additional.IsSubQuest))
                    quest.Additional.IsSubQuest = replace[quest.Additional.IsSubQuest];

                List<int> newSubQuestIDs = new List<int>();
                foreach (int subqID in quest.Additional.ListOfSubQuest)
                    newSubQuestIDs.Add(replace[subqID]);

                quest.Additional.ListOfSubQuest = newSubQuestIDs;
            }

            foreach (CQuest engQuest in engQuests)
            {
                engQuest.QuestID = replace[engQuest.QuestID];
                if (replace.Keys.Contains(engQuest.Additional.IsSubQuest))
                    engQuest.Additional.IsSubQuest = replace[engQuest.Additional.IsSubQuest];

                List<int> newSubQuestIDs = new List<int>();
                foreach (int subqID in engQuest.Additional.ListOfSubQuest)
                    newSubQuestIDs.Add(replace[subqID]);

                engQuest.Additional.ListOfSubQuest = newSubQuestIDs;
            }
        }

        //! Возвращает ID для нового квеста без учета списка excepts 
        private int getQuestNewID(int count)
        {
            string html = string.Empty;
            string url = @"http://k-adm.stalker.so:8011/getnextidrange?key=quest_id&count=" + count;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            try
            {
                JObject json = JObject.Parse(html);
                string str = json["quest_id"].ToString().Replace("[", "").Replace("]", "");

                string[] new_quest_ids = str.Split(',');
                int new_quest_id = int.Parse(new_quest_ids[0]);
                return new_quest_id;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка получения нового ID квеста. Проверьте своё подключение к hz-dev", "Ошибка");
            }
            int iFirstQuestID = 1;
            if (last_quest_id != 0)
            {
                last_quest_id++;
                return last_quest_id - 1;
            }
            return 0;
        }
        //! Сохраняет квесты quests в буфер обмена destination
        private void setBuffer(List<CQuest> quests, NPCQuestDict destination)
        {
            destination.Clear();
            foreach (CQuest quest in quests)
                destination.Add(quest.QuestID, quest);
        }

        //! Вставляет квесты из буфера обмена как подквест 
        public void PasteBuffer(int CurrentQuestID)
        {
            CQuest HeadQuest = getQuest(CurrentQuestID);
            List<CQuest> buffer = new List<CQuest>();
            foreach (CQuest q in m_Buffer.Values.ToList())
                buffer.Add((CQuest)q.Clone());

            HeadQuest.Additional.ListOfSubQuest.Add(buffer[0].QuestID);
            buffer[0].Additional.IsSubQuest = HeadQuest.QuestID;
            foreach (CQuest bufQuest in buffer)
            {
                bufQuest.Additional.Holder = HeadQuest.Additional.Holder;
                addQuest(bufQuest);
            }

            foreach (string lang in CSettings.getListLocales())
            {
                if (!m_LocBuffer.ContainsKey(lang)) continue;
                List<CQuest> langBuffer = new List<CQuest>();
                foreach (CQuest q in m_LocBuffer[lang].Values.ToList())
                    langBuffer.Add((CQuest)q.Clone());

                CQuest HeadEnglish = locales[lang][CurrentQuestID];
                HeadEnglish.Additional.ListOfSubQuest.Add(langBuffer[0].QuestID);
                langBuffer[0].Additional.IsSubQuest = HeadEnglish.QuestID;
                foreach (CQuest engQuest in langBuffer)
                {
                    engQuest.Additional.Holder = HeadEnglish.Additional.Holder;
                    locales[lang].Add(engQuest.QuestID, engQuest);
                }
            }
        }

        public int ReplaceBuffer(int CurrentQuestID)
        {
            CQuest HeadQuest = getQuest(CurrentQuestID);

            List<CQuest> buffer = new List<CQuest>();
            foreach (CQuest q in m_Buffer.Values.ToList())
                buffer.Add((CQuest)q.Clone());

            String name = HeadQuest.Additional.Holder;
            int questID = HeadQuest.QuestID;
            int parentID = HeadQuest.Additional.IsSubQuest;
            CQuest parentQuest = getQuest(parentID);

            removeQuestsWithLocals(questID, false);

            if (HeadQuest.Additional.IsSubQuest == 0)
            {
                buffer[0].QuestID = questID;
                foreach (var subQuest in buffer)
                {
                    if (buffer[0].Additional.ListOfSubQuest.Contains(subQuest.QuestID))
                        subQuest.Additional.IsSubQuest = questID;
                    subQuest.Additional.Holder = name;
                    addQuest(subQuest);
                }
                parentID = questID;
            }
            else
            {
                int index = parentQuest.Additional.ListOfSubQuest.IndexOf(questID);
                if (index == -1)
                {
                    MessageBox.Show("Ошибка вставки квеста", "Ошибка");
                    return 0;
                }
                parentQuest.Additional.ListOfSubQuest.Remove(questID);
                parentQuest.Additional.ListOfSubQuest.Insert(index, buffer[0].QuestID);
                buffer[0].Additional.IsSubQuest = parentID;
                foreach (var subQuest in buffer)
                {
                    subQuest.Additional.Holder = name;
                    addQuest(subQuest);
                }
            }

            foreach (string lang in CSettings.getListLocales())
            {
                CQuest HeadEnglish = getQuestFromLocale(CurrentQuestID, lang);
                List<CQuest> engBuffer = new List<CQuest>();
                if (!m_LocBuffer.ContainsKey(lang)) continue;
                foreach (CQuest q in m_LocBuffer[lang].Values.ToList())
                    engBuffer.Add((CQuest)q.Clone());
                CQuest parentEngQuest = getQuestFromLocale(parentID, lang);

                // копируем и вставляем локализации
                if (HeadEnglish.Additional.IsSubQuest == 0)
                {
                    engBuffer[0].QuestID = questID;
                    foreach (var subQuest in engBuffer)
                    {
                        if (engBuffer[0].Additional.ListOfSubQuest.Contains(subQuest.QuestID))
                            subQuest.Additional.IsSubQuest = questID;
                        subQuest.Additional.Holder = name;
                        locales[lang].Add(subQuest.QuestID, subQuest);
                    }
                    parentID = questID;
                }
                else
                {
                    int engIndex = parentEngQuest.Additional.ListOfSubQuest.IndexOf(questID);
                    parentEngQuest.Additional.ListOfSubQuest.Remove(questID);
                    parentEngQuest.Additional.ListOfSubQuest.Insert(engIndex, engBuffer[0].QuestID);
                    engBuffer[0].Additional.IsSubQuest = parentID;
                    foreach (var subQuest in engBuffer)
                    {
                        subQuest.Additional.Holder = name;
                        locales[lang].Add(subQuest.QuestID, subQuest);
                    }
                }
            }
            return getRoot(parentID);
        }

        public void lock_files()
        {
            foreach (var path in lock_paths)
                fs_list.Add(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public void unlock_files()
        {
            foreach(var file in fs_list)
            {
                file.Close();
            }
            fs_list.Clear();
        }
    }
}
