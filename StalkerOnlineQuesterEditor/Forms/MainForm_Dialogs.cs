﻿using System;
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
        CDialog getRootDialog()
        {
            DialogDict Dialogs = getDialogDictionary(currentNPC); //this.dialogs.dialogs[currentNPC];
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
                            string loc = settings.getCurrentLocale();
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
                                dialogs.locales[settings.getCurrentLocale()][currentNPC].Add(toadd.DialogID, toadd);
                            }
                            else
                                dialogs.locales[settings.getCurrentLocale()][currentNPC][dialog.DialogID].coordinates.RootDialog = true;
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
                return dialogs.dialogs[currentNPC][dialogID];
            else
                return null;
        }

        public List<int> getAllDialogsIDonCurrentNPC()
        {
            return dialogs.dialogs[currentNPC].Keys.ToList<int>();
        }

        //! Возвращает диалог по ID в зависимости от режима и локализации
        public CDialog getDialogOnIDConditional(int dialogID)
        {
            if (dialogID != 0)
            {
                if (settings.getMode() == settings.MODE_EDITOR)
                    return dialogs.dialogs[currentNPC][dialogID];
                else
                {
                    CDialog dd = new CDialog();
                    dd = dialogs.getLocaleDialog(dialogID, settings.getCurrentLocale(), currentNPC);
                    if (dd != null)
                        return dd;
                    else
                    {
                        dd = (CDialog) dialogs.dialogs[currentNPC][dialogID].Clone();
                        dd.version = 0;
                        return dd;
                    }
                }
            }
            else
                return null;
        }

        //! Возвращает словарь диалогов одного NPC в зависимости от локализации
        private DialogDict getDialogDictionary(string NPCName)
        {
            if (settings.getMode() == settings.MODE_EDITOR)
                return dialogs.dialogs[NPCName];
            else
            { 
                if (dialogs.locales[ settings.getCurrentLocale() ].ContainsKey( NPCName ) )
                    return dialogs.locales[ settings.getCurrentLocale() ][NPCName];
                else
                    return dialogs.dialogs[NPCName];
            }
        }

        //! Возвращает ID для нового диалога
        public int getDialogsNewID()
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
                JObject json = JObject.Parse(html);
                int new_dialog_id = (int)json["qdialog_id"];
                return new_dialog_id;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка получения нового ID диалога. Проверьте своё подключение к hz-dev", "Ошибка");
            }
            
            //Старый способ, если не получилось подключиться
            List<int> availableID = new List<int>();
            foreach (Dictionary<int, CDialog> pairDialog in dialogs.dialogs.Values)
                foreach (CDialog dial in pairDialog.Values)
                    availableID.Add(dial.DialogID);

            for (int i = 1; ; i++)
                if (!availableID.Contains(i))
                    return i;
        }

        //**********************WORK WITH FORM ****************************************************

        private void fillDialogTree(CDialog root, DialogDict dialogs)
        {
            this.treeDialogs.Nodes.Clear();//tree clear
            this.treeDialogs.Nodes.Add("Active", "Active");
            this.treeDialogs.Nodes.Add("Recycle", "Recycle");
            foreach (TreeNode treeNode in this.treeDialogs.Nodes.Find("Active", true))
                treeNode.Nodes.Add(root.DialogID.ToString(), root.DialogID.ToString());
            this.fillNPCBoxSubquests(root);

            TreeNode treeActiveNode = this.treeDialogs.Nodes.Find("Active", true)[0];
            TreeNode treeRecycleNode = this.treeDialogs.Nodes.Find("Recycle", true)[0];
            foreach (CDialog dialog in dialogs.Values)
                if (!treeActiveNode.Nodes.ContainsKey(dialog.DialogID.ToString()))
                {
                    treeRecycleNode.Nodes.Add(dialog.DialogID.ToString(), dialog.DialogID.ToString());
                    dialog.coordinates.Active = false;
                    setNonActiveDialog(dialog.Holder, dialog.DialogID);
                }

            this.treeDialogs.ExpandAll();
        }
        //! Удаляет диалог из локализаций при его удалении из русской части диалогов
        private void setNonActiveDialog(string holder, int id)
        {
            dialogs.locales[settings.getListLocales()[0]][holder][id].coordinates.Active = false;
        }

        //! Заполняет граф диалога нужными узлами
        private void fillDialogGraphView(CDialog root)
        {
            // Initialize, and create a layer for the edges (always underneath the nodes)
            this.DialogShower.Layer.RemoveAllChildren();
            this.DialogShower.Camera.RemoveAllChildren();
            nodeLayer = new PNodeList();
            edgeLayer = new PLayer();
            drawingLayer = new PLayer();

            DialogShower.Camera.AddChild(drawingLayer);                      
            DialogShower.Camera.AddChild(edgeLayer);
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
            rootNode.Brush = Brushes.Green;
            nodeLayer.Add(rootNode);
            if (!graphs.Keys.Contains(rootNode))
                graphs.Add(rootNode, new GraphProperties(root.DialogID));
            SaveCoordinates(root, rootNode, true);
            this.fillDialogSubgraphView(root, rootNode, 1, ref edgeLayer, ref nodeLayer, false);

            this.DialogShower.Camera.AddChildren(nodeLayer);            
        }

        //! @brief Отображает все дочерние узлы на графе диалогов 
        //! @param root Старший диалог, экземпляр CDialog
        //! @param rootNode Старший узел, экземпляр PNode
        //! @param level Уровень наследования узлов
        //! @param edgeLayer
        //! @param nodeLayer
        //! @param stopAfterThat
        private void fillDialogSubgraphView(CDialog root, PNode rootNode, float level, ref PLayer edgeLayer, ref PNodeList nodeLayer, bool stopAfterThat)
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
                            if ( dialogs.dialogs[currentNPC][root.Actions.ToDialog].Nodes.Any() )
                                this.fillDialogSubgraphView(this.dialogs.dialogs[currentNPC][root.Actions.ToDialog], toDialogNode, localLevel + 1, ref edgeLayer, ref nodeLayer, false);
                            else if ( dialogs.dialogs[currentNPC][root.Actions.ToDialog].Actions.ToDialog != 0 )
                                this.fillDialogSubgraphView(this.dialogs.dialogs[currentNPC][root.Actions.ToDialog], toDialogNode, localLevel, ref edgeLayer, ref nodeLayer, true);
                        }
                    }
                }
            }
            else
                foreach (int subdialogID in root.Nodes)
                {
                    PNode node = getNodeOnDialogID(subdialogID);
                    CDialog currentDialog = getDialogOnIDConditional(subdialogID);
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
                    SaveCoordinates(currentDialog, node);  
                    nodeLayer.Add(node);
                    if (!graphs.Keys.Contains(node))
                        graphs.Add(node, new GraphProperties(subdialogID));
                    if (!stopAfterThat)
                    {
                        if ( currentDialog.Nodes.Any() )
                            this.fillDialogSubgraphView(currentDialog, node, localLevel + 1, ref edgeLayer, ref nodeLayer, false);
                        else if ( currentDialog.Actions.ToDialog != 0 )
                            this.fillDialogSubgraphView(currentDialog, node, localLevel, ref edgeLayer, ref nodeLayer, true);
                    }
                }
        }

        //! Добавляет узел на граф
        private void addNodeOnDialogGraphView(int dialogID, int parentDialogID)
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

            DialogShower.Layer.AddChildren(nodeLayer);

            if (!graphs.Keys.Contains(newNode))
                graphs.Add(newNode, new GraphProperties(dialogID));

            if (currentDialog.Nodes.Any())
                foreach (int subdialog in currentDialog.Nodes)
                    addNodeOnDialogGraphView(subdialog, dialogID);

            DialogSelected(false);
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
                 text.X = newNode.X + 11;
                 text.Y = newNode.Y + 10;
             }
            newNode.Tag = new ArrayList();
            newNode.AddChild(text);

            if ((dialog.Actions.changeMoney != 0) && (dialog.Actions.changeMoneyFailNode != 0))
            {
                PText fail_node = new PText("(" + dialog.Actions.changeMoneyFailNode.ToString() + ")");
                fail_node.Font = new Font(fail_node.Font.Name, fail_node.Font.Size - 4, fail_node.Font.Style, fail_node.Font.Unit);
                fail_node.X = text.X + 4;
                fail_node.Y = text.Y + 13;
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
            if (settings.getMode() == settings.MODE_LOCALIZATION)
            {
                string locale = settings.getCurrentLocale();
                if (dialogs.locales[settings.getCurrentLocale()].ContainsKey(dialog.Holder))
                {
                    string npc = dialog.Holder;
                    if ( dialogs.locales[locale][npc].ContainsKey(dialog.DialogID) )
                     dialogs.locales[locale][npc][dialog.DialogID].coordinates.RootDialog = isRoot;
                }
            }
        }
        //! Сохраняет координаты узла со значением false для параметра isRoot
        public void SaveCoordinates(CDialog dialog, PNode node)
        {
            SaveCoordinates(dialog, node, false);
        }

        private void removeNodeFromDialogGraphView(int node)
        {
            bool haveBeenDeleted = false;
            CDialog dialog = this.dialogs.dialogs[currentNPC][node];

            foreach (KeyValuePair<int, CDialog> dial in dialogs.dialogs[currentNPC])
            {
                dial.Value.Nodes.Remove(node);
                dialogs.locales[settings.getListLocales()[0]][currentNPC][dial.Value.DialogID].Nodes.Remove(node);
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
                removePassiveNodeFromDialogGraphView();
        }

        private void removePassiveNodeFromDialogGraphView()
        {
            DialogSelected(false);

            TreeNode treeNodes = treeDialogs.Nodes["Recycle"];
            foreach (TreeNode treeNode in treeNodes.Nodes)
                removeNodeFromDialogGraphView(int.Parse(treeNode.Text));
        }

        public void selectSubNodesDialogGraphView(int dialogID)
        {
            subNodes.Clear();

            if (dialogs.dialogs[currentNPC][dialogID].Nodes.Any())
                foreach (int sub in dialogs.dialogs[currentNPC][dialogID].Nodes)
                {
                    PNode node = getNodeOnDialogID(sub);
                    if (node != null)
                        subNodes.Add(node);
                }

            if (dialogs.dialogs[currentNPC][dialogID].Actions.ToDialog != 0)
            {
                PNode node = getNodeOnDialogID(dialogs.dialogs[currentNPC][dialogID].Actions.ToDialog);
                if (node != null)
                    subNodes.Add(node);
            }

            if (subNodes.Any())
                foreach (PNode subNode in subNodes)
                    subNode.Brush = GetBrushForNode(subNode);
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
            dialogs.dialogs[currentNPC][dialogID] = dialog;
            dialogs.locales[settings.getListLocales()[0]][currentNPC][dialogID].InsertNonTextData(dialog);
        }
        //! Добавляет диалог в ветку (используется при добавлении диалога в форме EditDialogForm)
        public void addActiveDialog(int newID, CDialog dialog, int parentID)
        {
            // добавляем в русский словарь персонажей
            dialogs.dialogs[currentNPC].Add(newID, dialog);
            dialogs.dialogs[currentNPC][parentID].Nodes.Add(newID);
            // добавляем в английскую локаль
            CDialog newDialog = (CDialog) dialog.Clone();
            dialogs.locales[settings.getListLocales()[0]][currentNPC].Add(newID, newDialog);
            dialogs.locales[settings.getListLocales()[0]][currentNPC][parentID].Nodes.Add(newID);

            addNodeOnDialogGraphView(newID, parentID);
        }
        public void addPassiveDialog(int parentID, int dialogID)
        {
            this.dialogs.dialogs[currentNPC][parentID].Nodes.Add(dialogID);
            addNodeOnDialogGraphView(dialogID, parentID);
        }

        public void DialogSelected(bool withGraph)
        {
            CDialog root = new CDialog();
            DialogDict dialogs = getDialogDictionary(currentNPC);
            root = getRootDialog();
            root = getDialogOnIDConditional( root.DialogID );
            fillDialogTree(root, dialogs);
            if (withGraph)
            {
                graphs = new Dictionary<PNode, GraphProperties>();
                this.fillDialogGraphView(root);
            }
        }

        public void DrawRectangles()
        {
            Dictionary<int, CRectangle> rects = new Dictionary<int, CRectangle>();
            rects = RectManager.GetRectanglesForNpc(GetCurrentNPC());
            drawingLayer.RemoveAllChildren();
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
    }
}
