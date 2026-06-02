using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;

namespace StalkerOnlineQuesterEditor
{
    //! @todo: переделывать все нахер. 
    //! Класс констант квеста - типов. Лучше бы переделывать.
    public class CQuestConstants
    {
        List<СQuestType> ierarchyQuestsType = new List<СQuestType>();
        List<СQuestType> simpleQuestsType = new List<СQuestType>();
        List<СQuestType> pvpQuestsType = new List<СQuestType>();
        List<int> ontestQuestsType = new List<int>();

        public static int TYPE_FARM = 0; // собрать предметы
        public static int TYPE_FARM_AUTO = 16; // собранные предметы автоматически исчезают
        public static int TYPE_TALK = 1; // поговорить
        public static int TYPE_KILLMOBS_WITH_ONTEST = 2; // убить мобов со здачей НИП
        public static int TYPE_KILLMOBS = 3; // убить мобов
        public static int TYPE_AREA_DISCOVER = 4; // войти в зону
        public static int TYPE_MONEYBACK = 5; // заплатить НИП
        public static int TYPE_TRIGGER_ACTION = 6; // взаимодействвать с триггером
        public static int TYPE_QITEM_USE = 7; // использовать квестовый предмет
        public static int TYPE_AREA_LEAVE = 8; // покинуть зону
        public static int TYPE_TIMER = 9; // таймер

        public static int TYPE_DIE = 17; // умереть (отправиться на респ)
        public static int TYPE_GAME_EXIT = 18; // выйти из игры
        public static int TYPE_ITEM_EQIP = 19; // экипировать предмет

        public static int TYPE_GIVE_EFFECT = 21; // получить эффект
        public static int TYPE_REPUTATION = 22; // собрать количество репутации (со сдачей)
        public static int TYPE_REPUTATION_AUTO = 23; // автоматически закрывается при количестве репутации
        public static int TYPE_KILL = 24; // убить

        public static int TYPE_ITEM_CATEGORY = 25; // Собрать количество предметов категории.
        public static int TYPE_ITEM_CATEGORY_AUTO = 26; // Собрать количество предметов категории АВТО.
        public static int TYPE_HAVE_EFFECT = 27; // Находиться под действием эффекта
        public static int TYPE_IN_AREA = 28; // Находиться в зоне
        public static int TYPE_QUEST_COUNTER = 29;
        public static int TYPE_DUNGEON_EVENT = 30; //Событие происходит внутри подземелья
        public static int TYPE_CRAFT_ITEM = 31; //Событие создать предмет при помощи крафта
        public static int TYPE_CRAFT_ITEM_AUTO = 32; //Событие создать предмет при помощи крафта АВТО
        public static int TYPE_COOK_ITEM = 33; //Событие создать предмет при помощи готовки в котле
        public static int TYPE_COOK_ITEM_AUTO = 34; //Событие создать предмет при помощи  готовки в котле АВТО

        public static int TYPE_ANOMALY = 35;
        public static int TYPE_ANOMALY_AUTO = 36;

        public static int TYPE_KILLNPC = 37;
        public static int TYPE_KILLNPC_WITH_ONTEST = 38;

        //Эти квесты особые, у них есть доп условия
        public static int TYPE_PVP_MAP_KILL = 40; //Убить во время пвп матча
        public static int TYPE_PVP_MAP_CAPTURE_FLAG = 41; //Сделать что-то над флагом в пвп карте
        public static int TYPE_PVP_MAP_SCORE = 42; //Сделать набрать очки в пвп карте
        public static int TYPE_B2C_KILL = 43; //Убить во время захвата базы
        public static int TYPE_B2C_FLAG = 44; //Захватить флаг во время захвата базы
        public static int TYPE_B2C_REVIVE = 46; //Оживить игрока во время захвата

        public static int TYPE_DUNGEON_BOX_COUNTER = 45; // собрать коробки внутри танка в данже
        public static int TYPE_LEAVE_MAP = 47;
        public static int TYPE_ENTER_MAP = 48;
        public static int TYPE_CREATE_NPC = 51; // создать бегающего НИП
        public static int TYPE_CREATE_MOB = 52; // создать моба
        public static int TYPE_KILLSCENARIONPC = 53; // убить НИП созданного квестом
        public static int TYPE_KILLSCENARIONPC_WITH_ONTEST = 54; // убить НИП со сдачей

        public static int TYPE_ENTITY_SEEN = 55; // Увидеть сущность QuestEntity/TimeEntity/Creature
        public static int TYPE_ENTITY_SEEN_AUTO = 56; // Увидеть сущность QuestEntity/TimeEntity/Creature

        public CQuestConstants()
        {
            simpleQuestsType.Add(new СQuestType(TYPE_FARM, "0  Собрать нужное кол-во предметов (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_FARM_AUTO, "16 Собрать нужное кол-во предметов (--> Close)"));
            simpleQuestsType.Add(new СQuestType(TYPE_TALK, "1  Поговорить."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLMOBS_WITH_ONTEST, "2  Убить нужное кол-во мобов (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLMOBS, "3  Убить нужное кол-во мобов (--> Close)"));
            simpleQuestsType.Add(new СQuestType(TYPE_AREA_DISCOVER, "4  Посещение зоны."));
            simpleQuestsType.Add(new СQuestType(TYPE_MONEYBACK, "5  Сдача денег."));
            simpleQuestsType.Add(new СQuestType(TYPE_TRIGGER_ACTION, "6  Действие над триггером."));
            simpleQuestsType.Add(new СQuestType(TYPE_QITEM_USE, "7  Использовать предмет."));
            simpleQuestsType.Add(new СQuestType(TYPE_AREA_LEAVE, "8  Покинуть зону."));
            simpleQuestsType.Add(new СQuestType(TYPE_TIMER, "9  Таймер."));
            simpleQuestsType.Add(new СQuestType(TYPE_DIE, "17 Умереть."));
            simpleQuestsType.Add(new СQuestType(TYPE_GAME_EXIT, "18 Выйти из игры."));
            simpleQuestsType.Add(new СQuestType(TYPE_ITEM_EQIP, "19 Экипировка предмета."));
            //simpleQuestsType.Add(new СQuestType(TYPE_ITEM_ADD, "20 Добавление предмета."));
            simpleQuestsType.Add(new СQuestType(TYPE_GIVE_EFFECT, "21 Получение эффекта."));
            //simpleQuestsType.Add(new СQuestType(TYPE_REPUTATION, "22 Необходимое количество репутации."));
            //simpleQuestsType.Add(new СQuestType(TYPE_REPUTATION_AUTO, "23 Необходимое количество репутации АВТО."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILL, "24 Убийство."));
            //simpleQuestsType.Add(new СQuestType(TYPE_ITEM_CATEGORY, "25 Собрать количество предметов категории."));
            //simpleQuestsType.Add(new СQuestType(TYPE_ITEM_CATEGORY_AUTO, "26 Собрать количество предметов категории АВТО."));
            simpleQuestsType.Add(new СQuestType(TYPE_HAVE_EFFECT, "27 Находиться под действием эффекта (Open <--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_IN_AREA, "28 Находиться в зоне (Open <--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_QUEST_COUNTER, "29 Счетчик квестов."));
            simpleQuestsType.Add(new СQuestType(TYPE_DUNGEON_EVENT, "30 [Данж] Завершить фазу данжа"));
            simpleQuestsType.Add(new СQuestType(TYPE_CRAFT_ITEM, "31 Событие крафта (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_CRAFT_ITEM_AUTO, "32 Событие крафта (--> Close)"));
            simpleQuestsType.Add(new СQuestType(TYPE_COOK_ITEM, "33 Событие готовки в котле (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_COOK_ITEM_AUTO, "34 Событие готовки в котле (--> Close)"));

            simpleQuestsType.Add(new СQuestType(TYPE_ANOMALY, "35 Достать арт из аномалии (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_ANOMALY_AUTO, "36 Достать арт из аномалии (--> Close)"));

            simpleQuestsType.Add(new СQuestType(TYPE_KILLNPC, "37 Убить нужное кол-во NPC (--> Close)"));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLNPC_WITH_ONTEST, "38 Убить нужное кол-во NPC (--> OnTest)"));

            pvpQuestsType.Add(new СQuestType(TYPE_PVP_MAP_KILL, "40 [ПВП-арена] Убить игроков"));
            pvpQuestsType.Add(new СQuestType(TYPE_PVP_MAP_CAPTURE_FLAG, "41 [ПВП-арена] Действие с флагом"));
            pvpQuestsType.Add(new СQuestType(TYPE_PVP_MAP_SCORE, "42 [ПВП-арена] Заработать очки"));
            pvpQuestsType.Add(new СQuestType(TYPE_B2C_KILL, "43 [Захват баз] Убить противников"));
            pvpQuestsType.Add(new СQuestType(TYPE_B2C_FLAG, "44 [Захват баз] Поднять флаги"));
            pvpQuestsType.Add(new СQuestType(TYPE_B2C_REVIVE, "46 [Захват баз] Оживить союзников"));

            simpleQuestsType.Add(new СQuestType(TYPE_DUNGEON_BOX_COUNTER, "45 [Данж] Погрузить нужное кол-во коробок"));
            simpleQuestsType.Add(new СQuestType(TYPE_LEAVE_MAP, "47 Покинуть карту"));
            simpleQuestsType.Add(new СQuestType(TYPE_ENTER_MAP, "48 Попасть на карту"));
            // ierarchyQuestsType.Add(new СQuestType(50, "50 Игра против режиссера."));
            simpleQuestsType.Add(new СQuestType(TYPE_CREATE_NPC, "51 [Сценарий] Создать NPC"));
            simpleQuestsType.Add(new СQuestType(TYPE_CREATE_MOB, "52 [Сценарий] Создать моба"));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLSCENARIONPC, "53 [Сценарий] Убить созданного NPC (--> Close)"));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLSCENARIONPC_WITH_ONTEST, "54 [Сценарий] Убить созданного NPC (--> OnTest)"));

            simpleQuestsType.Add(new СQuestType(TYPE_ENTITY_SEEN, "55 Увидеть сущность (--> OnTest)"));
            simpleQuestsType.Add(new СQuestType(TYPE_ENTITY_SEEN_AUTO, "56 Увидеть сущность (--> Close)")); 

            



            ierarchyQuestsType.Add(new СQuestType(10,  "10 Выполнить подквесты в любом порядке (--> Close)"));
            ierarchyQuestsType.Add(new СQuestType(11,  "11 Выполнить подквесты по порядку (--> Close)"));
            ierarchyQuestsType.Add(new СQuestType(12, "12 Выполнить один подквест на выбор (--> Close)"));
            ierarchyQuestsType.Add(new СQuestType(13, "13  Выполнить подквесты в любом порядке (--> OnTest)"));
            ierarchyQuestsType.Add(new СQuestType(14, "14 Выполнить подквесты по порядку (--> OnTest)"));
            ierarchyQuestsType.Add(new СQuestType(15, "15 Выполнить один подквест на выбор (--> OnTest)"));
            ierarchyQuestsType.Add(new СQuestType(100, "100 Проверить все дочерние квесты."));

            ontestQuestsType.Add(TYPE_FARM);
            ontestQuestsType.Add(TYPE_KILLMOBS_WITH_ONTEST);
            ontestQuestsType.Add(13);
            ontestQuestsType.Add(14);
            ontestQuestsType.Add(15);
            ontestQuestsType.Add(TYPE_HAVE_EFFECT);
            ontestQuestsType.Add(TYPE_IN_AREA);
            ontestQuestsType.Add(TYPE_CRAFT_ITEM);
            ontestQuestsType.Add(TYPE_COOK_ITEM);
            ontestQuestsType.Add(TYPE_KILLNPC_WITH_ONTEST);
            ontestQuestsType.Add(TYPE_KILLSCENARIONPC_WITH_ONTEST);
            ontestQuestsType.Add(TYPE_ENTITY_SEEN_AUTO);
        }
        public bool isSimple(int questType)
        {
            foreach(СQuestType quest in simpleQuestsType)
                if (quest.getType().Equals(questType))
                    return true;
            return isPVPQuest(questType);
        }

        public bool isPVPQuest(int questType)
        {
            foreach (СQuestType quest in pvpQuestsType)
                if (quest.getType() == questType)
                    return true;
            return false;
        }

        public bool isOnTest(int questType)
        {
            return ontestQuestsType.Contains(questType);
        }

        public string getDescription(int questType)
        {
            foreach (СQuestType quest in simpleQuestsType)
                if (quest.getType().Equals(questType))
                    return quest.getDescription();

            foreach (СQuestType quest in pvpQuestsType)
                if (quest.getType().Equals(questType))
                    return quest.getDescription();
            

            foreach (СQuestType quest in ierarchyQuestsType)
                if (quest.getType().Equals(questType))
                    return quest.getDescription();
            return "";
        }

        public List<СQuestType> getListQuests()
        {
            List<СQuestType> ret = new List<СQuestType>();
            ret.AddRange(ierarchyQuestsType);
            ret.AddRange(simpleQuestsType);
            ret.AddRange(pvpQuestsType);
            return ret;
        }

        public int getQuestTypeOnDescription(string description)
        {
            foreach (СQuestType type in simpleQuestsType)
                if (type.getDescription().Equals(description))
                    return type.getType();
         
            foreach (СQuestType type in pvpQuestsType)
                if (type.getDescription().Equals(description))
                    return type.getType();
            foreach (СQuestType type in ierarchyQuestsType)
                if (type.getDescription().Equals(description))
                    return type.getType();
            return 0;
        }
    }

    public class СQuestType
    {
        int QuestType;
        string Description;

        public СQuestType(int typeID, string Description)
        {
            this.QuestType = typeID;
            this.Description = Description;
        }

        public string getDescription()
        {
            return Description;
        }

        public int getType()
        {
            return QuestType;
        }
    }

    public class BillboardQuests
    {
        protected List<int> _constants;

        public BillboardQuests()
        {
            _constants = new List<int>();
            System.Xml.Linq.XDocument doc;
            try
            {
                doc = System.Xml.Linq.XDocument.Load("source/QuestsInBoard.xml");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath("source/QuestsInBoard.xml"), "Ошибка");
                return;
            }
            foreach (System.Xml.Linq.XElement item in doc.Root.Elements())
            {
                int quest_id = 0;
                if (int.TryParse(item.Value, out quest_id))
                    _constants.Add(quest_id);
            }
        }

        public List<int> getKeys()
        {
            return _constants;
        }
    }

    public class QuestsOmnicounter
    {
        protected static List<int>  _quests = new List<int>();
        public static string JSON_PATH = "../../../res/scripts/base/data/omnicounter.json";

        public static void load()
        {

            JsonTextReader reader;
            try
            {
                reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Ошибка чтения файла: ../../../res/scripts/base/data/omnicounter.json", "Ошибка");
                return;
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value.ToString();
                    int quest_type;
                    if (int.TryParse(name, out quest_type))
                        _quests.Add(quest_type);
                }
            }

        }

        public static bool isOmni(int questId)
        {
            return _quests.Contains(questId);
        }

    }

    public class QuestsInMassQuestsReward
    {
        protected List<int> _quests = new List<int>();
        public static string JSON_PATH = "../../../res/scripts/server_data/massquest_config.json";
        JsonTextReader reader;

        public QuestsInMassQuestsReward()
        {
            reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            bool inName = false;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value.ToString();
                    if (name == "quest_open") inName = true;
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (inName) inName = false;
                }
                else if (reader.TokenType == JsonToken.Integer)
                    if (inName)
                    {
                        inName = false;
                        int id = Convert.ToInt32(reader.Value);
                        if (!_quests.Contains(id))
                            _quests.Add(id);
                    }
            }
            reader.Close();
        }
        public List<int> getKeys()
        {
            return _quests;
        }
    }

    public static class QuestPriorities
    {
        static Dictionary<int, string> priorities;

        public static Dictionary<int, string> data()
        {
            if (priorities != null) return priorities;
            priorities = new Dictionary<int, string>();
            parseFractions("source/QuestPriority.xml");
            return priorities;
        }

        static void parseFractions(string path)
        {
            XDocument doc = XDocument.Load(path);
            foreach (XElement item in doc.Root.Elements())
            {
                int id = int.Parse(item.Element("id").Value);
                string name = item.Element("name").Value.ToString();
                priorities.Add(id, name);
            }
        }

        public static int getIDByName(string name)
        {
            foreach (KeyValuePair<int, string> pair in data())
            {
                if (pair.Value == name) return pair.Key;
            }
            return -1;
        }

        public static string getNameByID(int frac_id)
        {
            foreach (KeyValuePair<int, string> pair in data())
            {
                if (pair.Key == frac_id) return pair.Value;
            }
            return "";
        }

        public static string[] getListNames()
        {
            return data().Values.ToArray();
        }
    }



    public static class QuestPVPConstance
    {

        public class Data
        {
            Dictionary<int, string> data = new Dictionary<int, string>();


            public int getIDByName(string name)
            {
                foreach (KeyValuePair<int, string> pair in data)
                {
                    if (pair.Value == name) return pair.Key;
                }
                return 0;
            }

            public string getNameByID(int frac_id)
            {
                foreach (KeyValuePair<int, string> pair in data)
                {
                    if (pair.Key == frac_id) return pair.Value;
                }
                return "";
            }

            public string[] getListNames()
            {
                return data.Values.ToArray();
            }

            public void Add(int id, string name)
            {
                data.Add(id, name);
            }
        }

        public static Data captureTheFlagTypes = new Data();
        public static Data bestTypes = new Data();
        public static Data targetCountTypes = new Data();

        public static void parse()
        {
            string path = "source/QuestPVPParam.xml";

            XDocument doc = XDocument.Load(path);
            foreach (XElement item in doc.Root.Elements())
            {
                foreach(XElement item2 in item.Elements())
                {
                    int id = int.Parse(item2.Element("id").Value);
                    string name = item2.Element("name").Value.ToString();
                    switch (item.Name.ToString())
                    {
                        case "captureTheFlag": captureTheFlagTypes.Add(id, name); break;
                        case "bestWin": bestTypes.Add(id, name); break;
                        case "targetCount": targetCountTypes.Add(id, name); break;
                    }
                }  
            }
        }
        
    }


    public static class TimeEntityModels
    {
        static Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();
        public static void parse()
        {
            string JSON_PATH = "../../../res/scripts/client/data/TimeEntitiesConfigs/";
            string[] files = Directory.GetFiles(JSON_PATH, "*.json");
            bool is_model = false;

            foreach (string configFile in files)
            {
                if (!File.Exists(configFile))
                    return;
                string configName = Path.GetFileNameWithoutExtension(configFile);
                List<string> modelsList = new List<string>();
                JsonTextReader reader = new JsonTextReader(new StreamReader(configFile, Encoding.UTF8));
                while (reader.Read())
                {

                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        if (reader.Value.ToString() == "model")
                            is_model = true;
                        else is_model = false;
                    }

                    if (reader.TokenType == JsonToken.String)
                    {
                        if (!is_model) continue;
                        string modelPath = reader.Value.ToString().Trim();
                        if (!modelPath.Any())
                            continue;
                        if (modelsList.Contains(modelPath)) continue;
                        modelsList.Add(modelPath);

                    }
                }
                _data.Add(configName, modelsList);
            }
        }

        public static List<string> getModelsByConfig(string configName)
        {
            foreach (var pair in _data)
            {
                if (pair.Key == configName) return pair.Value;
            }
            return null;
        }


        public static string[] getConfigNames()
        {
            return _data.Keys.ToArray();
        }
    }

    public static class AnomalyTypes
    {

        static Dictionary<string, string> anomalies = new Dictionary<string, string>();
        static Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
        static Dictionary<string, List<int>> loot = new Dictionary<string, List<int>>();

        public static void parse()
        {
            string JSON_PATH = "../../../res/scripts/common/data/Anomalies/__config__.json";

            JsonTextReader reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            bool init = false;
            string key = "", name = "";
            List<string> tmp = new List<string>();
            while (reader.Read())
            {

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (!init)
                    {
                        if ("category_types" == reader.Value.ToString())
                            init = true;
                        continue;
                    }
                    else
                    {
                        key = reader.Value.ToString();
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    if (init) { init = false; break; }
                }
                else if (reader.TokenType == JsonToken.String)
                {
                    if (!init) continue;

                    name = reader.Value.ToString();
                    tmp.Add(name);
                    
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    if (!init) continue;
                    name = tmp[0].Split('_')[0];
                    categories.Add(name, tmp);
                    tmp = new List<string>();
                    anomalies.Add(key, name);

                }
            }
            reader.Close();

            readLoot();
        }

        static void readLoot()
        {
            string JSON_PATH = "source/anomaly_loot.json";
            JsonTextReader reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            string key = "";
            List<int> items = new List<int>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (key.Any())
                        loot.Add(key, items);
                    key = reader.Value.ToString();
                    items = new List<int>();
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    items.Add(Convert.ToInt32(reader.Value));
                }
            }
        }

        public static List<int> getAnomalyLoot(string name)
        {
            List<int> result = new List<int>();
            foreach(var i in categories[name])
            {
                if (!loot.ContainsKey(i)) continue;
                foreach(var item in loot[i])
                {
                    if (result.Contains(item)) continue;
                    result.Add(item);
                }
            }
            return result;
        }


        public static string getIDByName(string name)
        {
            foreach (KeyValuePair<string, string> pair in anomalies)
            {
                if (pair.Value == name) return pair.Key;
            }
            return "";
        }

        public static string getNameByID(string key)
        {
            if (!anomalies.ContainsKey(key))
                return "";
            return anomalies[key];
        }

        public static string[] getListNames()
        {
            return anomalies.Values.ToArray();
        }
    }
}
