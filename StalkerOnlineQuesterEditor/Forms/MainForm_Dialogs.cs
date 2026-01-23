using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Event;
using System.Collections;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace StalkerOnlineQuesterEditor
{
    //! Словарь <DialogID, CDialog> - используется для диалогов 1 персонажа NPC
    using DialogDict = Dictionary<int, CDialog>;
    using NPCLocales = Dictionary<string, Dictionary<string, Dictionary<int, CDialog>>>;
    //! Тип выделенного элемента на экране
    public enum SelectedItemType
    { 
        none = 0,
        dialog = 1,
        rectangle = 2
    };

    public partial class MainForm : Form
    {
        //! Возвращает вершину графа диалогов - корневую фразу
        CDialog getRootDialog(DialogDict Dialogs)
        {
            CDialog result = null;
            foreach (CDialog dialog in Dialogs.Values)
            {
                if (dialog.coordinates.RootDialog)
                {
                    rootElements.Add(dialog.DialogID);
                    result = dialog;
                }
            }
            
            // костыль для локализации
            if (result == null)
            {
                Dialogs = this.dialogs.dialogs[currentNPC];
                foreach (CDialog dialog in Dialogs.Values)
                {
                    if (dialog.coordinates.RootDialog)
                    {
                        rootElements.Add(dialog.DialogID);
                            string loc = CSettings.getCurrentLocale();
                            if (!dialogs.locales[loc].ContainsKey(currentNPC))
                            {
                                CDialog toadd = new CDialog();
                                toadd = (CDialog)dialog.Clone();
                                toadd.Text = "";
                                toadd.Title = "";
                                toadd.version = 0;
                                Dictionary<int, CDialog> newdict = new Dictionary<int, CDialog>();
                                newdict.Add(toadd.DialogID, toadd);
                                dialogs.locales[loc].Add(currentNPC, newdict);
                            }

                            else if (!dialogs.locales[loc][currentNPC].ContainsKey(dialog.DialogID))
                            {
                                CDialog toadd = (CDialog)dialog.Clone();
                                toadd.Text = "";
                                toadd.Title = "";
                                toadd.version = 0;
                                dialogs.locales[CSettings.getCurrentLocale()][currentNPC].Add(toadd.DialogID, toadd);
                            }
                            else
                                dialogs.locales[CSettings.getCurrentLocale()][currentNPC][dialog.DialogID].coordinates.RootDialog = true;
                        result = dialog;
                    }
                }
            }
            return result;
        }

        //! Возвращает Узел по известному ID диалога
        public PNode getNodeOnDialogID(int dialogID)
        {
            return GraphProperties.findNodeOnID(graphs, dialogID);
        }
        //! Возвращает DialogID по известному узлу графа
        public int getDialogIDOnNode(PNode node)
        {
            if (graphs.Keys.Contains(node))
                return graphs[node].getDialogID();
            else return 0;
        }

        public List<CDialog> getDialogsWithDialogIDInNodes(int dialogID)
        {
            List<CDialog> ret = new List<CDialog>();
            foreach (CDialog dialog in dialogs.dialogs[currentNPC].Values)
                if (dialog.Nodes.Contains(dialogID))
                    ret.Add(dialog);
            return ret;
        }

        //! Возвращает экземпляр диалога по ID (всегда по-русски)
        public CDialog getDialogOnDialogID(int dialogID)
        {
            if (dialogID != 0)
            {
                if (dialogs.dialogs[currentNPC].ContainsKey(dialogID))
                    return dialogs.dialogs[currentNPC][dialogID];
                if (currentFraction.Any() && CFractionDialogs.dialogs[currentFraction].ContainsKey(dialogID))
                    return CFractionDialogs.dialogs[currentFraction][dialogID];
            }
            return null;
        }

        public List<int> getAllDialogsIDonCurrentNPC()
        {
            int index = CentralDock.SelectedIndex;  
            if (index == 2)
                return CFractionDialogs.dialogs[currentFraction].Keys.ToList<int>();
            return dialogs.dialogs[currentNPC].Keys.ToList<int>();
        }

        public bool isEmptyLocaledDialog(int dialogID)
        {
            if (dialogID == 0) return true;
            CDialog dialog;

            List<string> langs = CSettings.getListLocales();

            foreach(var lang in langs)
            {
                if (lang == "Russian") continue;
                if (CentralDock.SelectedIndex == 2)
                    dialog = CDialogs.getLocaleDialog(dialogID, lang, currentFraction, CFractionDialogs.locales);
                else
                    dialog = CDialogs.getLocaleDialog(dialogID, lang, currentNPC, dialogs.locales);
                if (dialog.Text.Any() || dialog.Title.Any()) return false;
            }

            return true;

        }

        //Возвращает какой-либо диалог, костыль
        public CDialog getAnyDialogOnID(int dialogID)
        {
            if (dialogID == 0) return null;
            if (CSettings.getMode() == CSettings.MODE_EDITOR)
            {
                if (dialogs.dialogs[currentNPC].ContainsKey(dialogID))
                    return dialogs.dialogs[currentNPC][dialogID];
                if (CFractionDialogs.dialogs[currentFraction].ContainsKey(dialogID))
                    return CFractionDialogs.dialogs[currentFraction][dialogID];
            }
            else
            {
                CDialog dd = new CDialog();
                if (CentralDock.SelectedIndex == 2)
                    dd = CDialogs.getLocaleDialog(dialogID, CSettings.getCurrentLocale(), currentFraction, CFractionDialogs.locales);
                else
                    dd = CDialogs.getLocaleDialog(dialogID, CSettings.getCurrentLocale(), currentNPC, dialogs.locales);
                if (dd != null)
                    return dd;
                else
                {
                    if (CentralDock.SelectedIndex == 2)
                        dd = (CDialog)CFractionDialogs.dialogs[currentFraction][dialogID].Clone();
                    else
                        dd = (CDialog)dialogs.dialogs[currentNPC][dialogID].Clone();
                    dd.version = 0;
                    return dd;
                }
            }
            return null;
        }

        //! Возвращает диалог по ID в зависимости от режима и локализации
        public CDialog getDialogOnIDConditional(int dialogID, DialogDict dialogs, NPCLocales locales)
        {
            if (dialogID != 0)
            {
                if (CSettings.getMode() == CSettings.MODE_EDITOR)
                    return dialogs[dialogID];
                else
                {
                    CDialog dd = new CDialog();
                    dd = CDialogs.getLocaleDialog(dialogID, CSettings.getCurrentLocale(), currentNPC, locales);
                    if (dd != null)
                        return dd;
                    else
                    {
                        dd = (CDialog)dialogs[dialogID].Clone();
                        dd.version = 0;
                        return dd;
                    }
                }
            }
            else
                return null;
        }

        //! Возвращает словарь диалогов одного NPC в зависимости от локализации
        public DialogDict getDialogDictionary(string key, Dictionary<string, DialogDict> dialogs, NPCLocales locales)
        {
            if (CSettings.getMode() == CSettings.MODE_EDITOR)
                return dialogs[key];
            else
            { 
                if (locales[CSettings.getCurrentLocale() ].ContainsKey(key) )
                    return locales[CSettings.getCurrentLocale() ][key];
                else
                    return dialogs[key];
            }
        }

        //**********************WORK WITH FORM ****************************************************

        private void fillDialogTree(CDialog root, DialogDict dialogs, TreeView tree, NPCLocales locales)
        {
            tree.Nodes.Clear();//tree clear
            tree.Nodes.Add("Active", "Active");
            tree.Nodes.Add("Recycle", "Recycle");
            foreach (TreeNode treeNode in tree.Nodes.Find("Active", true))
                treeNode.Nodes.Add(root.DialogID.ToString(), root.DialogID.ToString());
            this.fillNPCBoxSubquests(root, dialogs, tree);

            TreeNode treeActiveNode = tree.Nodes.Find("Active", true)[0];
            TreeNode treeRecycleNode = tree.Nodes.Find("Recycle", true)[0];
            foreach (CDialog dialog in dialogs.Values)
                if (!treeActiveNode.Nodes.ContainsKey(dialog.DialogID.ToString()))
                {
                    treeRecycleNode.Nodes.Add(dialog.DialogID.ToString(), dialog.DialogID.ToString());
                    dialog.coordinates.Active = false;
                    setNonActiveDialog(dialog.Holder, dialog.DialogID, locales);
                }

            tree.ExpandAll();
        }
        //! Удаляет диалог из локализаций при его удалении из русской части диалогов
        private void setNonActiveDialog(string holder, int id, NPCLocales locales)
        {
            foreach(var i in CSettings.getListLocales())
                locales[i][holder][id].coordinates.Active = false;
        }

        //! Заполняет граф диалога нужными узлами
        private void fillDialogGraphView(CDialog root, PCanvas dialogShower, DialogDict dialogs, NPCLocales locales)
        {
            // Initialize, and create a layer for the edges (always underneath the nodes)
            dialogShower.Layer.RemoveAllChildren();
            dialogShower.Camera.RemoveAllChildren();
            nodeLayer = new PNodeList();
            edgeLayer = new PLayer();
            drawingLayer = new PLayer();

            dialogShower.Camera.AddChild(drawingLayer);
            dialogShower.Camera.AddChild(edgeLayer);
            DrawRectangles();

            // Show root node
            float rootx = root.coordinates.X;
            float rooty = root.coordinates.Y;
            if (rootx == 0 && rooty == 0)
            {
                rootx = (float)this.ClientSize.Width / 5.0f;
                rooty = (float)this.ClientSize.Height / 5.0f;
            }
            PNode rootNode = CreateNode(root, new PointF(rootx, rooty));

            nodeLayer.Add(rootNode);
            if (!graphs.Keys.Contains(rootNode))
                graphs.Add(rootNode, new GraphProperties(root.DialogID));
            rootNode.Brush = GetBrushForNode(rootNode);
            //SaveCoordinates(root, rootNode, true);
            this.fillDialogSubgraphView(root, rootNode, 1.0f, ref edgeLayer, ref nodeLayer, false, dialogShower, dialogs, locales);

            dialogShower.Camera.AddChildren(nodeLayer);            
        }

        //! @brief Отображает все дочерние узлы на графе диалогов 
        //! @param root Старший диалог, экземпляр CDialog
        //! @param rootNode Старший узел, экземпляр PNode
        //! @param level Уровень наследования узлов
        //! @param edgeLayer
        //! @param nodeLayer
        //! @param stopAfterThat
        private void fillDialogSubgraphView(CDialog root, PNode rootNode, float level, ref PLayer edgeLayer, ref PNodeList nodeLayer, bool stopAfterThat, 
                            PCanvas dialogShower, DialogDict dialogs, NPCLocales locales)
        {

            float ix = rootNode.X;
            float iy = rootNode.Y;
            float i = 1;//Number of elements in string
            float localLevel = level;
            if (root.Actions.ToDialog != 0)
            {
                PNode toDialogNode = getNodeOnDialogID(root.Actions.ToDialog);

                if (toDialogNode != null)
                {
                    PrepareNodesForEdge(toDialogNode, rootNode, ref edgeLayer);
                    nodeLayer.Add(toDialogNode);
                    if (!stopAfterThat)
                    {
                        if (!isRoot(root.Actions.ToDialog))
                        {
                            if (dialogs[root.Actions.ToDialog].Nodes.Any())
                                this.fillDialogSubgraphView(dialogs[root.Actions.ToDialog], toDialogNode, localLevel + 1,
                                    ref edgeLayer, ref nodeLayer, false, dialogShower, dialogs, locales);
                            else if (dialogs[root.Actions.ToDialog].Actions.ToDialog != 0)
                                this.fillDialogSubgraphView(dialogs[root.Actions.ToDialog], toDialogNode, localLevel,
                                    ref edgeLayer, ref nodeLayer, true, dialogShower, dialogs, locales);
                        }
                    }
                }
            }
            else if (root.nextDialog > 0)
            {
                PNode node = null;
                string str_id = currentNPC + root.nextDialog.ToString();
                if (fake_nodes.ContainsKey(str_id))
                    node = fake_nodes[str_id];
                float _x = rootNode.X - 60;
                float _y = rootNode.Y + 60;
                if (this.dialogs.otherCoords.ContainsKey(str_id))
                {
                    _x = this.dialogs.otherCoords[str_id].X;
                    _y = this.dialogs.otherCoords[str_id].Y;
                }

                if (node == null)
                {
                    node = CreateFakeNode(root.nextDialog.ToString(), str_id, new PointF(_x, _y));
                    
                }
                nodeLayer.Add(node);
                PrepareNodesForEdge(node, rootNode, ref edgeLayer);
                node.Brush = GetBrushForNode(node);
            }
            else
            {

                foreach (int subdialogID in root.Nodes)
                {
                    PNode node = getNodeOnDialogID(subdialogID);
                    CDialog currentDialog = getDialogOnIDConditional(subdialogID, dialogs, locales);
                    float x = currentDialog.coordinates.X;
                    float y = currentDialog.coordinates.Y;

                    if (x == 0 && y == 0)
                    {
                        i++;
                        x = (float)(ix) + (120 * i) - 80 * root.Nodes.Count - 40 * level;
                        y = (float)(iy + 60) + 50 * level;
                    }

                    if (node == null)
                        node = CreateNode(currentDialog, new PointF(x, y));

                    PrepareNodesForEdge(node, rootNode, ref edgeLayer);
                    //SaveCoordinates(currentDialog, node);  
                    nodeLayer.Add(node);
                    if (!graphs.Keys.Contains(node))
                        graphs.Add(node, new GraphProperties(subdialogID));
                    node.Brush = GetBrushForNode(node);
                    if (!stopAfterThat)
                    {
                        if (currentDialog.Nodes.Any())
                            this.fillDialogSubgraphView(currentDialog, node, localLevel + 1, ref edgeLayer, ref nodeLayer, false, dialogShower, dialogs, locales);
                        else
                        {
                            if (currentDialog.Actions.ToDialog != 0 || currentDialog.nextDialog > 0)
                                this.fillDialogSubgraphView(currentDialog, node, localLevel, ref edgeLayer, ref nodeLayer, true, dialogShower, dialogs, locales);
                        }
                    }
                }

            }
        }

        //! Добавляет узел на граф
        private void addNodeOnDialogGraphView(int dialogID, int parentDialogID, DialogDict dialogs, NPCLocales locales, PCanvas dialogShower)
        {
            PNode parentNode = getNodeOnDialogID(parentDialogID);
            CDialog currentDialog = getDialogOnDialogID(dialogID);

            float x = parentNode.X - 60;
            float y = parentNode.Y + 60;
            PNode newNode = CreateNode(currentDialog, new PointF(x, y));

            PrepareNodesForEdge(newNode, parentNode, ref edgeLayer);
            nodeLayer.Add(newNode);

            if (!currentDialog.Actions.Exit && currentDialog.Actions.ToDialog != 0)
            {
                PNode target = getNodeOnDialogID(currentDialog.Actions.ToDialog);
                PrepareNodesForEdge(newNode, target, ref edgeLayer);
            }

            dialogShower.Layer.AddChildren(nodeLayer);

            if (!graphs.Keys.Contains(newNode))
                graphs.Add(newNode, new GraphProperties(dialogID));
            newNode.Brush = GetBrushForNode(newNode);
            if (currentDialog.Nodes.Any())
                foreach (int subdialog in currentDialog.Nodes)
                    addNodeOnDialogGraphView(subdialog, dialogID, dialogs, locales, dialogShower);
            
            // DialogDict dialogs = getDialogDictionary(currentNPC, this.dialogs.dialogs, this.dialogs.locales);
            DialogSelected(false);
        }

        private PNode CreateFakeNode(string text, string id, PointF location)
        {
            PNode newNode;
            int int_ind = 0;
            int.TryParse(text, out int_ind);
            SizeF size = CalcEllipsisSizeForNode(int_ind);
            PText ptext = new PText(text);
            newNode = PPath.CreateEllipse(location.X, location.Y, size.Height, size.Width);
            ptext.X = newNode.X;
            ptext.Y = newNode.Y + size.Height/2;
            newNode.AddChild(ptext);
            ptext.Pickable = false;
            newNode.Tag = new ArrayList();
            fake_nodes.Add(id, newNode);
            fake_node_to_dialog.Add(newNode, int.Parse(text));
            return newNode;
        }

        private PNode CreateNode(CDialog dialog, PointF location)
        {
            PNode newNode;
            SizeF size = CalcEllipsisSizeForNode(dialog.DialogID);
            PText text = new PText(dialog.DialogID.ToString());
             text.Pickable = false;
             if (dialog.isAutoNode)
             {
                 PointF[] listPoints = new PointF[4];
                 listPoints[0] = new PointF(location.X, location.Y + size.Height);
                 listPoints[1] = new PointF(location.X + size.Height, location.Y);
                 listPoints[2] = new PointF(location.X + 2 * size.Height, location.Y + size.Height);
                 listPoints[3] = new PointF(location.X + size.Height, location.Y + 2*size.Height);                 
                 newNode = PPath.CreatePolygon(listPoints);

                if (dialog.Precondition.hidden)
                {
                    float sw = size.Width;
                    float sh = size.Height * 2;
                    PNode child = PPath.CreateLine(location.X, location.Y, location.X + sh, location.Y + sh);
                    child.Pickable = false;
                    newNode.AddChild(child);
                    child = PPath.CreateLine(location.X, location.Y + sh, location.X + sh, location.Y);
                    child.Pickable = false;
                    newNode.AddChild(child);

                }


                text.X = newNode.X + 20;
                 text.Y = newNode.Y + 20;
             }
             else
             {
                 if (dialog.Precondition.Any())
                    if(dialog.Precondition.radioAvailable != RadioAvalible.None)
                    {
                        PointF[] listPoints = new PointF[8];
                        int size_angle = 8;
                        listPoints[0] = new PointF(location.X, location.Y + size_angle);
                        listPoints[1] = new PointF(location.X + size_angle, location.Y);

                        listPoints[2] = new PointF(location.X + size.Width - size_angle, location.Y);
                        listPoints[3] = new PointF(location.X + size.Width, location.Y + size_angle);

                        listPoints[4] = new PointF(location.X + size.Width, location.Y + size.Height - size_angle);
                        listPoints[5] = new PointF(location.X + size.Width - size_angle, location.Y + size.Height);

                        listPoints[6] = new PointF(location.X + size_angle, location.Y + size.Height);
                        listPoints[7] = new PointF(location.X, location.Y + size.Height - size_angle);
                        
                        newNode = PPath.CreatePolygon(listPoints);
                    }
                        
                    else
                        newNode = PPath.CreateRectangle(location.X, location.Y, size.Width, size.Height);
                 else
                     newNode = PPath.CreateEllipse(location.X, location.Y, size.Width, size.Height);
                if (dialog.Precondition.hidden)
                {
                    float sw = size.Width;
                    float sh = size.Height;
                    PNode child = PPath.CreateLine(location.X, location.Y, location.X + sw, location.Y + sh);
                    child.Pickable = false;
                    newNode.AddChild(child);
                    child = PPath.CreateLine(location.X, location.Y + sh, location.X + sw, location.Y);
                    child.Pickable = false;
                    newNode.AddChild(child);

                }
                text.X = newNode.X + 11;
                 text.Y = newNode.Y + 10;
             }

            if (dialog.CheckNodes.Any())
            {
                text.X = text.X - 3;
                text.Text = "(!)" + text.Text;
            }
            newNode.Tag = new ArrayList();
            newNode.AddChild(text);

            if ((dialog.Actions.changeMoney != 0) && (dialog.Actions.changeMoneyFailNode != 0))
            {
                PText fail_node = new PText("(" + dialog.Actions.changeMoneyFailNode.ToString() + ")");
                fail_node.Font = new Font(fail_node.Font.Name, fail_node.Font.Size - 4, fail_node.Font.Style, fail_node.Font.Unit);
                fail_node.X = text.X + 4;
                fail_node.Y = text.Y + 13;
                fail_node.Pickable = false;
                newNode.AddChild(fail_node);
            }

            return newNode;
        }

        //! Возвращает размер эллипса для Узла диалога по заданному ID диалога (дли широких надписей размер больше)
        private SizeF CalcEllipsisSizeForNode(int dialogId)
        {
            SizeF size = new SizeF(0,0);
            if (dialogId / 1000 == 0)
                size = new SizeF(50, 30);
            else if (dialogId / 1000 > 0)
                size = new SizeF(60, 40);
            CDialog dialog = getDialogOnDialogID(dialogId);
            if (dialog != null && dialog.CheckNodes.Any())
                size.Width += 7;

            return size;
        }

        //! Добавляем в теги узлов данные о гранях, в теги граней - данные об узлах
        private void PrepareNodesForEdge(PNode node1, PNode node2, ref PLayer edgeLayer)
        {
            PPath edge = new PPath();
            edge.Pickable = false;
            ((ArrayList)node1.Tag).Add(edge);
            ((ArrayList)node2.Tag).Add(edge);
            edge.Tag = new ArrayList();
            ((ArrayList)edge.Tag).Add(node1);
            ((ArrayList)edge.Tag).Add(node2);
            edgeLayer.AddChild(edge);
            updateEdge(edge);
        }

        //! Создает линии - связи между узлами на графе диалогов
        public static void updateEdge(PPath edge)
        {
            // Note that the node's "FullBounds" must be used (instead of just the "Bound") 
            // because the nodes have non-identity transforms which must be included when
            // determining their position.
            ArrayList nodes = (ArrayList)edge.Tag;
            PNode node1 = (PNode)nodes[0];
            PNode node2 = (PNode)nodes[1];
            PointF start = PUtil.CenterOfRectangle(node1.FullBounds);
            PointF end = PUtil.CenterOfRectangle(node2.FullBounds);
            edge.Reset();
            edge.AddLine(start.X, start.Y, end.X, end.Y);
        }
        
        //! Сохраняет координаты узла 
        public void SaveCoordinates(CDialog dialog, PNode node, bool isRoot)
        {
            dialog.coordinates.X = node.FullBounds.X;
            dialog.coordinates.Y = node.FullBounds.Y;
            dialog.coordinates.RootDialog = isRoot;
            //! костылек
            if (CSettings.getMode() == CSettings.MODE_LOCALIZATION)
            {
                string locale = CSettings.getCurrentLocale();
                string npc = dialog.Holder;
                NPCLocales temp;
                if (CentralDock.SelectedIndex == 2)
                    temp = CFractionDialogs.locales;
                else
                    temp = dialogs.locales;

                if (temp[CSettings.getCurrentLocale()].ContainsKey(dialog.Holder))
                {
                   
                    if (temp[locale][npc].ContainsKey(dialog.DialogID) )
                        temp[locale][npc][dialog.DialogID].coordinates.RootDialog = isRoot;
                }
            }
        }

        public void SaveOtherCoordinates(string key_id, PNode node)
        {
            if (!dialogs.otherCoords.ContainsKey(key_id))
            {
                dialogs.otherCoords.Add(key_id, new NodeCoordinates(0, 0, false, false));
            }
            dialogs.otherCoords[key_id].X = node.FullBounds.X;
            dialogs.otherCoords[key_id].Y = node.FullBounds.Y;
        }

        //! Сохраняет координаты узла со значением false для параметра isRoot
        public void SaveCoordinates(CDialog dialog, PNode node)
        {
            SaveCoordinates(dialog, node, false);
        }

        private void removeNodeFromDialogGraphView(int node, TreeView tree, DialogDict dialogs, NPCLocales locales, PCanvas dialogShower)
        {
            bool haveBeenDeleted = false;
            CDialog dialog = dialogs[node];

            foreach (KeyValuePair<int, CDialog> dial in dialogs)
            {
                dial.Value.Nodes.Remove(node);
                foreach (var i in CSettings.getListLocales())
                    locales[i][GetCurrentHolder()][dial.Value.DialogID].Nodes.Remove(node);
            }

            PNode removedNode = getNodeOnDialogID(node);

            if (removedNode != null)
            {
                foreach (PNode path in edgeLayer.AllNodes)
                    if (((ArrayList)path.Tag) != null)
                        if (((ArrayList)path.Tag).Contains(removedNode))
                        {
                            edgeLayer.RemoveChild(path);
                        }
                graphs.Remove(removedNode);
            }
            if (DialogShower.Layer.AllNodes.Contains(removedNode))
            {
                DialogShower.Layer.RemoveChild(removedNode);
                haveBeenDeleted = true;
            }
            if (haveBeenDeleted)
                removePassiveNodeFromDialogGraphView(tree, dialogs, locales, dialogShower);
        }

        private void removePassiveNodeFromDialogGraphView(TreeView tree, DialogDict dialogs, NPCLocales locales, PCanvas dialogShower)
        {
            DialogSelected(false);

            TreeNode treeNodes = treeDialogs.Nodes["Recycle"];
            foreach (TreeNode treeNode in treeNodes.Nodes)
                removeNodeFromDialogGraphView(int.Parse(treeNode.Text), tree, dialogs, locales, dialogShower);
        }

        public void selectSubNodesDialogGraphView(int dialogID)
        {
            subNodes.Clear();
            subToNodes.Clear();
            subNextNodes.Clear();

            CDialog d = getAnyDialogOnID(dialogID);
     
            foreach (int sub in d.Nodes)
            {
                PNode node = getNodeOnDialogID(sub);
                if (node != null)
                {
                    subNodes.Add(node);
                    node.Brush = GetBrushForNode(node);
                }
            }

            if (d.Actions.ToDialog != 0)
            {
                PNode node = getNodeOnDialogID(d.Actions.ToDialog);
                if (node != null)
                {
                    subToNodes.Add(node);
                    node.Brush = GetBrushForNode(node);
                }
            }

            if (d.nextDialog > 0)
            {
                string str_id = currentNPC + d.nextDialog.ToString();
                subNextNodes.Add(fake_nodes[str_id]);
                fake_nodes[str_id].Brush = GetBrushForNode(fake_nodes[str_id]);
                
            }
        }

        public void deselectSubNodesDialogGraphView()
        {
            List<PNode> nodesToClear = subNodes;
            subNodes = new List<PNode>();
            foreach (PNode subNode in nodesToClear)
                subNode.Brush = GetBrushForNode(subNode);
        }
        //! Заменяет диалог с dialogID на dialog (используется в форме редактирования диалогов)
        public void replaceDialog(CDialog dialog, int dialogID)
        {
            int index = CentralDock.SelectedIndex;
            if (index == 2)
            {
                CFractionDialogs.dialogs[currentFraction][dialogID] = dialog;
                foreach (var i in CSettings.getListLocales())
                    CFractionDialogs.locales[i][currentFraction][dialogID].InsertNonTextData(dialog);
            }
            else
            {
                dialogs.dialogs[currentNPC][dialogID] = dialog;
                foreach (var i in CSettings.getListLocales())
                    dialogs.locales[i][currentNPC][dialogID].InsertNonTextData(dialog);
            }
            this.isDirty = true;


        }
        //! Добавляет диалог в ветку (используется при добавлении диалога в форме EditDialogForm)
        public void addActiveDialog(int newID, CDialog dialog, int parentID)
        {
            // добавляем в русский словарь персонажей
            
            // добавляем в английскую локаль
            DialogDict dialogs;
            NPCLocales locales;
            PCanvas dialogShower;
            CDialog newDialog = (CDialog) dialog.Clone();
            newDialog.version -= 1;
            if (CentralDock.SelectedIndex == 2)
            {
                dialogs = getDialogDictionary(currentFraction, CFractionDialogs.dialogs, CFractionDialogs.locales);
                locales = CFractionDialogs.locales;
                foreach (var i in CSettings.getListLocales())
                {
                    locales[i][currentFraction].Add(newID, newDialog);
                    locales[i][currentFraction][parentID].Nodes.Add(newID);
                }
                dialogShower = fractionDialogShower;
            }
            else
            {
                dialogs = getDialogDictionary(GetCurrentNPC(), this.dialogs.dialogs, this.dialogs.locales);
                locales = this.dialogs.locales;
                foreach (var i in CSettings.getListLocales())
                {
                    locales[i][currentNPC].Add(newID, newDialog);
                    locales[i][currentNPC][parentID].Nodes.Add(newID);
                }
                dialogShower = DialogShower;
            }

            dialogs.Add(newID, dialog);
            dialogs[parentID].Nodes.Add(newID);
            this.isDirty = true;
            addNodeOnDialogGraphView(newID, parentID, dialogs, locales, dialogShower);
        }
        public void addPassiveDialog(int parentID, int dialogID, DialogDict dialogs, NPCLocales locales, PCanvas dialogShower)
        {
            if (CentralDock.SelectedIndex == 2)
                CFractionDialogs.dialogs[currentFraction][parentID].Nodes.Add(dialogID);
            else
                this.dialogs.dialogs[currentNPC][parentID].Nodes.Add(dialogID);
            addNodeOnDialogGraphView(dialogID, parentID, dialogs, locales, dialogShower);
        }

        public void DialogSelected(bool withGraph)
        {
            CDialog root = new CDialog();
            int dialogID = 0;
            if (Listener.SelectedNode != null)
                dialogID = getDialogIDOnNode(Listener.SelectedNode);
            DialogDict dialogs;
            NPCLocales locales;
            PCanvas dialogShower;
            TreeView tree;
            if (CentralDock.SelectedIndex == 2)
            {
                dialogs = CFractionDialogs.dialogs[currentFraction];
                locales = CFractionDialogs.locales;
                dialogShower = fractionDialogShower;
                tree = treeFractionDialogs;
                if (dialogID > 0 && (!dialogs.ContainsKey(dialogID)))
                {
                    root = getRootDialog(dialogs);
                    dialogID = root.DialogID;
                }
            }
            else
            {
                dialogs = this.dialogs.dialogs[currentNPC];
                locales = this.dialogs.locales;
                dialogShower = DialogShower;
                tree = treeDialogs;
            }

            root = getRootDialog(dialogs);
            root = getDialogOnIDConditional( root.DialogID, dialogs, locales);
            fillDialogTree(root, dialogs, tree, locales);
            if (withGraph)
            {
                graphs = new Dictionary<PNode, GraphProperties>();
                this.fillDialogGraphView(root, dialogShower, dialogs, locales);
            }
            if (dialogID > 0)
                Listener.SelectCurrentNode(dialogID);
        }

        public void DrawRectangles()
        {
            Dictionary<int, CRectangle> rects = new Dictionary<int, CRectangle>();
            rects = RectManager.GetRectanglesForNpc(GetCurrentNPC());
            drawingLayer.RemoveAllChildren();
            if (CentralDock.SelectedIndex == 2) return;
            foreach (CRectangle rect in rects.Values)
            {
                PPath newRect = PPath.CreateRectangle(rect.coordX, rect.coordY, rect.Width, rect.Height);
                newRect.Tag = RectManager.SetUniqueTag(rect.GetID());
                newRect.Pen = new Pen(rect.RectColor);
                PText rectText = new PText(rect.GetText());
                rectText.Bounds = newRect.Bounds;
                rectText.Pickable = false;
                newRect.AddChild(rectText);
                drawingLayer.AddChild(newRect);
            }
        }

        public void DeselectRectangles()
        {
            PNodeList rectList = drawingLayer.ChildrenReference;
            for (int i = 0; i < rectList.Count; i++)
                rectList[i].Brush = Brushes.White;
        }


        public string getKeyByFakeNode(PNode node)
        {
            foreach(var i in fake_nodes)
            {
                if (i.Value == node) return i.Key;
            }
            return "";
        }

        public void onClick_FakeNode(string str_id, PNode node)
        {
            int dialogID = fake_node_to_dialog[node];
            findDialogByID(dialogID);
        }
    }
}
