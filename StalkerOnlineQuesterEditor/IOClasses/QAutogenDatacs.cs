﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerOnlineQuesterEditor
{
    public static class QAutogenDatacs
    {
        public static Dictionary<string, AutogenQuestData> data = new Dictionary<string, AutogenQuestData>();
        public static Dictionary<int, string> QuestTypes = new Dictionary<int, string>() { { 1, "Убить мобов" }, { 2, "Принести предметы" } };

        static string JSON_PATH = "../../../res/scripts/common/data/Quests/QuestsAutogenerator/";

        public static void Load()
        {
            string[] files = Directory.GetFiles(JSON_PATH, "*.json");
            foreach (string dialogFile in files)
            {
                string npc_name = Path.GetFileNameWithoutExtension(dialogFile);
                using (StreamReader reader = new StreamReader(dialogFile))
                {
                    string json_string = reader.ReadToEnd();
                    AutogenQuestData myDeserializedClass = JsonConvert.DeserializeObject<AutogenQuestData>(json_string);
                    QAutogenDatacs.data.Add(npc_name, myDeserializedClass);
                    reader.Close();
                }
            }
        }

        public static void Save()
        {
            foreach(var i in data)
            {
                string npc_name = i.Key;
                string path = JSON_PATH + npc_name + ".json";
                if (!i.Value.data.Any()) continue;
                using (StreamWriter writer = new StreamWriter(path))
                {
                    string json = JsonConvert.SerializeObject(i.Value, Formatting.Indented);
                    writer.Write(json);
                    writer.Close();
                }
            }
        }
    }

    public class Reward
    {
        public int money;
        public int exp;
    }

    public class AutogenTarget
    {
        public string name { get; set; }
        public int id { get; set; }
        public int int_param { get; set; }
        public string str_param { get; set; }
        public List<int> counts { get; set; }
        public List<Reward> rewards;

        public AutogenTarget()
        {
            counts = new List<int>();
            rewards = new List<Reward>();
        }
    }

    public class AutogenQuestType
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<AutogenTarget> targets { get; set; }


        public AutogenQuestType()
        {
            targets = new List<AutogenTarget>();
        }

        public AutogenTarget getTargetByType(int target_type)
        {
            foreach (var i in targets)
                if (i.id == target_type) return i;
            return null;
        }

        public void setTargetByType(int target_type, AutogenTarget new_target)
        {
            foreach (var i in targets)
                if (i.id == target_type)
                {
                    i.int_param = new_target.int_param;
                    i.name = new_target.name;
                    i.str_param = new_target.str_param;
                    i.counts = new_target.counts;
                    return;
                };

            targets.Add(new_target);
        }

        public int getNewTargetID()
        {
            int result = 0;
            foreach (var i in targets)
                result = Math.Max(i.id, result);
            return result + 1;
        }

        public void deleteTarget(int target_type)
        {
            foreach (var i in targets)
                if (i.id == target_type)
                {
                    targets.Remove(i);
                    return;
                }
        }

    }

    public class AutogenQuestData
    {
        public List<AutogenQuestType> data { get; set; }

        public AutogenQuestData()
        {
            data = new List<AutogenQuestType>();
        }

        public AutogenQuestType getQuestTypeByName(string name)
        {
            foreach (var i in data)
                if (i.name == name) return i;
            return null;
        }

        public int getQuestTargetByName(int quest_type, string name)
        {
            foreach (var i in data)
            {
                if (i.id == quest_type)
                    foreach (var j in i.targets)
                        if (j.name == name) return j.id;
            }
            return 0;
        }

        public int getNewTargetID(int quest_type)
        {
            return data[quest_type].getNewTargetID();
        }

    }

}