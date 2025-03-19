using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace StalkerOnlineQuesterEditor
{

    //! Класс квест - все параметры получения, выполнения и вознаграждения за квест
    public class CQuest : ICloneable
    {
        public int QuestID;
        public int Version;
        public int Priority;
        public int Level;
        public CQuestInformation QuestInformation;
        public CQuestTarget Target;
        public CQuestPrecondition Precondition;
        public CQuestRules QuestRules;
        public CQuestReward Reward;
        public CQuestReward QuestPenalty;
        public CQuestAdditional Additional;
        public CQuestAdditionalConditions Conditions;
        public bool hidden;
        public bool isOld;
        public int questLink;
        public int questLinkType;

        public object Clone()
        {
            CQuest copy = new CQuest();
            copy.QuestID = this.QuestID;
            copy.Priority = this.Priority;
            copy.Level = this.Level;
            copy.Version = 0;
            copy.QuestInformation = (CQuestInformation)this.QuestInformation.Clone();
            copy.Precondition = (CQuestPrecondition)this.Precondition.Clone();
            copy.QuestRules = (CQuestRules)this.QuestRules.Clone();
            copy.Reward = (CQuestReward)this.Reward.Clone();
            copy.QuestPenalty = (CQuestReward)this.QuestPenalty.Clone();
            copy.Additional = (CQuestAdditional)this.Additional.Clone();
            copy.Target = (CQuestTarget)this.Target.Clone();
            copy.Conditions = (CQuestAdditionalConditions)Conditions.Clone();
            copy.hidden = this.hidden;
            copy.isOld = this.isOld;
            copy.questLinkType = this.questLinkType;
            copy.questLink = this.questLink;
            return copy;
        }

        public CQuest()
        {
            this.QuestID = new int();
            this.Version = new int();
            this.Priority = new int();
            this.Level = new int();
            this.QuestInformation = new CQuestInformation();
            this.Precondition = new CQuestPrecondition();
            this.QuestRules = new CQuestRules();
            this.Reward = new CQuestReward();
            this.QuestPenalty = new CQuestReward();
            this.Additional = new CQuestAdditional();
            this.Target = new CQuestTarget();
            this.Conditions = new CQuestAdditionalConditions();
            this.hidden = false;
            this.isOld = false;
            questLinkType = 0;
            questLink = 0;
        }

        public CQuest(int questID, int Version, int Priority, int Level, int questLinkType, int questLink, CQuestInformation questInformation, CQuestPrecondition precondition,
                        CQuestRules questRules, CQuestReward reward, CQuestAdditional additional, CQuestTarget target, CQuestReward penalty,
                        CQuestAdditionalConditions conditions, bool hidden = false, bool isOld = false)
        {
            this.QuestID = questID;
            this.Version = Version;
            this.Priority = Priority;
            this.Level = Level;
            this.questLinkType = questLinkType;
            this.questLink = questLink;
            this.QuestInformation = questInformation;
            this.Precondition = precondition;
            this.QuestRules = questRules;
            this.Reward = reward;
            this.Additional = additional;
            this.Target = target;
            this.QuestPenalty = penalty;
            this.Conditions = conditions;
            this.hidden = hidden;
            this.isOld = isOld;
        }

        public void InsertNonTextData(CQuest source)
        {
            this.Precondition = (CQuestPrecondition) source.Precondition.Clone();
            this.QuestRules = (CQuestRules) source.QuestRules.Clone();
            this.Reward = (CQuestReward) source.Reward.Clone();
            this.Additional = (CQuestAdditional) source.Additional.Clone();
            this.Target = (CQuestTarget) source.Target.Clone();
            this.QuestPenalty = (CQuestReward)source.QuestPenalty.Clone();        
        }
    }

    public class QuestMessages
    {
        public string message = "";
        public int state1 = -1;
        public int state2 = -1;
        public bool isHighMessage = false;

        public QuestMessages()
        {

        }

        public QuestMessages(QuestMessages original)
        {
            message = original.message;
            state1 = original.state1;
            state2 = original.state2;
            isHighMessage = original.isHighMessage;
        }

    }

    public static class QuestMessageType
    {
        static Dictionary<string, int> types = new Dictionary<string, int>()
        { { "Open", 0 }, { "On test", 1 }, { "Closed", 2 }, { "Freez", 3 }, { "Absient", 4 }, {"Any", -1 } };



        public static int getType(string name)
        {
            return types[name];
        }

        public static string getName(int type)
        {
            foreach(var i in types)
            {
                if (i.Value == type) return i.Key;
            }
            return "";
        }
    }

    //! Класс текстовой информации о квесте - название, описание, надписи на победу и проигрыш
    public class CQuestInformation : ICloneable
    {
    	public string Title;
		public string Description;
        public string DescriptionOnTest;
        public string DescriptionClosed;
        public List<QuestMessages> messages = new List<QuestMessages>();
        public Dictionary<int, QuestItemInfo> Items;

        public object Clone()
        {
            CQuestInformation copy = new CQuestInformation();
            copy.Title = (string)this.Title.Clone();
            copy.Description = (string)this.Description.Clone();
            foreach(var message in messages)
                copy.messages.Add(new QuestMessages(message));
            foreach (KeyValuePair<int, QuestItemInfo> item in this.Items)
                copy.Items.Add(item.Key, (QuestItemInfo)item.Value.Clone());
            return copy;
        }
        
        public CQuestInformation()
        {
            this.Title = "";
            this.Description = "";
            this.DescriptionOnTest = "";
            this.DescriptionClosed = "";


            this.Items = new Dictionary<int, QuestItemInfo>();
        }

        public Dictionary<int, QuestItemInfo> getItems()
        {
            return Items;
        }
        public void addItem(int itemID, string title, string description, string activation, string content)
        {
            this.Items.Add(itemID, new QuestItemInfo(title, description, activation, content));
        }
    }

    public class CQuestTarget : ICloneable
    {
        public int QuestType;
		public int ObjectType;
		public int NumOfObjects;
        public int ObjectAttr;
        public List<int> AObjectAttrs;
        public string AreaName;
        public float Time;
        public string ObjectName;
        public string additional = "";
        public int IsGroup;
        public bool IsClan;
        public int onFin;
        public float percent;
        public bool usePercent;
        public string str_param;
        public string str_param2;

        public object Clone()
        {
            CQuestTarget copy = new CQuestTarget();

            copy.QuestType = this.QuestType;
            copy.ObjectType = this.ObjectType;
            copy.NumOfObjects = this.NumOfObjects;
            copy.ObjectAttr = this.ObjectAttr;
            copy.AObjectAttrs = new List<int>(this.AObjectAttrs);
            copy.AreaName = (string)this.AreaName.Clone();
            copy.Time = this.Time;
            copy.ObjectName = (string)this.ObjectName.Clone();
            copy.IsGroup = this.IsGroup;
            copy.IsClan = this.IsClan;
            copy.onFin = this.onFin;
            copy.percent = this.percent;
            copy.usePercent = this.usePercent;
            copy.str_param = this.str_param;
            copy.str_param2 = this.str_param2;
            return copy;
        }

        public CQuestTarget()
        {
            this.QuestType = new int();
            this.ObjectType = new int();
            this.NumOfObjects = new int();
            this.ObjectAttr = new int();
            this.AreaName = "";
            this.Time = 0.0f;
            this.IsGroup = new int();
            this.IsClan = new bool();
            this.ObjectName = "";
            this.AObjectAttrs = new List<int>();
            this.onFin = new int();
            this.percent = new float();
            this.usePercent = false;
            this.str_param = "";
            this.str_param2 = "";
        }

        public bool Any()
        {
            return QuestType != 0 || ObjectType != 0 || NumOfObjects != 0 || ObjectAttr != 0 || AreaName != "" ||
                    Time != 0.0f || IsGroup != 0 || IsClan || ObjectName != "" || AObjectAttrs.Any() ||
                    onFin != 0 || percent != 0 || usePercent || str_param.Any() || str_param2.Any();
        }
        public CQuestTarget(int QuestType)
        {
            this.QuestType = QuestType;
            this.ObjectType = new int();
            this.NumOfObjects = new int();
            this.ObjectName = "";
            this.AObjectAttrs = new List<int>();
        }
    }

    public class CQuestPrecondition
    {
       	public int Repeat;
        public double TakenPeriod;
        public bool omniCounter;
        public bool isGroup;
        public CQuestPrecondition()
        {
            this.Repeat = new int();
            this.TakenPeriod = new double();
            this.omniCounter = false;
            this.isGroup = false;
        }

        public object Clone()
        {
            CQuestPrecondition copy = new CQuestPrecondition();
            copy.Repeat = this.Repeat;
            copy.TakenPeriod = this.TakenPeriod;
            copy.omniCounter = this.omniCounter;
            copy.isGroup = this.isGroup;
            return copy;
        }
        public bool Any()
        {
            return Repeat != 0 || TakenPeriod != 0 || omniCounter || isGroup;
        }
    }

    public class CQuestRules : ICloneable
    {
        public List<QuestItem> items;
        public List<int> Scenarios;
        public List<int> MassQuests;
        public List<MapMark> mapMarks;
        public string TeleportTo;
        public bool dontTakeItems;
        public bool dontHideMarks;
        public int MaxGroup;
        public int MinGroup;
        public int MaxMember;
        public int MinMember;
        public float basePercent;
        public NPC npc;
        public Mob mobs;
        public long space;

        public object Clone()
        {
            CQuestRules copy = new CQuestRules();

            copy.items = new List<QuestItem>(this.items);
            copy.Scenarios = new List<int>(this.Scenarios);
            copy.MassQuests = new List<int>(this.MassQuests);
            copy.TeleportTo = (string)this.TeleportTo.Clone();
            copy.MaxGroup = this.MaxGroup;
            copy.MinGroup = this.MinGroup;
            copy.MaxMember = this.MaxMember;
            copy.MinMember = this.MinMember;
            copy.basePercent = this.basePercent;
            copy.npc = this.npc;
            copy.mobs = this.mobs;
            copy.dontTakeItems = this.dontTakeItems;
            copy.dontHideMarks = this.dontHideMarks;
            copy.space = space;
            copy.mapMarks = new List<MapMark>(this.mapMarks);
            return copy;
        }

        public bool Any()
        {
            return items.Any() || Scenarios.Any() || MassQuests.Any() || dontTakeItems || dontHideMarks ||
                MaxGroup != 0 || MinGroup != 0 || TeleportTo != "" || basePercent != 0 || 
                MinMember != 0 || MaxMember != 0 || npc.Any() || mobs.Any() || space != 0 || mapMarks.Any();

        }
        public CQuestRules()
        {
            this.items = new List<QuestItem>();
            this.Scenarios = new List<int>();
            this.MassQuests = new List<int>();
            this.space = 0;
            this.MaxGroup = new int();
            this.MinGroup = new int();
            this.MaxMember = new int();
            this.MinMember = new int();
            this.TeleportTo = "";
            this.basePercent = new float();
            this.npc = new NPC();
            this.mobs = new Mob();
            this.mapMarks = new List<MapMark>();
        }

        public class Mob
        {
            public int mob_type = 0;
            public string level = "0";
            public string way = "";
            public int count = 0;
            public int scen_type = 0;
            public bool uniq = false;
            public string questID = "";
            public bool invulnerable = false;


            public bool Any()
            {
                return mob_type > 0 && count > 0 && scen_type > 0;
            }

            public XElement getXML()
            {
                XElement result = null;
                if (!this.Any())
                    return null;

                result = new XElement("mobs");
                result.Add(new XElement("mobType", mob_type));
                if (level.Any())
                    result.Add(new XElement("level", level));
                result.Add(new XElement("count", count));
                result.Add(new XElement("scenType", scen_type));
                if (way.Any())
                    result.Add(new XElement("way", way));
                if (invulnerable)
                    result.Add(new XElement("invulnerable", "1"));

                if (uniq)
                    result.Add(new XElement("uniq", "1"));
                if (questID.Any())
                    result.Add(new XElement("questID", questID));
                return result;
            }

            public void setXML(XElement element)
            {
                if (element.Element("mobType") != null)
                    int.TryParse(element.Element("mobType").Value.ToString(), out mob_type);
                if (element.Element("level") != null)
                    level = element.Element("level").Value.ToString();
                if (element.Element("count") != null)
                    int.TryParse(element.Element("count").Value.ToString(), out count);
                if (element.Element("scenType") != null)
                    int.TryParse(element.Element("scenType").Value.ToString(), out scen_type);
                if (element.Element("way") != null)
                    way = element.Element("way").Value;
                if (element.Element("uniq") != null)
                    uniq = true;
                if (element.Element("invulnerable") != null)
                    invulnerable = true;
                if (element.Element("questID") != null)
                    questID = element.Element("questID").Value.ToString();
            }
        }

        public class NPC
        {
            public string name = "";
            public string displayName = "";
            public string way = "";
            public int fraction = 0;
            public int reputation = 0;
            public string animation = "";
            public int npcWeapon = 0;
            public string NPCEquippedArmor = "";
            public int npcWeaponPrimary = 0;
            public List<int> npcWeaponPrimaryAttachItemIDs = new List<int>();
            public int npcWeaponPrimary2 = 0;
            public List<int> npcWeaponPrimary2AttachItemIDs = new List<int>();
            public int npcWeaponSecondary = 0;
            public List<int> npcWeaponSecondaryAttachItemIDs = new List<int>();
            public int npcSuit = 0;
            public float walkSpeed = 0;
            public float shootRange = 0;
            public float shootRangeOnCreature = 0;
            public bool uniq = false;
            public bool invulnerable = false;
            public bool mobNoAggr = false;
            public int ignoreWAR = 0;

            public bool Any()
            {
                return name.Any() || displayName.Any() || way.Any() || fraction != 0 || reputation != 0 || animation.Any() ||
                        npcWeapon != 0 || NPCEquippedArmor.Any() || npcWeaponPrimary != 0|| npcWeaponPrimaryAttachItemIDs.Any() || 
                        npcWeaponPrimary2!=0 || npcWeaponPrimary2AttachItemIDs.Any() || npcWeaponSecondary!=0 || npcWeaponSecondaryAttachItemIDs.Any() || 
                        npcSuit != 0;
            }

            public void setXML(XElement element)
            {
                if (element.Element("name") != null)
                    name = element.Element("name").Value;
                if (element.Element("displayName") != null)
                    displayName = element.Element("displayName").Value;
                if (element.Element("way") != null)
                    way = element.Element("way").Value;
                if (element.Element("fraction") != null)
                    int.TryParse(element.Element("fraction").Value, out fraction);
                if (element.Element("reputation") != null)
                    int.TryParse(element.Element("reputation").Value, out reputation);
                if (element.Element("animation") != null)
                    animation = element.Element("animation").Value;
                if (element.Element("npcWeapon") != null)
                    int.TryParse(element.Element("npcWeapon").Value, out npcWeapon);
                if (element.Element("npcWeapon") != null)
                    int.TryParse(element.Element("npcWeapon").Value, out npcWeapon);
                if (element.Element("npcWeaponPrimary") != null)
                    int.TryParse(element.Element("npcWeaponPrimary").Value, out npcWeaponPrimary);
                if (element.Element("npcWeaponPrimaryAttachItemIDs") != null)
                    CQuests.AddDataToList(element, "npcWeaponPrimaryAttachItemIDs", npcWeaponPrimaryAttachItemIDs);
                if (element.Element("npcWeaponPrimary2") != null)
                    int.TryParse(element.Element("npcWeaponPrimary2").Value, out npcWeaponPrimary2);
                if (element.Element("npcWeaponPrimary2AttachItemIDs") != null)
                    CQuests.AddDataToList(element, "npcWeaponPrimary2AttachItemIDs", npcWeaponPrimary2AttachItemIDs);
                if (element.Element("npcWeaponSecondary") != null)
                    int.TryParse(element.Element("npcWeaponSecondary").Value, out npcWeaponSecondary);
                if (element.Element("npcWeaponSecondaryAttachItemIDs") != null)
                    CQuests.AddDataToList(element, "npcWeaponSecondaryAttachItemIDs", npcWeaponSecondaryAttachItemIDs);
                if (element.Element("NPCEquippedArmor") != null)
                    NPCEquippedArmor = element.Element("NPCEquippedArmor").Value;
                if (element.Element("uniq") != null)
                    uniq = true;
                if (element.Element("invulnerable") != null)
                    invulnerable = true;
                if (element.Element("mobNoAggr") != null)
                    mobNoAggr = true;
                if (element.Element("ignoreWAR") != null)
                    int.TryParse(element.Element("ignoreWAR").Value, out ignoreWAR);
                if (element.Element("walkSpeed") != null)
                    float.TryParse(element.Element("walkSpeed").Value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out walkSpeed);
                if (element.Element("shootRange") != null)
                    float.TryParse(element.Element("shootRange").Value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out shootRange);
                if (element.Element("shootRangeOnCreature") != null)
                    float.TryParse(element.Element("shootRangeOnCreature").Value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out shootRangeOnCreature);
            }

            public XElement getXML()
            {
                XElement result = null;
                if (!this.Any())
                    return null;

                result = new XElement("npc");
                if (name.Any())
                    result.Add(new XElement("name", name));
                if (displayName.Any())
                    result.Add(new XElement("displayName", displayName));
                if (way.Any())
                    result.Add(new XElement("way", way));
                if (fraction != 0)
                    result.Add(new XElement("fraction", Global.GetIntAsString(fraction)));
                if (reputation != 0)
                    result.Add(new XElement("reputation", Global.GetIntAsString(reputation)));
                if (animation.Any())
                    result.Add(new XElement("animation", animation));
                if (invulnerable)
                    result.Add(new XElement("invulnerable", "1"));
                if (walkSpeed != 0.0f)
                    result.Add(new XElement("walkSpeed", walkSpeed.ToString("G6", CultureInfo.InvariantCulture)));
                if (shootRange != 0.0f)
                    result.Add(new XElement("shootRange", shootRange.ToString("G6", CultureInfo.InvariantCulture)));
                if (shootRangeOnCreature != 0.0f)
                    result.Add(new XElement("shootRangeOnCreature", shootRangeOnCreature.ToString("G6", CultureInfo.InvariantCulture)));
                if (npcWeapon != 0)
                    result.Add(new XElement("npcWeapon", Global.GetIntAsString(npcWeapon)));
                if (npcWeaponPrimary != 0)
                    result.Add(new XElement("npcWeaponPrimary", Global.GetIntAsString(npcWeaponPrimary)));
                if (npcWeaponPrimaryAttachItemIDs.Any())
                    result.Add(new XElement("npcWeaponPrimary", Global.GetListAsString(npcWeaponPrimaryAttachItemIDs)));
                if (npcWeaponPrimary2 != 0)
                    result.Add(new XElement("npcWeaponPrimary2", Global.GetIntAsString(npcWeaponPrimary2)));
                if (npcWeaponPrimary2AttachItemIDs.Any())
                    result.Add(new XElement("npcWeaponPrimary2AttachItemIDs", Global.GetListAsString(npcWeaponPrimary2AttachItemIDs)));
                if (npcWeaponSecondary != 0)
                    result.Add(new XElement("npcWeaponSecondary", Global.GetIntAsString(npcWeaponSecondary)));
                if (npcWeaponSecondaryAttachItemIDs.Any())
                    result.Add(new XElement("npcWeaponSecondaryAttachItemIDs", Global.GetListAsString(npcWeaponSecondaryAttachItemIDs)));
                if (NPCEquippedArmor.Any())
                    result.Add(new XElement("NPCEquippedArmor", NPCEquippedArmor));
                if (uniq) result.Add(new XElement("uniq", "1"));
                if (mobNoAggr) result.Add(new XElement("mobNoAggr", "1"));
                if (ignoreWAR > 0) result.Add(new XElement("ignoreWAR", Global.GetIntAsString(ignoreWAR)));
                return result;
            }
        }
    }

    public enum ItemAttribute { NORMAL, QUEST, USE }
    


    public class QuestItem
    {
        public int itemType = 0;
        public int count = 1;
        public ItemAttribute attribute = 0;
        public int questID = 0;
        public float condition = new float();

        public static bool hasQuestItem(List<QuestItem> list)
        {
            foreach (QuestItem item in list)
                if (item.attribute == ItemAttribute.QUEST) return true;
            return false;
        }
    }

   

    //! Награда за успешное выполнение квеста
    public class CQuestReward : ICloneable
    {
        //! Опыт:
        public int Experience;
        // Предметы
        public List<QuestItem> items;

        //! Денежное вознаграждение
        public float Credits;
        //! Словарь репутаций в награду, выглядит так <id фракции>:<значение награды>;
        public Dictionary<int, int> Reputation;
        public Dictionary<int, int> Reputation2;
        //! Словарь личной репутаций NPC в награду, выглядит так <имя NPC квеста>:<установленный статус>;
        public Dictionary<string, int> NPCReputation;
        //! Словарь изменений квеста в награду, выглядит так <id квеста>:<установленный статус>;
        public Dictionary<int, int> ChangeQuests;
        public List<int> GetKnowleges = new List<int>();
        public List<string> blackBoxes = new List<string>();
        public bool randomQuest = false;
        public int KarmaPK;
        public List<CEffect> Effects;
        public bool RewardWindow;
        public string teleportTo;
        public string dungeonTarget;
        public int dungeonTargetMode;
        public int OTfraction;
        public int OTvalue;
        public int clanPoints;

        public object Clone()
        {
            CQuestReward copy = new CQuestReward();
            copy.Experience = this.Experience;
            copy.items = new List<QuestItem>(this.items);
            copy.ChangeQuests = new Dictionary<int, int>(this.ChangeQuests);
            copy.Credits = this.Credits;
            copy.Reputation = new Dictionary<int, int>(this.Reputation);
            copy.Reputation2 = new Dictionary<int, int>(this.Reputation2);
            copy.NPCReputation = new Dictionary<string, int>(this.NPCReputation);
            copy.KarmaPK = this.KarmaPK;
            copy.Effects = new List<CEffect>(this.Effects);
            copy.RewardWindow = this.RewardWindow;
            copy.blackBoxes = new List<string>(this.blackBoxes);
            copy.GetKnowleges = new List<int>(GetKnowleges);
            copy.teleportTo = teleportTo;
            copy.dungeonTarget = dungeonTarget;
            copy.dungeonTargetMode = dungeonTargetMode;
            return copy;
        }

        public CQuestReward()
        {
            this.Experience = new int();
            this.items = new List<QuestItem>();
            this.Credits = new float();            
            this.Reputation = new Dictionary<int, int>();
            this.Reputation2 = new Dictionary<int, int>();
            this.NPCReputation = new Dictionary<string, int>();
            this.ChangeQuests = new Dictionary<int, int>();
            this.KarmaPK = new int();
            this.Effects = new List<CEffect>();
            this.RewardWindow = false;
            this.teleportTo = "";
            dungeonTarget = "";
            dungeonTargetMode = 1;
        }
        public bool Any()
        {
            return hasExperience() || items.Any() || Credits != 0 || ReputationNotEmpty() || teleportTo.Any() || OTvalue > 0 ||
                KarmaPK != 0 || Effects.Any() || RewardWindow || ChangeQuests.Any() || NPCReputation.Any() || blackBoxes.Any() ||
                Reputation2NotEmpty() || GetKnowleges.Any() || clanPoints != 0 || dungeonTarget.Any();
        }

        private bool hasExperience()
        {
            return Experience != 0;
        }

        public string getReputation(bool is_npc = false)
        {
            string result = "";
            if (!ReputationNotEmpty(is_npc))
                return result;
            if (is_npc)
            {
                foreach (string key in this.NPCReputation.Keys)
                {
                    if (this.NPCReputation[key] == 0)
                        continue;
                    if (!result.Equals(""))
                        result += ";";
                    result += (key.ToString() + ":" + this.NPCReputation[key].ToString());
                }
            }
            else
            {
                foreach (int key in this.Reputation.Keys)
                {
                    if (this.Reputation[key] == 0)
                        continue;
                    if (!result.Equals(""))
                        result += ";";
                    result += (key.ToString() + ":" + this.Reputation[key].ToString());
                }
            }

            return result;
        }

        public string getReputation2()
        {
            string result = "";

            foreach (int key in this.Reputation2.Keys)
            {
                if (this.Reputation2[key] == 0)
                    continue;
                if (!result.Equals(""))
                    result += ";";
                result += (key.ToString() + ":" + this.Reputation2[key].ToString());
            }


            return result;
        }

        public string getChangeQuests()
        {
            string result = "";
            if (ChangeQuests.Any())
            {
                foreach (int key in this.ChangeQuests.Keys)
                {
                    if (!result.Equals(""))
                        result += ";";
                    result += (key.ToString() + ":" + this.ChangeQuests[key].ToString());
                }
            }
            return result;
        }

        public bool ReputationNotEmpty(bool is_npc = false)
        {
            List<int> values;
            values = is_npc ? new List<int>(this.NPCReputation.Values) : new List<int>(this.Reputation.Values);
            foreach (int Value in values)
                if (Value != 0)
                    return true;
            return false;
        }

        public bool Reputation2NotEmpty()
        {
            foreach (int Value in this.Reputation2.Values)
                if (Value != 0)
                    return true;
            return false;
        }
    }

    public class CQuestPenalty : ICloneable
    {
        public List<int> Experience;
        public List<int> TypeOfItems;
        public List<int> NumOfItems;
        public float Credits;

        public object Clone()
        {
            CQuestPenalty copy = new CQuestPenalty();
            copy.Experience = new List<int>(this.Experience);
            copy.TypeOfItems = new List<int>(this.TypeOfItems);
            copy.NumOfItems = new List<int>(this.NumOfItems);
            copy.Credits = this.Credits;
            return copy;
        }

        public bool Any()
        {
            return Experience.Any() || TypeOfItems.Any() || NumOfItems.Any() || Credits != 0;
        }

        public CQuestPenalty()
        {
            this.Experience = new List<int>();
            this.TypeOfItems = new List<int>();
            this.NumOfItems = new List<int>();
            this.Credits = new float();
        }
    }

    public class CQuestAdditionalConditions : ICloneable
    {
        public int useWeaponType;
        public int pvpWinTeam;
        public int notDieCount;
        public int bePvpWinner; //0 - нет, 1 - среди своих, 2 - среди всех
        public int duration;

        public object Clone()
        {
            CQuestAdditionalConditions copy = new CQuestAdditionalConditions();
            copy.useWeaponType = useWeaponType;
            copy.pvpWinTeam = pvpWinTeam;
            copy.notDieCount = notDieCount;
            copy.bePvpWinner = bePvpWinner;
            copy.duration = duration;
            return copy;
        }

        public bool Any()
        {
            return (useWeaponType + pvpWinTeam + notDieCount + bePvpWinner + duration) > 0;
        }
    }
    
    public class CQuestAdditional : ICloneable
    {
        public int IsSubQuest;
        public List<int> ListOfSubQuest;
        public int ShowProgress;
        public bool CantCancel;
        public bool CantFail;
        public string Holder;
        public string DebugData;
        public int isFractionBonus;

        public object Clone()
        {
            CQuestAdditional copy = new CQuestAdditional();
            copy.IsSubQuest = this.IsSubQuest;
            copy.ListOfSubQuest = new List<int>(this.ListOfSubQuest);
            copy.ShowProgress = this.ShowProgress;
            copy.CantCancel = this.CantCancel;
            copy.CantFail = this.CantFail;
            copy.Holder = this.Holder;
            copy.DebugData = this.DebugData;
            copy.isFractionBonus = this.isFractionBonus;
            return copy;
        }

        public CQuestAdditional()
        {
            this.IsSubQuest = new int();
            this.ListOfSubQuest = new List<int>();
            this.ShowProgress = new int();
            this.CantCancel = false;
            this.Holder = "";
            this.DebugData = "";
            this.CantFail = false;
        }

        public CQuestAdditional(string Holder)
        {
            this.IsSubQuest = new int();
            this.ListOfSubQuest = new List<int>();
            this.ShowProgress = new int();
            this.CantCancel = false;
            this.Holder = Holder;
            this.DebugData = "";
    }

        public bool Any()
        {
            return ListOfSubQuest.Any() || IsSubQuest != 0 || ShowProgress != 0 || CantCancel || Holder != "" || DebugData != "" || CantFail || isFractionBonus > 0 ;
        }
    }

    public class QuestItemInfo : ICloneable
    {
        public string title;
        public string description;
        public string activation;
        public string content;

        public object Clone()
        {
            QuestItemInfo copy = new QuestItemInfo();

            copy.title = this.title;
            copy.description = this.description;
            copy.activation = this.activation;
            copy.content = this.content;
            return copy;
        }

        public QuestItemInfo()
        {
            this.title = "";
            this.description = "";
            this.activation = "";
            this.content = "";
        }

        public QuestItemInfo(string title, string description, string activation, string content)
        {
            this.title = title;
            this.description = description;
            this.activation = activation;
            this.content = content;
        }
    }

}