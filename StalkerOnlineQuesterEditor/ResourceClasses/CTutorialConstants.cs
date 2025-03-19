using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml;


namespace StalkerOnlineQuesterEditor
{

    public class CTutorialConstants
    {
        public static string JSON_PATH = "../../../res/scripts/common/data/TutorialPhases.json";
        public static string OTHER_JSON_PATH = "source/TutorialPhases.json";

        JsonTextReader reader;
        public Dictionary<int, string> tutorial_phases = new Dictionary<int, string>();

        public CTutorialConstants()
        {
            string path = CTutorialConstants.getPath();

            reader = new JsonTextReader(new StreamReader(path, Encoding.UTF8));
            string property_name = "";
            int index = 0;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    property_name = reader.Value.ToString();
                    continue;
                }
                if ((reader.TokenType == JsonToken.Integer) && (property_name == "index"))
                    {
                        index = Convert.ToInt32(reader.Value);
                    }
                if ((reader.TokenType == JsonToken.String) && (property_name == "local_name"))
                {
                    if (tutorial_phases.ContainsKey(index))
                    {
                        //System.Windows.Forms.MessageBox.Show("Ошибка парсинга фаз обучения. TutorialPhases.json, Что-то пошло не так  индекс - " + index.ToString(), "Ошибка");
                        continue;
                    }
                    tutorial_phases.Add(index, index.ToString() + " " + reader.Value.ToString());
                }
                
            }
            reader.Close();
        }

        public static string getPath()
        {
            if (File.Exists(CTutorialConstants.JSON_PATH))
                return CTutorialConstants.JSON_PATH;
            else
                return CTutorialConstants.OTHER_JSON_PATH;
        }

        public List<string> getAllNames()
        {
            return tutorial_phases.Values.ToList<string>();
        }

        public string getNameByID(int id)
        {
            string n = "";
            tutorial_phases.TryGetValue(id, out n);
            return n;
        }

        public int getIDByName(string name)
        {
            foreach (KeyValuePair<int, string> pair in tutorial_phases)
                if (pair.Value == name)
                    return pair.Key;
            return -1;
        }
    }


}
