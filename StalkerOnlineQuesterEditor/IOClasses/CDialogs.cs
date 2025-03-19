using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using UMD.HCIL.Piccolo;
using System.Windows.Forms;
using System.Net;

namespace StalkerOnlineQuesterEditor
{
    //! Словарь <DialogID, CDialog>
    using NPCDialogDict = Dictionary<int, CDialog>;
    //! Словарь <NPCName, <DialogID, CDialog>>
    using NPCDicts = Dictionary<string, Dictionary<int, CDialog>>;
    //! Словарь <LocaleName, <NPCName, <DialogID, CDialog>>>
    using NPCLocales = Dictionary<string, Dictionary<string, Dictionary<int, CDialog>>>;
    //! Словарь <NPCName, <DialogID, CDifference>>
    using DifferenceDict = Dictionary<string, Dictionary<int, CDifference>>;
    //! Словарь <Имя NPC, < ID диалога, Структура NodeCoordinates > 
    using CoordinatesDict = Dictionary<string, Dictionary<int, NodeCoordinates>>;

    //! Класс обработки диалогов
    public class CDialogs
    {
        private MainForm parent;
        //! XML файл диалогов для хранения информации
        XDocument doc = new XDocument();
        //! Словарь диалогов: < Имя NPC, <DialogID,  CDialog> >
        public NPCDicts dialogs = new NPCDicts();
        //! Словарь локалей
        public NPCLocales locales = new NPCLocales();
        public CoordinatesDict tempCoordinates = new CoordinatesDict();
        public Dictionary<string, NodeCoordinates> otherCoords = new Dictionary<string, NodeCoordinates>();
        private CManagerNPC ManagerNPC;
        public Dictionary<int, string> dialogIDList = new Dictionary<int, string>();
        private Dictionary<int, List<string>> dialogErrors = new Dictionary<int, List<string>>();

        public List<string> deleted_NPC = new List<string>();
        Dictionary<int, string> todoTooltips = new Dictionary<int, string>();
        List<FileStream> fs_list = new List<FileStream>();
        List<string> lock_paths = new List<string>();

        //! Конструктор - парсит текущий файл диалогов, ищет локализации и парсит их тоже
        public CDialogs(MainForm parent, CManagerNPC managerNPC)
        {
            this.parent = parent;
            ManagerNPC = managerNPC;
            ParseNodeCoordinates("NodeCoordinates/");
            ParseNodeCoordinatesOther();

            ParseDialogsData(CSettings.GetDialogDataPath(), this.dialogs, true);
            ParseDialogToolTips();
            ParseDialogsTexts(CSettings.GetDialogTextPath(CSettings.ORIGINAL_PATH), this.dialogs);

            foreach (var locale in CSettings.getListLocales())
            {
                if (!locales.Keys.Contains(locale))
                {
                    //locales.Add(locale, new NPCDicts());
                    locales.Add(locale, new NPCDicts(this.dialogs.ToDictionary(entry => entry.Key, entry => entry.Value.ToDictionary(a => a.Key, a => a.Value.Clone()))));
                }
                ParseDialogsTexts(CSettings.GetDialogTextPath(locale), this.locales[locale]);
            }
            lock_files();
        }

        //! Парсер xml - файла данных диалогов, записывает результат в target
        private void ParseDialogsData(String dialogDataPath, NPCDicts target, bool findErrors = false)
        {
            string[] files = Directory.GetFiles(dialogDataPath, "*.xml");
            foreach (string dialogFile in files)
            {
                if (!File.Exists(dialogFile))
                    return;
                List<int> tests;
                doc = XDocument.Load(dialogFile);
                lock_paths.Add(dialogFile);
                
                dialogErrors = new Dictionary<int, List<string>>();
                string npc_name = Path.GetFileNameWithoutExtension(dialogFile);
                target.Add(npc_name, new Dictionary<int, CDialog>());

                foreach (XElement dialog in doc.Root.Elements("Dialog"))
                {
                    int DialogID = int.Parse(dialog.Element("ID").Value);
                    if (findErrors)
                        if (dialogIDList.ContainsKey(DialogID))
                        {
                            if (dialogErrors.ContainsKey(DialogID))
                                dialogErrors[DialogID].Add(npc_name);
                            else
                                dialogErrors.Add(DialogID, new List<string> { npc_name, dialogIDList[DialogID] });
                        }
                        else
                            dialogIDList.Add(DialogID, npc_name);
                    string DebugData = "";
                    bool isAutoNode = false;
                    string defaultNode = "";
                    List<int> Nodes = new List<int>();
                    List<int> CheckNodes = new List<int>();
                    Actions Actions = new Actions();
                    CDialogPrecondition Precondition = new CDialogPrecondition();

                    if ((dialog.Element("Nodes") != null) && (dialog.Element("Nodes").Value != ""))
                        foreach (string node in dialog.Element("Nodes").Value.Split(','))
                            if (node != "")
                                Nodes.Add(int.Parse(node));
                    if ((dialog.Element("CheckNodes") != null) && (dialog.Element("CheckNodes").Value != ""))
                        foreach (string node in dialog.Element("CheckNodes").Value.Split(','))
                            if (node != "")
                                CheckNodes.Add(int.Parse(node));
                    if (dialog.Element("Actions") != null)
                    {
                        if (dialog.Element("Actions").Element("GoToCamera") != null)
                        {
                            Actions.actionCamera = dialog.Element("Actions").Element("GoToCamera").Value;
                            if (dialog.Element("Actions").Element("CameraSmoothly") == null)
                                Actions.actionCameraSmoothly = false;
                        }
                        if (dialog.Element("Actions").Element("AnimationPlayer") != null)
                            Actions.actionAnimationPlayer = dialog.Element("Actions").Element("AnimationPlayer").Value;
                        if (dialog.Element("Actions").Element("AnimationNPC") != null)
                            Actions.actionAnimationNPC = dialog.Element("Actions").Element("AnimationNPC").Value;
                        if (dialog.Element("Actions").Element("ActionNPC") != null)
                            Actions.actionActionNPC = int.Parse(dialog.Element("Actions").Element("ActionNPC").Value);
                        if (dialog.Element("Actions").Element("AdditionalActionNPC") != null)
                            Actions.actionAdditionalActionNPC = dialog.Element("Actions").Element("AdditionalActionNPC").Value;
                        if (dialog.Element("Actions").Element("AvatarPoint") != null)
                            Actions.actionAvatarPoint = dialog.Element("Actions").Element("AvatarPoint").Value;
                        if (dialog.Element("Actions").Element("PlaySound") != null)
                            Actions.actionPlaySound = dialog.Element("Actions").Element("PlaySound").Value;
                        if (dialog.Element("Actions").Element("ChangeMoney") != null)
                        {
                            Actions.changeMoney = int.Parse(dialog.Element("Actions").Element("ChangeMoney").Value);
                            if (dialog.Element("Actions").Element("ChangeMoneyFailNode") != null)
                                Actions.changeMoneyFailNode = int.Parse(dialog.Element("Actions").Element("ChangeMoneyFailNode").Value);
                        }
                        if (dialog.Element("Actions").Element("Exit") != null)
                            Actions.Exit = dialog.Element("Actions").Element("Exit").Value == "1";
                        Actions.ToDialog = ParseIntIfNotEmpty(dialog, "Actions", "ToDialog", 0);
                        Actions.Event = parent.dialogEvents.GetEventFromID(ParseIntIfNotEmpty(dialog, "Actions", "Event", 0));
                        if (dialog.Element("Actions").Element("Data") != null)
                            Actions.Data = dialog.Element("Actions").Element("Data").Value;

                        AddDataToList(dialog, "Actions", "GetQuest", Actions.GetQuests);
                        AddDataToList(dialog, "Actions", "CompleteQuest", Actions.CompleteQuests);
                        AddDataToList(dialog, "Actions", "GetKnowleges", Actions.GetKnowleges);
                        if (dialog.Element("Actions").Descendants().Any(item => item.Name == "CancelQuest"))
                        {
                            AddDataToList(dialog, "Actions", "CancelQuest", Actions.CancelQuests);
                        }
                        AddDataToList(dialog, "Actions", "FailQuest", Actions.FailQuests);
                    }
                    if (dialog.Element("Precondition") != null)
                    {

                        if (dialog.Element("Precondition").Element("ListOfNecessaryQuests") != null)
                        {
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfQuests", Precondition.ListOfNecessaryQuests.ListOfHaveQuests, ref Precondition.ListOfNecessaryQuests.conditionOfQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfOpenedQuests", Precondition.ListOfNecessaryQuests.ListOfOpenedQuests, ref Precondition.ListOfNecessaryQuests.conditionOfOpenedQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfFailQuests", Precondition.ListOfNecessaryQuests.ListOfFailQuests, ref Precondition.ListOfNecessaryQuests.conditionOfFailQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfOnTestQuests", Precondition.ListOfNecessaryQuests.ListOfOnTestQuests, ref Precondition.ListOfNecessaryQuests.conditionOfOnTestQuest);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfCompletedQuests", Precondition.ListOfNecessaryQuests.ListOfCompletedQuests, ref Precondition.ListOfNecessaryQuests.conditionOfCompletedQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfOmniCounters", Precondition.ListOfNecessaryQuests.ListOfCounters, ref Precondition.ListOfNecessaryQuests.conditionOfCounterss);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfNecessaryQuests", "listOfRepeat", Precondition.ListOfNecessaryQuests.ListOfRepeat, ref Precondition.ListOfNecessaryQuests.conditionOfRepeat);

                            if (dialog.Element("Precondition").Element("ListOfNecessaryQuests").Element("listOfMassQuests") != null)
                            {
                                if (dialog.Element("Precondition").Element("ListOfNecessaryQuests").Element("listOfMassQuests").Value.Contains('|'))
                                    Precondition.ListOfNecessaryQuests.conditionOfMassQuests = '|';
                                else
                                    Precondition.ListOfNecessaryQuests.conditionOfMassQuests = '&';
                                Precondition.ListOfNecessaryQuests.ListOfMassQuests = dialog.Element("Precondition").Element("ListOfNecessaryQuests").Element("listOfMassQuests").Value;
                                Precondition.ListOfNecessaryQuests.ListOfMassQuests.Replace(Precondition.ListOfNecessaryQuests.conditionOfMassQuests, ',');
                            }
                        }
                        if (dialog.Element("Precondition").Element("ListOfMustNoQuests") != null)
                        {
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfQuests", Precondition.ListOfMustNoQuests.ListOfHaveQuests, ref Precondition.ListOfMustNoQuests.conditionOfQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfOpenedQuests", Precondition.ListOfMustNoQuests.ListOfOpenedQuests, ref Precondition.ListOfMustNoQuests.conditionOfOpenedQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfFailQuests", Precondition.ListOfMustNoQuests.ListOfFailQuests, ref Precondition.ListOfMustNoQuests.conditionOfFailQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfOnTestQuests", Precondition.ListOfMustNoQuests.ListOfOnTestQuests, ref Precondition.ListOfMustNoQuests.conditionOfOnTestQuest);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfCompletedQuests", Precondition.ListOfMustNoQuests.ListOfCompletedQuests, ref Precondition.ListOfMustNoQuests.conditionOfCompletedQuests);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfOmniCounters", Precondition.ListOfMustNoQuests.ListOfCounters, ref Precondition.ListOfMustNoQuests.conditionOfCounterss);
                            CDialogs.AddPreconditionQuests(dialog, "ListOfMustNoQuests", "listOfRepeat", Precondition.ListOfMustNoQuests.ListOfRepeat, ref Precondition.ListOfMustNoQuests.conditionOfRepeat);

                            if (dialog.Element("Precondition").Element("ListOfMustNoQuests").Element("listOfMassQuests") != null)
                            {
                                if (dialog.Element("Precondition").Element("ListOfMustNoQuests").Element("listOfMassQuests").Value.Contains('|'))
                                    Precondition.ListOfMustNoQuests.conditionOfMassQuests = '|';
                                else
                                    Precondition.ListOfMustNoQuests.conditionOfMassQuests = '&';
                                Precondition.ListOfMustNoQuests.ListOfMassQuests = dialog.Element("Precondition").Element("ListOfMustNoQuests").Element("listOfMassQuests").Value;
                                Precondition.ListOfMustNoQuests.ListOfMassQuests.Replace(Precondition.ListOfMustNoQuests.conditionOfMassQuests, ',');
                            }
                        }
                        if (dialog.Element("Precondition").Element("NecessaryEffects") != null)
                            CDialogs.AddDialogEffectsToList(dialog, "Precondition", "NecessaryEffects", Precondition.NecessaryEffects, ref Precondition.nec_effects_is_or);
                        
                        if (dialog.Element("Precondition").Element("MustNoEffects") != null)
                            CDialogs.AddDialogEffectsToList(dialog, "Precondition", "MustNoEffects", Precondition.MustNoEffects, ref Precondition.must_no_effects_is_or);

                        if (dialog.Element("Precondition").Element("Skills") != null)
                            AddDialogSkillsToListSkills(dialog, "Precondition", "Skills", Precondition.Skills);

                        Precondition.Perks = new List<int>();
                        if (dialog.Element("Precondition").Element("Perks") != null)
                            AddDataToList(dialog, "Precondition", "Perks", Precondition.Perks);
                        Precondition.noPerks = new List<int>();
                        if (dialog.Element("Precondition").Element("noPerks") != null)
                            AddDataToList(dialog, "Precondition", "noPerks", Precondition.noPerks);
                        Precondition.KarmaPK = new List<int>();

                        Precondition.Achievements = new List<int>();
                        if (dialog.Element("Precondition").Element("Achievements") != null)
                            AddDataToList(dialog, "Precondition", "Achievements", Precondition.Achievements);
                        Precondition.noAchievements = new List<int>();
                        if (dialog.Element("Precondition").Element("noAchievements") != null)
                            AddDataToList(dialog, "Precondition", "noAchievements", Precondition.noAchievements);


                        AddDataToList(dialog, "Precondition", "KarmaPK", Precondition.KarmaPK);
                        AddDataToList(dialog, "Precondition", "playerCoords", Precondition.playerCoords);
                        if (dialog.Element("Precondition").Element("coordsRadius") != null)
                            Precondition.coordsRadius = int.Parse(dialog.Element("Precondition").Element("coordsRadius").Value);
                        if (dialog.Element("Precondition").Element("map") != null)
                            Precondition.coordsMap = int.Parse(dialog.Element("Precondition").Element("map").Value);
                        if (dialog.Element("Precondition").Element("forDev") != null)
                            Precondition.forDev = true;
                        if (dialog.Element("Precondition").Element("hidden") != null)
                            Precondition.hidden = true;
                        if (dialog.Element("Precondition").Element("tutorialPhase") != null)
                            Precondition.tutorialPhase = int.Parse(dialog.Element("Precondition").Element("tutorialPhase").Value);
                        if (dialog.Element("Precondition").Element("pvpRank") != null)
                        {
                            string[] value = dialog.Element("Precondition").Element("pvpRank").Value.Split('-');
                            for (int i = 0; i < 2; i++)
                            {
                                Precondition.PVPranks[i] = Convert.ToInt16(value[i]);
                            }
                        }
                        if (dialog.Element("Precondition").Element("pvpMode") != null)
                            Precondition.PVPMode = int.Parse(dialog.Element("Precondition").Element("pvpMode").Value);

                        if (dialog.Element("Precondition").Element("groupBonus") != null)
                        {
                            string[] value = dialog.Element("Precondition").Element("groupBonus").Value.Split(':');
                            for (int i = 0; i < 3; i++)
                            {
                                Precondition.fracBonus[i] = Convert.ToInt16(value[i]);
                            }
                        }
                        

                        if (dialog.Element("Precondition").Element("Transport") != null)
                        {
                            if (dialog.Element("Precondition").Element("Transport").Element("inTransportList") != null)
                                Precondition.transport.inTransportList = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("notInTransportList") != null)
                                Precondition.transport.notInTransportList = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("inBoatList") != null)
                                Precondition.transport.inBoatList = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("notInBoatList") != null)
                                Precondition.transport.notInBoatList = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("boatInTransit") != null)
                                Precondition.transport.boatInTransit = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("boatStopped") != null)
                                Precondition.transport.boatStopped = true;
                            if (dialog.Element("Precondition").Element("Transport").Element("boatName") != null)
                                Precondition.transport.boatName = dialog.Element("Precondition").Element("Transport").Element("boatName").Value;
                        }
                        if (dialog.Element("Precondition").Element("clanOptions") != null)
                        {
                            Precondition.clanOptions = dialog.Element("Precondition").Element("clanOptions").Value.ToString();
                        }
                        if (dialog.Element("Precondition").Element("clanLevel") != null)
                        {
                            Precondition.clanLevel = new List<int>();
                            string tmp = dialog.Element("Precondition").Element("clanLevel").Value.ToString();
                            foreach (var i in tmp.Split(','))
                                Precondition.clanLevel.Add(Convert.ToInt32(i));
                        }

                        if (dialog.Element("Precondition").Element("radioAvailable") != null)
                            Precondition.radioAvailable = (RadioAvalible)Convert.ToInt32(dialog.Element("Precondition").Element("radioAvailable").Value);
                        if (dialog.Element("Precondition").Element("dungeonPhase") != null)
                            Precondition.dungeonPhase = Convert.ToInt32(dialog.Element("Precondition").Element("dungeonPhase").Value);
                        if (dialog.Element("Precondition").Element("seasonEvent") != null)
                            Precondition.seasonEvent = dialog.Element("Precondition").Element("seasonEvent").Value;
                        if (dialog.Element("Precondition").Element("dungeonNot") != null)
                            Precondition.dungeonNot = true;
                        if (dialog.Element("Precondition").Element("tests") != null)
                        {
                            tests = new List<int>();
                            AddDataToList(dialog, "Precondition", "tests", tests);
                            for (int i = 0; i < tests.Count; i++)
                            {
                                if (tests[i] == 0) tests[i] = 7;
                                if (tests[i] == 2) tests[i] = 8;
                            }
                            if (Precondition.clanOptions == "")
                                Precondition.clanOptions = Global.GetListAsString(tests);
                            else
                                Precondition.clanOptions += "," + Global.GetListAsString(tests);
                        }
                        if (dialog.Element("Precondition").Element("PlayerLevel") != null)
                        {
                            Precondition.PlayerLevel = dialog.Element("Precondition").Element("PlayerLevel").Value;
                        }
                        if (dialog.Element("Precondition").Element("Reputation") != null)
                        {
                            foreach (string el in dialog.Element("Precondition").Element("Reputation").Value.Split(';'))
                            {
                                if (el == "")
                                    continue;
                                string[] fr = el.Split(':');
                                int fractionID = int.Parse(fr[0]);
                                Precondition.Reputation.Add(fractionID, new List<double>());
                                double A = double.Parse(fr[1], System.Globalization.CultureInfo.InvariantCulture);
                                double B = double.Parse(fr[2], System.Globalization.CultureInfo.InvariantCulture);
                                Precondition.Reputation[fractionID].Add(A);
                                Precondition.Reputation[fractionID].Add(B);
                            }
                        }
                        if (dialog.Element("Precondition").Element("Reputation2") != null)
                        {
                            foreach (string el in dialog.Element("Precondition").Element("Reputation2").Value.Split(';'))
                            {
                                if (el == "")
                                    continue;
                                string[] fr = el.Split(':');
                                int fractionID = int.Parse(fr[0]);
                                Precondition.Reputation2.Add(fractionID, new List<double>());
                                double A = double.Parse(fr[1], System.Globalization.CultureInfo.InvariantCulture);
                                double B = double.Parse(fr[2], System.Globalization.CultureInfo.InvariantCulture);
                                Precondition.Reputation2[fractionID].Add(A);
                                Precondition.Reputation2[fractionID].Add(B);
                            }
                        }
                        if (dialog.Element("Precondition").Element("NPCReputation") != null)
                        {
                            foreach (string el in dialog.Element("Precondition").Element("NPCReputation").Value.Split(';'))
                            {
                                if (el == "")
                                    continue;
                                string[] fr = el.Split(':');
                                string NPC_name = fr[0];
                                Precondition.NPCReputation.Add(NPC_name, new List<double>());
                                double A = double.Parse(fr[1], System.Globalization.CultureInfo.InvariantCulture);
                                double B = double.Parse(fr[2], System.Globalization.CultureInfo.InvariantCulture);
                                Precondition.NPCReputation[NPC_name].Add(A);
                                Precondition.NPCReputation[NPC_name].Add(B);
                            }
                        }

                        if (dialog.Element("Precondition").Element("items") != null)
                        {
                            if (dialog.Element("Precondition").Element("items").Element("itemCategory") != null)
                                Precondition.items.itemCategory = int.Parse(dialog.Element("Precondition").Element("items").Element("itemCategory").Value);
                            else
                            if (dialog.Element("Precondition").Element("items") != null)
                            {
                                if (dialog.Element("Precondition").Element("items").Element("or") != null)
                                    Precondition.items.is_or = true;
                                if (dialog.Element("Precondition").Element("items").Element("equipped") != null)
                                    Precondition.items.equipped = true;
                                CQuests.parceItems(dialog.Element("Precondition").Element("items").Element("Items"), Precondition.items.items);
                            }
                        }
                        if (dialog.Element("Precondition").Element("noneItems") != null)
                        {
                            if (dialog.Element("Precondition").Element("noneItems").Element("itemCategory") != null)
                                Precondition.itemsNone.itemCategory = int.Parse(dialog.Element("Precondition").Element("noneItems").Element("itemCategory").Value);
                            else if (dialog.Element("Precondition").Element("noneItems") != null)
                            {
                                if (dialog.Element("Precondition").Element("noneItems").Element("or") != null)
                                    Precondition.itemsNone.is_or = true;
                                if (dialog.Element("Precondition").Element("noneItems").Element("equipped") != null)
                                    Precondition.items.equipped = true;
                                CQuests.parceItems(dialog.Element("Precondition").Element("noneItems").Element("Items"), Precondition.itemsNone.items);
                            }
                        }

                        if (dialog.Element("Precondition").Element("Knowledges") != null)
                        {
                            CDialogs.AddPreconditionQuests(dialog, "Knowledges", "mustKnowledge", Precondition.knowledges.mustKnowledge, ref Precondition.knowledges.conditionMustKnowledge);
                            CDialogs.AddPreconditionQuests(dialog, "Knowledges", "shouldntKnowledge", Precondition.knowledges.shouldntKnowledge, ref Precondition.knowledges.conditionShouldntKnowledge);
                        }

                        if (dialog.Element("Precondition").Element("TimeWeather") != null)
                        {
                            WeatherData weather = new WeatherData();
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("space") != null)
                                weather.space = Convert.ToInt32(dialog.Element("Precondition").Element("TimeWeather").Element("space").Value);
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("weathers") != null)
                                weather.weathers = new List<string>(dialog.Element("Precondition").Element("TimeWeather").Element("weathers").Value.Split(','));
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("or") != null)
                                weather.is_or = true;
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("timeStart") != null)
                                weather.timeStart = dialog.Element("Precondition").Element("TimeWeather").Element("timeStart").Value;
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("timeEnd") != null)
                                weather.timeEnd = dialog.Element("Precondition").Element("TimeWeather").Element("timeEnd").Value;
                            if (dialog.Element("Precondition").Element("TimeWeather").Element("only_no") != null)
                                weather.only_no = true;
                            Precondition.weather = weather;
                        }

                    }
                    NodeCoordinates nodeCoord = new NodeCoordinates();
                    if (dialog.Element("RootDialog") != null)
                        nodeCoord.RootDialog = dialog.Element("RootDialog").Value.Trim().Equals("1");
                    if (dialog.Element("Active") != null)
                        nodeCoord.Active = dialog.Element("Active").Value.Trim().Equals("1");
                    else nodeCoord.Active = false;
                    if (tempCoordinates.ContainsKey(npc_name) && tempCoordinates[npc_name].ContainsKey(DialogID))
                    {
                        nodeCoord.X = tempCoordinates[npc_name][DialogID].X;
                        nodeCoord.Y = tempCoordinates[npc_name][DialogID].Y;
                    }
                    
                    if (dialog.Element("DebugData") != null)
                        DebugData = dialog.Element("DebugData").Value.ToString();

                    bool noLocale = false;
                    if (dialog.Element("noLocale") != null)
                        noLocale = true;
                    if (dialog.Element("isAutoNode") != null)
                    {
                        isAutoNode = dialog.Element("isAutoNode").Value.Trim().Equals("1");
                        if (dialog.Element("defaultNode") != null) defaultNode = dialog.Element("defaultNode").Value;
                    }
                    int nextDialog = 0;
                    if (dialog.Element("nextDialog") != null)
                        int.TryParse(dialog.Element("nextDialog").Value.ToString(), out nextDialog);
                    if (!target[npc_name].Keys.Contains(DialogID))
                        target[npc_name].Add(DialogID, new CDialog(npc_name, "", "", Precondition, Actions, Nodes, CheckNodes, DialogID, 0, 
                            nodeCoord, DebugData, noLocale, nextDialog, isAutoNode, defaultNode));
                }
            }


            foreach (KeyValuePair<int, List<string>> dialogError in dialogErrors)
            {
                string npc_name_list = "";
                foreach (string npc_name in dialogError.Value)
                {
                    npc_name_list += npc_name + ", ";
                }
                int new_id = CDialogs.getDialogsNewID();
                MessageBox.Show("Ошибка дублирования диалогов (Всего " + dialogErrors.Count + ") (" + npc_name_list + ") " + dialogError.Key + " => " + (new_id));
                throw new Exception("Ошибка дублирования диалогов " + dialogError.Key + " => " + (new_id));
            }
        }

        private void ParseDialogToolTips()
        {
            string todoFilePath = "ToDoToolTips.xml";
            if (!File.Exists(todoFilePath))
                return;

            doc = XDocument.Load(todoFilePath);
            lock_paths.Add(todoFilePath);
            foreach (XElement dialog in doc.Root.Elements("Dialog"))
            {
                int DialogID = int.Parse(dialog.Element("ID").Value);
                todoTooltips.Add(DialogID, dialog.Element("Text").Value);
            }
        }

        public static void AddPreconditionKnowlege(XElement Element, String Name1, List<int> list, ref char condition)
        {
            if (Element.Element("Precondition").Element(Name1) == null)
                return;
            if (Element.Element("Precondition").Element(Name1).Value != "")

                if (Element.Element("Precondition").Element(Name1).Value.Contains('|'))
                    condition = '|';
                else if (Element.Element("Precondition").Element(Name1).Value.Contains('&'))
                    condition = '&';
                else
                    condition = ',';
            foreach (string quest in Element.Element("Precondition").Element(Name1).Value.Split(condition))
                list.Add(int.Parse(quest));
        }

        public static void AddPreconditionQuests(XElement Element, String Name1, String Name2, List<int> list, ref char condition)
        {
            if (Element.Element("Precondition").Element(Name1).Element(Name2) == null)
                return;
            if (Element.Element("Precondition").Element(Name1).Element(Name2).Value != "")

                if (Element.Element("Precondition").Element(Name1).Element(Name2).Value.Contains('|'))
                    condition = '|';
                else if (Element.Element("Precondition").Element(Name1).Element(Name2).Value.Contains('&'))
                    condition = '&';
                else
                    condition = ',';
            foreach (string quest in Element.Element("Precondition").Element(Name1).Element(Name2).Value.Split(condition))
                list.Add(int.Parse(quest));
        }
        public static void AddDialogEffectsToList(XElement Element, String Name1, String Name2, List<DialogEffect> list, ref bool is_or)
        {
            if (Element.Element(Name1).Element(Name2) == null)
                return;

            foreach (XElement effect in Element.Element(Name1).Element(Name2).Elements())
            {
                if (effect.Name == "is_or")
                {
                    is_or = true;
                    continue;
                }
                int ID = int.Parse(effect.Element("id").Value);
                string stack = effect.Element("stack").Value;
                list.Add(new DialogEffect(ID, stack));
            }
        }
        public static void AddDialogSkillsToListSkills(XElement Element, String Name1, String Name2, ListDialogSkills list)
        {
            if (Element.Element(Name1).Element(Name2) == null)
                return;

            foreach (XElement effect in Element.Element(Name1).Element(Name2).Elements())
            {
                string name = effect.Element("id").Value;
                string[] val = effect.Element("value").Value.Split(':');
                list.Add(name, val[0], val[1]);
            }
        }

        public static void AddDataToList(XElement Element, String Name1, String Name2, List<int> list)
        {
            if (Element.Element(Name1).Element(Name2) == null)
                return;
            if (Element.Element(Name1).Element(Name2).Value != "")
                foreach (string quest in Element.Element(Name1).Element(Name2).Value.Split(','))
                    list.Add(int.Parse(quest));
        }

        public static void AddDataToList(XElement Element, String Name1, String Name2, List<float> list)
        {
            if (Element.Element(Name1).Element(Name2) == null)
                return;
            if (Element.Element(Name1).Element(Name2).Value != "")
                foreach (string quest in Element.Element(Name1).Element(Name2).Value.Split(','))
                    list.Add(float.Parse(quest, System.Globalization.CultureInfo.InvariantCulture));
        }

        public static int ParseIntIfNotEmpty(XElement Element, String Name1, String Name2, int defaultValue)
        {
            if (Element.Element(Name1).Element(Name2) == null)
                return defaultValue;
            if (!Element.Element(Name1).Element(Name2).Value.Equals(""))
                return int.Parse(Element.Element(Name1).Element(Name2).Value);
            return defaultValue;
        }

        private void ParseDialogsTexts(String dataPath, NPCDicts target)
        {
            string[] files = Directory.GetFiles(dataPath, "*.xml");
            foreach (string dialogFile in files)
            {
                try
                {
                    doc = XDocument.Load(dialogFile);
                    lock_paths.Add(dialogFile);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Произошла ошибка при чтении файла: " + dialogFile + "\n" + e.Message, "Ошибка чтения");
                    return;
                }

                string npc_name = Path.GetFileNameWithoutExtension(dialogFile);
                if (!target.ContainsKey(npc_name))
                {
                    MessageBox.Show("В DialogsData отсутствует NPC:" + npc_name, "Ошибка парсинга текстов");
                    continue;
                }

                foreach (XElement dialog in doc.Root.Elements("Dialog"))
                {


                    int DialogID = int.Parse(dialog.Element("ID").Value);

                    if (!target[npc_name].ContainsKey(DialogID))
                    {
                        string msg = "Проблема с диалогом ID:" + DialogID.ToString() + ". У NPC в DialogData отсутствует диалог с таким ID";
                        if (dialog.Element("Title") != null)
                            msg += " Title:" + dialog.Element("Title").Value.Trim();
                        MessageBox.Show(msg, "Ошибка парсинга текстов");
                        continue;
                    }

                    if (dialog.Element("Title") != null)
                    {
                        target[npc_name][DialogID].Title = dialog.Element("Title").Value.Trim();
                    }
                    if (dialog.Element("Text") != null)
                    {
                        target[npc_name][DialogID].Text = dialog.Element("Text").Value.Trim();
                    }
                    int Version = 0;
                    if ((!dialog.Element("Version").Value.Equals("")))
                        Version = int.Parse(dialog.Element("Version").Value);
                    target[npc_name][DialogID].version = Version;

                }
            }
        }

        //! Сохранить все диалоги в xml файл
        public void SaveDialogs()
        {
            SaveNodeCoordinates("NodeCoordinates/", this.dialogs);
            SaveNodeCoordinatesOther();
            SaveDialogsTexts(CSettings.GetDialogTextPath(CSettings.ORIGINAL_PATH), this.dialogs);
            SaveDialogsData(CSettings.GetDialogDataPath(), this.dialogs);
            SaveToDoTooltips();
        }

        //! Сохраняет текущую локализацию диалогов в файл
        public void SaveLocales()
        {
            foreach(string locale in CSettings.getListLocales() )
                SaveDialogsTexts(CSettings.GetDialogTextPath(locale), this.locales[locale]);
        }

        private void SaveDialogsTexts(string filePath, NPCDicts target)
        {

            XElement element;

            foreach (string npcName in target.Keys)
            {
                XDocument resultDoc = new XDocument(new XElement("root"));
                //npcElement = new XElement("NPC", new XElement("Name", npcName));
                NPCDialogDict Dictdialog = target[npcName];
                foreach (CDialog dialog in Dictdialog.Values)
                {
                    element = new XElement("Dialog",
                       new XElement("ID", dialog.DialogID.ToString()),
                       new XElement("Version", dialog.version.ToString()));
                    if (dialog.Title != "")
                        element.Add(new XElement("Title", dialog.Title));
                    if (dialog.Text != "")
                        element.Add(new XElement("Text", dialog.Text));

                    resultDoc.Root.Add(element);
                }
                string fileName = filePath + npcName + ".xml";
                System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
                using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(fileName, settings))
                {
                    Console.WriteLine("PATH!:" + fileName);
                    resultDoc.Save(w);

                }
            }

            foreach (string npc_name in deleted_NPC)
            {
                string path = filePath + npc_name + ".xml";
                if (File.Exists(path)) File.Delete(path);
            }

        }

        private void SaveToDoTooltips()
        {
            XDocument resultDoc = new XDocument(new XElement("root"));
            XElement element;
            foreach (KeyValuePair<int, string> i in todoTooltips)
            {
                element = new XElement("Dialog",
                         new XElement("ID", i.Key.ToString()),
                         new XElement("Text", i.Value));
                resultDoc.Root.Add(element);
            }
            using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create("ToDoToolTips.xml"))
            {
                resultDoc.Save(w);
            }
        }

        private void SaveDialogsData(string data_path, NPCDicts target)
        {
            //XElement npcElement;
            foreach (string npcName in target.Keys)
            {
                XDocument resultDoc = new XDocument(new XElement("root"));
                //npcElement = new XElement("NPC", new XElement("Name", npcName));
                NPCDialogDict Dictdialog = target[npcName];
                XElement element;
                foreach (CDialog dialog in Dictdialog.Values)
                {
                    element = new XElement("Dialog",
                       new XElement("ID", dialog.DialogID.ToString()));
                    if (dialog.Precondition.Exists())
                    {

                        XElement prec = new XElement("Precondition");
                        element.Add(prec);

                        if (dialog.Precondition.ListOfNecessaryQuests.Any())
                        {
                            prec.Add(new XElement("ListOfNecessaryQuests"));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfCompletedQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfCompletedQuests)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfHaveQuests.Any())
                                element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfHaveQuests,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfQuests)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfOpenedQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfOpenedQuests)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfFailQuests.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfFailQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfFailQuests,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfFailQuests)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfOnTestQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfOnTestQuest)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfCounters.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfOmniCounters",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfCounters,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfCounterss)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfRepeat.Any())
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfRepeat",
                                              Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfRepeat,
                                                                    dialog.Precondition.ListOfNecessaryQuests.conditionOfRepeat)));
                            if (dialog.Precondition.ListOfNecessaryQuests.ListOfMassQuests != "")
                                prec.Element("ListOfNecessaryQuests").Add(new XElement("listOfMassQuests",
                                                                         dialog.Precondition.ListOfNecessaryQuests.ListOfMassQuests.Replace(',', dialog.Precondition.ListOfNecessaryQuests.conditionOfMassQuests)));
                        }
                        if (dialog.Precondition.ListOfMustNoQuests.Any())
                        {
                            prec.Add(new XElement("ListOfMustNoQuests"));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfCompletedQuests.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfCompletedQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfCompletedQuests,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfCompletedQuests)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfHaveQuests.Any())
                                element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfHaveQuests,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfQuests)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfOpenedQuests.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfOpenedQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfOpenedQuests,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfOpenedQuests)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfFailQuests.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfFailQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfFailQuests,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfFailQuests)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfOnTestQuests.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfOnTestQuests",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfOnTestQuests,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfOnTestQuest)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfCounters.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfOmniCounters",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfCounters,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfCounterss)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfRepeat.Any())
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfRepeat",
                                              Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfRepeat,
                                                                    dialog.Precondition.ListOfMustNoQuests.conditionOfRepeat)));
                            if (dialog.Precondition.ListOfMustNoQuests.ListOfMassQuests != "")
                                prec.Element("ListOfMustNoQuests").Add(new XElement("listOfMassQuests",
                                                                         dialog.Precondition.ListOfMustNoQuests.ListOfMassQuests.Replace(',', dialog.Precondition.ListOfMustNoQuests.conditionOfMassQuests)));
                        }
                        if (dialog.Precondition.knowledges.Any())
                        {
                            prec.Add(new XElement("Knowledges"));
                            if (dialog.Precondition.knowledges.mustKnowledge.Any())
                                prec.Element("Knowledges").Add(new XElement("mustKnowledge",
                                              Global.GetListAsString(dialog.Precondition.knowledges.mustKnowledge,
                                                                    dialog.Precondition.knowledges.conditionMustKnowledge)));
                            if (dialog.Precondition.knowledges.shouldntKnowledge.Any())
                                prec.Element("Knowledges").Add(new XElement("shouldntKnowledge",
                                              Global.GetListAsString(dialog.Precondition.knowledges.shouldntKnowledge,
                                                                    dialog.Precondition.knowledges.conditionShouldntKnowledge)));
                        }
                        if (dialog.Precondition.clanOptions != "")
                            prec.Add(new XElement("clanOptions", dialog.Precondition.clanOptions));
                        if (dialog.Precondition.clanLevel.Sum() > 0)
                            prec.Add(new XElement("clanLevel", string.Join(",", dialog.Precondition.clanLevel)));
                        if (dialog.Precondition.radioAvailable != RadioAvalible.None)
                            prec.Add(new XElement("radioAvailable", Convert.ToInt32(dialog.Precondition.radioAvailable).ToString()));
                        if (dialog.Precondition.dungeonPhase > 0)
                            prec.Add(new XElement("dungeonPhase", Convert.ToInt32(dialog.Precondition.dungeonPhase).ToString()));
                        if (dialog.Precondition.seasonEvent.Any())
                            prec.Add(new XElement("seasonEvent", dialog.Precondition.seasonEvent));
                        if (dialog.Precondition.dungeonNot)
                            prec.Add(new XElement("dungeonNot", "1"));
                        if (dialog.Precondition.MustNoEffects.Any())
                            prec.Add(dialog.Precondition.getMustNoEffects());
                        if (dialog.Precondition.NecessaryEffects.Any())
                            prec.Add(dialog.Precondition.getNecessaryEffects());
                        if (dialog.Precondition.Skills.Any())
                            prec.Add(dialog.Precondition.Skills.getSkills());
                        if (dialog.Precondition.Perks.Any())
                            prec.Add(new XElement("Perks", Global.GetListAsString(dialog.Precondition.Perks)));
                        if (dialog.Precondition.noPerks.Any())
                            prec.Add(new XElement("noPerks", Global.GetListAsString(dialog.Precondition.noPerks)));
                        if (dialog.Precondition.Achievements.Any())
                            prec.Add(new XElement("Achievements", Global.GetListAsString(dialog.Precondition.Achievements)));
                        if (dialog.Precondition.noAchievements.Any())
                            prec.Add(new XElement("noAchievements", Global.GetListAsString(dialog.Precondition.noAchievements)));
                        if (dialog.Precondition.PlayerLevel != "" && dialog.Precondition.PlayerLevel != ":")
                            prec.Add(new XElement("PlayerLevel", dialog.Precondition.PlayerLevel));
                        if (dialog.Precondition.getReputation() != "")
                            prec.Add(new XElement("Reputation", dialog.Precondition.getReputation()));
                        if (dialog.Precondition.getReputation2() != "")
                            prec.Add(new XElement("Reputation2", dialog.Precondition.getReputation2()));
                        if (dialog.Precondition.getNPCReputation() != "")
                            prec.Add(new XElement("NPCReputation", dialog.Precondition.getNPCReputation()));
                        if (dialog.Precondition.KarmaPK.Any())
                            prec.Add(new XElement("KarmaPK", Global.GetListAsString(dialog.Precondition.KarmaPK)));
                        if (dialog.Precondition.playerCoords.Any())
                        {
                            prec.Add(new XElement("playerCoords", Global.GetListAsString(dialog.Precondition.playerCoords)));
                            prec.Add(new XElement("coordsRadius", Global.GetIntAsString(dialog.Precondition.coordsRadius)));
                        }
                        if (dialog.Precondition.coordsMap != -1)
                            prec.Add(new XElement("map", Global.GetIntAsString(dialog.Precondition.coordsMap)));


                        if (dialog.Precondition.weather.Any())
                            prec.Add(dialog.Precondition.weather.getXML());

                        if (dialog.Precondition.forDev)
                            prec.Add(new XElement("forDev", Global.GetBoolAsString(dialog.Precondition.forDev)));
                        if (dialog.Precondition.hidden)
                            prec.Add(new XElement("hidden", Global.GetBoolAsString(dialog.Precondition.hidden)));

                        if (dialog.Precondition.transport.Any())
                        {
                            prec.Add(new XElement("Transport"));
                            if (dialog.Precondition.transport.inTransportList)
                                prec.Element("Transport").Add(new XElement("inTransportList", Global.GetBoolAsString(dialog.Precondition.transport.inTransportList)));
                            if (dialog.Precondition.transport.notInTransportList)
                                prec.Element("Transport").Add(new XElement("notInTransportList", Global.GetBoolAsString(dialog.Precondition.transport.notInTransportList)));
                            if (dialog.Precondition.transport.inBoatList)
                                prec.Element("Transport").Add(new XElement("inBoatList", Global.GetBoolAsString(dialog.Precondition.transport.inBoatList)));
                            if (dialog.Precondition.transport.notInBoatList)
                                prec.Element("Transport").Add(new XElement("notInBoatList", Global.GetBoolAsString(dialog.Precondition.transport.notInBoatList)));
                            if (dialog.Precondition.transport.boatInTransit)
                                prec.Element("Transport").Add(new XElement("boatInTransit", Global.GetBoolAsString(dialog.Precondition.transport.boatInTransit)));
                            if (dialog.Precondition.transport.boatStopped)
                                prec.Element("Transport").Add(new XElement("boatStopped", Global.GetBoolAsString(dialog.Precondition.transport.boatStopped)));
                            if (dialog.Precondition.transport.boatName.Any())
                                prec.Element("Transport").Add(new XElement("boatName", dialog.Precondition.transport.boatName));
                        }

                        if (dialog.Precondition.tutorialPhase != -1)
                        {
                            prec.Add(new XElement("tutorialPhase", dialog.Precondition.tutorialPhase.ToString()));
                        }

                        if (dialog.Precondition.PVPranks.Sum() > 0)
                        {
                            prec.Add(new XElement("pvpRank", dialog.Precondition.PVPranks[0].ToString() + "-" + dialog.Precondition.PVPranks[1].ToString()));
                        }
						if (dialog.Precondition.PVPMode >= 0)
                            prec.Add(new XElement("pvpMode", dialog.Precondition.PVPMode.ToString()));

                        if (dialog.Precondition.fracBonus.Sum() > 0)
                        {
                            prec.Add(new XElement("groupBonus", string.Join(":", dialog.Precondition.fracBonus)));
                        }
                        if (dialog.Precondition.items.itemCategory != -1)
                        {
                            prec.Add(new XElement("items", new XElement("itemCategory", dialog.Precondition.items.itemCategory.ToString())));
                        }
                        else if (dialog.Precondition.items.items.Any())
                        {
                            prec.Add(new XElement("items", ""));
                            if (dialog.Precondition.items.is_or)
                                prec.Element("items").Add(new XElement("or", "1"));
                            if (dialog.Precondition.items.equipped)
                                prec.Element("items").Add(new XElement("equipped", "1"));
                            prec.Element("items").Add(CQuests.getItemsNode(dialog.Precondition.items.items));
                        }
                        if (dialog.Precondition.itemsNone.itemCategory != -1)
                        {
                            prec.Add(new XElement("noneItems", new XElement("itemCategory", dialog.Precondition.itemsNone.itemCategory.ToString())));
                        }
                        else if (dialog.Precondition.itemsNone.items.Any())
                        {
                            prec.Add(new XElement("noneItems", ""));
                            if (dialog.Precondition.itemsNone.is_or)
                                prec.Element("noneItems").Add(new XElement("or", "1"));
                            if (dialog.Precondition.itemsNone.equipped)
                                prec.Element("noneItems").Add(new XElement("equipped", "1"));
                            prec.Element("noneItems").Add(CQuests.getItemsNode(dialog.Precondition.itemsNone.items));
                        }
                    }
                    if (dialog.Actions.Any())
                    {
                        element.Add(new XElement("Actions", ""));

                        if (dialog.Actions.Exit)
                            element.Element("Actions").Add(new XElement("Exit", Global.GetBoolAsString(dialog.Actions.Exit)));
                        if (dialog.Actions.ToDialog != 0)
                            element.Element("Actions").Add(new XElement("ToDialog", Global.GetIntAsString(dialog.Actions.ToDialog)));
                        if (dialog.Actions.Data != "")
                            element.Element("Actions").Add(new XElement("Data", dialog.Actions.Data));
                        if (dialog.Actions.Event.Value != 0)
                            element.Element("Actions").Add(new XElement("Event", dialog.Actions.Event.Value.ToString()));
                        if (dialog.Actions.GetQuests.Any())
                            element.Element("Actions").Add(new XElement("GetQuest", Global.GetListAsString(dialog.Actions.GetQuests)));
                        if (dialog.Actions.CompleteQuests.Any())
                            element.Element("Actions").Add(new XElement("CompleteQuest", Global.GetListAsString(dialog.Actions.CompleteQuests)));
                        if (dialog.Actions.CancelQuests.Any())
                            element.Element("Actions").Add(new XElement("CancelQuest", Global.GetListAsString(dialog.Actions.CancelQuests)));
                        if (dialog.Actions.FailQuests.Any())
                            element.Element("Actions").Add(new XElement("FailQuest", Global.GetListAsString(dialog.Actions.FailQuests)));
                        if (dialog.Actions.GetKnowleges.Any())
                            element.Element("Actions").Add(new XElement("GetKnowleges", Global.GetListAsString(dialog.Actions.GetKnowleges)));
                        if (dialog.Actions.actionCamera.Any())
                        {
                            element.Element("Actions").Add(new XElement("GoToCamera", dialog.Actions.actionCamera));
                            if (dialog.Actions.actionCameraSmoothly)
                                element.Element("Actions").Add(new XElement("CameraSmoothly", Global.GetBoolAsString(dialog.Actions.actionCameraSmoothly)));
                        }
                        if (dialog.Actions.actionAnimationPlayer.Any())
                            element.Element("Actions").Add(new XElement("AnimationPlayer", dialog.Actions.actionAnimationPlayer));
                        if (dialog.Actions.actionAnimationNPC.Any())
                            element.Element("Actions").Add(new XElement("AnimationNPC", dialog.Actions.actionAnimationNPC));
                        if (dialog.Actions.actionActionNPC != 0)
                            element.Element("Actions").Add(new XElement("ActionNPC", Global.GetIntAsString(dialog.Actions.actionActionNPC)));
                        if (dialog.Actions.actionAdditionalActionNPC.Any())
                            element.Element("Actions").Add(new XElement("AdditionalActionNPC", dialog.Actions.actionAdditionalActionNPC));
                        if (dialog.Actions.actionAvatarPoint.Any())
                            element.Element("Actions").Add(new XElement("AvatarPoint", dialog.Actions.actionAvatarPoint));
                        if (dialog.Actions.actionPlaySound.Any())
                            element.Element("Actions").Add(new XElement("PlaySound", dialog.Actions.actionPlaySound));
                        if (dialog.Actions.changeMoney != 0)
                        {
                            element.Element("Actions").Add(new XElement("ChangeMoney", Global.GetIntAsString(dialog.Actions.changeMoney)));
                            if (dialog.Actions.changeMoneyFailNode != 0)
                            {
                                element.Element("Actions").Add(new XElement("ChangeMoneyFailNode", Global.GetIntAsString(dialog.Actions.changeMoneyFailNode)));
                            }
                        }
                    }

                    if (dialog.Nodes.Any())
                        element.Add(new XElement("Nodes", Global.GetListAsString(dialog.Nodes)));
                    if (dialog.CheckNodes.Any())
                        element.Add(new XElement("CheckNodes", Global.GetListAsString(dialog.CheckNodes)));
                    if (dialog.coordinates.RootDialog)
                        element.Add(new XElement("RootDialog", Global.GetBoolAsString(dialog.coordinates.RootDialog)));
                    if (dialog.coordinates.Active)
                        element.Add(new XElement("Active", Global.GetBoolAsString(dialog.coordinates.Active)));
                    if (dialog.DebugData != "") element.Add(new XElement("DebugData", dialog.DebugData));
                    if (dialog.noLocale) element.Add(new XElement("noLocale", "1"));
                    if (dialog.isAutoNode)
                    {
                        element.Add(new XElement("isAutoNode", Global.GetBoolAsString(dialog.isAutoNode)));
                        if (dialog.defaultNode.Any()) element.Add(new XElement("defaultNode", dialog.defaultNode));
                    }

                    if (dialog.nextDialog > 0)
                        element.Add(new XElement("nextDialog", dialog.nextDialog));

                    resultDoc.Root.Add(element);
                }
                System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
                using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(data_path + "/" + npcName + ".xml", settings))
                {
                    resultDoc.Save(w);
                }

                foreach (string npc_name in deleted_NPC)
                {
                    string path = data_path + "/" + npc_name + ".xml";
                    if (File.Exists(path)) File.Delete(path);
                }
                //resultDoc.Root.Add(npcElement);
            }
        }

        private void SaveNodeCoordinatesOther()
        {
            XDocument resultDoc = new XDocument(new XElement("root"));
            foreach (var key_id in otherCoords)
            {
                
                     resultDoc.Root.Add(new XElement("Node",
                        new XAttribute("ID", key_id.Key),
                        new XElement("X", Convert.ToString(key_id.Value.X, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))),
                        new XElement("Y", Convert.ToString(key_id.Value.Y, System.Globalization.CultureInfo.GetCultureInfo("ru-RU")))));
            }
            System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
            using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create("OtherNodes.xml", settings))
            {
                resultDoc.Save(w);
            }
        }
        

        private void SaveNodeCoordinates(string data_path, NPCDicts target)
        {
            //XElement npc_element;
            foreach (String NPC_Name in target.Keys)
            {
                XDocument resultDoc = new XDocument(new XElement("root"));
                //npc_element = new XElement("NPC", new XAttribute("NPC_Name", NPC_Name));
                foreach (CDialog dialog in target[NPC_Name].Values)
                {
                    resultDoc.Root.Add(new XElement("Dialog",
                        new XAttribute("ID", dialog.DialogID.ToString()),
                        new XElement("X", Convert.ToString(dialog.coordinates.X, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))),
                        new XElement("Y", Convert.ToString(dialog.coordinates.Y, System.Globalization.CultureInfo.GetCultureInfo("ru-RU")))));
                }
                System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
                using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(data_path + NPC_Name + ".xml", settings))
                {
                    resultDoc.Save(w);
                }
                //resultDoc.Root.Add(npc_element);
            }

            foreach(string npc_name in deleted_NPC)
            {
                string path = data_path + npc_name + ".xml";
                if (File.Exists(path)) File.Delete(path);
            }

        }

        private void ParseNodeCoordinatesOther()
        {
            string filename = "OtherNodes.xml";
            doc = XDocument.Load(filename);
            lock_paths.Add(filename);
            foreach (XElement dialog in doc.Root.Elements())
            {
                string id = dialog.Attribute("ID").Value;
                float x = float.Parse(dialog.Element("X").Value, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                float y = float.Parse(dialog.Element("Y").Value, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                otherCoords.Add(id, new NodeCoordinates(x, y, false, false));
            }
        }

        private void ParseNodeCoordinates(string data_path)
        {
            string[] dirs = Directory.GetFiles(data_path, "*.xml");
            foreach (string filename in dirs)
            {
                try
                {
                    doc = XDocument.Load(filename);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Произошла ошибка при чтении файла: " + filename + "\n" + e.Message, "Ошибка чтения");
                    continue;
                }
                bool find_error = false;

                string npc_name = Path.GetFileNameWithoutExtension(filename);
                if (!tempCoordinates.ContainsKey(npc_name))
                    tempCoordinates.Add(npc_name, new Dictionary<int, NodeCoordinates>());

                foreach (XElement dialog in doc.Root.Elements())
                {


                    int id = int.Parse(dialog.Attribute("ID").Value);

                    float x = float.Parse(dialog.Element("X").Value, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                    float y = float.Parse(dialog.Element("Y").Value, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                    try
                    {
                        tempCoordinates[npc_name].Add(id, new NodeCoordinates(x, y, false, false));
                    }
                    catch (Exception e)
                    {
                        if (!find_error)
                        {
                            MessageBox.Show("Произошла ошибка при чтении файла: " + filename + "\n" + e.Message, "Ошибка чтения");
                            find_error = true;
                        }
                        continue;
                    }

                }
            }
        }

        //! Возвращает список всех NPC
        public List<string> getListOfNPC()
        {
            List<string> npc = new List<string>();
            foreach (string key in dialogs.Keys)
                npc.Add(key);
            return npc;
        }

        //--------------------------locale dialogs-------------------------------------------------------

        //! Возвращает CDialog по заданной локали, имени NPC и ID диалога
        public static CDialog getLocaleDialog(int dialogID, string locale, string npcName, NPCLocales dialog_locales)
        {
            if (dialog_locales.Keys.Contains(locale))
                if (dialog_locales[locale].Keys.Contains(npcName))
                    if (dialog_locales[locale][npcName].Keys.Contains(dialogID))
                        return dialog_locales[locale][npcName][dialogID];
            return null;
        }

        //! Добавить диалог к локали
        public void addLocaleDialog(CDialog dialog, string locale)
        {
            if (!this.locales.Keys.Contains(locale))
            {
                this.locales.Add(locale, new NPCDicts());
            }
            if (!this.locales[locale].Keys.Contains(dialog.Holder))
            {
                this.locales[locale].Add(dialog.Holder, new Dictionary<int, CDialog>());
            }

            if (this.locales[locale][dialog.Holder].Keys.Contains(dialog.DialogID))
                this.locales[locale][dialog.Holder].Remove(dialog.DialogID);
            this.locales[locale][dialog.Holder].Add(dialog.DialogID, dialog);
        }

        //! Возвращает словарь из диалогов для локализации (устаревшие, актуальные или все)
        public DifferenceDict getDialogDifference(string locale, FindType findType)
        {
            DifferenceDict ret = new DifferenceDict();
            if (this.locales.Keys.Contains(locale))
            {
                var cur_locale_info = this.locales[locale];
                foreach (var npc_name in dialogs.Keys)
                {
                    if (!cur_locale_info.Keys.Contains(npc_name))
                    {
                        //NPCDialogDict dict = parent.getDialogDictionary(npc_name);
                        NPCDialogDict dict = new NPCDialogDict();
                        dict.Add(dialogs[npc_name].First().Key, new CDialog());
                        cur_locale_info.Add(npc_name, dict);
                    }
                    var locale_dialogs = cur_locale_info[npc_name];
                    foreach (var dialog in dialogs[npc_name].Values)
                    {
                        if (!dialog.coordinates.Active)
                            continue;
                        if (!ManagerNPC.NpcData.ContainsKey(npc_name) || ManagerNPC.NpcData[npc_name].location == "notfound")
                            continue;

                        var locale_version = 0;
                        if (locale_dialogs.Keys.Contains(dialog.DialogID))
                            locale_version = locale_dialogs[dialog.DialogID].version;

                        if (!ret.Keys.Contains(npc_name))
                            ret.Add(npc_name, new Dictionary<int, CDifference>());
                        switch (findType)
                        {
                            case FindType.all:
                                ret[npc_name].Add(dialog.DialogID, new CDifference(dialog.version, locale_version));
                                break;
                            case FindType.outdatedOnly:
                                if (dialog.version != locale_version)
                                    ret[npc_name].Add(dialog.DialogID, new CDifference(dialog.version, locale_version));
                                break;
                            case FindType.actualOnly:
                                if (dialog.version == locale_version)
                                    ret[npc_name].Add(dialog.DialogID, new CDifference(dialog.version, locale_version));
                                break;
                        }
                    }
                }
            }
            return ret;
        }

        public string getDialogToDoToolTip(int dialogID)
        {
            if (todoTooltips.ContainsKey(dialogID))
                return todoTooltips[dialogID];
            return "";
        }

        public void setDialogToDoToolTip(int dialogID, string text)
        {
            if (todoTooltips.ContainsKey(dialogID))
                todoTooltips[dialogID] = text;
            else
                todoTooltips.Add(dialogID, text);
        }

        public static List<int> getDialogNewIDs(int count)
        {
            List<int> result = new List<int>();
            if (count < 1)
                return result;
            string html = string.Empty;
            string url = @"http://hz-dev2.stalker.so:8011/getnextidrange?key=qdialog_id&count=" + count.ToString();

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
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(html);
                List<int> new_dialog_id = json["qdialog_id"].ToObject<List<int>>();
                for (int i = new_dialog_id[0]; i < new_dialog_id[1]; i++)
                    result.Add(i);
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка получения нового ID диалога. Проверьте своё подключение к hz-dev", "Ошибка");
                return new List<int>();
            }
        }

        public static int getDialogsNewID()
        {

            string html = string.Empty;
            string url = @"http://hz-dev2.stalker.so:8011/getnextid?key=qdialog_id";

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
                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(html);
                int new_dialog_id = (int)json["qdialog_id"];
                return new_dialog_id;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка получения нового ID диалога. Проверьте своё подключение к hz-dev", "Ошибка");
                return 0;
            }
        }

        public void lock_files()
        {
            foreach (var path in lock_paths)
                fs_list.Add(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public void unlock_files()
        {
            foreach (var file in fs_list)
            {
                file.Close();
            }
            fs_list.Clear();
        }
    }
}