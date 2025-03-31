using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StalkerOnlineQuesterEditor
{
    public struct npc_data
    {
        public string rusName;
        public string engName;
        public string location;
        public string coordinates;
        public string nature; //характер персонажа

        public npc_data(string rn, string en, string loc, string cd)
        {
            rusName = rn;
            engName = en;
            location = loc;
            coordinates = cd;
            nature = "";
        }
    }

    public static class NPCAdditionalData
    {
        public static Dictionary<int, string> nature_id_to_name = new Dictionary<int, string>();
        public static Dictionary<string, int> npc_natures = new Dictionary<string, int>();

        public static Dictionary<string, List<int>> npc_groups  = new Dictionary<string, List<int>>();

        enum PropType { none = 0, nature = 1, group = 2 };

        public static void load_data()
        {
            string JSON_PATH = "../../../res/scripts/common/data/AdditionalNPCParametersData.json";

            XDocument doc = XDocument.Load("source/NPCNature.xml");
            foreach (XElement item in doc.Root.Elements())
            {
                try
                {
                    nature_id_to_name.Add(int.Parse(item.Element("id").Value.ToString()), item.Element("name").Value.ToString());
                }
                catch
                {
                    System.Console.WriteLine("Error with item category:" + item.Element("id").Value.ToString());
                }
            }


            JsonTextReader reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            bool inNPC = false;

            PropType it_is = PropType.none;

            var name = "";
            int value = -1;
            npc_natures = new Dictionary<string, int>();
            npc_groups = new Dictionary<string, List<int>>();
            List<int> groups = new List<int>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (!inNPC)
                    {
                        name = reader.Value.ToString();
                        inNPC = true;
                    }
                    else if ("nature" == reader.Value.ToString())
                        it_is = PropType.nature;
                    else if ("groups" == reader.Value.ToString())
                        it_is = PropType.group;
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {

                    if (inNPC) inNPC = false;
                    if (!name.Any()) continue;
                    if (value < 0)
                        value = npc_natures["!!default"];
                    npc_natures.Add(name, value);
                    name = "";

                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    if (it_is == PropType.nature)
                    {
                        it_is = PropType.none;
                        value = Convert.ToInt32(reader.Value);
                    }
                    if (it_is == PropType.group)
                        groups.Add(Convert.ToInt32(reader.Value));
                }
                else if (reader.TokenType == JsonToken.EndArray)
                    if (it_is == PropType.group)
                    {
                        it_is = PropType.none;
                        npc_groups.Add(name, groups);
                        groups = new List<int>();
                    }
            }
            reader.Close();
       }

       public static int getNatureByName(string name)
        {
            foreach (var i in nature_id_to_name)
                if (name == i.Value) return i.Key;
            return -1;
        }


        public static string[] getNatures()
        {
            return nature_id_to_name.Values.ToArray();
        }
       public static List<int> getGroupsByName(string name)
        {
            if (npc_groups.ContainsKey(name))
                return npc_groups[name];
            return new List<int>();
        }

    }

    public class CManagerNPC
    {
        //! Словарь информации об NPC - локализованное имя, карта, координаты
        public Dictionary<string, npc_data> NpcData = new Dictionary<string, npc_data>();
        //! Словарь соответствия русское имя - общее имя в игре (для поиска в комбобоксе NPCBox)
        public Dictionary<string, string> rusNamesToNPC = new Dictionary<string, string>();
        //! Словарь cоответствия англ имя - общее имя в игре (для поиска в комбобоксе NPCBox)
        public Dictionary<string, string> engNamesToNPC = new Dictionary<string, string>();
        public Dictionary<string, List<string>> mapToNPCList = new Dictionary<string, List<string>>();

        //! Список всех локаций
        public List<string> locationNames = new List<string>();
        public CManagerNPC()
        {

            NPCAdditionalData.load_data();
            parseNpcLocationFile("npc_stat.xml");
        }

        public Dictionary<string, bool> getSpaces()
        {
            Dictionary<string, bool> data = new Dictionary<string, bool>();
            foreach(string spaceName in locationNames)
            {
                data.Add(spaceName, true);
            }
            data.Add("no map", true);
            return data;
        }

        public string getSpaceByNPCname(string npcname)
        {
            if (NpcData.ContainsKey(npcname))
            {
                return NpcData[npcname].location;
            }
            return "";
        }
        
        //! Парсит файл с местонахождением NPC
        private void parseNpcLocationFile(string fileName)
        {
            if (!File.Exists(fileName))
                return;
            XDocument doc;
            try
            {
                doc = XDocument.Load(fileName);
            }
            catch
            {
                MessageBox.Show("Не могу прочитать файл npc_stat.xml", caption: "Ошибка чтения");
                return;
            }
            foreach (XElement item in doc.Root.Elements())
            {
                string name = item.Element("Name").Value.ToString();
                string map = item.Element("map").Value.ToString();
                string rusName = item.Element("npcLocal").Value.ToString();
                string engName = item.Element("npcEngName").Value.ToString();
                string coord = item.Element("coord").Value.ToString();
                string nature;
                if (NPCAdditionalData.npc_natures.ContainsKey(name))
                {
                    nature = NPCAdditionalData.nature_id_to_name[NPCAdditionalData.npc_natures[name]];
                }
                else nature = NPCAdditionalData.nature_id_to_name[0];

                if (!NpcData.ContainsKey(name))
                    NpcData.Add(name, new npc_data(rusName, engName, map, coord));
                if (!mapToNPCList.ContainsKey(map))
                    mapToNPCList.Add(map, new List<string>());
                mapToNPCList[map].Add(name);

                if (!locationNames.Contains(map))
                    locationNames.Add(map);
                if (!rusNamesToNPC.ContainsKey(rusName))
                    rusNamesToNPC.Add(rusName, name);
                if (!engNamesToNPC.ContainsKey(engName))
                    engNamesToNPC.Add(engName, name);

            }
        }
    }

    //! Класс объекта, хранящий данные о русском и английском имени NPC 
    public class NPCNameDataSourceObject : IComparable
    {
        //! Имя NPC по-английски
        private string value;
        //! Имя NPC по-русски
        private string displayString;

        public NPCNameDataSourceObject(string _value, string _display)
        {
            value = _value;
            displayString = _display;
        }

        public string DisplayString
        {
            get { return displayString; }
        }

        public string Value
        {
            get { return value; }
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            NPCNameDataSourceObject otherNPCName = obj as NPCNameDataSourceObject;
            if (otherNPCName != null)
                return this.Value.CompareTo(otherNPCName.Value);
            else
                throw new ArgumentException("Object is not an NPC name");
        }
    };

}
