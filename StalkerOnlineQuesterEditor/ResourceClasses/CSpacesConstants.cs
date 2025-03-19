using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StalkerOnlineQuesterEditor
{

    public class dialog_checker
    {
        public dialog_checker(MainForm main)
        {
            System.IO.StreamReader fileReader = new StreamReader("SpeakAreas.txt");
            string line;
            while ((line = fileReader.ReadLine()) != null)
            {
                if (!line.Any())
                    continue;
                CDialog dialog = main.getDialogOnDialogID(Convert.ToInt32(line));
                if (!dialog.coordinates.RootDialog) System.Console.WriteLine(line);
            }
            fileReader.Close();
        }
    }

    public class CSpacesConstants
    {
        List<CSpaceDescription> spaces;
        public CSpacesConstants()
        {
            spaces = new List<CSpaceDescription>();
            XDocument doc = XDocument.Load("../../../res/scripts/common/data/Spaces.xml"); 
            try
            {
                doc = XDocument.Load("../../../res/scripts/common/data/Spaces.xml");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Ошибка чтения файла: ../../../res/scripts/common/data/Spaces.xml", "Ошибка");
                return;
            }
            foreach (XElement item in doc.Root.Elements())
            {
                try
                {
                    string name = item.Element("name").Value.ToString();
                    string dir = item.Element("dir").Value.ToString();
                    int id = Convert.ToInt32(item.Element("id").Value.ToString());
                    spaces.Add(new CSpaceDescription(id, dir, name));
                }
                catch
                {
                    System.Console.WriteLine("Error with space name:" + item.Element("dir").Value.ToString());
                }
            }
        }

        public List<string> getSpacesNames()
        {
            List<string> ret = new List<string>();
            foreach (CSpaceDescription key in this.spaces)
                ret.Add(key.id.ToString() + " " + key.name);
            return ret;
        }

        public List<string> getSpacesNamesWithoutInstances()
        {
            List<string> ret = new List<string>();
            CDungeonSpacesConstants dungeonConst = new CDungeonSpacesConstants();
            List<string> exs = new List<string>() { "start_station", "start_station_sewers" };
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.dir.StartsWith("inst")) continue;
                if (dungeonConst.getAllSpaceNames().Contains("spaces/" + key.dir)) continue;
                if (exs.Contains(key.dir)) continue;
                ret.Add(key.id.ToString() + " " + key.name);
            }
            return ret;
        }

        public string getSpaceNameByID(int id)
        {
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.id == id) return key.dir;
            }
            return null;
        }

        public string getSpaceByID(int id)
        {
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.id == id) return key.id.ToString() + " " + key.name;
            }
            return null;
        } 
        
        public string getLocalName(string dir)
        {
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.dir == dir) return key.name;
            }
            return dir;
        }

        public string getSpaceNameByLocal(string name)
        {
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.name == name) return key.dir;
            }
            return name;
        }

        public int getSpaceID(string name)
        {
            foreach (CSpaceDescription key in this.spaces)
            {
                if (key.name == name) return key.id;
            }
            return 0;
        }

        public long getSpacesToInt(Dictionary<string, bool> spaces)
        {
            long result = 0;
            foreach(KeyValuePair<string, bool> space in spaces)
            {
                if (space.Value)
                {
                    result = result | (((long) 1) << getSpaceID(space.Key));
                }
                else result = result & ~(((long)1) << getSpaceID(space.Key));
            }
            return result;
        }

        public Dictionary<string, bool> getIntToSpaces(long number)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (CSpaceDescription key in this.spaces)
            {
                result.Add(key.name, Convert.ToBoolean(number & (((long)1) << key.id)));
            }
            return result;
        }
    }

    public class CSpaceDescription
    {
        public string dir;
        public string name;
        public int id;

        public CSpaceDescription(int id, string dir, string name)
        {
            this.dir = dir;
            this.name = name;
            this.id = id;
        }

    }

    public class DungeonBoss
    {
        public string type = "CreatureBoss";
        public string data = "0"; //Либо MobAttackManager.eventParamName, либо имя по CreatureType
        public int id = 0;

        
    }

    public class DungeonSpace
    {
        public string name = "";
        public List<DungeonBoss> bosses = new List<DungeonBoss>();

        public List<string> getBosses()
        {
            List<string> result = new List<string>();
            foreach (DungeonBoss i in bosses)
            {
                result.Add(i.data);
            }
            return result;
        }
    }

    public class CDungeonSpacesConstants
    {
        public static string JSON_PATH = "../../../res/scripts/common/data/dungeon_data.json";
        public static string OTHER_JSON_PATH = "source/dungeon_data.json";
        public static string BOSSES_DATA_PATH = "source/dungeon_spaces.xml";
        JsonTextReader reader;
        protected Dictionary<int, DungeonSpace> dungeons = new Dictionary<int, DungeonSpace>();


        public CDungeonSpacesConstants()
        {
            string path = CDungeonSpacesConstants.getPath();
            Dictionary<int, string> dungeons_tmp = new Dictionary<int, string>();

            reader = new JsonTextReader(new StreamReader(path, Encoding.UTF8));
            string space_name = "";
            int level = 0;
            int dung_id = 0;

           // Console.WriteLine("CDungeonSpacesConstants:");

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    level += 1;
                    continue;
                }
                
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    level -= 1;
                    continue;
                }

                if ((reader.TokenType == JsonToken.PropertyName) && (level == 1) )
                {
                    dung_id = int.Parse(reader.Value.ToString());
                    continue;
                }

                if ((reader.TokenType == JsonToken.PropertyName) && (reader.Value.ToString() == "space"))
                {
                    space_name = "";
                }
                else if ((reader.TokenType == JsonToken.String) && (!space_name.Any()))
                {
                    space_name = reader.Value.ToString();
                    dungeons_tmp.Add(dung_id, space_name);
                }
            }
            reader.Close();

            dungeons.Clear();
            XDocument doc = XDocument.Load(BOSSES_DATA_PATH);
            CMobConstants mobs = new CMobConstants();
            foreach (XElement dungeon in doc.Root.Elements())
            {
                
                try{
                    dung_id = int.Parse(dungeon.Element("id").Value.ToString());
                    DungeonSpace space = new DungeonSpace();
                    space.name = dungeons_tmp[dung_id];
                    foreach(XElement boss_node in dungeon.Element("bosses").Elements())
                    {
                        int boss_id = int.Parse(boss_node.Element("id").Value.ToString());
                        string type = boss_node.Element("type").Value.ToString();
                        string additional_data = boss_node.Element("data").Value.ToString();
                        if (type == "CreatureBoss")
                        {
                            additional_data = boss_id.ToString() + " " + mobs.getDescriptionOnType(int.Parse(additional_data.Split()[0])).getName();
                        }
                        DungeonBoss boss = new DungeonBoss();
                        boss.id = boss_id;
                        boss.data = additional_data;
                        boss.type = type;
                        space.bosses.Add(boss);
                    }
                    //Console.WriteLine("ADD DUNGEON " + dung_id.ToString() + " " + space.name);
                    dungeons.Add(dung_id, space);

                }
                catch
                {
                    System.Console.WriteLine("Error with dung_id:" + dungeon.Element("id").Value.ToString());
                }
            }
        }

        public static string getPath()
        {
            if (File.Exists(CDungeonSpacesConstants.JSON_PATH))
                return CDungeonSpacesConstants.JSON_PATH;
            return CDungeonSpacesConstants.OTHER_JSON_PATH;
        }

        public List<string> getAllSpaceNames()
        {
            List<string> result = new List<string>();
            foreach (KeyValuePair<int, DungeonSpace> space in dungeons)
            {
                result.Add(space.Value.name);
            }
            return result;
        }

        public List<string> getBossesByDungID(int dung_id)
        {
            if (!dungeons.ContainsKey(dung_id))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка получения данных данжа с ID:" + dung_id.ToString(), "Ошибка");
                return new List<string>();
            }

            return dungeons[dung_id].getBosses();
        }

        public string getBossName(int dung_id, int boss_id)
        {
            if (!dungeons.ContainsKey(dung_id))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка получения данных данжа с ID:" + dung_id.ToString(), "Ошибка");
                return " ";
            }
            foreach (DungeonBoss i in dungeons[dung_id].bosses)
            {
                if (i.id == boss_id) return i.data;
            }
            System.Windows.Forms.MessageBox.Show("Ошибка получения данных данжа с ID:" + dung_id.ToString(), "Ошибка");
            return " ";
        }

        public int getBossID(int dung_id, string boss_name)
        {
            if (!dungeons.ContainsKey(dung_id))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка получения данных данжа с ID:" + dung_id.ToString(), "Ошибка");
                return -1;
            }
            foreach(DungeonBoss i in dungeons[dung_id].bosses)
            {
                if (i.data == boss_name) return i.id;
            }
            System.Windows.Forms.MessageBox.Show("Ошибка получения данных данжа с ID:" + dung_id.ToString(), "Ошибка");
            return -1;
        }

        public string getNameByID(int id)
        {
            DungeonSpace space = new DungeonSpace();
            dungeons.TryGetValue(id, out space);
            return space.name;
        }

        public int getIDByName(string name)
        {
            foreach (KeyValuePair<int, DungeonSpace> pair in dungeons)
                if (pair.Value.name == name)
                    return pair.Key;
            return -1;
        }
    }

    public class MapMark
    {
        public string coords;
        public float radius;
        public string space;

        public MapMark(string coords, float radius, string space)
        {
            this.coords = coords;
            this.radius = radius;
            this.space = space;
        }
    }
}