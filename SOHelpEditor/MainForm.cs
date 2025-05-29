using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Net.Sgoliver.NRtfTree.Core;


namespace SOHelpEditor
{
    public partial class MainForm : Form
    {
        HelpDataLoader dataLoader = new HelpDataLoader();
        Dictionary<int, List<Token>> data = new Dictionary<int, List<Token>>();
        int selectedIndex = -1;
        enum STATUS { None, subTitle, Image, Text, Pre };
        LittleCardForm little_card_form;
        InputForm input;

        public MainForm()
        {
            InitializeComponent();
        }

        private string getInputText(string text = "")
        {
            if (input != null) input.Close();
            input = new InputForm(text);
            input.ShowDialog();
            return input.getText();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach(Chapter chapter in dataLoader.helpData.chapters)
            {
                TreeNode node = new TreeNode(dataLoader.getText(chapter.name));
                node.Tag = chapter.id;
                Console.WriteLine("Видим Глава " + chapter.id.ToString() + ":" + chapter.cards.Count);
                int chapter_id = chapter.id;
                data.Add(chapter_id, new List<Token>());
                treeChapters.Nodes.Add(node);
                if (chapter.full_card != null)
                {
                    foreach (Token t in chapter.full_card)
                    {
                        Token tmp = new Token();
                        tmp.type = t.type;
                        tmp.value = t.value;
                        data[chapter_id].Add(tmp);
                    }
                }
                if (chapter.cards != null)
                {
                    foreach(Card card in chapter.cards)
                    {
                        Console.WriteLine(card.id);
                        TreeNode subNode = new TreeNode(dataLoader.getText(card.title));
                        subNode.Tag = card.id;
                        node.Nodes.Add(subNode);
                        data.Add(card.id, new List<Token>());
                        if (card.full_card == null) card.full_card = new List<Token>();
                        foreach (Token t in card.full_card)
                        {
                            Token tmp = new Token();
                            tmp.type = t.type;
                            tmp.value = t.value;
                            data[card.id].Add(tmp);
                        }
                    }
                }


            }
        }

        public void pasteTitle(string text)
        {
            rtbText.SelectionIndent = 10;
            Font font = new Font(rtbText.SelectionFont.FontFamily, 18, FontStyle.Bold, GraphicsUnit.Pixel);
            Font old_font = rtbText.SelectionFont;
            rtbText.SelectionFont = font;
            rtbText.AppendText(dataLoader.getText(text) + "\n");
            rtbText.SelectionIndent = 0;
            rtbText.SelectionFont = old_font;
        }

        public void pastePre(string text, bool in_end = false)
        {
            Clipboard.Clear();
            rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            //rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;

            Color old = rtbText.SelectionColor;
            Font old_font = rtbText.SelectionFont;
            Font subtitle_font = new Font(rtbText.SelectionFont.FontFamily, 14, FontStyle.Italic, GraphicsUnit.Pixel);
            rtbText.SelectionFont = subtitle_font;
            text = dataLoader.getText(text);
            if (in_end)
            {
                //rtbText.SelectionColor = Color.White;
                rtbText.AppendText("\n<pre>");
                rtbText.SelectionColor = old;
                rtbText.AppendText(text);
                //rtbText.SelectionColor = Color.White;
                rtbText.AppendText("</pre>\n\n");
            }
            else
            {
                //rtbText.SelectionColor = Color.White;
                Clipboard.SetText("\n<pre>"); rtbText.Paste();
                rtbText.SelectionColor = old;
                Clipboard.SetText(text); rtbText.Paste();
                //rtbText.SelectionColor = Color.White;
                Clipboard.SetText("</pre>\n\n"); rtbText.Paste();
            }
            rtbText.SelectionColor = old;
            rtbText.SelectionFont = old_font;
            Clipboard.Clear();
        }

        public void pasteSubTitle(string text, bool in_end = false)
        {
            rtbText.SelectionIndent = 0;
            Color old_color = rtbText.SelectionColor;
            Font old_font = rtbText.SelectionFont;
            Font subtitle_font = new Font(rtbText.SelectionFont.FontFamily, 15, FontStyle.Bold, GraphicsUnit.Pixel);
            rtbText.SelectionFont = subtitle_font;

            if (in_end)
            {
                //rtbText.SelectionColor = Color.White;
                rtbText.AppendText("\n<S>");
                rtbText.SelectionColor = old_color;
                rtbText.AppendText(dataLoader.getText(text) + "\n\n");
            }
            else
            {
                //rtbText.SelectionColor = Color.White;
                Clipboard.SetText("\n<S>");
                rtbText.Paste();
                rtbText.SelectionColor = old_color;
                Clipboard.SetText(dataLoader.getText(text) + "\n\n");
                rtbText.Paste();
            }
            rtbText.SelectionIndent = 0;
            rtbText.SelectionFont = old_font;
        }

        public void pasteImage(Image img)
        {
            Clipboard.Clear();
            Clipboard.SetImage(img);
            if (Clipboard.ContainsImage())
            {
                rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
                Color old = rtbText.SelectionColor;
                //rtbText.SelectionColor = Color.White;
                Clipboard.SetText("\n <img>");
                rtbText.Paste();
                Clipboard.Clear();
                //rtbText.AppendText("<img>");
                Clipboard.SetImage(img);
                rtbText.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
                rtbText.SelectionColor = old;
                Clipboard.SetText("\n");
                rtbText.Paste();
                rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            }
            Clipboard.Clear();
        }

        private void treeChapters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            rtbText.Text = "";
            selectedIndex = Convert.ToInt16(treeChapters.SelectedNode.Tag);

            if (data[selectedIndex].Any())
                foreach (Token token in data[selectedIndex])
                {
                    switch(token.type)
                    {
                        case "text":
                            string text = dataLoader.getText(token.value);
                            rtbText.AppendText(text+"\n"); break;
                        case "image":
                            Image img = dataLoader.getImage(token.value);
                            pasteImage(img); break;
                        case "subtitle":
                            pasteSubTitle(token.value, true); break;
                        case "pre":
                            pastePre(token.value, true); break;
                        default:
                            rtbText.AppendText(token.type + " " + token.value + "\n");
                            break;
                    }
                }
            updateButtonsEnabled();
        }

        private void updateButtonsEnabled()
        {
            btnUp.Enabled = (treeChapters.SelectedNode.PrevNode != null);
            btnDown.Enabled = (treeChapters.SelectedNode.NextNode != null);
            changeLittleCard.Enabled = (getCurrentNodeID() / 100 * 100 != getCurrentNodeID());
            labelCurrentID.Text = getCurrentNodeID().ToString();
        }

        protected void setButtonsPasteEnabled(bool value)
        {
            btnAddImage.Enabled = value;
            btnLink.Enabled = value;
            btnAddSubTitle.Enabled = value;
        }

        private void btnAddSubTitle_Click(object sender, EventArgs e)
        {
            string text = getInputText();
            if (text.Trim().Any())
                pasteSubTitle(text);
            input.Close();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp; *.dds)|*.jpg; *.jpeg; *.png; *.bmp; *.dds";
            if (DialogResult.OK != openFile.ShowDialog()) return;
            Image image = dataLoader.loadImage(openFile.FileName);
            pasteImage(image);
        }

        protected void addToken(string value, string type, int id)
        {
            Token token = new Token();
            token.type = type;
            token.value = value;
            data[id].Add(token);
            dataLoader.addTokenCard(id, token);
        }

        protected void onFoundImage(Image img, ref int index)
        {
            index += 1;
            int current_id = getCurrentNodeID();
            string img_name = "Image_" + current_id.ToString() + "_" + index.ToString() + ".png";
            string img_path = HelpDataLoader.PICTURES_SAVE_PATH + img_name;
            dataLoader.setImage(img_name, img, img_path);
            addToken(img_name, "image", current_id);
        }

        protected void onFoundText(string text, ref int index)
        {
            index += 1;
            int current_id = getCurrentNodeID();
            string local_name = "Text_" + index.ToString();
            string local_path = "ID_" + current_id.ToString() + "." + local_name;
            
            dataLoader.addText(local_path, local_name, text.Trim());
            string value = local_path;
            addToken(value, "text", current_id);
        }

        protected void onFoundPre(string text, ref int index)
        {
            index += 1;
            int current_id = getCurrentNodeID();
            string local_name = "Pre_" + index.ToString();
            string local_path = "ID_" + current_id.ToString() + "." + local_name;

            dataLoader.addText(local_path, local_name, text.Trim());
            string value = local_path;
            addToken(value, "pre", current_id);
        }


        protected void onFoundSubTitles(string text, ref int index)
        {
            index += 1;
            int current_id = getCurrentNodeID();
            string local_name = "SubTitle_" + index.ToString();
            string local_path = "ID_" + current_id.ToString() + "." + local_name;
            
            dataLoader.addText(local_path, local_name, text);
            string value = local_path;
            addToken(value, "subtitle", current_id);
        }

        protected int getCurrentNodeID()
        {
            if (this.treeChapters.SelectedNode == null)
                return -1;
            return Convert.ToInt32(this.treeChapters.SelectedNode.Tag);
        }

        protected int getLastChapterID()
        {
            int max_chapter_id = 0;
            foreach(var chapter in dataLoader.helpData.chapters)
            {
                if (chapter.id > max_chapter_id) max_chapter_id = chapter.id;
            }
            return max_chapter_id;
        }
        protected int getLastParagraph(int chapter_id)
        {
            int max_paragraph_id = chapter_id;
            foreach (var chapter in dataLoader.helpData.chapters)
            {
                if (chapter.id != chapter_id) continue;

                if (chapter.cards == null) continue;

                foreach(var card in chapter.cards)
                {
                    if (card.id > max_paragraph_id) max_paragraph_id = card.id;
                }
            }
            return max_paragraph_id;
        }

        protected void parseCurrentText()
        {
            string path = "~_" + this.selectedIndex + "_tmp.rtf";
            int current_node_id = this.getCurrentNodeID();

            dataLoader.clear_locale_nodes(current_node_id);
            dataLoader.clearDataByID(current_node_id);
            data[current_node_id] = new List<Token>();
            int text_count = 0;
            int titles_count = 0;
            int images_count = 0;

            STATUS status = STATUS.Text;
            string current_text = "";
            RtfTree tree = new RtfTree();
            tree.LoadRtfText(rtbText.Rtf);
            int index = 0;

            StreamWriter writer = new StreamWriter("1.txt");
            writer.Write(tree.ToString());
            writer.Close();

            foreach (RtfTreeNode a in tree.RootNode.ChildNodes)
            {
                foreach (RtfTreeNode b in a.ChildNodes)
                {
                    index += b.Text.Replace("\r", "").Length;

                    if (b.Text.Contains("<pre>"))
                    {
                        if ((current_text.Trim().Any()) && (status == STATUS.Text))
                            onFoundText(current_text, ref text_count);
                        status = STATUS.Pre;
                        current_text = "";
                        continue;
                    }
                    else if (b.Text.Contains("</pre>") && status == STATUS.Pre)
                    {
                        if (current_text.Trim().Any())
                            onFoundPre(current_text, ref text_count);
                        status = STATUS.Text;
                        current_text = "";
                        continue;
                    }
                    else if (b.Text.Contains("\n") && status != STATUS.Text)
                    {
                        if (status == STATUS.subTitle)
                        {
                            if (current_text.Trim().Any()) onFoundSubTitles(current_text, ref titles_count);
                            current_text = "";
                            status = STATUS.Text;
                            continue;
                        }
                    }
                   
                    else if (b.Text.Contains("<S>"))
                    {
                        if ((current_text.Trim().Any()) && (status == STATUS.Text))
                            onFoundText(current_text, ref text_count);
                        status = STATUS.subTitle;
                        current_text = "";
                        continue;
                    }
                    else if (b.Text.Contains("<img>"))
                    {
                        if ((current_text.Trim().Any()) && (status == STATUS.Text))
                            onFoundText(current_text, ref text_count);
                        status = STATUS.Text;
                        current_text = "";
                        IDataObject data = Clipboard.GetDataObject();
                        Clipboard.Clear();
                        rtbText.Select(index, 1);
                        rtbText.Copy();
                        Image img = Clipboard.GetImage();
                        onFoundImage(img, ref images_count);
                        Clipboard.Clear();
                        Clipboard.SetDataObject(data);
                        index++;
                        continue;
                    };
                    current_text += b.Text;

                }
                if (current_text.Trim().Any())
                {
                    if (status == STATUS.Text) onFoundText(current_text, ref text_count);
                    else if (status == STATUS.subTitle) onFoundSubTitles(current_text, ref titles_count);
                    current_text = "";
                }
            }
            dataLoader.saveData();
            MessageBox.Show("Сохранение завершено");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            parseCurrentText();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int current_id = getCurrentNodeID();
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить данные ветки " + current_id + " " + treeChapters.SelectedNode.Text, "Внимание", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;
            dataLoader.removeDataByID(current_id);
            data.Remove(current_id);
            treeChapters.Nodes.Remove(treeChapters.SelectedNode);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string text = getInputText();
            TreeNode node = new TreeNode(dataLoader.getText(text));
            int new_id = getLastChapterID() + 100;
            node.Tag = new_id;
            string local_name = "Title";
            string local_path = "ID_" + new_id.ToString() + "." + local_name; ;
            

            Chapter charapter = new Chapter();
            charapter.id = new_id;
            charapter.name = local_path;
           
            dataLoader.addText(local_path, local_name, text);
            dataLoader.helpData.chapters.Add(charapter);

            data.Add(new_id, new List<Token>());
            this.treeChapters.Nodes.Add(node);
            input.Close();
        }

        private void btnAddParagraph_Click(object sender, EventArgs e)
        {
            string text = getInputText();
            TreeNode node = new TreeNode(dataLoader.getText(text));
            int current_chapter_id = getCurrentNodeID() / 100 * 100;
            int new_id = getLastParagraph(current_chapter_id) + 1;
            node.Tag = new_id;
            string local_name = "Title";
            string local_path = "ID_" + new_id.ToString() + "." + local_name;
            
            dataLoader.addText(local_path, local_name, text);

            Card new_card = new Card();
            new_card.id = new_id;
            new_card.title = local_path;
            foreach (Chapter charapter in dataLoader.helpData.chapters)
            {
                if (charapter.id != current_chapter_id) continue;
                if (charapter.cards == null) charapter.cards = new List<Card>();
                charapter.cards.Add(new_card);
            }
           
            data.Add(new_id, new List<Token>());

            foreach(TreeNode tree_node in this.treeChapters.Nodes)
            {
                if (Convert.ToInt32(tree_node.Tag) == current_chapter_id)
                {
                    tree_node.Nodes.Add(node);
                    break;
                }
                    
            }
            input.Close();
        }

        private void saveLittleCard()
        {

        }

        private void changeLittleCard_Click(object sender, EventArgs e)
        {
            if (little_card_form != null) little_card_form.Close();
            int node_id = getCurrentNodeID();
            if (node_id == -1)
            {
                MessageBox.Show("Не выбран параграф");
                return;
            }
            int chapter_id = node_id / 100 * 100;
            string title = "";
            string text = "";
            Card current_paragraph = null;
            foreach (Chapter chapter in dataLoader.helpData.chapters)
            {
                if (chapter.id != chapter_id) continue;
                if (chapter_id == node_id)
                {
                    MessageBox.Show("У глав нет своих карточек");
                    return;
                }

                foreach(var paragraph in chapter.cards)
                {
                    if (paragraph.id != node_id) continue;
                    text = paragraph.little_card.value;
                    title = paragraph.title;
                    current_paragraph = paragraph;
                    break;
                }
                break;
            }
            title = dataLoader.getText(title);
            text = dataLoader.getText(text);
            little_card_form = new LittleCardForm(title, text);
            little_card_form.ShowDialog();
            string path = "ID_" + node_id.ToString() + ".little_card";
            dataLoader.addText(path, "little_card", little_card_form.text);

            current_paragraph.little_card.value = path;
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            int node_id = getCurrentNodeID();
            if (node_id == -1)
            {
                MessageBox.Show("Не выбран параграф/глава");
                return;
            }
            string text = getInputText(treeChapters.SelectedNode.Text);
            treeChapters.SelectedNode.Text = text;
            string local_path = "ID_" + node_id.ToString() + ".Title";
            dataLoader.addText(local_path, "Title", text);
            dataLoader.helpData.setElementName(node_id, local_path);
        }

        protected void moveTreeNode(TreeNode tmpNode, int changeIndex)
        {
            TreeNodeCollection tmpNodes;
            if (tmpNode.Level > 0)
                tmpNodes = tmpNode.Parent.Nodes;
            else
                tmpNodes = treeChapters.Nodes;
            int index = tmpNodes.IndexOf(tmpNode);
            tmpNodes.Remove(tmpNode);
            int new_index = Math.Max(Math.Min(index + changeIndex, tmpNodes.Count - 1), 0);
            tmpNodes.Insert(new_index, tmpNode);
            treeChapters.SelectedNode = tmpNode;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (treeChapters.SelectedNode.PrevNode == null)
                return;
            int node_id = Convert.ToInt32(treeChapters.SelectedNode.Tag);
            moveTreeNode(treeChapters.SelectedNode, -1);
            dataLoader.helpData.moveElement(node_id, -1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (treeChapters.SelectedNode.NextNode == null)
                return;
            int node_id = Convert.ToInt32(treeChapters.SelectedNode.Tag);
            moveTreeNode(treeChapters.SelectedNode, +1);
            dataLoader.helpData.moveElement(node_id, 1);
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            CreateLinkForm form = new CreateLinkForm(treeChapters.Nodes);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Clipboard.Clear();
                Clipboard.SetText(form.getText());
                rtbText.Paste();
                Clipboard.Clear();
            }
            form.Close();
        }

        private void rtbText_Leave(object sender, EventArgs e)
        {
            //setButtonsPasteEnabled(false);
        }

        private void rtbText_Enter(object sender, EventArgs e)
        {
            setButtonsPasteEnabled(true);
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            CreateCommentForm form = new CreateCommentForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                pastePre(form.getText());
            }
            form.Close();
        }

        private void menuStripItemCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbText.SelectedText);
            rtbText.SelectedText = "";
        }

        private void menuStripItemCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtbText.SelectedText);
        }

        private void menuStripItemPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
                Color old = rtbText.SelectionColor;
                //rtbText.SelectionColor = Color.White;
                Clipboard.SetText("\n <img>");
                rtbText.Paste();
                Clipboard.Clear();
                //rtbText.AppendText("<img>");
                Clipboard.SetImage(img);
                rtbText.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
                rtbText.SelectionColor = old;
                Clipboard.SetText("\n");
                rtbText.Paste();
                rtbText.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            }
            else rtbText.Paste();
        }
    }
}
