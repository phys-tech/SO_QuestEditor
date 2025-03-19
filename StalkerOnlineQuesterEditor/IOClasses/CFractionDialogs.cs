using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using UMD.HCIL.Piccolo;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor
{
    //! Словарь <DialogID, CDialog>
    using DialogDict = Dictionary<int, CDialog>;
    //! Словарь <FractionID, <DialogID, CDialog>>
    using FractionDicts = Dictionary<string, Dictionary<int, CDialog>>;

    //! Словарь <LocaleName, <NPCName, <DialogID, CDialog>>>
    using NPCLocales = Dictionary<string, Dictionary<string, Dictionary<int, CDialog>>>;

    //! Словарь <FractionID, <DialogID, CDifference>>
    using DifferenceDict = Dictionary<string, Dictionary<int, CDifference>>;
    //! Словарь <Имя NPC, < ID диалога, Структура NodeCoordinates > 
    using CoordinatesDict = Dictionary<string, Dictionary<int, NodeCoordinates>>;

    //! Класс обработки диалогов
    public abstract class CFractionDialogs
    {
        static private MainForm parent;
        static public FractionDicts dialogs = new FractionDicts();
        //! Словарь локалей
        static public NPCLocales locales = new NPCLocales();
        static private CoordinatesDict tempCoordinates = new CoordinatesDict();
        static public Dictionary<int, string> dialogIDList = new Dictionary<int, string>();
        static private Dictionary<int, List<string>> dialogErrors = new Dictionary<int, List<string>>();

        public static void load(MainForm parent)
        {
            CFractionDialogs.parent = parent;

            ParseNodeCoordinates("NodeCoordinates/FractionDialogs/");

            ParseDialogsData(CSettings.GetDialogDataPath() + "FractionDialogs\\", CFractionDialogs.dialogs, true);
            ParseDialogsTexts(CSettings.GetDialogTextPath(CSettings.ORIGINAL_PATH) + "FractionDialogs\\", CFractionDialogs.dialogs);

            foreach (var locale in CSettings.getListLocales())
            {
                if (!locales.Keys.Contains(locale))
                    locales.Add(locale, new FractionDicts());
                ParseDialogsData(CSettings.GetDialogDataPath() + "FractionDialogs\\", CFractionDialogs.locales[locale]);
                ParseDialogsTexts(CSettings.GetDialogTextPath(locale) + "FractionDialogs\\", CFractionDialogs.locales[locale]);
            }
        }
        
        //! Парсер xml - файла данных диалогов, записывает результат в target
        private static void ParseDialogsData(String dialogDataPath, FractionDicts target, bool findErrors = false)
        {
            string[] files = Directory.GetFiles(dialogDataPath, "*.xml");
            foreach (string dialogFile in files)
            {
                if (!File.Exists(dialogFile))
                    return;
                List<int> tests;
                XDocument doc = XDocument.Load(dialogFile);
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
                        Actions.ToDialog = CDialogs.ParseIntIfNotEmpty(dialog, "Actions", "ToDialog", 0);
                        Actions.Event = parent.dialogEvents.GetEventFromID(CDialogs.ParseIntIfNotEmpty(dialog, "Actions", "Event", 0));
                        if (dialog.Element("Actions").Element("Data") != null)
                            Actions.Data = dialog.Element("Actions").Element("Data").Value;

                        CDialogs.AddDataToList(dialog, "Actions", "GetQuest", Actions.GetQuests);
                        CDialogs.AddDataToList(dialog, "Actions", "CompleteQuest", Actions.CompleteQuests);
                        CDialogs.AddDataToList(dialog, "Actions", "GetKnowleges", Actions.GetKnowleges);
                        if (dialog.Element("Actions").Descendants().Any(item => item.Name == "CancelQuest"))
                        {
                            CDialogs.AddDataToList(dialog, "Actions", "CancelQuest", Actions.CancelQuests);
                        }
                        CDialogs.AddDataToList(dialog, "Actions", "FailQuest", Actions.FailQuests);
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
                            CDialogs.AddDialogSkillsToListSkills(dialog, "Precondition", "Skills", Precondition.Skills);
                        Precondition.KarmaPK = new List<int>();

                        Precondition.Perks = new List<int>();
                        if (dialog.Element("Precondition").Element("Perks") != null)
                            CDialogs.AddDataToList(dialog, "Precondition", "Perks", Precondition.Perks);
                        Precondition.noPerks = new List<int>();
                        if (dialog.Element("Precondition").Element("noPerks") != null)
                            CDialogs.AddDataToList(dialog, "Precondition", "noPerks", Precondition.noPerks);
                        Precondition.KarmaPK = new List<int>();

                        Precondition.Achievements = new List<int>();
                        if (dialog.Element("Precondition").Element("Achievements") != null)
                            CDialogs.AddDataToList(dialog, "Precondition", "Achievements", Precondition.Achievements);
                        Precondition.noAchievements = new List<int>();
                        if (dialog.Element("Precondition").Element("noAchievements") != null)
                            CDialogs.AddDataToList(dialog, "Precondition", "noAchievements", Precondition.noAchievements);

                        CDialogs.AddDataToList(dialog, "Precondition", "KarmaPK", Precondition.KarmaPK);
                        CDialogs.AddDataToList(dialog, "Precondition", "playerCoords", Precondition.playerCoords);
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
                            CDialogs.AddDataToList(dialog, "Precondition", "tests", tests);
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
                    if (dialog.Element("nextDialog") != null) nextDialog = int.Parse(dialog.Element("nextDialog").Value);
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
        
        private static void ParseDialogsTexts(String dataPath, FractionDicts target)
        {
         string[] files = Directory.GetFiles(dataPath, "*.xml");
            foreach (string dialogFile in files)
            {
                XDocument doc;
                try
                {
                    doc = XDocument.Load(dialogFile);
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
                        target[npc_name][DialogID].Text = dialog.Element("Text").Value.Trim();
                    int Version = 0;
                    if ((!dialog.Element("Version").Value.Equals("")))
                        Version = int.Parse(dialog.Element("Version").Value);
                    target[npc_name][DialogID].version = Version;

                }
            }
        }
   
        //! Сохранить все диалоги в xml файл
        public static void SaveDialogs()
        {
            SaveNodeCoordinates("NodeCoordinates/FractionDialogs/", CFractionDialogs.dialogs);
            SaveDialogsTexts(CSettings.GetDialogTextPath(CSettings.ORIGINAL_PATH) + "FractionDialogs/", CFractionDialogs.dialogs);
            SaveDialogsData(CSettings.GetDialogDataPath()+ "FractionDialogs/", CFractionDialogs.dialogs);
        }

        //! Сохраняет текущую локализацию диалогов в файл
        public static void SaveLocales()
        {
            foreach(var locale in CSettings.getListLocales())
                SaveDialogsTexts(CSettings.GetDialogLocaleTextPath(locale) + "FractionDialogs/", CFractionDialogs.locales[locale]);
        }

        private static void SaveDialogsTexts(string filePath, FractionDicts target)
        {
            
            XElement element;

            foreach (string fractionID in target.Keys)
            {
                XDocument resultDoc = new XDocument(new XElement("root"));
                DialogDict Dictdialog = target[fractionID];
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
                string fileName = filePath + fractionID + ".xml";
                System.Xml.XmlWriterSettings settings = Global.GetXmlSettings();
                using (System.Xml.XmlWriter w = System.Xml.XmlWriter.Create(fileName, settings))
                {
                    resultDoc.Save(w);
                }
            }
           
        }

        private static void SaveDialogsData(string data_path, FractionDicts target)
        {  
            //XElement npcElement;
            foreach (string npcName in target.Keys)
            {
                XDocument resultDoc = new XDocument(new XElement("root"));
                //npcElement = new XElement("NPC", new XElement("Name", npcName));
                DialogDict Dictdialog = target[npcName];
                XElement element;
                foreach (CDialog dialog in Dictdialog.Values)
                {
                    element = new XElement("Dialog",
                       new XElement("ID", dialog.DialogID.ToString()));
                    if (dialog.Precondition.Exists())
                    {
                         element.Add(new XElement("Precondition"));
                    
                    if (dialog.Precondition.ListOfNecessaryQuests.Any())
                    {
                        element.Element("Precondition").Add(new XElement("ListOfNecessaryQuests"));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfCompletedQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfCompletedQuests, 
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfCompletedQuests)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfHaveQuests.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfHaveQuests,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfQuests)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfOpenedQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfOpenedQuests,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfOpenedQuests)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfFailQuests.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfFailQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfFailQuests,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfFailQuests)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfOnTestQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfOnTestQuests,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfOnTestQuest)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfCounters.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfOmniCounters",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfCounters,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfCounterss)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfRepeat.Any())
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfRepeat",
                                          Global.GetListAsString(dialog.Precondition.ListOfNecessaryQuests.ListOfRepeat,
                                                                dialog.Precondition.ListOfNecessaryQuests.conditionOfRepeat)));
                        if (dialog.Precondition.ListOfNecessaryQuests.ListOfMassQuests != "")
                            element.Element("Precondition").Element("ListOfNecessaryQuests").Add(new XElement("listOfMassQuests",
                                                                     dialog.Precondition.ListOfNecessaryQuests.ListOfMassQuests.Replace(',', dialog.Precondition.ListOfNecessaryQuests.conditionOfMassQuests)));
                    }
                    if (dialog.Precondition.ListOfMustNoQuests.Any())
                    {
                        element.Element("Precondition").Add(new XElement("ListOfMustNoQuests"));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfCompletedQuests.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfCompletedQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfCompletedQuests,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfCompletedQuests)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfHaveQuests.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfHaveQuests,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfQuests)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfOpenedQuests.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfOpenedQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfOpenedQuests,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfOpenedQuests)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfFailQuests.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfFailQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfFailQuests,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfFailQuests)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfOnTestQuests.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfOnTestQuests",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfOnTestQuests,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfOnTestQuest)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfCounters.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfOmniCounters",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfCounters,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfCounterss)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfRepeat.Any())
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfRepeat",
                                          Global.GetListAsString(dialog.Precondition.ListOfMustNoQuests.ListOfRepeat,
                                                                dialog.Precondition.ListOfMustNoQuests.conditionOfRepeat)));
                        if (dialog.Precondition.ListOfMustNoQuests.ListOfMassQuests != "")
                            element.Element("Precondition").Element("ListOfMustNoQuests").Add(new XElement("listOfMassQuests",
                                                                     dialog.Precondition.ListOfMustNoQuests.ListOfMassQuests.Replace(',', dialog.Precondition.ListOfMustNoQuests.conditionOfMassQuests)));
                    }
                        if (dialog.Precondition.knowledges.Any())
                        {
                            element.Element("Precondition").Add(new XElement("Knowledges"));
                            if (dialog.Precondition.knowledges.mustKnowledge.Any())
                                element.Element("Precondition").Element("Knowledges").Add(new XElement("mustKnowledge",
                                              Global.GetListAsString(dialog.Precondition.knowledges.mustKnowledge,
                                                                    dialog.Precondition.knowledges.conditionMustKnowledge)));
                            if (dialog.Precondition.knowledges.shouldntKnowledge.Any())
                                element.Element("Precondition").Element("Knowledges").Add(new XElement("shouldntKnowledge",
                                              Global.GetListAsString(dialog.Precondition.knowledges.shouldntKnowledge,
                                                                    dialog.Precondition.knowledges.conditionShouldntKnowledge)));
                        }
                    if (dialog.Precondition.clanOptions != "")
                        element.Element("Precondition").Add(new XElement("clanOptions", dialog.Precondition.clanOptions));
                    if (dialog.Precondition.clanLevel.Sum() > 0)
                        element.Element("Precondition").Add(new XElement("clanLevel", string.Join(",", dialog.Precondition.clanLevel)));
                    if (dialog.Precondition.radioAvailable != RadioAvalible.None)
                        element.Element("Precondition").Add(new XElement("radioAvailable", Convert.ToInt32(dialog.Precondition.radioAvailable).ToString()));
                    if (dialog.Precondition.dungeonPhase > 0)
                        element.Element("Precondition").Add(new XElement("dungeonPhase", Convert.ToInt32(dialog.Precondition.dungeonPhase).ToString()));
                    if (dialog.Precondition.seasonEvent.Any())
                        element.Element("Precondition").Add(new XElement("seasonEvent", dialog.Precondition.seasonEvent));
                    if (dialog.Precondition.dungeonNot)
                        element.Element("Precondition").Add(new XElement("dungeonNot", "1"));
                    if (dialog.Precondition.MustNoEffects.Any())
                        element.Element("Precondition").Add(dialog.Precondition.getMustNoEffects());
                    if (dialog.Precondition.NecessaryEffects.Any())
                        element.Element("Precondition").Add(dialog.Precondition.getNecessaryEffects());
                    if (dialog.Precondition.Skills.Any())
                        element.Element("Precondition").Add(dialog.Precondition.Skills.getSkills());
                        if (dialog.Precondition.Perks.Any())
                            element.Element("Precondition").Add(new XElement("Perks", Global.GetListAsString(dialog.Precondition.Perks)));
                        if (dialog.Precondition.noPerks.Any())
                            element.Element("Precondition").Add(new XElement("noPerks", Global.GetListAsString(dialog.Precondition.noPerks)));
                        if (dialog.Precondition.Achievements.Any())
                            element.Element("Precondition").Add(new XElement("Achievements", Global.GetListAsString(dialog.Precondition.Achievements)));
                        if (dialog.Precondition.noAchievements.Any())
                            element.Element("Precondition").Add(new XElement("noAchievements", Global.GetListAsString(dialog.Precondition.noAchievements)));
                        if (dialog.Precondition.PlayerLevel != "" && dialog.Precondition.PlayerLevel != ":")
                    if (dialog.Precondition.PlayerLevel != "" && dialog.Precondition.PlayerLevel != ":")
                        element.Element("Precondition").Add(new XElement("PlayerLevel", dialog.Precondition.PlayerLevel));
                    if (dialog.Precondition.getReputation() != "")
                        element.Element("Precondition").Add(new XElement("Reputation", dialog.Precondition.getReputation()));
                    if (dialog.Precondition.getReputation2() != "")
                        element.Element("Precondition").Add(new XElement("Reputation2", dialog.Precondition.getReputation2()));
                    if (dialog.Precondition.getNPCReputation() != "")
                        element.Element("Precondition").Add(new XElement("NPCReputation", dialog.Precondition.getNPCReputation()));
                    if (dialog.Precondition.KarmaPK.Any())
                        element.Element("Precondition").Add(new XElement("KarmaPK", Global.GetListAsString(dialog.Precondition.KarmaPK)));
                    }

                    if (dialog.Precondition.playerCoords.Any())
                    {
                        element.Element("Precondition").Add(new XElement("playerCoords", Global.GetListAsString(dialog.Precondition.playerCoords)));
                        element.Element("Precondition").Add(new XElement("coordsRadius", Global.GetIntAsString(dialog.Precondition.coordsRadius)));
                    }
                    if (dialog.Precondition.coordsMap != -1)
                        element.Element("Precondition").Add(new XElement("map", Global.GetIntAsString(dialog.Precondition.coordsMap)));

                    if (dialog.Precondition.weather.Any())
                        element.Element("Precondition").Add(dialog.Precondition.weather.getXML());

                    if (dialog.Precondition.forDev)
                        element.Element("Precondition").Add(new XElement("forDev", Global.GetBoolAsString(dialog.Precondition.forDev)));
                    if (dialog.Precondition.hidden)
                        element.Element("Precondition").Add(new XElement("hidden", Global.GetBoolAsString(dialog.Precondition.hidden)));

                    if (dialog.Precondition.transport.Any())
                    {
                        element.Element("Precondition").Add(new XElement("Transport"));
                        if (dialog.Precondition.transport.inTransportList)
                            element.Element("Precondition").Element("Transport").Add(new XElement("inTransportList", Global.GetBoolAsString(dialog.Precondition.transport.inTransportList)));
                        if (dialog.Precondition.transport.notInTransportList)
                            element.Element("Precondition").Element("Transport").Add(new XElement("notInTransportList", Global.GetBoolAsString(dialog.Precondition.transport.notInTransportList)));
                        if (dialog.Precondition.transport.inBoatList)
                            element.Element("Precondition").Element("Transport").Add(new XElement("inBoatList", Global.GetBoolAsString(dialog.Precondition.transport.inBoatList)));
                        if (dialog.Precondition.transport.notInBoatList)
                            element.Element("Precondition").Element("Transport").Add(new XElement("notInBoatList", Global.GetBoolAsString(dialog.Precondition.transport.notInBoatList)));
                        if (dialog.Precondition.transport.boatInTransit)
                            element.Element("Precondition").Element("Transport").Add(new XElement("boatInTransit", Global.GetBoolAsString(dialog.Precondition.transport.boatInTransit)));
                        if (dialog.Precondition.transport.boatStopped)
                            element.Element("Precondition").Element("Transport").Add(new XElement("boatStopped", Global.GetBoolAsString(dialog.Precondition.transport.boatStopped)));
                        if (dialog.Precondition.transport.boatName.Any())
                            element.Element("Precondition").Element("Transport").Add(new XElement("boatName", dialog.Precondition.transport.boatName));
                    }

                    if (dialog.Precondition.tutorialPhase != -1)
                    {
                        element.Element("Precondition").Add(new XElement("tutorialPhase", dialog.Precondition.tutorialPhase.ToString()));
                    }

                    if (dialog.Precondition.PVPranks.Sum() > 0)
                    {
                        element.Element("Precondition").Add(new XElement("pvpRank", dialog.Precondition.PVPranks[0].ToString() + "-" + dialog.Precondition.PVPranks[1].ToString()));
                    }

                    if (dialog.Precondition.PVPMode >= 0)
                        element.Element("Precondition").Add(new XElement("pvpMode", dialog.Precondition.PVPMode.ToString()));

                    if (dialog.Precondition.fracBonus.Sum() > 0)
                    {
                        element.Element("Precondition").Add(new XElement("groupBonus", string.Join(":", dialog.Precondition.fracBonus)));
                    }

                    if (dialog.Precondition.items.itemCategory != -1)
                    {
                        element.Element("Precondition").Add(new XElement("items", new XElement("itemCategory", dialog.Precondition.items.itemCategory.ToString())));
                    }
                    else if (dialog.Precondition.items.items.Any())
                    {
                        element.Element("Precondition").Add(new XElement("items", ""));
                        if (dialog.Precondition.items.is_or)
                            element.Element("Precondition").Element("items").Add(new XElement("or", "1"));
                        element.Element("Precondition").Element("items").Add(CQuests.getItemsNode(dialog.Precondition.items.items));                     
                    }
                    if (dialog.Precondition.itemsNone.itemCategory != -1)
                    {
                        element.Element("Precondition").Add(new XElement("noneItems", new XElement("itemCategory", dialog.Precondition.itemsNone.itemCategory.ToString())));
                    }
                    else if (dialog.Precondition.itemsNone.items.Any())
                    {
                        element.Element("Precondition").Add(new XElement("noneItems", ""));
                        if (dialog.Precondition.itemsNone.is_or)
                            element.Element("Precondition").Element("noneItems").Add(new XElement("or", "1"));
                        element.Element("Precondition").Element("noneItems").Add(CQuests.getItemsNode(dialog.Precondition.itemsNone.items));
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
                            element.Element("Actions").Add( new XElement("FailQuest", Global.GetListAsString(dialog.Actions.FailQuests)));
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
                    if (dialog.DebugData != "") element.Add(new XElement("DebugData",dialog.DebugData ));
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
                //resultDoc.Root.Add(npcElement);
            }       
        }

        private static void SaveNodeCoordinates(string data_path, FractionDicts target)
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
            
        }

        private static void ParseNodeCoordinates(string data_path)
        {
            string[] dirs = Directory.GetFiles(data_path, "*.xml");
            XDocument doc;
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
        
        //--------------------------locale dialogs-------------------------------------------------------

        //! Добавить диалог к локали
        public static void addLocaleDialog(CDialog dialog, string locale)
        {
            if (!CFractionDialogs.locales.Keys.Contains(locale))
            {
                CFractionDialogs.locales.Add(locale, new FractionDicts());
            }
            if (!CFractionDialogs.locales[locale].Keys.Contains(dialog.Holder))
            {
                CFractionDialogs.locales[locale].Add(dialog.Holder, new Dictionary<int, CDialog>());
            }

            if (CFractionDialogs.locales[locale][dialog.Holder].Keys.Contains(dialog.DialogID))
                CFractionDialogs.locales[locale][dialog.Holder].Remove(dialog.DialogID);
            CFractionDialogs.locales[locale][dialog.Holder].Add(dialog.DialogID, dialog);
        }

        //! Возвращает словарь из диалогов для локализации (устаревшие, актуальные или все)
        public static DifferenceDict getDialogDifference(string locale, FindType findType)
        {
            DifferenceDict ret = new DifferenceDict();
            if (CFractionDialogs.locales.Keys.Contains(locale))
            {
                var cur_locale_info = CFractionDialogs.locales[locale];
                foreach (var npc_name in dialogs.Keys)
                {
                    if (!cur_locale_info.Keys.Contains(npc_name))
                    {
                        //NPCDialogDict dict = parent.getDialogDictionary(npc_name);
                        DialogDict dict = new DialogDict();
                        dict.Add(dialogs[npc_name].First().Key, new CDialog() );
                        cur_locale_info.Add(npc_name, dict);
                    }
                    var locale_dialogs = cur_locale_info[npc_name];
                    foreach (var dialog in dialogs[npc_name].Values)
                    {
                        if (!dialog.coordinates.Active)
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

    }
}