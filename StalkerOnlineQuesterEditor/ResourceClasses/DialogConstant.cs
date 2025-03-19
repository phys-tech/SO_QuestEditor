using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StalkerOnlineQuesterEditor
{
    public abstract class DialogConstants
    {
        //Константы для настройки клановых опций
        public static DialogConstant CLANOPT_MEMBER = new DialogConstant(1, "соклановец");
        public static DialogConstant CLANOPT_NOTMEMBER = new DialogConstant(2, "не_соклановец");
        public static DialogConstant CLANOPT_ENEMY = new DialogConstant(3, "враг");
        public static DialogConstant CLANOPT_NOTENEMY = new DialogConstant(4, "не_враг");
        public static DialogConstant CLANOPT_PEACE = new DialogConstant(5, "мирное_время");
        public static DialogConstant CLANOPT_CAPTURE = new DialogConstant(6, "время_захвата");
        public static DialogConstant CLANOPT_ANYCLAN = new DialogConstant(7, "клан");
        public static DialogConstant CLANOPT_SINGLE = new DialogConstant(8, "бесклановый_одиночка");


        public static int getNumByText(string text)
        {
            return 0;
        }

        public static string getTextByNum(int num)
        {
            return "";
        }

    }

    public sealed class DialogConstant
    {
        int num;
        public string text;


        public DialogConstant(int num, string text)
        {
            this.num = num;
            this.text = text;
        }

        public string getText()
        {
            return text;
        }

        public int getNum()
        {
            return num;
        }
    }


    public class Constants
    {
        protected Dictionary<string, string> _constants = new Dictionary<string, string>();
        protected XDocument doc = new XDocument();

        //! Конструктор, заполняет словарь на основе файлов xml
        public Constants()
        {

        }
        //! Возвращает ID комманды
        public string getTtID(string name)
        {
            return _constants[name];
        }
        //! Возвращает название по ID комманды
        public string getName(string tpID)
        {
            string ret = "";
            foreach (KeyValuePair<string, string> value in _constants)
                if (value.Value.Equals(tpID))
                    ret = value.Key;
            return ret;
        }
        //! Возвращает список всех названий по-русски комманд
        public List<string> getKeys()
        {
            List<string> ret = new List<string>();
            foreach (string key in _constants.Keys)
                ret.Add(key);
            return ret;
        }

        protected void loadFile(string path)
        {
            try
            {
                doc = XDocument.Load(path);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath(path), "Ошибка");
            }


        }
    }

    public class Items
    {
        protected Dictionary<int, string> _constants = new Dictionary<int, string>();
        public Items() { }

        public void Add(int id, string name)
        {
            if (_constants.ContainsKey(id)) return;
            _constants.Add(id, name);
        }

        public string getNameByID(int id)
        {
            if (!_constants.ContainsKey(id)) return "";
            return _constants[id];
        }

        public int getIDByName(string name)
        {
            int ret = 0;
            foreach (KeyValuePair<int, string> value in _constants)
                if (value.Value.Equals(name))
                    ret = value.Key;
            return ret;
        }

        public List<int> getIDs()
        {
            List<int> ret = new List<int>();
            foreach (int key in _constants.Keys)
                ret.Add(key);
            return ret;
        }

        public List<string> getNames()
        {
            List<string> ret = new List<string>();
            foreach (string value in _constants.Values)
                ret.Add(value);
            return ret;
        }
    }


    public class RepairConstants : Constants
    {
        //Dictionary<string, string> _constants = new Dictionary<string, string>();
        //! Конструктор, заполняет словарь на основе файлов xml
        public RepairConstants()
        {
            loadFile("source/NPCrepairs.xml");
            foreach (XElement item in doc.Root.Elements())
            {
                string cmID = item.Element("id").Value;
                string name = item.Element("name").Value;
                _constants.Add(name, cmID);
            }
        }
    }

    //! Класс, содержащий информацию командах для NPC
    public class CommandConstants : Constants
    {
        //Dictionary<string, string> _constants = new Dictionary<string, string>();
        //! Конструктор, заполняет словарь на основе файлов xml
        public CommandConstants()
        {
            loadFile("source/NPCcommands.xml");
            foreach (XElement item in doc.Root.Elements())
            {
                item.Name.ToString();
                string cmID = item.Element("id").Value;
                string name = item.Element("name").Value;
                _constants.Add(name, cmID);
            }
        }
    }

    //! Класс, содержащий информацию предметах для NPC
    public class NPCItems
    {
        public Items primaryWeapons = new Items();
        public Items secondaryyWeapons = new Items();
        public Items body = new Items();
        //! Конструктор, заполняет словарь на основе файлов xml

        public NPCItems()
        {
            XDocument doc;
            try
            {
                doc = XDocument.Load("source/NPCItems.xml");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath("source/NPCItems.xml"), "Ошибка");
                return;
            }
            foreach (XElement items in doc.Root.Elements())
            {
                string items_name = items.Name.ToString();

                foreach (XElement item in items.Elements())
                {
                    int ID = Convert.ToInt32(item.Element("id").Value.ToString());

                    string name = item.Element("name").Value;
                    switch (items_name)
                    {
                        case "NPC_BODY": body.Add(ID, name); break;
                    }
                }

            }
            if (!System.IO.File.Exists("source/ItemWeapons.xml"))
            {
                System.Windows.Forms.MessageBox.Show("Отсуствует файл ItemWeapons.xml, нужно распарсить предметы", "Ошибка");
                return;
            }
            try
            {
                doc = XDocument.Load("source/ItemWeapons.xml");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath("source/ItemWeapons.xml"), "Ошибка");
                return;
            }
            foreach (XElement items in doc.Root.Elements())
            {
                string weapon_type = items.Name.ToString();

                foreach (XElement item in items.Elements())
                {
                    int ID = Convert.ToInt32(item.Element("id").Value.ToString());

                    string name = item.Element("Name").Value;
                    switch (weapon_type)
                    {
                        case "PRIMARY_WEAPONS": primaryWeapons.Add(ID, name); break;
                        case "SECONDARY_WEAPONS": secondaryyWeapons.Add(ID, name); break;
                    }
                }

            }
        }
    }

    public class AvatarActions
    {
        protected List<string> _constants;

        public AvatarActions()
        {
            _constants = new List<string>();
            XDocument doc;
            try
            {
                doc = XDocument.Load("source/AvatarActions.xml");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath("source/AvatarActions.xml"), "Ошибка");
                return;
            }
            foreach (XElement item in doc.Root.Elements())
            {
                string name = item.Value;
                _constants.Add(name);
            }
        }

        public List<string> getKeys()
        {
            return _constants;
        }
    }

    public class NPCActions : Constants
    {
        public NPCActions()
        {
            loadFile("source/NPCDialogActions.xml");
            foreach (XElement item in doc.Root.Elements())
            {
                item.Name.ToString();
                string cmID = item.Element("id").Value;
                string name = item.Element("name").Value;
                _constants.Add(name, cmID);
            }
        }
    }

    public class WorkbenchTypes : Constants
    {
        public WorkbenchTypes()
        {
            loadFile("source/WorkbenchTypes.xml");
            foreach (XElement item in doc.Root.Elements())
            {
                item.Name.ToString();
                string cmID = item.Element("id").Value;
                string name = item.Element("name").Value;
                _constants.Add(name, cmID);
            }
        }
    }

    public static class Weathers
    {
        static Dictionary<string, List<string>> _constants = new Dictionary<string, List<string>>();

        public static void Load()
        {
            string JSON_PATH = "../../../res/scripts/common/data/space_weather.json";
            JsonTextReader reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            int is_period = 0;
            bool tmp = false;
            bool s = false;
            bool def_weather = false;
            var space = "";
            int value = -1;
            List<string> weathers = new List<string>();
            Dictionary<string, string> sync = new Dictionary<string, string>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (!space.Any())
                    {
                        space = reader.Value.ToString().Replace("spaces/", "");
                        continue;
                    }
                    if ("periods" == reader.Value.ToString())
                        tmp = true;
                    if ("sync" == reader.Value.ToString())
                    {
                        s = true;
                    }
                    if ("default_weather" == reader.Value.ToString())
                        def_weather = true;
                }
                else if (reader.TokenType == JsonToken.StartArray)
                {
                    is_period++;
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    is_period--;
                }
                else if (reader.TokenType == JsonToken.String && s)
                {
                    sync.Add(space, reader.Value.ToString().Replace("spaces/", ""));
                    s = false;
                }
                else if (reader.TokenType == JsonToken.String && (is_period > 0 || def_weather))
                {
                    string weather_name = reader.Value.ToString();
                    def_weather = false;
                    if (weathers.Contains(weather_name))
                        continue;

                    weathers.Add(weather_name);
                }
                else if (reader.TokenType == JsonToken.EndObject && (tmp || s))
                {
                    _constants.Add(space, weathers);
                    space = "";
                    is_period = 0;
                    tmp = false;
                    s = false;
                    def_weather = false;
                    weathers = new List<string>();
                }
            }

            foreach (var i in sync)
            {
                _constants[i.Key] = new List<string>(_constants[i.Value]);
            }

        }

        public static List<string> getWeathers(string spaceName)
        {
            return _constants[spaceName];
        }

        public static List<string> getAllWeathers()
        {
            List<string> weathers = new List<string>();
            foreach (var i in _constants.Values)
                foreach (var weather in i)
                {
                    if (!weathers.Contains(weather)) weathers.Add(weather);
                }
            weathers.Sort();
            return weathers;
        }
    }


    public class ListSounds
    {
        protected List<string> _constants;

        protected string path = "../../../res/audio/Metadata/fmod_sounds.txt";

        public ListSounds()
        {
            _constants = new List<string>();

            if (!File.Exists(path))
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath(path), "Ошибка");
            System.IO.StreamReader fileReader = new StreamReader(path);
            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Any())
                    continue;
                _constants.Add(line.Replace("event:/", "").Replace("snapshot:/", ""));
            }
            fileReader.Close();
        }

        public List<string> getKeys()
        {
            return _constants;
        }
    }



    public static class SeasonEvents
    {

        static List<string> events = new List<string>();

        public static void parse()
        {
            string JSON_PATH = "../../../res/scripts/server_data/season_events_config.json";

            JsonTextReader reader = new JsonTextReader(new StreamReader(JSON_PATH, Encoding.UTF8));
            int level = 0;
            while (reader.Read())
            {

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (level == 1)
                        events.Add(reader.Value.ToString());
                }
                else if (reader.TokenType == JsonToken.EndObject)
                    level--;
                else if (reader.TokenType == JsonToken.StartObject)
                    level++;

            }
            reader.Close();
        }


        public static string[] getListNames()
        {
            return events.ToArray();
        }
    }
}
