using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;

namespace StalkerOnlineQuesterEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitDialogs = new System.Windows.Forms.SplitContainer();
            this.gbDialogsEditor = new System.Windows.Forms.GroupBox();
            this.splitDialogsTreeAndCanvas = new System.Windows.Forms.SplitContainer();
            this.treeDialogs = new System.Windows.Forms.TreeView();
            this.DialogShower = new UMD.HCIL.Piccolo.PCanvas();
            this.panelDialogTools = new System.Windows.Forms.Panel();
            this.bCopyDialogTree = new System.Windows.Forms.Button();
            this.lFindDialogID = new System.Windows.Forms.Label();
            this.tbFindDialogID = new System.Windows.Forms.TextBox();
            this.btnClearRecycle = new System.Windows.Forms.Button();
            this.labelDrawingTip = new System.Windows.Forms.Label();
            this.bCenterizeDialogShower = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.labelYNode = new System.Windows.Forms.Label();
            this.labelXNode = new System.Windows.Forms.Label();
            this.bRemoveDialog = new System.Windows.Forms.Button();
            this.bEditDialog = new System.Windows.Forms.Button();
            this.bAddDialog = new System.Windows.Forms.Button();
            this.gbNPCWorkSpace = new System.Windows.Forms.GroupBox();
            this.bWSDel = new System.Windows.Forms.Button();
            this.bWSAdd = new System.Windows.Forms.Button();
            this.lbWorkSpace = new System.Windows.Forms.ListBox();
            this.gbEmulator = new System.Windows.Forms.GroupBox();
            this.splitDialogsEmulator = new System.Windows.Forms.SplitContainer();
            this.splitQuestsContainer = new System.Windows.Forms.SplitContainer();
            this.treeQuest = new System.Windows.Forms.TreeView();
            this.labelQuestTree = new System.Windows.Forms.Label();
            this.treeQuestBuffer = new System.Windows.Forms.TreeView();
            this.labelBuffer = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.NPCBox = new System.Windows.Forms.ComboBox();
            this.labelChosenNPC = new System.Windows.Forms.Label();
            this.CentralDock = new System.Windows.Forms.TabControl();
            this.tabDialogs = new System.Windows.Forms.TabPage();
            this.tabQuests = new System.Windows.Forms.TabPage();
            this.panelQuestTools = new System.Windows.Forms.Panel();
            this.bClearBuffer = new System.Windows.Forms.Button();
            this.bCutEvents = new System.Windows.Forms.Button();
            this.bPasteEvents = new System.Windows.Forms.Button();
            this.bCopyEvents = new System.Windows.Forms.Button();
            this.bQuestDown = new System.Windows.Forms.Button();
            this.bQuestUp = new System.Windows.Forms.Button();
            this.bRemoveEvent = new System.Windows.Forms.Button();
            this.bEditEvent = new System.Windows.Forms.Button();
            this.bAddEvent = new System.Windows.Forms.Button();
            this.panelSelectQuest = new System.Windows.Forms.Panel();
            this.bRemoveQuest = new System.Windows.Forms.Button();
            this.bAddQuest = new System.Windows.Forms.Button();
            this.QuestBox = new System.Windows.Forms.ComboBox();
            this.labelChosenQuest = new System.Windows.Forms.Label();
            this.FakeQuestBox = new System.Windows.Forms.ComboBox();
            this.tabFraction = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bAddFracDialog = new System.Windows.Forms.Button();
            this.bEditFracDialog = new System.Windows.Forms.Button();
            this.bRemoveFracDialog = new System.Windows.Forms.Button();
            this.gbEmul2 = new System.Windows.Forms.GroupBox();
            this.treeFractionDialogs = new System.Windows.Forms.TreeView();
            this.fractionDialogShower = new UMD.HCIL.Piccolo.PCanvas();
            this.label1 = new System.Windows.Forms.Label();
            this.fractionBox = new System.Windows.Forms.ComboBox();
            this.tabInfoNPC = new System.Windows.Forms.TabPage();
            this.npcLinkShower = new UMD.HCIL.Piccolo.PCanvas();
            this.panelNpcLinkControls = new System.Windows.Forms.Panel();
            this.labelAdviceNpcLink = new System.Windows.Forms.Label();
            this.bNpcLinkExecute = new System.Windows.Forms.Button();
            this.tabReview = new System.Windows.Forms.TabPage();
            this.dgvReview = new System.Windows.Forms.DataGridView();
            this.colNPCName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDialogsNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuestsNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCoordinates = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRussianName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelReviewButtons = new System.Windows.Forms.Panel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tbKnowledgeFind = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.bFindKnowledgeQuest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbOnlyTradePoints = new System.Windows.Forms.CheckBox();
            this.cbRepLocations = new System.Windows.Forms.ComboBox();
            this.cbOnRepLocation = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRep2List = new System.Windows.Forms.ComboBox();
            this.bFindNPCReputation = new System.Windows.Forms.Button();
            this.gbNPCcheck = new System.Windows.Forms.GroupBox();
            this.cbOnlyOnLocation = new System.Windows.Forms.CheckBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.numQuests = new System.Windows.Forms.NumericUpDown();
            this.labelLessThan2 = new System.Windows.Forms.Label();
            this.cbNumQuests = new System.Windows.Forms.CheckBox();
            this.numDialogs = new System.Windows.Forms.NumericUpDown();
            this.labelLessThan1 = new System.Windows.Forms.Label();
            this.cbNumDialogs = new System.Windows.Forms.CheckBox();
            this.bFindNPC = new System.Windows.Forms.Button();
            this.gbQuestCheck = new System.Windows.Forms.GroupBox();
            this.cbQuestLocations = new System.Windows.Forms.ComboBox();
            this.cbOnQuestLocation = new System.Windows.Forms.CheckBox();
            this.labelItemTarget = new System.Windows.Forms.Label();
            this.cbItemTarget = new System.Windows.Forms.ComboBox();
            this.labelItemReward = new System.Windows.Forms.Label();
            this.cbItemReward = new System.Windows.Forms.ComboBox();
            this.bFindQuest = new System.Windows.Forms.Button();
            this.tabManage = new System.Windows.Forms.TabPage();
            this.dgvManage = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subevents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.npcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subNPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dialogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardBattle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rewardItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repeatPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Legend = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.worked = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bSaveManage = new System.Windows.Forms.Button();
            this.tabTranslate = new System.Windows.Forms.TabPage();
            this.dgvLocaleDiff = new System.Windows.Forms.DataGridView();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.npc_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.identif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cur_ver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.new_ver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RusText1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngText1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDiffLocale = new System.Windows.Forms.Panel();
            this.labelLocalizeOuput = new System.Windows.Forms.Label();
            this.cbActualFinder = new System.Windows.Forms.CheckBox();
            this.cbOutdatedFinder = new System.Windows.Forms.CheckBox();
            this.bFindQuestDifference = new System.Windows.Forms.Button();
            this.bFindDialogDifference = new System.Windows.Forms.Button();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNPC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEngText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelSearchTools = new System.Windows.Forms.Panel();
            this.cbIgnoreCase = new System.Windows.Forms.CheckBox();
            this.labelSearchResult = new System.Windows.Forms.Label();
            this.labelPhraseToSearch = new System.Windows.Forms.Label();
            this.tbPhraseToSearch = new System.Windows.Forms.TextBox();
            this.bStartSearch = new System.Windows.Forms.Button();
            this.tabKnowledge = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnAddKnowlege = new System.Windows.Forms.Button();
            this.btnDelKnowlege = new System.Windows.Forms.Button();
            this.treeKnowladge = new System.Windows.Forms.TreeView();
            this.cbKnowlegeTypeValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbKnowledgeCategory = new System.Windows.Forms.ComboBox();
            this.tabAutoGen = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxReward = new System.Windows.Forms.ListBox();
            this.btnAddReward = new System.Windows.Forms.Button();
            this.btnChangeReward = new System.Windows.Forms.Button();
            this.btnDelReward = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxTarget = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAddTarget = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDelTarget = new System.Windows.Forms.Button();
            this.nupToTargetCount = new System.Windows.Forms.NumericUpDown();
            this.btnChange = new System.Windows.Forms.Button();
            this.nupFromTargetCount = new System.Windows.Forms.NumericUpDown();
            this.btnDelQType = new System.Windows.Forms.Button();
            this.btnAddQType = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.listBoxQT = new System.Windows.Forms.ListBox();
            this.tabPageAGDialog = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lbAGHello = new System.Windows.Forms.ListBox();
            this.btnAGHelloAdd = new System.Windows.Forms.Button();
            this.btnAGHelloChange = new System.Windows.Forms.Button();
            this.btnAGHelloDel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnAGClosed2Add = new System.Windows.Forms.Button();
            this.btnAGClosed2Change = new System.Windows.Forms.Button();
            this.btnAGClosed2Del = new System.Windows.Forms.Button();
            this.lbAGClosed2 = new System.Windows.Forms.ListBox();
            this.lbAGClosed = new System.Windows.Forms.ListBox();
            this.btnAGClosedAdd = new System.Windows.Forms.Button();
            this.btnAGClosedChange = new System.Windows.Forms.Button();
            this.btnAGClosedDel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAGOnTest2Add = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnAGOnTest2Change = new System.Windows.Forms.Button();
            this.btnAGOnTest2Del = new System.Windows.Forms.Button();
            this.lbAGOnTest2 = new System.Windows.Forms.ListBox();
            this.lbAGOnTest = new System.Windows.Forms.ListBox();
            this.btnAGOnTestAdd = new System.Windows.Forms.Button();
            this.btnAGOnTestChange = new System.Windows.Forms.Button();
            this.btnAGOnTestDel = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnAGOpened2Add = new System.Windows.Forms.Button();
            this.btnAGOpened2Change = new System.Windows.Forms.Button();
            this.btnAGOpened2Del = new System.Windows.Forms.Button();
            this.lbAGOpened2 = new System.Windows.Forms.ListBox();
            this.lbAGOpened = new System.Windows.Forms.ListBox();
            this.btnAGOpenedAdd = new System.Windows.Forms.Button();
            this.btnAGOpenedChange = new System.Windows.Forms.Button();
            this.btnAGOpenedDel = new System.Windows.Forms.Button();
            this.cbNPCNature = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.lbAGDeclineQuest = new System.Windows.Forms.ListBox();
            this.btnAGDeclineQuestAdd = new System.Windows.Forms.Button();
            this.btnAGDeclineQuestChange = new System.Windows.Forms.Button();
            this.btnAGDeclineQuestDel = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.lbAGAcceptQuest = new System.Windows.Forms.ListBox();
            this.btnAGAcceptQuestAdd = new System.Windows.Forms.Button();
            this.btnAGAcceptQuestChange = new System.Windows.Forms.Button();
            this.btnAGAcceptQuestDel = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.lbAGGetQuest = new System.Windows.Forms.ListBox();
            this.btnAGGetQuestAdd = new System.Windows.Forms.Button();
            this.btnAGGetQuestChange = new System.Windows.Forms.Button();
            this.btnAGGetQuestDel = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panelSelectNPC = new System.Windows.Forms.Panel();
            this.btnFilterGroupNPC = new System.Windows.Forms.Button();
            this.btnCheckNPC = new System.Windows.Forms.Button();
            this.btnFilterNPC = new System.Windows.Forms.Button();
            this.btnNextNPC = new System.Windows.Forms.Button();
            this.btnBackNPC = new System.Windows.Forms.Button();
            this.bAddNPC = new System.Windows.Forms.Button();
            this.FakeNPCBox = new System.Windows.Forms.ComboBox();
            this.menuMainControl = new System.Windows.Forms.MenuStrip();
            this.menuMain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSynchronize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.русскийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.обновленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.парсерыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаОшибокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поискДиалоговПоQuestIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поискДиалоговПоЗнаниюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.способыПолученияКвестаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.собратьЭдиторДляПередачиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вытащитьНепереведённыеТекстыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьПереводToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.диалоговToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.квестовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolTipDialogs = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogs)).BeginInit();
            this.splitDialogs.Panel1.SuspendLayout();
            this.splitDialogs.Panel2.SuspendLayout();
            this.splitDialogs.SuspendLayout();
            this.gbDialogsEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogsTreeAndCanvas)).BeginInit();
            this.splitDialogsTreeAndCanvas.Panel1.SuspendLayout();
            this.splitDialogsTreeAndCanvas.Panel2.SuspendLayout();
            this.splitDialogsTreeAndCanvas.SuspendLayout();
            this.panelDialogTools.SuspendLayout();
            this.gbNPCWorkSpace.SuspendLayout();
            this.gbEmulator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogsEmulator)).BeginInit();
            this.splitDialogsEmulator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitQuestsContainer)).BeginInit();
            this.splitQuestsContainer.Panel1.SuspendLayout();
            this.splitQuestsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.CentralDock.SuspendLayout();
            this.tabDialogs.SuspendLayout();
            this.tabQuests.SuspendLayout();
            this.panelQuestTools.SuspendLayout();
            this.panelSelectQuest.SuspendLayout();
            this.tabFraction.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbEmul2.SuspendLayout();
            this.tabInfoNPC.SuspendLayout();
            this.panelNpcLinkControls.SuspendLayout();
            this.tabReview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReview)).BeginInit();
            this.panelReviewButtons.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbNPCcheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDialogs)).BeginInit();
            this.gbQuestCheck.SuspendLayout();
            this.tabManage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManage)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabTranslate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocaleDiff)).BeginInit();
            this.panelDiffLocale.SuspendLayout();
            this.tabSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.panelSearchTools.SuspendLayout();
            this.tabKnowledge.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabAutoGen.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupToTargetCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFromTargetCount)).BeginInit();
            this.tabPageAGDialog.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panelSelectNPC.SuspendLayout();
            this.menuMainControl.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitDialogs
            // 
            resources.ApplyResources(this.splitDialogs, "splitDialogs");
            this.splitDialogs.Name = "splitDialogs";
            // 
            // splitDialogs.Panel1
            // 
            this.splitDialogs.Panel1.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.splitDialogs.Panel1, "splitDialogs.Panel1");
            this.splitDialogs.Panel1.Controls.Add(this.gbDialogsEditor);
            // 
            // splitDialogs.Panel2
            // 
            this.splitDialogs.Panel2.Controls.Add(this.gbNPCWorkSpace);
            this.splitDialogs.Panel2.Controls.Add(this.gbEmulator);
            this.splitDialogs.TabStop = false;
            // 
            // gbDialogsEditor
            // 
            this.gbDialogsEditor.Controls.Add(this.splitDialogsTreeAndCanvas);
            this.gbDialogsEditor.Controls.Add(this.panelDialogTools);
            resources.ApplyResources(this.gbDialogsEditor, "gbDialogsEditor");
            this.gbDialogsEditor.Name = "gbDialogsEditor";
            this.gbDialogsEditor.TabStop = false;
            // 
            // splitDialogsTreeAndCanvas
            // 
            this.splitDialogsTreeAndCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitDialogsTreeAndCanvas, "splitDialogsTreeAndCanvas");
            this.splitDialogsTreeAndCanvas.Name = "splitDialogsTreeAndCanvas";
            // 
            // splitDialogsTreeAndCanvas.Panel1
            // 
            this.splitDialogsTreeAndCanvas.Panel1.Controls.Add(this.treeDialogs);
            // 
            // splitDialogsTreeAndCanvas.Panel2
            // 
            this.splitDialogsTreeAndCanvas.Panel2.Controls.Add(this.DialogShower);
            this.splitDialogsTreeAndCanvas.TabStop = false;
            // 
            // treeDialogs
            // 
            resources.ApplyResources(this.treeDialogs, "treeDialogs");
            this.treeDialogs.Name = "treeDialogs";
            this.treeDialogs.TabStop = false;
            this.treeDialogs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDialogs_GotFocus);
            // 
            // DialogShower
            // 
            this.DialogShower.AllowDrop = true;
            this.DialogShower.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.DialogShower, "DialogShower");
            this.DialogShower.GridFitText = false;
            this.DialogShower.Name = "DialogShower";
            this.DialogShower.RegionManagement = true;
            this.DialogShower.Click += new System.EventHandler(this.DialogShower_Click);
            this.DialogShower.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DialogShower_MouseMove);
            // 
            // panelDialogTools
            // 
            resources.ApplyResources(this.panelDialogTools, "panelDialogTools");
            this.panelDialogTools.Controls.Add(this.bCopyDialogTree);
            this.panelDialogTools.Controls.Add(this.lFindDialogID);
            this.panelDialogTools.Controls.Add(this.tbFindDialogID);
            this.panelDialogTools.Controls.Add(this.btnClearRecycle);
            this.panelDialogTools.Controls.Add(this.labelDrawingTip);
            this.panelDialogTools.Controls.Add(this.bCenterizeDialogShower);
            this.panelDialogTools.Controls.Add(this.labelYNode);
            this.panelDialogTools.Controls.Add(this.labelXNode);
            this.panelDialogTools.Controls.Add(this.bRemoveDialog);
            this.panelDialogTools.Controls.Add(this.bEditDialog);
            this.panelDialogTools.Controls.Add(this.bAddDialog);
            this.panelDialogTools.Name = "panelDialogTools";
            // 
            // bCopyDialogTree
            // 
            resources.ApplyResources(this.bCopyDialogTree, "bCopyDialogTree");
            this.bCopyDialogTree.Name = "bCopyDialogTree";
            this.bCopyDialogTree.UseVisualStyleBackColor = true;
            this.bCopyDialogTree.Click += new System.EventHandler(this.bCopyDialogTree_Click);
            // 
            // lFindDialogID
            // 
            resources.ApplyResources(this.lFindDialogID, "lFindDialogID");
            this.lFindDialogID.Name = "lFindDialogID";
            // 
            // tbFindDialogID
            // 
            resources.ApplyResources(this.tbFindDialogID, "tbFindDialogID");
            this.tbFindDialogID.Name = "tbFindDialogID";
            this.tbFindDialogID.TextChanged += new System.EventHandler(this.tbFindDialogID_TextChanged);
            // 
            // btnClearRecycle
            // 
            resources.ApplyResources(this.btnClearRecycle, "btnClearRecycle");
            this.btnClearRecycle.Name = "btnClearRecycle";
            this.btnClearRecycle.UseVisualStyleBackColor = true;
            this.btnClearRecycle.Click += new System.EventHandler(this.btnClearRecycle_Click);
            // 
            // labelDrawingTip
            // 
            resources.ApplyResources(this.labelDrawingTip, "labelDrawingTip");
            this.labelDrawingTip.Name = "labelDrawingTip";
            // 
            // bCenterizeDialogShower
            // 
            resources.ApplyResources(this.bCenterizeDialogShower, "bCenterizeDialogShower");
            this.bCenterizeDialogShower.ImageList = this.imageList;
            this.bCenterizeDialogShower.Name = "bCenterizeDialogShower";
            this.bCenterizeDialogShower.UseVisualStyleBackColor = true;
            this.bCenterizeDialogShower.Click += new System.EventHandler(this.bCenterizeDialogShower_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "add.png");
            this.imageList.Images.SetKeyName(1, "Edit.png");
            this.imageList.Images.SetKeyName(2, "delete.png");
            this.imageList.Images.SetKeyName(3, "ArrowsBig.png");
            // 
            // labelYNode
            // 
            resources.ApplyResources(this.labelYNode, "labelYNode");
            this.labelYNode.Name = "labelYNode";
            // 
            // labelXNode
            // 
            resources.ApplyResources(this.labelXNode, "labelXNode");
            this.labelXNode.Name = "labelXNode";
            // 
            // bRemoveDialog
            // 
            resources.ApplyResources(this.bRemoveDialog, "bRemoveDialog");
            this.bRemoveDialog.ImageList = this.imageList;
            this.bRemoveDialog.Name = "bRemoveDialog";
            this.bRemoveDialog.UseVisualStyleBackColor = true;
            this.bRemoveDialog.Click += new System.EventHandler(this.bRemoveDialog_Click);
            // 
            // bEditDialog
            // 
            resources.ApplyResources(this.bEditDialog, "bEditDialog");
            this.bEditDialog.ImageList = this.imageList;
            this.bEditDialog.Name = "bEditDialog";
            this.bEditDialog.UseVisualStyleBackColor = true;
            this.bEditDialog.Click += new System.EventHandler(this.bEditDialog_Click);
            // 
            // bAddDialog
            // 
            resources.ApplyResources(this.bAddDialog, "bAddDialog");
            this.bAddDialog.ImageList = this.imageList;
            this.bAddDialog.Name = "bAddDialog";
            this.bAddDialog.UseVisualStyleBackColor = true;
            this.bAddDialog.Click += new System.EventHandler(this.bAddDialog_Click);
            // 
            // gbNPCWorkSpace
            // 
            resources.ApplyResources(this.gbNPCWorkSpace, "gbNPCWorkSpace");
            this.gbNPCWorkSpace.Controls.Add(this.bWSDel);
            this.gbNPCWorkSpace.Controls.Add(this.bWSAdd);
            this.gbNPCWorkSpace.Controls.Add(this.lbWorkSpace);
            this.gbNPCWorkSpace.Name = "gbNPCWorkSpace";
            this.gbNPCWorkSpace.TabStop = false;
            // 
            // bWSDel
            // 
            resources.ApplyResources(this.bWSDel, "bWSDel");
            this.bWSDel.Name = "bWSDel";
            this.bWSDel.UseVisualStyleBackColor = true;
            this.bWSDel.Click += new System.EventHandler(this.bWSDel_Click);
            // 
            // bWSAdd
            // 
            resources.ApplyResources(this.bWSAdd, "bWSAdd");
            this.bWSAdd.Name = "bWSAdd";
            this.bWSAdd.UseVisualStyleBackColor = true;
            this.bWSAdd.Click += new System.EventHandler(this.bWSAdd_Click);
            // 
            // lbWorkSpace
            // 
            resources.ApplyResources(this.lbWorkSpace, "lbWorkSpace");
            this.lbWorkSpace.FormattingEnabled = true;
            this.lbWorkSpace.Name = "lbWorkSpace";
            this.lbWorkSpace.SelectedIndexChanged += new System.EventHandler(this.lbWorkSpace_SelectedIndexChanged);
            // 
            // gbEmulator
            // 
            resources.ApplyResources(this.gbEmulator, "gbEmulator");
            this.gbEmulator.Controls.Add(this.splitDialogsEmulator);
            this.gbEmulator.Name = "gbEmulator";
            this.gbEmulator.TabStop = false;
            // 
            // splitDialogsEmulator
            // 
            resources.ApplyResources(this.splitDialogsEmulator, "splitDialogsEmulator");
            this.splitDialogsEmulator.Name = "splitDialogsEmulator";
            // 
            // splitDialogsEmulator.Panel2
            // 
            resources.ApplyResources(this.splitDialogsEmulator.Panel2, "splitDialogsEmulator.Panel2");
            this.splitDialogsEmulator.TabStop = false;
            // 
            // splitQuestsContainer
            // 
            this.splitQuestsContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.splitQuestsContainer, "splitQuestsContainer");
            this.splitQuestsContainer.Name = "splitQuestsContainer";
            // 
            // splitQuestsContainer.Panel1
            // 
            this.splitQuestsContainer.Panel1.Controls.Add(this.treeQuest);
            this.splitQuestsContainer.Panel1.Controls.Add(this.labelQuestTree);
            this.splitQuestsContainer.Panel1.Controls.Add(this.treeQuestBuffer);
            this.splitQuestsContainer.Panel1.Controls.Add(this.labelBuffer);
            // 
            // splitQuestsContainer.Panel2
            // 
            resources.ApplyResources(this.splitQuestsContainer.Panel2, "splitQuestsContainer.Panel2");
            this.splitQuestsContainer.TabStop = false;
            // 
            // treeQuest
            // 
            resources.ApplyResources(this.treeQuest, "treeQuest");
            this.treeQuest.Name = "treeQuest";
            this.treeQuest.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeQuest_AfterSelect);
            this.treeQuest.Click += new System.EventHandler(this.treeQuest_Click);
            this.treeQuest.DoubleClick += new System.EventHandler(this.treeQuestClicked);
            this.treeQuest.Leave += new System.EventHandler(this.treeQuest_Leave);
            // 
            // labelQuestTree
            // 
            resources.ApplyResources(this.labelQuestTree, "labelQuestTree");
            this.labelQuestTree.Name = "labelQuestTree";
            // 
            // treeQuestBuffer
            // 
            resources.ApplyResources(this.treeQuestBuffer, "treeQuestBuffer");
            this.treeQuestBuffer.Name = "treeQuestBuffer";
            this.treeQuestBuffer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeQuestBuffer_AfterSelect);
            this.treeQuestBuffer.Click += new System.EventHandler(this.treeQuestBuffer_Click);
            this.treeQuestBuffer.DoubleClick += new System.EventHandler(this.treeQuestBuffer_DoubleClick);
            // 
            // labelBuffer
            // 
            resources.ApplyResources(this.labelBuffer, "labelBuffer");
            this.labelBuffer.Name = "labelBuffer";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.TabStop = false;
            // 
            // NPCBox
            // 
            this.NPCBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.NPCBox.DropDownWidth = 280;
            this.NPCBox.FormattingEnabled = true;
            resources.ApplyResources(this.NPCBox, "NPCBox");
            this.NPCBox.Name = "NPCBox";
            this.NPCBox.SelectedIndexChanged += new System.EventHandler(this.NPCBox_SelectedIndexChanged);
            this.NPCBox.TextChanged += new System.EventHandler(this.NPCBox_TextChanged);
            this.NPCBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NPCBox_KeyDown);
            this.NPCBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NPCBox_KeyPress);
            // 
            // labelChosenNPC
            // 
            resources.ApplyResources(this.labelChosenNPC, "labelChosenNPC");
            this.labelChosenNPC.Name = "labelChosenNPC";
            // 
            // CentralDock
            // 
            this.CentralDock.Controls.Add(this.tabDialogs);
            this.CentralDock.Controls.Add(this.tabQuests);
            this.CentralDock.Controls.Add(this.tabFraction);
            this.CentralDock.Controls.Add(this.tabInfoNPC);
            this.CentralDock.Controls.Add(this.tabReview);
            this.CentralDock.Controls.Add(this.tabManage);
            this.CentralDock.Controls.Add(this.tabTranslate);
            this.CentralDock.Controls.Add(this.tabSearch);
            this.CentralDock.Controls.Add(this.tabKnowledge);
            this.CentralDock.Controls.Add(this.tabAutoGen);
            resources.ApplyResources(this.CentralDock, "CentralDock");
            this.CentralDock.HotTrack = true;
            this.CentralDock.Name = "CentralDock";
            this.CentralDock.SelectedIndex = 0;
            this.CentralDock.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.CentralDock.TabStop = false;
            this.CentralDock.SelectedIndexChanged += new System.EventHandler(this.onSelectTab);
            // 
            // tabDialogs
            // 
            this.tabDialogs.Controls.Add(this.splitDialogs);
            resources.ApplyResources(this.tabDialogs, "tabDialogs");
            this.tabDialogs.Name = "tabDialogs";
            this.tabDialogs.UseVisualStyleBackColor = true;
            // 
            // tabQuests
            // 
            this.tabQuests.Controls.Add(this.splitQuestsContainer);
            this.tabQuests.Controls.Add(this.panelQuestTools);
            this.tabQuests.Controls.Add(this.panelSelectQuest);
            resources.ApplyResources(this.tabQuests, "tabQuests");
            this.tabQuests.Name = "tabQuests";
            this.tabQuests.UseVisualStyleBackColor = true;
            // 
            // panelQuestTools
            // 
            resources.ApplyResources(this.panelQuestTools, "panelQuestTools");
            this.panelQuestTools.Controls.Add(this.bClearBuffer);
            this.panelQuestTools.Controls.Add(this.bCutEvents);
            this.panelQuestTools.Controls.Add(this.bPasteEvents);
            this.panelQuestTools.Controls.Add(this.bCopyEvents);
            this.panelQuestTools.Controls.Add(this.bQuestDown);
            this.panelQuestTools.Controls.Add(this.bQuestUp);
            this.panelQuestTools.Controls.Add(this.bRemoveEvent);
            this.panelQuestTools.Controls.Add(this.bEditEvent);
            this.panelQuestTools.Controls.Add(this.bAddEvent);
            this.panelQuestTools.Name = "panelQuestTools";
            // 
            // bClearBuffer
            // 
            resources.ApplyResources(this.bClearBuffer, "bClearBuffer");
            this.bClearBuffer.Name = "bClearBuffer";
            this.bClearBuffer.UseVisualStyleBackColor = true;
            this.bClearBuffer.Click += new System.EventHandler(this.bClearBuffer_Click);
            // 
            // bCutEvents
            // 
            resources.ApplyResources(this.bCutEvents, "bCutEvents");
            this.bCutEvents.Name = "bCutEvents";
            this.bCutEvents.UseVisualStyleBackColor = true;
            this.bCutEvents.Click += new System.EventHandler(this.bCutEvents_Click);
            // 
            // bPasteEvents
            // 
            resources.ApplyResources(this.bPasteEvents, "bPasteEvents");
            this.bPasteEvents.Name = "bPasteEvents";
            this.bPasteEvents.UseVisualStyleBackColor = true;
            this.bPasteEvents.Click += new System.EventHandler(this.bPasteEvents_Click);
            // 
            // bCopyEvents
            // 
            resources.ApplyResources(this.bCopyEvents, "bCopyEvents");
            this.bCopyEvents.Name = "bCopyEvents";
            this.bCopyEvents.UseVisualStyleBackColor = true;
            this.bCopyEvents.Click += new System.EventHandler(this.bCopyEvents_Click);
            // 
            // bQuestDown
            // 
            resources.ApplyResources(this.bQuestDown, "bQuestDown");
            this.bQuestDown.Name = "bQuestDown";
            this.bQuestDown.UseVisualStyleBackColor = true;
            this.bQuestDown.Click += new System.EventHandler(this.bQuestDown_Click);
            // 
            // bQuestUp
            // 
            resources.ApplyResources(this.bQuestUp, "bQuestUp");
            this.bQuestUp.Name = "bQuestUp";
            this.bQuestUp.UseVisualStyleBackColor = true;
            this.bQuestUp.Click += new System.EventHandler(this.bQuestUp_Click);
            // 
            // bRemoveEvent
            // 
            resources.ApplyResources(this.bRemoveEvent, "bRemoveEvent");
            this.bRemoveEvent.ImageList = this.imageList;
            this.bRemoveEvent.Name = "bRemoveEvent";
            this.bRemoveEvent.UseVisualStyleBackColor = true;
            this.bRemoveEvent.Click += new System.EventHandler(this.bRemoveEvent_Click);
            // 
            // bEditEvent
            // 
            resources.ApplyResources(this.bEditEvent, "bEditEvent");
            this.bEditEvent.ImageList = this.imageList;
            this.bEditEvent.Name = "bEditEvent";
            this.bEditEvent.UseVisualStyleBackColor = true;
            this.bEditEvent.Click += new System.EventHandler(this.bEditEvent_Click);
            // 
            // bAddEvent
            // 
            resources.ApplyResources(this.bAddEvent, "bAddEvent");
            this.bAddEvent.ImageList = this.imageList;
            this.bAddEvent.Name = "bAddEvent";
            this.bAddEvent.UseMnemonic = false;
            this.bAddEvent.UseVisualStyleBackColor = false;
            this.bAddEvent.Click += new System.EventHandler(this.bAddEvent_Click);
            // 
            // panelSelectQuest
            // 
            this.panelSelectQuest.Controls.Add(this.bRemoveQuest);
            this.panelSelectQuest.Controls.Add(this.bAddQuest);
            this.panelSelectQuest.Controls.Add(this.QuestBox);
            this.panelSelectQuest.Controls.Add(this.labelChosenQuest);
            this.panelSelectQuest.Controls.Add(this.FakeQuestBox);
            resources.ApplyResources(this.panelSelectQuest, "panelSelectQuest");
            this.panelSelectQuest.Name = "panelSelectQuest";
            // 
            // bRemoveQuest
            // 
            resources.ApplyResources(this.bRemoveQuest, "bRemoveQuest");
            this.bRemoveQuest.Name = "bRemoveQuest";
            this.bRemoveQuest.UseVisualStyleBackColor = true;
            this.bRemoveQuest.Click += new System.EventHandler(this.bRemoveQuest_Click);
            // 
            // bAddQuest
            // 
            resources.ApplyResources(this.bAddQuest, "bAddQuest");
            this.bAddQuest.Name = "bAddQuest";
            this.bAddQuest.UseVisualStyleBackColor = true;
            this.bAddQuest.Click += new System.EventHandler(this.bAddQuest_Click);
            // 
            // QuestBox
            // 
            this.QuestBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            resources.ApplyResources(this.QuestBox, "QuestBox");
            this.QuestBox.FormattingEnabled = true;
            this.QuestBox.Name = "QuestBox";
            this.QuestBox.SelectedIndexChanged += new System.EventHandler(this.QuestBox_SelectedIndexChanged);
            this.QuestBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.QuestBox_KeyDown);
            this.QuestBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.QuestBox_KeyPress);
            // 
            // labelChosenQuest
            // 
            resources.ApplyResources(this.labelChosenQuest, "labelChosenQuest");
            this.labelChosenQuest.Name = "labelChosenQuest";
            // 
            // FakeQuestBox
            // 
            this.FakeQuestBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.FakeQuestBox.FormattingEnabled = true;
            resources.ApplyResources(this.FakeQuestBox, "FakeQuestBox");
            this.FakeQuestBox.Name = "FakeQuestBox";
            this.FakeQuestBox.SelectedIndexChanged += new System.EventHandler(this.FakeQuestBox_SelectedIndexChanged);
            // 
            // tabFraction
            // 
            this.tabFraction.BackColor = System.Drawing.SystemColors.Control;
            this.tabFraction.Controls.Add(this.panel1);
            this.tabFraction.Controls.Add(this.gbEmul2);
            this.tabFraction.Controls.Add(this.treeFractionDialogs);
            this.tabFraction.Controls.Add(this.fractionDialogShower);
            this.tabFraction.Controls.Add(this.label1);
            this.tabFraction.Controls.Add(this.fractionBox);
            resources.ApplyResources(this.tabFraction, "tabFraction");
            this.tabFraction.Name = "tabFraction";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bAddFracDialog);
            this.panel1.Controls.Add(this.bEditFracDialog);
            this.panel1.Controls.Add(this.bRemoveFracDialog);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bAddFracDialog
            // 
            resources.ApplyResources(this.bAddFracDialog, "bAddFracDialog");
            this.bAddFracDialog.ImageList = this.imageList;
            this.bAddFracDialog.Name = "bAddFracDialog";
            this.bAddFracDialog.UseVisualStyleBackColor = true;
            this.bAddFracDialog.Click += new System.EventHandler(this.bAddFracDialog_Click);
            // 
            // bEditFracDialog
            // 
            resources.ApplyResources(this.bEditFracDialog, "bEditFracDialog");
            this.bEditFracDialog.ImageList = this.imageList;
            this.bEditFracDialog.Name = "bEditFracDialog";
            this.bEditFracDialog.UseVisualStyleBackColor = true;
            this.bEditFracDialog.Click += new System.EventHandler(this.bEditDialog_Click);
            // 
            // bRemoveFracDialog
            // 
            resources.ApplyResources(this.bRemoveFracDialog, "bRemoveFracDialog");
            this.bRemoveFracDialog.ImageList = this.imageList;
            this.bRemoveFracDialog.Name = "bRemoveFracDialog";
            this.bRemoveFracDialog.UseVisualStyleBackColor = true;
            this.bRemoveFracDialog.Click += new System.EventHandler(this.bRemoveFracDialog_Click);
            // 
            // gbEmul2
            // 
            resources.ApplyResources(this.gbEmul2, "gbEmul2");
            this.gbEmul2.Controls.Add(this.splitContainer1);
            this.gbEmul2.Name = "gbEmul2";
            this.gbEmul2.TabStop = false;
            // 
            // treeFractionDialogs
            // 
            resources.ApplyResources(this.treeFractionDialogs, "treeFractionDialogs");
            this.treeFractionDialogs.Name = "treeFractionDialogs";
            this.treeFractionDialogs.TabStop = false;
            // 
            // fractionDialogShower
            // 
            this.fractionDialogShower.AllowDrop = true;
            resources.ApplyResources(this.fractionDialogShower, "fractionDialogShower");
            this.fractionDialogShower.BackColor = System.Drawing.Color.White;
            this.fractionDialogShower.GridFitText = false;
            this.fractionDialogShower.Name = "fractionDialogShower";
            this.fractionDialogShower.RegionManagement = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // fractionBox
            // 
            this.fractionBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.fractionBox.DropDownWidth = 280;
            this.fractionBox.FormattingEnabled = true;
            resources.ApplyResources(this.fractionBox, "fractionBox");
            this.fractionBox.Name = "fractionBox";
            this.fractionBox.SelectedIndexChanged += new System.EventHandler(this.fractionBox_SelectedIndexChanged);
            // 
            // tabInfoNPC
            // 
            this.tabInfoNPC.Controls.Add(this.npcLinkShower);
            this.tabInfoNPC.Controls.Add(this.panelNpcLinkControls);
            resources.ApplyResources(this.tabInfoNPC, "tabInfoNPC");
            this.tabInfoNPC.Name = "tabInfoNPC";
            this.tabInfoNPC.UseVisualStyleBackColor = true;
            // 
            // npcLinkShower
            // 
            this.npcLinkShower.AllowDrop = true;
            this.npcLinkShower.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.npcLinkShower, "npcLinkShower");
            this.npcLinkShower.GridFitText = false;
            this.npcLinkShower.Name = "npcLinkShower";
            this.npcLinkShower.RegionManagement = true;
            // 
            // panelNpcLinkControls
            // 
            this.panelNpcLinkControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNpcLinkControls.Controls.Add(this.labelAdviceNpcLink);
            this.panelNpcLinkControls.Controls.Add(this.bNpcLinkExecute);
            resources.ApplyResources(this.panelNpcLinkControls, "panelNpcLinkControls");
            this.panelNpcLinkControls.Name = "panelNpcLinkControls";
            // 
            // labelAdviceNpcLink
            // 
            resources.ApplyResources(this.labelAdviceNpcLink, "labelAdviceNpcLink");
            this.labelAdviceNpcLink.Name = "labelAdviceNpcLink";
            // 
            // bNpcLinkExecute
            // 
            resources.ApplyResources(this.bNpcLinkExecute, "bNpcLinkExecute");
            this.bNpcLinkExecute.Name = "bNpcLinkExecute";
            this.bNpcLinkExecute.UseVisualStyleBackColor = true;
            this.bNpcLinkExecute.Click += new System.EventHandler(this.bNpcLinkExecute_Click);
            // 
            // tabReview
            // 
            this.tabReview.Controls.Add(this.dgvReview);
            this.tabReview.Controls.Add(this.panelReviewButtons);
            resources.ApplyResources(this.tabReview, "tabReview");
            this.tabReview.Name = "tabReview";
            this.tabReview.UseVisualStyleBackColor = true;
            // 
            // dgvReview
            // 
            this.dgvReview.AllowUserToAddRows = false;
            this.dgvReview.AllowUserToDeleteRows = false;
            this.dgvReview.AllowUserToOrderColumns = true;
            this.dgvReview.AllowUserToResizeRows = false;
            this.dgvReview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNPCName,
            this.colDialogsNum,
            this.colQuestsNum,
            this.colLocation,
            this.colCoordinates,
            this.colRussianName});
            resources.ApplyResources(this.dgvReview, "dgvReview");
            this.dgvReview.Name = "dgvReview";
            // 
            // colNPCName
            // 
            resources.ApplyResources(this.colNPCName, "colNPCName");
            this.colNPCName.Name = "colNPCName";
            // 
            // colDialogsNum
            // 
            resources.ApplyResources(this.colDialogsNum, "colDialogsNum");
            this.colDialogsNum.Name = "colDialogsNum";
            // 
            // colQuestsNum
            // 
            resources.ApplyResources(this.colQuestsNum, "colQuestsNum");
            this.colQuestsNum.Name = "colQuestsNum";
            // 
            // colLocation
            // 
            resources.ApplyResources(this.colLocation, "colLocation");
            this.colLocation.Name = "colLocation";
            // 
            // colCoordinates
            // 
            resources.ApplyResources(this.colCoordinates, "colCoordinates");
            this.colCoordinates.Name = "colCoordinates";
            this.colCoordinates.ReadOnly = true;
            // 
            // colRussianName
            // 
            resources.ApplyResources(this.colRussianName, "colRussianName");
            this.colRussianName.Name = "colRussianName";
            // 
            // panelReviewButtons
            // 
            this.panelReviewButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReviewButtons.Controls.Add(this.groupBox8);
            this.panelReviewButtons.Controls.Add(this.groupBox1);
            this.panelReviewButtons.Controls.Add(this.gbNPCcheck);
            this.panelReviewButtons.Controls.Add(this.gbQuestCheck);
            resources.ApplyResources(this.panelReviewButtons, "panelReviewButtons");
            this.panelReviewButtons.Name = "panelReviewButtons";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tbKnowledgeFind);
            this.groupBox8.Controls.Add(this.label21);
            this.groupBox8.Controls.Add(this.bFindKnowledgeQuest);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // tbKnowledgeFind
            // 
            resources.ApplyResources(this.tbKnowledgeFind, "tbKnowledgeFind");
            this.tbKnowledgeFind.Name = "tbKnowledgeFind";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // bFindKnowledgeQuest
            // 
            resources.ApplyResources(this.bFindKnowledgeQuest, "bFindKnowledgeQuest");
            this.bFindKnowledgeQuest.Name = "bFindKnowledgeQuest";
            this.bFindKnowledgeQuest.UseVisualStyleBackColor = true;
            this.bFindKnowledgeQuest.Click += new System.EventHandler(this.bFindKnowledgeQuest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbOnlyTradePoints);
            this.groupBox1.Controls.Add(this.cbRepLocations);
            this.groupBox1.Controls.Add(this.cbOnRepLocation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbRep2List);
            this.groupBox1.Controls.Add(this.bFindNPCReputation);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbOnlyTradePoints
            // 
            resources.ApplyResources(this.cbOnlyTradePoints, "cbOnlyTradePoints");
            this.cbOnlyTradePoints.Name = "cbOnlyTradePoints";
            this.cbOnlyTradePoints.UseVisualStyleBackColor = true;
            // 
            // cbRepLocations
            // 
            this.cbRepLocations.FormattingEnabled = true;
            resources.ApplyResources(this.cbRepLocations, "cbRepLocations");
            this.cbRepLocations.Name = "cbRepLocations";
            // 
            // cbOnRepLocation
            // 
            resources.ApplyResources(this.cbOnRepLocation, "cbOnRepLocation");
            this.cbOnRepLocation.Name = "cbOnRepLocation";
            this.cbOnRepLocation.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbRep2List
            // 
            this.cbRep2List.FormattingEnabled = true;
            resources.ApplyResources(this.cbRep2List, "cbRep2List");
            this.cbRep2List.Name = "cbRep2List";
            // 
            // bFindNPCReputation
            // 
            resources.ApplyResources(this.bFindNPCReputation, "bFindNPCReputation");
            this.bFindNPCReputation.Name = "bFindNPCReputation";
            this.bFindNPCReputation.UseVisualStyleBackColor = true;
            this.bFindNPCReputation.Click += new System.EventHandler(this.bFindNPCReputation_Click);
            // 
            // gbNPCcheck
            // 
            this.gbNPCcheck.Controls.Add(this.cbOnlyOnLocation);
            this.gbNPCcheck.Controls.Add(this.cbLocation);
            this.gbNPCcheck.Controls.Add(this.numQuests);
            this.gbNPCcheck.Controls.Add(this.labelLessThan2);
            this.gbNPCcheck.Controls.Add(this.cbNumQuests);
            this.gbNPCcheck.Controls.Add(this.numDialogs);
            this.gbNPCcheck.Controls.Add(this.labelLessThan1);
            this.gbNPCcheck.Controls.Add(this.cbNumDialogs);
            this.gbNPCcheck.Controls.Add(this.bFindNPC);
            resources.ApplyResources(this.gbNPCcheck, "gbNPCcheck");
            this.gbNPCcheck.Name = "gbNPCcheck";
            this.gbNPCcheck.TabStop = false;
            // 
            // cbOnlyOnLocation
            // 
            resources.ApplyResources(this.cbOnlyOnLocation, "cbOnlyOnLocation");
            this.cbOnlyOnLocation.Name = "cbOnlyOnLocation";
            this.cbOnlyOnLocation.UseVisualStyleBackColor = true;
            // 
            // cbLocation
            // 
            this.cbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cbLocation, "cbLocation");
            this.cbLocation.Name = "cbLocation";
            // 
            // numQuests
            // 
            resources.ApplyResources(this.numQuests, "numQuests");
            this.numQuests.Name = "numQuests";
            this.numQuests.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelLessThan2
            // 
            resources.ApplyResources(this.labelLessThan2, "labelLessThan2");
            this.labelLessThan2.Name = "labelLessThan2";
            // 
            // cbNumQuests
            // 
            resources.ApplyResources(this.cbNumQuests, "cbNumQuests");
            this.cbNumQuests.Name = "cbNumQuests";
            this.cbNumQuests.UseVisualStyleBackColor = true;
            // 
            // numDialogs
            // 
            resources.ApplyResources(this.numDialogs, "numDialogs");
            this.numDialogs.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numDialogs.Name = "numDialogs";
            this.numDialogs.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelLessThan1
            // 
            resources.ApplyResources(this.labelLessThan1, "labelLessThan1");
            this.labelLessThan1.Name = "labelLessThan1";
            // 
            // cbNumDialogs
            // 
            resources.ApplyResources(this.cbNumDialogs, "cbNumDialogs");
            this.cbNumDialogs.Name = "cbNumDialogs";
            this.cbNumDialogs.UseVisualStyleBackColor = true;
            // 
            // bFindNPC
            // 
            resources.ApplyResources(this.bFindNPC, "bFindNPC");
            this.bFindNPC.Name = "bFindNPC";
            this.bFindNPC.UseVisualStyleBackColor = true;
            this.bFindNPC.Click += new System.EventHandler(this.bFindNPC_Click);
            // 
            // gbQuestCheck
            // 
            this.gbQuestCheck.Controls.Add(this.cbQuestLocations);
            this.gbQuestCheck.Controls.Add(this.cbOnQuestLocation);
            this.gbQuestCheck.Controls.Add(this.labelItemTarget);
            this.gbQuestCheck.Controls.Add(this.cbItemTarget);
            this.gbQuestCheck.Controls.Add(this.labelItemReward);
            this.gbQuestCheck.Controls.Add(this.cbItemReward);
            this.gbQuestCheck.Controls.Add(this.bFindQuest);
            resources.ApplyResources(this.gbQuestCheck, "gbQuestCheck");
            this.gbQuestCheck.Name = "gbQuestCheck";
            this.gbQuestCheck.TabStop = false;
            // 
            // cbQuestLocations
            // 
            this.cbQuestLocations.FormattingEnabled = true;
            resources.ApplyResources(this.cbQuestLocations, "cbQuestLocations");
            this.cbQuestLocations.Name = "cbQuestLocations";
            // 
            // cbOnQuestLocation
            // 
            resources.ApplyResources(this.cbOnQuestLocation, "cbOnQuestLocation");
            this.cbOnQuestLocation.Name = "cbOnQuestLocation";
            this.cbOnQuestLocation.UseVisualStyleBackColor = true;
            // 
            // labelItemTarget
            // 
            resources.ApplyResources(this.labelItemTarget, "labelItemTarget");
            this.labelItemTarget.Name = "labelItemTarget";
            // 
            // cbItemTarget
            // 
            this.cbItemTarget.FormattingEnabled = true;
            resources.ApplyResources(this.cbItemTarget, "cbItemTarget");
            this.cbItemTarget.Name = "cbItemTarget";
            // 
            // labelItemReward
            // 
            resources.ApplyResources(this.labelItemReward, "labelItemReward");
            this.labelItemReward.Name = "labelItemReward";
            // 
            // cbItemReward
            // 
            this.cbItemReward.FormattingEnabled = true;
            resources.ApplyResources(this.cbItemReward, "cbItemReward");
            this.cbItemReward.Name = "cbItemReward";
            // 
            // bFindQuest
            // 
            resources.ApplyResources(this.bFindQuest, "bFindQuest");
            this.bFindQuest.Name = "bFindQuest";
            this.bFindQuest.UseVisualStyleBackColor = true;
            this.bFindQuest.Click += new System.EventHandler(this.bFindQuest_Click);
            // 
            // tabManage
            // 
            this.tabManage.Controls.Add(this.dgvManage);
            this.tabManage.Controls.Add(this.panel2);
            resources.ApplyResources(this.tabManage, "tabManage");
            this.tabManage.Name = "tabManage";
            this.tabManage.UseVisualStyleBackColor = true;
            // 
            // dgvManage
            // 
            this.dgvManage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.subevents,
            this.title,
            this.Description,
            this.npcName,
            this.subNPC,
            this.dialogID,
            this.rewardBattle,
            this.rewardCredits,
            this.rewardItems,
            this.repeat,
            this.repeatPeriod,
            this.Level,
            this.author,
            this.Legend,
            this.worked});
            resources.ApplyResources(this.dgvManage, "dgvManage");
            this.dgvManage.Name = "dgvManage";
            // 
            // id
            // 
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // subevents
            // 
            resources.ApplyResources(this.subevents, "subevents");
            this.subevents.Name = "subevents";
            this.subevents.ReadOnly = true;
            // 
            // title
            // 
            resources.ApplyResources(this.title, "title");
            this.title.Name = "title";
            this.title.ReadOnly = true;
            // 
            // Description
            // 
            resources.ApplyResources(this.Description, "Description");
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // npcName
            // 
            resources.ApplyResources(this.npcName, "npcName");
            this.npcName.Name = "npcName";
            this.npcName.ReadOnly = true;
            // 
            // subNPC
            // 
            resources.ApplyResources(this.subNPC, "subNPC");
            this.subNPC.Name = "subNPC";
            this.subNPC.ReadOnly = true;
            // 
            // dialogID
            // 
            resources.ApplyResources(this.dialogID, "dialogID");
            this.dialogID.Name = "dialogID";
            this.dialogID.ReadOnly = true;
            // 
            // rewardBattle
            // 
            resources.ApplyResources(this.rewardBattle, "rewardBattle");
            this.rewardBattle.Name = "rewardBattle";
            this.rewardBattle.ReadOnly = true;
            // 
            // rewardCredits
            // 
            resources.ApplyResources(this.rewardCredits, "rewardCredits");
            this.rewardCredits.Name = "rewardCredits";
            this.rewardCredits.ReadOnly = true;
            // 
            // rewardItems
            // 
            resources.ApplyResources(this.rewardItems, "rewardItems");
            this.rewardItems.Name = "rewardItems";
            this.rewardItems.ReadOnly = true;
            // 
            // repeat
            // 
            resources.ApplyResources(this.repeat, "repeat");
            this.repeat.Name = "repeat";
            this.repeat.ReadOnly = true;
            // 
            // repeatPeriod
            // 
            resources.ApplyResources(this.repeatPeriod, "repeatPeriod");
            this.repeatPeriod.Name = "repeatPeriod";
            this.repeatPeriod.ReadOnly = true;
            // 
            // Level
            // 
            resources.ApplyResources(this.Level, "Level");
            this.Level.Name = "Level";
            // 
            // author
            // 
            resources.ApplyResources(this.author, "author");
            this.author.Name = "author";
            // 
            // Legend
            // 
            resources.ApplyResources(this.Legend, "Legend");
            this.Legend.Name = "Legend";
            // 
            // worked
            // 
            resources.ApplyResources(this.worked, "worked");
            this.worked.Name = "worked";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bSaveManage);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // bSaveManage
            // 
            resources.ApplyResources(this.bSaveManage, "bSaveManage");
            this.bSaveManage.Name = "bSaveManage";
            this.bSaveManage.UseVisualStyleBackColor = true;
            this.bSaveManage.Click += new System.EventHandler(this.bSaveManage_Click);
            // 
            // tabTranslate
            // 
            this.tabTranslate.Controls.Add(this.dgvLocaleDiff);
            this.tabTranslate.Controls.Add(this.panelDiffLocale);
            resources.ApplyResources(this.tabTranslate, "tabTranslate");
            this.tabTranslate.Name = "tabTranslate";
            this.tabTranslate.UseVisualStyleBackColor = true;
            // 
            // dgvLocaleDiff
            // 
            this.dgvLocaleDiff.AllowUserToAddRows = false;
            this.dgvLocaleDiff.AllowUserToDeleteRows = false;
            this.dgvLocaleDiff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocaleDiff.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.type,
            this.npc_name,
            this.identif,
            this.cur_ver,
            this.new_ver,
            this.ColumnLocation,
            this.RusText1,
            this.EngText1});
            resources.ApplyResources(this.dgvLocaleDiff, "dgvLocaleDiff");
            this.dgvLocaleDiff.Name = "dgvLocaleDiff";
            this.dgvLocaleDiff.ReadOnly = true;
            this.dgvLocaleDiff.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLocaleDiff_CellDoubleClick);
            // 
            // type
            // 
            resources.ApplyResources(this.type, "type");
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // npc_name
            // 
            resources.ApplyResources(this.npc_name, "npc_name");
            this.npc_name.Name = "npc_name";
            this.npc_name.ReadOnly = true;
            // 
            // identif
            // 
            resources.ApplyResources(this.identif, "identif");
            this.identif.Name = "identif";
            this.identif.ReadOnly = true;
            // 
            // cur_ver
            // 
            resources.ApplyResources(this.cur_ver, "cur_ver");
            this.cur_ver.Name = "cur_ver";
            this.cur_ver.ReadOnly = true;
            // 
            // new_ver
            // 
            resources.ApplyResources(this.new_ver, "new_ver");
            this.new_ver.Name = "new_ver";
            this.new_ver.ReadOnly = true;
            // 
            // ColumnLocation
            // 
            resources.ApplyResources(this.ColumnLocation, "ColumnLocation");
            this.ColumnLocation.Name = "ColumnLocation";
            this.ColumnLocation.ReadOnly = true;
            // 
            // RusText1
            // 
            resources.ApplyResources(this.RusText1, "RusText1");
            this.RusText1.Name = "RusText1";
            this.RusText1.ReadOnly = true;
            // 
            // EngText1
            // 
            resources.ApplyResources(this.EngText1, "EngText1");
            this.EngText1.Name = "EngText1";
            this.EngText1.ReadOnly = true;
            // 
            // panelDiffLocale
            // 
            this.panelDiffLocale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDiffLocale.Controls.Add(this.labelLocalizeOuput);
            this.panelDiffLocale.Controls.Add(this.cbActualFinder);
            this.panelDiffLocale.Controls.Add(this.cbOutdatedFinder);
            this.panelDiffLocale.Controls.Add(this.bFindQuestDifference);
            this.panelDiffLocale.Controls.Add(this.bFindDialogDifference);
            resources.ApplyResources(this.panelDiffLocale, "panelDiffLocale");
            this.panelDiffLocale.Name = "panelDiffLocale";
            // 
            // labelLocalizeOuput
            // 
            resources.ApplyResources(this.labelLocalizeOuput, "labelLocalizeOuput");
            this.labelLocalizeOuput.Name = "labelLocalizeOuput";
            // 
            // cbActualFinder
            // 
            resources.ApplyResources(this.cbActualFinder, "cbActualFinder");
            this.cbActualFinder.Name = "cbActualFinder";
            this.cbActualFinder.UseVisualStyleBackColor = true;
            // 
            // cbOutdatedFinder
            // 
            resources.ApplyResources(this.cbOutdatedFinder, "cbOutdatedFinder");
            this.cbOutdatedFinder.Checked = true;
            this.cbOutdatedFinder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOutdatedFinder.Name = "cbOutdatedFinder";
            this.cbOutdatedFinder.UseVisualStyleBackColor = true;
            // 
            // bFindQuestDifference
            // 
            resources.ApplyResources(this.bFindQuestDifference, "bFindQuestDifference");
            this.bFindQuestDifference.Name = "bFindQuestDifference";
            this.bFindQuestDifference.UseVisualStyleBackColor = true;
            this.bFindQuestDifference.Click += new System.EventHandler(this.bFindQuestDifference_Click);
            // 
            // bFindDialogDifference
            // 
            resources.ApplyResources(this.bFindDialogDifference, "bFindDialogDifference");
            this.bFindDialogDifference.Name = "bFindDialogDifference";
            this.bFindDialogDifference.UseVisualStyleBackColor = true;
            this.bFindDialogDifference.Click += new System.EventHandler(this.bFindDialogDifference_Click);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.dgvSearch);
            this.tabSearch.Controls.Add(this.panelSearchTools);
            resources.ApplyResources(this.tabSearch, "tabSearch");
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // dgvSearch
            // 
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colType,
            this.colNPC,
            this.colID,
            this.colText,
            this.colEngText});
            resources.ApplyResources(this.dgvSearch, "dgvSearch");
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearch_CellDoubleClick);
            // 
            // colType
            // 
            resources.ApplyResources(this.colType, "colType");
            this.colType.Name = "colType";
            // 
            // colNPC
            // 
            resources.ApplyResources(this.colNPC, "colNPC");
            this.colNPC.Name = "colNPC";
            // 
            // colID
            // 
            resources.ApplyResources(this.colID, "colID");
            this.colID.Name = "colID";
            // 
            // colText
            // 
            resources.ApplyResources(this.colText, "colText");
            this.colText.Name = "colText";
            // 
            // colEngText
            // 
            resources.ApplyResources(this.colEngText, "colEngText");
            this.colEngText.Name = "colEngText";
            // 
            // panelSearchTools
            // 
            this.panelSearchTools.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearchTools.Controls.Add(this.cbIgnoreCase);
            this.panelSearchTools.Controls.Add(this.labelSearchResult);
            this.panelSearchTools.Controls.Add(this.labelPhraseToSearch);
            this.panelSearchTools.Controls.Add(this.tbPhraseToSearch);
            this.panelSearchTools.Controls.Add(this.bStartSearch);
            resources.ApplyResources(this.panelSearchTools, "panelSearchTools");
            this.panelSearchTools.Name = "panelSearchTools";
            // 
            // cbIgnoreCase
            // 
            resources.ApplyResources(this.cbIgnoreCase, "cbIgnoreCase");
            this.cbIgnoreCase.Checked = true;
            this.cbIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIgnoreCase.Name = "cbIgnoreCase";
            this.cbIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // labelSearchResult
            // 
            resources.ApplyResources(this.labelSearchResult, "labelSearchResult");
            this.labelSearchResult.Name = "labelSearchResult";
            // 
            // labelPhraseToSearch
            // 
            resources.ApplyResources(this.labelPhraseToSearch, "labelPhraseToSearch");
            this.labelPhraseToSearch.Name = "labelPhraseToSearch";
            // 
            // tbPhraseToSearch
            // 
            resources.ApplyResources(this.tbPhraseToSearch, "tbPhraseToSearch");
            this.tbPhraseToSearch.Name = "tbPhraseToSearch";
            // 
            // bStartSearch
            // 
            resources.ApplyResources(this.bStartSearch, "bStartSearch");
            this.bStartSearch.Name = "bStartSearch";
            this.bStartSearch.UseVisualStyleBackColor = true;
            this.bStartSearch.Click += new System.EventHandler(this.bStartSearch_Click);
            // 
            // tabKnowledge
            // 
            this.tabKnowledge.Controls.Add(this.groupBox7);
            this.tabKnowledge.Controls.Add(this.treeKnowladge);
            this.tabKnowledge.Controls.Add(this.cbKnowlegeTypeValue);
            this.tabKnowledge.Controls.Add(this.label4);
            this.tabKnowledge.Controls.Add(this.label3);
            this.tabKnowledge.Controls.Add(this.cbKnowledgeCategory);
            resources.ApplyResources(this.tabKnowledge, "tabKnowledge");
            this.tabKnowledge.Name = "tabKnowledge";
            this.tabKnowledge.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnAddKnowlege);
            this.groupBox7.Controls.Add(this.btnDelKnowlege);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // btnAddKnowlege
            // 
            resources.ApplyResources(this.btnAddKnowlege, "btnAddKnowlege");
            this.btnAddKnowlege.ImageList = this.imageList;
            this.btnAddKnowlege.Name = "btnAddKnowlege";
            this.btnAddKnowlege.UseVisualStyleBackColor = true;
            // 
            // btnDelKnowlege
            // 
            resources.ApplyResources(this.btnDelKnowlege, "btnDelKnowlege");
            this.btnDelKnowlege.ImageList = this.imageList;
            this.btnDelKnowlege.Name = "btnDelKnowlege";
            this.btnDelKnowlege.UseVisualStyleBackColor = true;
            // 
            // treeKnowladge
            // 
            resources.ApplyResources(this.treeKnowladge, "treeKnowladge");
            this.treeKnowladge.Name = "treeKnowladge";
            // 
            // cbKnowlegeTypeValue
            // 
            this.cbKnowlegeTypeValue.FormattingEnabled = true;
            resources.ApplyResources(this.cbKnowlegeTypeValue, "cbKnowlegeTypeValue");
            this.cbKnowlegeTypeValue.Name = "cbKnowlegeTypeValue";
            this.cbKnowlegeTypeValue.SelectedIndexChanged += new System.EventHandler(this.cbKnowlegeTypeValue_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbKnowledgeCategory
            // 
            this.cbKnowledgeCategory.FormattingEnabled = true;
            resources.ApplyResources(this.cbKnowledgeCategory, "cbKnowledgeCategory");
            this.cbKnowledgeCategory.Name = "cbKnowledgeCategory";
            this.cbKnowledgeCategory.SelectedIndexChanged += new System.EventHandler(this.cbKnowledgeCategory_SelectedIndexChanged);
            // 
            // tabAutoGen
            // 
            this.tabAutoGen.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.tabAutoGen, "tabAutoGen");
            this.tabAutoGen.Name = "tabAutoGen";
            this.tabAutoGen.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPageAGDialog);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.btnDelQType);
            this.tabPage1.Controls.Add(this.btnAddQType);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.listBoxQT);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxReward);
            this.groupBox3.Controls.Add(this.btnAddReward);
            this.groupBox3.Controls.Add(this.btnChangeReward);
            this.groupBox3.Controls.Add(this.btnDelReward);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // listBoxReward
            // 
            this.listBoxReward.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxReward, "listBoxReward");
            this.listBoxReward.Name = "listBoxReward";
            // 
            // btnAddReward
            // 
            resources.ApplyResources(this.btnAddReward, "btnAddReward");
            this.btnAddReward.Name = "btnAddReward";
            this.toolTipDialogs.SetToolTip(this.btnAddReward, resources.GetString("btnAddReward.ToolTip"));
            this.btnAddReward.UseVisualStyleBackColor = true;
            this.btnAddReward.Click += new System.EventHandler(this.btnAddReward_Click);
            // 
            // btnChangeReward
            // 
            resources.ApplyResources(this.btnChangeReward, "btnChangeReward");
            this.btnChangeReward.Name = "btnChangeReward";
            this.toolTipDialogs.SetToolTip(this.btnChangeReward, resources.GetString("btnChangeReward.ToolTip"));
            this.btnChangeReward.UseVisualStyleBackColor = true;
            this.btnChangeReward.Click += new System.EventHandler(this.btnChangeReward_Click);
            // 
            // btnDelReward
            // 
            resources.ApplyResources(this.btnDelReward, "btnDelReward");
            this.btnDelReward.Name = "btnDelReward";
            this.toolTipDialogs.SetToolTip(this.btnDelReward, resources.GetString("btnDelReward.ToolTip"));
            this.btnDelReward.UseVisualStyleBackColor = true;
            this.btnDelReward.Click += new System.EventHandler(this.btnDelReward_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxTarget);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.btnAddTarget);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnDelTarget);
            this.groupBox2.Controls.Add(this.nupToTargetCount);
            this.groupBox2.Controls.Add(this.btnChange);
            this.groupBox2.Controls.Add(this.nupFromTargetCount);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // listBoxTarget
            // 
            this.listBoxTarget.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxTarget, "listBoxTarget");
            this.listBoxTarget.Name = "listBoxTarget";
            this.listBoxTarget.SelectedIndexChanged += new System.EventHandler(this.listBoxTarget_SelectedIndexChanged);
            this.listBoxTarget.DoubleClick += new System.EventHandler(this.listBoxTarget_DoubleClick);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnAddTarget
            // 
            resources.ApplyResources(this.btnAddTarget, "btnAddTarget");
            this.btnAddTarget.Name = "btnAddTarget";
            this.toolTipDialogs.SetToolTip(this.btnAddTarget, resources.GetString("btnAddTarget.ToolTip"));
            this.btnAddTarget.UseVisualStyleBackColor = true;
            this.btnAddTarget.Click += new System.EventHandler(this.btnAddTarget_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btnDelTarget
            // 
            resources.ApplyResources(this.btnDelTarget, "btnDelTarget");
            this.btnDelTarget.Name = "btnDelTarget";
            this.toolTipDialogs.SetToolTip(this.btnDelTarget, resources.GetString("btnDelTarget.ToolTip"));
            this.btnDelTarget.UseVisualStyleBackColor = true;
            this.btnDelTarget.Click += new System.EventHandler(this.btnDelTarget_Click);
            // 
            // nupToTargetCount
            // 
            resources.ApplyResources(this.nupToTargetCount, "nupToTargetCount");
            this.nupToTargetCount.Name = "nupToTargetCount";
            this.nupToTargetCount.ValueChanged += new System.EventHandler(this.nupToTargetCount_ValueChanged);
            // 
            // btnChange
            // 
            resources.ApplyResources(this.btnChange, "btnChange");
            this.btnChange.Name = "btnChange";
            this.toolTipDialogs.SetToolTip(this.btnChange, resources.GetString("btnChange.ToolTip"));
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // nupFromTargetCount
            // 
            resources.ApplyResources(this.nupFromTargetCount, "nupFromTargetCount");
            this.nupFromTargetCount.Name = "nupFromTargetCount";
            this.nupFromTargetCount.ValueChanged += new System.EventHandler(this.nupFromTargetCount_ValueChanged);
            // 
            // btnDelQType
            // 
            resources.ApplyResources(this.btnDelQType, "btnDelQType");
            this.btnDelQType.Name = "btnDelQType";
            this.toolTipDialogs.SetToolTip(this.btnDelQType, resources.GetString("btnDelQType.ToolTip"));
            this.btnDelQType.UseVisualStyleBackColor = true;
            this.btnDelQType.Click += new System.EventHandler(this.btnDelQType_Click);
            // 
            // btnAddQType
            // 
            resources.ApplyResources(this.btnAddQType, "btnAddQType");
            this.btnAddQType.Name = "btnAddQType";
            this.toolTipDialogs.SetToolTip(this.btnAddQType, resources.GetString("btnAddQType.ToolTip"));
            this.btnAddQType.UseVisualStyleBackColor = true;
            this.btnAddQType.Click += new System.EventHandler(this.btnAddQType_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // listBoxQT
            // 
            this.listBoxQT.FormattingEnabled = true;
            resources.ApplyResources(this.listBoxQT, "listBoxQT");
            this.listBoxQT.Name = "listBoxQT";
            this.listBoxQT.SelectedIndexChanged += new System.EventHandler(this.listBoxQT_SelectedIndexChanged);
            // 
            // tabPageAGDialog
            // 
            resources.ApplyResources(this.tabPageAGDialog, "tabPageAGDialog");
            this.tabPageAGDialog.Controls.Add(this.groupBox9);
            this.tabPageAGDialog.Controls.Add(this.label11);
            this.tabPageAGDialog.Controls.Add(this.label17);
            this.tabPageAGDialog.Controls.Add(this.groupBox6);
            this.tabPageAGDialog.Controls.Add(this.groupBox5);
            this.tabPageAGDialog.Controls.Add(this.groupBox4);
            this.tabPageAGDialog.Controls.Add(this.cbNPCNature);
            this.tabPageAGDialog.Controls.Add(this.label6);
            this.tabPageAGDialog.Name = "tabPageAGDialog";
            this.tabPageAGDialog.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label22);
            this.groupBox9.Controls.Add(this.button1);
            this.groupBox9.Controls.Add(this.button2);
            this.groupBox9.Controls.Add(this.button3);
            this.groupBox9.Controls.Add(this.lbAGHello);
            this.groupBox9.Controls.Add(this.btnAGHelloAdd);
            this.groupBox9.Controls.Add(this.btnAGHelloChange);
            this.groupBox9.Controls.Add(this.btnAGHelloDel);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTipDialogs.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTipDialogs.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTipDialogs.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            // 
            // lbAGHello
            // 
            this.lbAGHello.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGHello, "lbAGHello");
            this.lbAGHello.Name = "lbAGHello";
            // 
            // btnAGHelloAdd
            // 
            resources.ApplyResources(this.btnAGHelloAdd, "btnAGHelloAdd");
            this.btnAGHelloAdd.Name = "btnAGHelloAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGHelloAdd, resources.GetString("btnAGHelloAdd.ToolTip"));
            this.btnAGHelloAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGHelloChange
            // 
            resources.ApplyResources(this.btnAGHelloChange, "btnAGHelloChange");
            this.btnAGHelloChange.Name = "btnAGHelloChange";
            this.toolTipDialogs.SetToolTip(this.btnAGHelloChange, resources.GetString("btnAGHelloChange.ToolTip"));
            this.btnAGHelloChange.UseVisualStyleBackColor = true;
            // 
            // btnAGHelloDel
            // 
            resources.ApplyResources(this.btnAGHelloDel, "btnAGHelloDel");
            this.btnAGHelloDel.Name = "btnAGHelloDel";
            this.toolTipDialogs.SetToolTip(this.btnAGHelloDel, resources.GetString("btnAGHelloDel.ToolTip"));
            this.btnAGHelloDel.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.btnAGClosed2Add);
            this.groupBox6.Controls.Add(this.btnAGClosed2Change);
            this.groupBox6.Controls.Add(this.btnAGClosed2Del);
            this.groupBox6.Controls.Add(this.lbAGClosed2);
            this.groupBox6.Controls.Add(this.lbAGClosed);
            this.groupBox6.Controls.Add(this.btnAGClosedAdd);
            this.groupBox6.Controls.Add(this.btnAGClosedChange);
            this.groupBox6.Controls.Add(this.btnAGClosedDel);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // btnAGClosed2Add
            // 
            resources.ApplyResources(this.btnAGClosed2Add, "btnAGClosed2Add");
            this.btnAGClosed2Add.Name = "btnAGClosed2Add";
            this.toolTipDialogs.SetToolTip(this.btnAGClosed2Add, resources.GetString("btnAGClosed2Add.ToolTip"));
            this.btnAGClosed2Add.UseVisualStyleBackColor = true;
            // 
            // btnAGClosed2Change
            // 
            resources.ApplyResources(this.btnAGClosed2Change, "btnAGClosed2Change");
            this.btnAGClosed2Change.Name = "btnAGClosed2Change";
            this.toolTipDialogs.SetToolTip(this.btnAGClosed2Change, resources.GetString("btnAGClosed2Change.ToolTip"));
            this.btnAGClosed2Change.UseVisualStyleBackColor = true;
            // 
            // btnAGClosed2Del
            // 
            resources.ApplyResources(this.btnAGClosed2Del, "btnAGClosed2Del");
            this.btnAGClosed2Del.Name = "btnAGClosed2Del";
            this.toolTipDialogs.SetToolTip(this.btnAGClosed2Del, resources.GetString("btnAGClosed2Del.ToolTip"));
            this.btnAGClosed2Del.UseVisualStyleBackColor = true;
            // 
            // lbAGClosed2
            // 
            this.lbAGClosed2.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGClosed2, "lbAGClosed2");
            this.lbAGClosed2.Name = "lbAGClosed2";
            // 
            // lbAGClosed
            // 
            this.lbAGClosed.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGClosed, "lbAGClosed");
            this.lbAGClosed.Name = "lbAGClosed";
            // 
            // btnAGClosedAdd
            // 
            resources.ApplyResources(this.btnAGClosedAdd, "btnAGClosedAdd");
            this.btnAGClosedAdd.Name = "btnAGClosedAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGClosedAdd, resources.GetString("btnAGClosedAdd.ToolTip"));
            this.btnAGClosedAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGClosedChange
            // 
            resources.ApplyResources(this.btnAGClosedChange, "btnAGClosedChange");
            this.btnAGClosedChange.Name = "btnAGClosedChange";
            this.toolTipDialogs.SetToolTip(this.btnAGClosedChange, resources.GetString("btnAGClosedChange.ToolTip"));
            this.btnAGClosedChange.UseVisualStyleBackColor = true;
            // 
            // btnAGClosedDel
            // 
            resources.ApplyResources(this.btnAGClosedDel, "btnAGClosedDel");
            this.btnAGClosedDel.Name = "btnAGClosedDel";
            this.toolTipDialogs.SetToolTip(this.btnAGClosedDel, resources.GetString("btnAGClosedDel.ToolTip"));
            this.btnAGClosedDel.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.btnAGOnTest2Add);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.btnAGOnTest2Change);
            this.groupBox5.Controls.Add(this.btnAGOnTest2Del);
            this.groupBox5.Controls.Add(this.lbAGOnTest2);
            this.groupBox5.Controls.Add(this.lbAGOnTest);
            this.groupBox5.Controls.Add(this.btnAGOnTestAdd);
            this.groupBox5.Controls.Add(this.btnAGOnTestChange);
            this.groupBox5.Controls.Add(this.btnAGOnTestDel);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // btnAGOnTest2Add
            // 
            resources.ApplyResources(this.btnAGOnTest2Add, "btnAGOnTest2Add");
            this.btnAGOnTest2Add.Name = "btnAGOnTest2Add";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTest2Add, resources.GetString("btnAGOnTest2Add.ToolTip"));
            this.btnAGOnTest2Add.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // btnAGOnTest2Change
            // 
            resources.ApplyResources(this.btnAGOnTest2Change, "btnAGOnTest2Change");
            this.btnAGOnTest2Change.Name = "btnAGOnTest2Change";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTest2Change, resources.GetString("btnAGOnTest2Change.ToolTip"));
            this.btnAGOnTest2Change.UseVisualStyleBackColor = true;
            // 
            // btnAGOnTest2Del
            // 
            resources.ApplyResources(this.btnAGOnTest2Del, "btnAGOnTest2Del");
            this.btnAGOnTest2Del.Name = "btnAGOnTest2Del";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTest2Del, resources.GetString("btnAGOnTest2Del.ToolTip"));
            this.btnAGOnTest2Del.UseVisualStyleBackColor = true;
            // 
            // lbAGOnTest2
            // 
            this.lbAGOnTest2.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGOnTest2, "lbAGOnTest2");
            this.lbAGOnTest2.Name = "lbAGOnTest2";
            // 
            // lbAGOnTest
            // 
            this.lbAGOnTest.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGOnTest, "lbAGOnTest");
            this.lbAGOnTest.Name = "lbAGOnTest";
            // 
            // btnAGOnTestAdd
            // 
            resources.ApplyResources(this.btnAGOnTestAdd, "btnAGOnTestAdd");
            this.btnAGOnTestAdd.Name = "btnAGOnTestAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTestAdd, resources.GetString("btnAGOnTestAdd.ToolTip"));
            this.btnAGOnTestAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGOnTestChange
            // 
            resources.ApplyResources(this.btnAGOnTestChange, "btnAGOnTestChange");
            this.btnAGOnTestChange.Name = "btnAGOnTestChange";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTestChange, resources.GetString("btnAGOnTestChange.ToolTip"));
            this.btnAGOnTestChange.UseVisualStyleBackColor = true;
            // 
            // btnAGOnTestDel
            // 
            resources.ApplyResources(this.btnAGOnTestDel, "btnAGOnTestDel");
            this.btnAGOnTestDel.Name = "btnAGOnTestDel";
            this.toolTipDialogs.SetToolTip(this.btnAGOnTestDel, resources.GetString("btnAGOnTestDel.ToolTip"));
            this.btnAGOnTestDel.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.btnAGOpened2Add);
            this.groupBox4.Controls.Add(this.btnAGOpened2Change);
            this.groupBox4.Controls.Add(this.btnAGOpened2Del);
            this.groupBox4.Controls.Add(this.lbAGOpened2);
            this.groupBox4.Controls.Add(this.lbAGOpened);
            this.groupBox4.Controls.Add(this.btnAGOpenedAdd);
            this.groupBox4.Controls.Add(this.btnAGOpenedChange);
            this.groupBox4.Controls.Add(this.btnAGOpenedDel);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // btnAGOpened2Add
            // 
            resources.ApplyResources(this.btnAGOpened2Add, "btnAGOpened2Add");
            this.btnAGOpened2Add.Name = "btnAGOpened2Add";
            this.toolTipDialogs.SetToolTip(this.btnAGOpened2Add, resources.GetString("btnAGOpened2Add.ToolTip"));
            this.btnAGOpened2Add.UseVisualStyleBackColor = true;
            // 
            // btnAGOpened2Change
            // 
            resources.ApplyResources(this.btnAGOpened2Change, "btnAGOpened2Change");
            this.btnAGOpened2Change.Name = "btnAGOpened2Change";
            this.toolTipDialogs.SetToolTip(this.btnAGOpened2Change, resources.GetString("btnAGOpened2Change.ToolTip"));
            this.btnAGOpened2Change.UseVisualStyleBackColor = true;
            // 
            // btnAGOpened2Del
            // 
            resources.ApplyResources(this.btnAGOpened2Del, "btnAGOpened2Del");
            this.btnAGOpened2Del.Name = "btnAGOpened2Del";
            this.toolTipDialogs.SetToolTip(this.btnAGOpened2Del, resources.GetString("btnAGOpened2Del.ToolTip"));
            this.btnAGOpened2Del.UseVisualStyleBackColor = true;
            // 
            // lbAGOpened2
            // 
            this.lbAGOpened2.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGOpened2, "lbAGOpened2");
            this.lbAGOpened2.Name = "lbAGOpened2";
            // 
            // lbAGOpened
            // 
            this.lbAGOpened.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGOpened, "lbAGOpened");
            this.lbAGOpened.Name = "lbAGOpened";
            // 
            // btnAGOpenedAdd
            // 
            resources.ApplyResources(this.btnAGOpenedAdd, "btnAGOpenedAdd");
            this.btnAGOpenedAdd.Name = "btnAGOpenedAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGOpenedAdd, resources.GetString("btnAGOpenedAdd.ToolTip"));
            this.btnAGOpenedAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGOpenedChange
            // 
            resources.ApplyResources(this.btnAGOpenedChange, "btnAGOpenedChange");
            this.btnAGOpenedChange.Name = "btnAGOpenedChange";
            this.toolTipDialogs.SetToolTip(this.btnAGOpenedChange, resources.GetString("btnAGOpenedChange.ToolTip"));
            this.btnAGOpenedChange.UseVisualStyleBackColor = true;
            // 
            // btnAGOpenedDel
            // 
            resources.ApplyResources(this.btnAGOpenedDel, "btnAGOpenedDel");
            this.btnAGOpenedDel.Name = "btnAGOpenedDel";
            this.toolTipDialogs.SetToolTip(this.btnAGOpenedDel, resources.GetString("btnAGOpenedDel.ToolTip"));
            this.btnAGOpenedDel.UseVisualStyleBackColor = true;
            // 
            // cbNPCNature
            // 
            this.cbNPCNature.FormattingEnabled = true;
            this.cbNPCNature.Items.AddRange(new object[] {
            resources.GetString("cbNPCNature.Items"),
            resources.GetString("cbNPCNature.Items1"),
            resources.GetString("cbNPCNature.Items2")});
            resources.ApplyResources(this.cbNPCNature, "cbNPCNature");
            this.cbNPCNature.Name = "cbNPCNature";
            this.cbNPCNature.SelectedIndexChanged += new System.EventHandler(this.cbNPCNature_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.lbAGDeclineQuest);
            this.tabPage3.Controls.Add(this.btnAGDeclineQuestAdd);
            this.tabPage3.Controls.Add(this.btnAGDeclineQuestChange);
            this.tabPage3.Controls.Add(this.btnAGDeclineQuestDel);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.lbAGAcceptQuest);
            this.tabPage3.Controls.Add(this.btnAGAcceptQuestAdd);
            this.tabPage3.Controls.Add(this.btnAGAcceptQuestChange);
            this.tabPage3.Controls.Add(this.btnAGAcceptQuestDel);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.lbAGGetQuest);
            this.tabPage3.Controls.Add(this.btnAGGetQuestAdd);
            this.tabPage3.Controls.Add(this.btnAGGetQuestChange);
            this.tabPage3.Controls.Add(this.btnAGGetQuestDel);
            this.tabPage3.Controls.Add(this.label9);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // lbAGDeclineQuest
            // 
            this.lbAGDeclineQuest.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGDeclineQuest, "lbAGDeclineQuest");
            this.lbAGDeclineQuest.Name = "lbAGDeclineQuest";
            // 
            // btnAGDeclineQuestAdd
            // 
            resources.ApplyResources(this.btnAGDeclineQuestAdd, "btnAGDeclineQuestAdd");
            this.btnAGDeclineQuestAdd.Name = "btnAGDeclineQuestAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGDeclineQuestAdd, resources.GetString("btnAGDeclineQuestAdd.ToolTip"));
            this.btnAGDeclineQuestAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGDeclineQuestChange
            // 
            resources.ApplyResources(this.btnAGDeclineQuestChange, "btnAGDeclineQuestChange");
            this.btnAGDeclineQuestChange.Name = "btnAGDeclineQuestChange";
            this.toolTipDialogs.SetToolTip(this.btnAGDeclineQuestChange, resources.GetString("btnAGDeclineQuestChange.ToolTip"));
            this.btnAGDeclineQuestChange.UseVisualStyleBackColor = true;
            // 
            // btnAGDeclineQuestDel
            // 
            resources.ApplyResources(this.btnAGDeclineQuestDel, "btnAGDeclineQuestDel");
            this.btnAGDeclineQuestDel.Name = "btnAGDeclineQuestDel";
            this.toolTipDialogs.SetToolTip(this.btnAGDeclineQuestDel, resources.GetString("btnAGDeclineQuestDel.ToolTip"));
            this.btnAGDeclineQuestDel.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // lbAGAcceptQuest
            // 
            this.lbAGAcceptQuest.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGAcceptQuest, "lbAGAcceptQuest");
            this.lbAGAcceptQuest.Name = "lbAGAcceptQuest";
            // 
            // btnAGAcceptQuestAdd
            // 
            resources.ApplyResources(this.btnAGAcceptQuestAdd, "btnAGAcceptQuestAdd");
            this.btnAGAcceptQuestAdd.Name = "btnAGAcceptQuestAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGAcceptQuestAdd, resources.GetString("btnAGAcceptQuestAdd.ToolTip"));
            this.btnAGAcceptQuestAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGAcceptQuestChange
            // 
            resources.ApplyResources(this.btnAGAcceptQuestChange, "btnAGAcceptQuestChange");
            this.btnAGAcceptQuestChange.Name = "btnAGAcceptQuestChange";
            this.toolTipDialogs.SetToolTip(this.btnAGAcceptQuestChange, resources.GetString("btnAGAcceptQuestChange.ToolTip"));
            this.btnAGAcceptQuestChange.UseVisualStyleBackColor = true;
            // 
            // btnAGAcceptQuestDel
            // 
            resources.ApplyResources(this.btnAGAcceptQuestDel, "btnAGAcceptQuestDel");
            this.btnAGAcceptQuestDel.Name = "btnAGAcceptQuestDel";
            this.toolTipDialogs.SetToolTip(this.btnAGAcceptQuestDel, resources.GetString("btnAGAcceptQuestDel.ToolTip"));
            this.btnAGAcceptQuestDel.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // lbAGGetQuest
            // 
            this.lbAGGetQuest.FormattingEnabled = true;
            resources.ApplyResources(this.lbAGGetQuest, "lbAGGetQuest");
            this.lbAGGetQuest.Name = "lbAGGetQuest";
            // 
            // btnAGGetQuestAdd
            // 
            resources.ApplyResources(this.btnAGGetQuestAdd, "btnAGGetQuestAdd");
            this.btnAGGetQuestAdd.Name = "btnAGGetQuestAdd";
            this.toolTipDialogs.SetToolTip(this.btnAGGetQuestAdd, resources.GetString("btnAGGetQuestAdd.ToolTip"));
            this.btnAGGetQuestAdd.UseVisualStyleBackColor = true;
            // 
            // btnAGGetQuestChange
            // 
            resources.ApplyResources(this.btnAGGetQuestChange, "btnAGGetQuestChange");
            this.btnAGGetQuestChange.Name = "btnAGGetQuestChange";
            this.toolTipDialogs.SetToolTip(this.btnAGGetQuestChange, resources.GetString("btnAGGetQuestChange.ToolTip"));
            this.btnAGGetQuestChange.UseVisualStyleBackColor = true;
            // 
            // btnAGGetQuestDel
            // 
            resources.ApplyResources(this.btnAGGetQuestDel, "btnAGGetQuestDel");
            this.btnAGGetQuestDel.Name = "btnAGGetQuestDel";
            this.toolTipDialogs.SetToolTip(this.btnAGGetQuestDel, resources.GetString("btnAGGetQuestDel.ToolTip"));
            this.btnAGGetQuestDel.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // panelSelectNPC
            // 
            this.panelSelectNPC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSelectNPC.Controls.Add(this.btnFilterGroupNPC);
            this.panelSelectNPC.Controls.Add(this.btnCheckNPC);
            this.panelSelectNPC.Controls.Add(this.btnFilterNPC);
            this.panelSelectNPC.Controls.Add(this.btnNextNPC);
            this.panelSelectNPC.Controls.Add(this.btnBackNPC);
            this.panelSelectNPC.Controls.Add(this.bAddNPC);
            this.panelSelectNPC.Controls.Add(this.labelChosenNPC);
            this.panelSelectNPC.Controls.Add(this.NPCBox);
            this.panelSelectNPC.Controls.Add(this.FakeNPCBox);
            resources.ApplyResources(this.panelSelectNPC, "panelSelectNPC");
            this.panelSelectNPC.Name = "panelSelectNPC";
            // 
            // btnFilterGroupNPC
            // 
            this.btnFilterGroupNPC.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.filter_g_24;
            resources.ApplyResources(this.btnFilterGroupNPC, "btnFilterGroupNPC");
            this.btnFilterGroupNPC.Name = "btnFilterGroupNPC";
            this.toolTipDialogs.SetToolTip(this.btnFilterGroupNPC, resources.GetString("btnFilterGroupNPC.ToolTip"));
            this.btnFilterGroupNPC.UseVisualStyleBackColor = true;
            this.btnFilterGroupNPC.Click += new System.EventHandler(this.btnFilterGroupNPC_Click);
            // 
            // btnCheckNPC
            // 
            resources.ApplyResources(this.btnCheckNPC, "btnCheckNPC");
            this.btnCheckNPC.Name = "btnCheckNPC";
            this.btnCheckNPC.UseVisualStyleBackColor = true;
            this.btnCheckNPC.Click += new System.EventHandler(this.btnCheckNPC_Click);
            // 
            // btnFilterNPC
            // 
            this.btnFilterNPC.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.filter_24;
            resources.ApplyResources(this.btnFilterNPC, "btnFilterNPC");
            this.btnFilterNPC.Name = "btnFilterNPC";
            this.toolTipDialogs.SetToolTip(this.btnFilterNPC, resources.GetString("btnFilterNPC.ToolTip"));
            this.btnFilterNPC.UseVisualStyleBackColor = true;
            this.btnFilterNPC.Click += new System.EventHandler(this.btnFilterNPC_Click);
            // 
            // btnNextNPC
            // 
            resources.ApplyResources(this.btnNextNPC, "btnNextNPC");
            this.btnNextNPC.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.next_24;
            this.btnNextNPC.Name = "btnNextNPC";
            this.toolTipDialogs.SetToolTip(this.btnNextNPC, resources.GetString("btnNextNPC.ToolTip"));
            this.btnNextNPC.UseVisualStyleBackColor = true;
            this.btnNextNPC.Click += new System.EventHandler(this.btnNextNPC_Click);
            // 
            // btnBackNPC
            // 
            resources.ApplyResources(this.btnBackNPC, "btnBackNPC");
            this.btnBackNPC.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.back_24;
            this.btnBackNPC.Name = "btnBackNPC";
            this.toolTipDialogs.SetToolTip(this.btnBackNPC, resources.GetString("btnBackNPC.ToolTip"));
            this.btnBackNPC.UseVisualStyleBackColor = true;
            this.btnBackNPC.Click += new System.EventHandler(this.btnBackNPC_Click);
            // 
            // bAddNPC
            // 
            resources.ApplyResources(this.bAddNPC, "bAddNPC");
            this.bAddNPC.Name = "bAddNPC";
            this.bAddNPC.UseVisualStyleBackColor = true;
            this.bAddNPC.Click += new System.EventHandler(this.bAddNPC_Click);
            // 
            // FakeNPCBox
            // 
            this.FakeNPCBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.FakeNPCBox.DropDownWidth = 280;
            this.FakeNPCBox.FormattingEnabled = true;
            resources.ApplyResources(this.FakeNPCBox, "FakeNPCBox");
            this.FakeNPCBox.Name = "FakeNPCBox";
            this.FakeNPCBox.SelectedIndexChanged += new System.EventHandler(this.FakeNPCBox_SelectedIndexChanged);
            // 
            // menuMainControl
            // 
            this.menuMainControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMain,
            this.menuSaveAll,
            this.menuExplorer,
            this.обновленияToolStripMenuItem,
            this.данныеToolStripMenuItem});
            resources.ApplyResources(this.menuMainControl, "menuMainControl");
            this.menuMainControl.Name = "menuMainControl";
            this.menuMainControl.TabStop = true;
            // 
            // menuMain
            // 
            this.menuMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings,
            this.menuSynchronize,
            this.menuStatistics,
            this.changeLanguageToolStripMenuItem,
            this.menuExit});
            this.menuMain.Name = "menuMain";
            resources.ApplyResources(this.menuMain, "menuMain");
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            resources.ApplyResources(this.menuSettings, "menuSettings");
            this.menuSettings.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // menuSynchronize
            // 
            this.menuSynchronize.Name = "menuSynchronize";
            resources.ApplyResources(this.menuSynchronize, "menuSynchronize");
            this.menuSynchronize.Click += new System.EventHandler(this.SynchroToolStripMenuItem_Click);
            // 
            // menuStatistics
            // 
            this.menuStatistics.Name = "menuStatistics";
            resources.ApplyResources(this.menuStatistics, "menuStatistics");
            this.menuStatistics.Click += new System.EventHandler(this.StatisticsToolStripMenuItem_Click);
            // 
            // changeLanguageToolStripMenuItem
            // 
            this.changeLanguageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.русскийToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.changeLanguageToolStripMenuItem.Name = "changeLanguageToolStripMenuItem";
            resources.ApplyResources(this.changeLanguageToolStripMenuItem, "changeLanguageToolStripMenuItem");
            // 
            // русскийToolStripMenuItem
            // 
            this.русскийToolStripMenuItem.Name = "русскийToolStripMenuItem";
            resources.ApplyResources(this.русскийToolStripMenuItem, "русскийToolStripMenuItem");
            this.русскийToolStripMenuItem.Click += new System.EventHandler(this.русскийToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            resources.ApplyResources(this.menuExit, "menuExit");
            this.menuExit.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // menuSaveAll
            // 
            this.menuSaveAll.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.SaveDisk;
            this.menuSaveAll.Name = "menuSaveAll";
            resources.ApplyResources(this.menuSaveAll, "menuSaveAll");
            this.menuSaveAll.Click += new System.EventHandler(this.SaveAllToolStripMenuItem_Click);
            // 
            // menuExplorer
            // 
            this.menuExplorer.Image = global::StalkerOnlineQuesterEditor.Properties.Resources.Explorer;
            this.menuExplorer.Name = "menuExplorer";
            resources.ApplyResources(this.menuExplorer, "menuExplorer");
            this.menuExplorer.Click += new System.EventHandler(this.ExplorerToolStripMenuItem_Click);
            // 
            // обновленияToolStripMenuItem
            // 
            this.обновленияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.парсерыToolStripMenuItem,
            this.проверкаОшибокToolStripMenuItem,
            this.поискДиалоговПоQuestIDToolStripMenuItem,
            this.поискДиалоговПоЗнаниюToolStripMenuItem,
            this.способыПолученияКвестаToolStripMenuItem});
            this.обновленияToolStripMenuItem.Name = "обновленияToolStripMenuItem";
            resources.ApplyResources(this.обновленияToolStripMenuItem, "обновленияToolStripMenuItem");
            // 
            // парсерыToolStripMenuItem
            // 
            this.парсерыToolStripMenuItem.Name = "парсерыToolStripMenuItem";
            resources.ApplyResources(this.парсерыToolStripMenuItem, "парсерыToolStripMenuItem");
            this.парсерыToolStripMenuItem.Click += new System.EventHandler(this.парсерыToolStripMenuItem_Click);
            // 
            // проверкаОшибокToolStripMenuItem
            // 
            this.проверкаОшибокToolStripMenuItem.Name = "проверкаОшибокToolStripMenuItem";
            resources.ApplyResources(this.проверкаОшибокToolStripMenuItem, "проверкаОшибокToolStripMenuItem");
            this.проверкаОшибокToolStripMenuItem.Click += new System.EventHandler(this.проверкаОшибокToolStripMenuItem_Click);
            // 
            // поискДиалоговПоQuestIDToolStripMenuItem
            // 
            this.поискДиалоговПоQuestIDToolStripMenuItem.Name = "поискДиалоговПоQuestIDToolStripMenuItem";
            resources.ApplyResources(this.поискДиалоговПоQuestIDToolStripMenuItem, "поискДиалоговПоQuestIDToolStripMenuItem");
            this.поискДиалоговПоQuestIDToolStripMenuItem.Click += new System.EventHandler(this.поискДиалоговПоQuestIDToolStripMenuItem_Click);
            // 
            // поискДиалоговПоЗнаниюToolStripMenuItem
            // 
            this.поискДиалоговПоЗнаниюToolStripMenuItem.Name = "поискДиалоговПоЗнаниюToolStripMenuItem";
            resources.ApplyResources(this.поискДиалоговПоЗнаниюToolStripMenuItem, "поискДиалоговПоЗнаниюToolStripMenuItem");
            this.поискДиалоговПоЗнаниюToolStripMenuItem.Click += new System.EventHandler(this.поискДиалоговПоЗнаниюToolStripMenuItem_Click);
            // 
            // способыПолученияКвестаToolStripMenuItem
            // 
            this.способыПолученияКвестаToolStripMenuItem.Name = "способыПолученияКвестаToolStripMenuItem";
            resources.ApplyResources(this.способыПолученияКвестаToolStripMenuItem, "способыПолученияКвестаToolStripMenuItem");
            this.способыПолученияКвестаToolStripMenuItem.Click += new System.EventHandler(this.способыПолученияКвестаToolStripMenuItem_Click);
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.собратьЭдиторДляПередачиToolStripMenuItem,
            this.вытащитьНепереведённыеТекстыToolStripMenuItem,
            this.загрузитьПереводToolStripMenuItem});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            resources.ApplyResources(this.данныеToolStripMenuItem, "данныеToolStripMenuItem");
            // 
            // собратьЭдиторДляПередачиToolStripMenuItem
            // 
            this.собратьЭдиторДляПередачиToolStripMenuItem.Name = "собратьЭдиторДляПередачиToolStripMenuItem";
            resources.ApplyResources(this.собратьЭдиторДляПередачиToolStripMenuItem, "собратьЭдиторДляПередачиToolStripMenuItem");
            this.собратьЭдиторДляПередачиToolStripMenuItem.Click += new System.EventHandler(this.собратьЭдиторДляПередачиToolStripMenuItem_Click);
            // 
            // вытащитьНепереведённыеТекстыToolStripMenuItem
            // 
            this.вытащитьНепереведённыеТекстыToolStripMenuItem.Name = "вытащитьНепереведённыеТекстыToolStripMenuItem";
            resources.ApplyResources(this.вытащитьНепереведённыеТекстыToolStripMenuItem, "вытащитьНепереведённыеТекстыToolStripMenuItem");
            this.вытащитьНепереведённыеТекстыToolStripMenuItem.Click += new System.EventHandler(this.вытащитьНепереведённыеТекстыToolStripMenuItem_Click);
            // 
            // загрузитьПереводToolStripMenuItem
            // 
            this.загрузитьПереводToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.диалоговToolStripMenuItem,
            this.квестовToolStripMenuItem});
            this.загрузитьПереводToolStripMenuItem.Name = "загрузитьПереводToolStripMenuItem";
            resources.ApplyResources(this.загрузитьПереводToolStripMenuItem, "загрузитьПереводToolStripMenuItem");
            // 
            // диалоговToolStripMenuItem
            // 
            this.диалоговToolStripMenuItem.Name = "диалоговToolStripMenuItem";
            resources.ApplyResources(this.диалоговToolStripMenuItem, "диалоговToolStripMenuItem");
            this.диалоговToolStripMenuItem.Click += new System.EventHandler(this.диалоговToolStripMenuItem_Click);
            // 
            // квестовToolStripMenuItem
            // 
            this.квестовToolStripMenuItem.Name = "квестовToolStripMenuItem";
            resources.ApplyResources(this.квестовToolStripMenuItem, "квестовToolStripMenuItem");
            this.квестовToolStripMenuItem.Click += new System.EventHandler(this.квестовToolStripMenuItem_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Stretch = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CentralDock);
            this.Controls.Add(this.panelSelectNPC);
            this.Controls.Add(this.menuMainControl);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.splitDialogs.Panel1.ResumeLayout(false);
            this.splitDialogs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogs)).EndInit();
            this.splitDialogs.ResumeLayout(false);
            this.gbDialogsEditor.ResumeLayout(false);
            this.gbDialogsEditor.PerformLayout();
            this.splitDialogsTreeAndCanvas.Panel1.ResumeLayout(false);
            this.splitDialogsTreeAndCanvas.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogsTreeAndCanvas)).EndInit();
            this.splitDialogsTreeAndCanvas.ResumeLayout(false);
            this.panelDialogTools.ResumeLayout(false);
            this.panelDialogTools.PerformLayout();
            this.gbNPCWorkSpace.ResumeLayout(false);
            this.gbEmulator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDialogsEmulator)).EndInit();
            this.splitDialogsEmulator.ResumeLayout(false);
            this.splitQuestsContainer.Panel1.ResumeLayout(false);
            this.splitQuestsContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitQuestsContainer)).EndInit();
            this.splitQuestsContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.CentralDock.ResumeLayout(false);
            this.tabDialogs.ResumeLayout(false);
            this.tabQuests.ResumeLayout(false);
            this.tabQuests.PerformLayout();
            this.panelQuestTools.ResumeLayout(false);
            this.panelSelectQuest.ResumeLayout(false);
            this.panelSelectQuest.PerformLayout();
            this.tabFraction.ResumeLayout(false);
            this.tabFraction.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbEmul2.ResumeLayout(false);
            this.tabInfoNPC.ResumeLayout(false);
            this.panelNpcLinkControls.ResumeLayout(false);
            this.panelNpcLinkControls.PerformLayout();
            this.tabReview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReview)).EndInit();
            this.panelReviewButtons.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbNPCcheck.ResumeLayout(false);
            this.gbNPCcheck.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDialogs)).EndInit();
            this.gbQuestCheck.ResumeLayout(false);
            this.gbQuestCheck.PerformLayout();
            this.tabManage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManage)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabTranslate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocaleDiff)).EndInit();
            this.panelDiffLocale.ResumeLayout(false);
            this.panelDiffLocale.PerformLayout();
            this.tabSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.panelSearchTools.ResumeLayout(false);
            this.panelSearchTools.PerformLayout();
            this.tabKnowledge.ResumeLayout(false);
            this.tabKnowledge.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tabAutoGen.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupToTargetCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFromTargetCount)).EndInit();
            this.tabPageAGDialog.ResumeLayout(false);
            this.tabPageAGDialog.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panelSelectNPC.ResumeLayout(false);
            this.panelSelectNPC.PerformLayout();
            this.menuMainControl.ResumeLayout(false);
            this.menuMainControl.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox NPCBox;
        private System.Windows.Forms.Label labelChosenNPC;
        private System.Windows.Forms.TabPage tabDialogs;
        private System.Windows.Forms.TabPage tabQuests;
        public PCanvas DialogShower;
        private System.Windows.Forms.SplitContainer splitDialogs;
        private System.Windows.Forms.GroupBox gbDialogsEditor;
        private System.Windows.Forms.Panel panelSelectNPC;
        private System.Windows.Forms.Panel panelDialogTools;
        private System.Windows.Forms.SplitContainer splitDialogsTreeAndCanvas;
        public System.Windows.Forms.TreeView treeDialogs;
        private System.Windows.Forms.Button bAddDialog;
        private System.Windows.Forms.Button bEditDialog;
        private System.Windows.Forms.Button bRemoveDialog;
        private System.Windows.Forms.GroupBox gbEmulator;
        private System.Windows.Forms.SplitContainer splitDialogsEmulator;
        private System.Windows.Forms.SplitContainer splitQuestsContainer;
        private System.Windows.Forms.TreeView treeQuest;
        private System.Windows.Forms.Button bAddNPC;
        private System.Windows.Forms.MenuStrip menuMainControl;
        private System.Windows.Forms.ToolStripMenuItem menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.TabPage tabInfoNPC;
        private PCanvas npcLinkShower;
        private System.Windows.Forms.TabPage tabManage;
        private System.Windows.Forms.DataGridView dgvManage;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button bSaveManage;
        private System.Windows.Forms.TreeView treeQuestBuffer;
        private System.Windows.Forms.Label labelQuestTree;
        private System.Windows.Forms.Label labelBuffer;
        private System.Windows.Forms.TabPage tabTranslate;
        private System.Windows.Forms.Panel panelDiffLocale;
        private System.Windows.Forms.Button bFindDialogDifference;
        private System.Windows.Forms.DataGridView dgvLocaleDiff;
        private System.Windows.Forms.Button bFindQuestDifference;
        private System.Windows.Forms.CheckBox cbActualFinder;
        private System.Windows.Forms.CheckBox cbOutdatedFinder;
        private System.Windows.Forms.Label labelLocalizeOuput;
        private System.Windows.Forms.TabControl CentralDock;
        private System.Windows.Forms.Label labelYNode;
        private System.Windows.Forms.Label labelXNode;
        private System.Windows.Forms.Button bCenterizeDialogShower;
        private System.Windows.Forms.TabPage tabReview;
        private System.Windows.Forms.Panel panelReviewButtons;
        private System.Windows.Forms.GroupBox gbNPCcheck;
        private System.Windows.Forms.NumericUpDown numDialogs;
        private System.Windows.Forms.Label labelLessThan1;
        private System.Windows.Forms.CheckBox cbNumDialogs;
        private System.Windows.Forms.Button bFindNPC;
        private System.Windows.Forms.DataGridView dgvReview;
        private System.Windows.Forms.NumericUpDown numQuests;
        private System.Windows.Forms.Label labelLessThan2;
        private System.Windows.Forms.CheckBox cbNumQuests;
        private System.Windows.Forms.Panel panelNpcLinkControls;
        private System.Windows.Forms.Button bNpcLinkExecute;
        private System.Windows.Forms.Label labelAdviceNpcLink;
        private System.Windows.Forms.GroupBox gbQuestCheck;
        private System.Windows.Forms.Button bFindQuest;
        private System.Windows.Forms.Panel panelQuestTools;
        private System.Windows.Forms.Button bPasteEvents;
        private System.Windows.Forms.Button bCopyEvents;
        private System.Windows.Forms.Button bQuestDown;
        private System.Windows.Forms.Button bQuestUp;
        private System.Windows.Forms.Button bRemoveEvent;
        private System.Windows.Forms.Button bEditEvent;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.CheckBox cbOnlyOnLocation;
        private System.Windows.Forms.Button bCutEvents;
        private System.Windows.Forms.Button bClearBuffer;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.Panel panelSearchTools;
        private System.Windows.Forms.Button bStartSearch;
        private System.Windows.Forms.Label labelPhraseToSearch;
        private System.Windows.Forms.TextBox tbPhraseToSearch;
        private System.Windows.Forms.Label labelSearchResult;
        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.CheckBox cbIgnoreCase;
        private System.Windows.Forms.ToolStripMenuItem menuExplorer;
        private System.Windows.Forms.ToolStripMenuItem menuStatistics;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAll;
        private System.Windows.Forms.Panel panelSelectQuest;
        private System.Windows.Forms.Button bRemoveQuest;
        private System.Windows.Forms.Button bAddQuest;
        private System.Windows.Forms.ComboBox QuestBox;
        private System.Windows.Forms.Label labelChosenQuest;
        private System.Windows.Forms.Button bAddEvent;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem menuSynchronize;
        private System.Windows.Forms.Label labelItemReward;
        private System.Windows.Forms.ComboBox cbItemReward;
        private System.Windows.Forms.Label labelDrawingTip;
        private System.Windows.Forms.ToolTip toolTipDialogs;
        private System.Windows.Forms.ComboBox cbItemTarget;
        private System.Windows.Forms.Label labelItemTarget;
        private System.Windows.Forms.ToolStripMenuItem обновленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem парсерыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаОшибокToolStripMenuItem;
        private System.Windows.Forms.Button btnBackNPC;
        private System.Windows.Forms.Button btnNextNPC;
        private System.Windows.Forms.Button btnClearRecycle;
        private System.Windows.Forms.Button btnFilterNPC;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem собратьЭдиторДляПередачиToolStripMenuItem;
        private System.Windows.Forms.Label lFindDialogID;
        private System.Windows.Forms.TextBox tbFindDialogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn npc_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn identif;
        private System.Windows.Forms.DataGridViewTextBoxColumn cur_ver;
        private System.Windows.Forms.DataGridViewTextBoxColumn new_ver;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn RusText1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngText1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEngText;
        private System.Windows.Forms.ToolStripMenuItem вытащитьНепереведённыеТекстыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьПереводToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem диалоговToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem квестовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem русскийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRep2List;
        private System.Windows.Forms.Button bFindNPCReputation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNPCName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDialogsNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuestsNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCoordinates;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRussianName;
        private System.Windows.Forms.TabPage tabFraction;
        private System.Windows.Forms.TreeView treeFractionDialogs;
        private PCanvas fractionDialogShower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox fractionBox;
        private System.Windows.Forms.Button bRemoveFracDialog;
        private System.Windows.Forms.Button bEditFracDialog;
        private System.Windows.Forms.Button bAddFracDialog;
        private System.Windows.Forms.TabPage tabKnowledge;
        private System.Windows.Forms.ComboBox cbKnowlegeTypeValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbKnowledgeCategory;
        private System.Windows.Forms.TreeView treeKnowladge;
        private System.Windows.Forms.ComboBox FakeNPCBox;
        private System.Windows.Forms.Button bCopyDialogTree;
        private System.Windows.Forms.GroupBox gbNPCWorkSpace;
        private System.Windows.Forms.ListBox lbWorkSpace;
        private System.Windows.Forms.Button bWSDel;
        private System.Windows.Forms.Button bWSAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn subevents;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn npcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn subNPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dialogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardBattle;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn rewardItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn repeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn repeatPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
        private System.Windows.Forms.DataGridViewTextBoxColumn author;
        private System.Windows.Forms.DataGridViewTextBoxColumn Legend;
        private System.Windows.Forms.DataGridViewComboBoxColumn worked;
        private System.Windows.Forms.TabPage tabAutoGen;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnDelTarget;
        private System.Windows.Forms.Button btnAddTarget;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBoxTarget;
        private System.Windows.Forms.ListBox listBoxQT;
        private System.Windows.Forms.TabPage tabPageAGDialog;
        private System.Windows.Forms.Button btnCheckNPC;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnDelQType;
        private System.Windows.Forms.Button btnAddQType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nupToTargetCount;
        private System.Windows.Forms.NumericUpDown nupFromTargetCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxReward;
        private System.Windows.Forms.Button btnAddReward;
        private System.Windows.Forms.Button btnChangeReward;
        private System.Windows.Forms.Button btnDelReward;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnAGClosed2Add;
        private System.Windows.Forms.Button btnAGClosed2Change;
        private System.Windows.Forms.Button btnAGClosed2Del;
        private System.Windows.Forms.ListBox lbAGClosed2;
        private System.Windows.Forms.ListBox lbAGClosed;
        private System.Windows.Forms.Button btnAGClosedAdd;
        private System.Windows.Forms.Button btnAGClosedChange;
        private System.Windows.Forms.Button btnAGClosedDel;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnAGOnTest2Add;
        private System.Windows.Forms.Button btnAGOnTest2Change;
        private System.Windows.Forms.Button btnAGOnTest2Del;
        private System.Windows.Forms.ListBox lbAGOnTest2;
        private System.Windows.Forms.ListBox lbAGOnTest;
        private System.Windows.Forms.Button btnAGOnTestAdd;
        private System.Windows.Forms.Button btnAGOnTestChange;
        private System.Windows.Forms.Button btnAGOnTestDel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAGOpened2Add;
        private System.Windows.Forms.Button btnAGOpened2Change;
        private System.Windows.Forms.Button btnAGOpened2Del;
        private System.Windows.Forms.ListBox lbAGOpened2;
        private System.Windows.Forms.ListBox lbAGOpened;
        private System.Windows.Forms.Button btnAGOpenedAdd;
        private System.Windows.Forms.Button btnAGOpenedChange;
        private System.Windows.Forms.Button btnAGOpenedDel;
        private System.Windows.Forms.ComboBox cbNPCNature;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnAddKnowlege;
        private System.Windows.Forms.Button btnDelKnowlege;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListBox lbAGDeclineQuest;
        private System.Windows.Forms.Button btnAGDeclineQuestAdd;
        private System.Windows.Forms.Button btnAGDeclineQuestChange;
        private System.Windows.Forms.Button btnAGDeclineQuestDel;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ListBox lbAGAcceptQuest;
        private System.Windows.Forms.Button btnAGAcceptQuestAdd;
        private System.Windows.Forms.Button btnAGAcceptQuestChange;
        private System.Windows.Forms.Button btnAGAcceptQuestDel;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ListBox lbAGGetQuest;
        private System.Windows.Forms.Button btnAGGetQuestAdd;
        private System.Windows.Forms.Button btnAGGetQuestChange;
        private System.Windows.Forms.Button btnAGGetQuestDel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem поискДиалоговПоQuestIDToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbQuestLocations;
        private System.Windows.Forms.CheckBox cbOnQuestLocation;
        private System.Windows.Forms.ToolStripMenuItem поискДиалоговПоЗнаниюToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button bFindKnowledgeQuest;
        private System.Windows.Forms.ComboBox cbRepLocations;
        private System.Windows.Forms.CheckBox cbOnRepLocation;
        private System.Windows.Forms.TextBox tbKnowledgeFind;
        private System.Windows.Forms.CheckBox cbOnlyTradePoints;
        private System.Windows.Forms.Button btnFilterGroupNPC;
        private System.Windows.Forms.GroupBox gbEmul2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox lbAGHello;
        private System.Windows.Forms.Button btnAGHelloAdd;
        private System.Windows.Forms.Button btnAGHelloChange;
        private System.Windows.Forms.Button btnAGHelloDel;
        private System.Windows.Forms.ComboBox FakeQuestBox;
        private System.Windows.Forms.ToolStripMenuItem способыПолученияКвестаToolStripMenuItem;
    }
}

