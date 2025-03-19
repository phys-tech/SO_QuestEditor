using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace StalkerOnlineQuesterEditor
{
    //! Класс, содержащий данные о территориях, которые нужно посетить по квестам
    public class CZoneConstants
    {
        //! Словарь ID территории (mark в xml файле) - Имя территории (по-русски, для геймдевов)
        protected Dictionary<string, CZoneDescription> zones;

        //! Конструктор, создает словарь на основе xml файлов areas и AllAreas
        public CZoneConstants()
        {
            zones = new Dictionary<string, CZoneDescription>();
           
            // добавление неназванных зон из AllAreas.xml - создается парсером по всем spaces
            XDocument allAreas = XDocument.Load("source/AllAreas.xml");
            foreach(XElement item in allAreas.Root.Elements())
            {
                string id = item.Element("mark").Value.ToString().Trim();
                string space = item.Element("space").Value.ToString().Trim();
                string position = item.Element("position").Value.ToString().Trim();
                List<int> quests = new List<int>();
                string[] q;
                if (item.Element("quests") != null)
                {
                    q = item.Element("quests").Value.ToString().Trim().Split(' ');
                    foreach (string i in q)
                    {
                        if (i != "0")
                            quests.Add(Convert.ToInt32(i));
                    }
                }
                if (!zones.ContainsKey(id))
                {
                    zones.Add(id, new CZoneDescription(id, quests, space, position));
                }
                else
                {
                    zones[id].addQuests(quests);
                }
            }
        }

        public bool checkAreaGiveQuestByID(int quest_id)
        {
            foreach (CZoneDescription area in zones.Values)
            {
                List<int> area_quest;
                area_quest = area.getQuests();
                if (area_quest == null) continue;
                if (area_quest.Count == 0) continue;
                if (area_quest.Contains(quest_id))
                    return true;
            }
            return false;
        }

        public List<string> getAreasGiveQuestByID(int quest_id)
        {
            List<string> result = new List<string>();
            foreach (CZoneDescription area in zones.Values)
            {
                List<int> area_quest;
                area_quest = area.getQuests();
                if (area_quest == null) continue;
                if (area_quest.Count == 0) continue;
                if (area_quest.Contains(quest_id))
                    result.Add(area.getSpace() + " " + area.getPos());
            }
            return result;
        }

        public bool checkHaveArea(string key)
        {
            return zones.ContainsKey(key.Trim());
        }

        //! Возвращает словарь всех территорий
        public Dictionary<string, CZoneDescription> getAllZones()
        {
            return zones;
        }


        //! Возвращает описание территории по-русски по ее ключу mark
        public CZoneDescription getDescriptionOnKey(string key)
        {
            //System.Console.WriteLine("key:" + key);
            if (!zones.ContainsKey(key.Trim()))
                return new CZoneDescription("");
            return zones[key.Trim()];
        }
        //! Возвращает ключ по описанию территории
        public string getKeyOnDescription(string description)
        {
            foreach (string key in zones.Keys)
                if (zones[key].getName().Equals(description))
                    return key;
            return "";
        }
    }

    public class CZoneMobConstants : CZoneConstants
    {
        public CZoneMobConstants()
        {
            zones = new Dictionary<string, CZoneDescription>();
            if (!File.Exists("source/MobAreas.xml"))
                return;
            zones.Add("", new CZoneDescription("", new List<int>()));
            XDocument mobAreas = XDocument.Load("source/MobAreas.xml");
            foreach (XElement item in mobAreas.Root.Elements())
            {
                string id = item.Element("mark").Value.ToString().Trim();
                List<int> quests = new List<int>();
                string[] q;
                if (item.Element("quests") != null)
                {
                    q = item.Element("quests").Value.ToString().Trim().Split(' ');
                    foreach (string i in q)
                    {
                        if (i != "0")
                            quests.Add(Convert.ToInt32(i));
                    }
                }
                if (!zones.ContainsKey(id))
                    zones.Add(id, new CZoneDescription(id, quests));
                else { zones[id].addQuests(quests); }

            }

        }
    }
    //! Класс описания территории для посещения. Имеет только поле имя. Что за пиздец???
    public class CZoneDescription
    {
        string Name;
        string space;
        string position;
        List<int> quests;

        public CZoneDescription(string name, List<int> quests = null, string space = "", string position = "")
        {
            this.Name = name;
            this.quests = quests;
            this.position = position;
            this.space = space; 
        }

        public string getName()
        {
            return this.Name;
        }

        public List<int> getQuests()
        {
            return quests;
        }

        public string getSpace()
        {
            return this.space;
        }

        public string getPos()
        {
            return position;
        }


        public void addQuests(List<int> value)
        {
            foreach(int quest_id in value)
            {
                if (quests.Contains(quest_id)) continue;
                quests.Add(quest_id);
            }
        }
    }
}
