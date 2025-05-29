using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Drawing;
using System.Xml;
using Pfim;
using System.Drawing.Imaging;
using System;
using CSharpImageLibrary;

namespace SOHelpEditor
{
    class HelpDataLoader
    {

        protected static string JSON_PATH = "../../../res/scripts/client/data/help_cards.json";
        protected static string PICTURES_JSON_PATH = "../../../res/scripts/client/data/help_images.json";
        protected static string LOCAL_XML_PATH = "../../../res/local/Russian/Help.xml";
        public static string PICTURES_SAVE_PATH = "../soGUI/maps/help/";

        protected Dictionary<string, ImageToken> imageData;
        protected XmlDocument localXmlDocument;
        public RootObject helpData;
        protected ImagesObject imagesData;

        public HelpDataLoader()
        {
            loadData();
        }

        public Image getImage(string name)
        {
            return imageData[name].image;
        }

        public void setImage(string name, Image image, string path = "")
        {
            if (imageData.ContainsKey(name))
                imageData[name].image = image;
            else
            {
                ImageToken imageToken = new ImageToken();
                imageToken.image = image;
                imageToken.path = path;
                imageData.Add(name, imageToken);
            }
            
        }

        public void clear_locale_nodes(int node_id)
        {
            foreach (XmlNode childNode in localXmlDocument.DocumentElement.ChildNodes)
            {
                if (childNode.Name == node_id.ToString())
                {
                    childNode.RemoveAll();
                    childNode.InnerText = "";
                    return;
                }
            }
        }

        public void addText(string local_path, string tag_name, string text)
        {
            XmlElement xnode = localXmlDocument.DocumentElement;
            XmlElement node = xnode;
            List<string> local_path_list = new List<string>(local_path.Split('.'));
            int index = 0;
            foreach (string key in local_path_list)
            {
                bool found_node = false;
                foreach (XmlElement childNode in node.ChildNodes)
                {
                    if (childNode.Name == key)
                    {
                        found_node = true;
                        node = childNode;
                    }
                }
                if (!found_node)
                {
                    XmlElement child;
                    if (index == local_path_list.Count-1)
                    {
                        child = localXmlDocument.CreateElement(tag_name);
                        node.AppendChild(child);
                        child.InnerText = text;
                        return;
                    }
                    child = localXmlDocument.CreateElement(key);
                    node.AppendChild(child);
                    node = child;
                }
                else
                {
                    if (index == local_path_list.Count-1)
                    {
                        node.InnerText = text;
                        return;
                    }
                }
                
                index++;
            }
        }

        public string getText(string local_path)
        {
            XmlElement xnode = localXmlDocument.DocumentElement;
            XmlElement node = xnode;
            List<string> local_path_list = new List<string>(local_path.Split('.'));
            int index = 0;
            foreach (string key in local_path_list)
            {
                foreach(XmlElement childNode in node.ChildNodes)
                {
                    if (childNode.Name == key)
                    {
                        if (index == local_path_list.Count - 1)
                        {
                            return childNode.InnerText;
                        }
                        node = childNode;
                        break;
                    }
                }
                index++;
            }
            return local_path;
        }

        public bool removeText(string local_path)
        {
            XmlElement xnode = localXmlDocument.DocumentElement;
            XmlElement node = xnode;
            List<string> local_path_list = new List<string>(local_path.Split('.'));
            int index = 0;
            foreach (string key in local_path_list)
            {
                foreach (XmlElement childNode in node.ChildNodes)
                {
                    if (childNode.Name == key)
                    {
                        if (index == local_path_list.Count - 1)
                        {
                            node.RemoveChild(childNode);
                            return true;
                        }
                        node = childNode;
                        break;
                    }
                }
                index++;
            }
            return false;
        }
        
        public bool clearDataByID(int id)
        {
            return helpData.clearDataByID(id);
        }

        public void removeDataByID(int id)
        {
            List<string> image_names, text_names;
            helpData.removeChapterByID(id, out image_names, out text_names);

            //Удаляем картинки
            foreach(string img_name in image_names)
            {
                string path = "../../../res/soGUI/" + imageData[img_name].path;
                if (File.Exists(path)) File.Delete(path);
                imageData.Remove(img_name);
            }

            //Удаляем текста
            foreach(string text_name in text_names)
            {
                removeText(text_name);
            }

            saveData();
        }

        public void addTokenCard(int id, Token token)
        {
            int charapter_id = id / 100 * 100;
            foreach (Chapter charapter in helpData.chapters)
            {
                if (charapter.id != charapter_id)
                    continue;
                if (charapter_id == id)
                {
                    charapter.full_card.Add(token);
                    return;
                }
                else
                {
                    if (charapter.cards == null)
                        charapter.cards = new List<Card>();
                    foreach (Card card in charapter.cards)
                    {
                        if (card.id == id)
                        {
                            if (card.full_card == null)
                                card.full_card = new List<Token>();
                            card.full_card.Add(token);
                            return;
                        }
                    }
                }
            }
        }

        public void loadData()
        {
            if (!File.Exists(JSON_PATH))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка загрузки данных помощи:" + JSON_PATH);
                return;
            }          
            helpData = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(JSON_PATH));
            foreach(var i in helpData.chapters)
            {
                Console.WriteLine("Глава: " + i.id.ToString() + ":" + i.cards.Count.ToString());
                foreach(var j in i.cards)
                {
                    Console.WriteLine("\t" + j.id.ToString());
                }
                
            }
            loadImages();
            loadText();
        }

        public void loadImages()
        {
            if (!File.Exists(PICTURES_JSON_PATH))
            {
                System.Windows.Forms.MessageBox.Show("Ошибка загрузки данных картинок:" + PICTURES_JSON_PATH);
                return;
            }
            imageData = new Dictionary<string, ImageToken>();
            StreamReader reader = new StreamReader(PICTURES_JSON_PATH, Encoding.UTF8);
            imagesData = JsonConvert.DeserializeObject<ImagesObject>(reader.ReadToEnd());
            foreach (SOImage img in imagesData.images)
            {
                ImageToken imageToken = new ImageToken();
                imageToken.image = loadImage("../../../res/soGUI/" + img.path);
                imageToken.path = img.path;
                imageData.Add(img.name, imageToken);
            }
            reader.Close();


        }

        public Image loadImage(string path)
        {
            Image image;
            try
            {
                IImage img = Pfim.Pfim.FromFile(path);
                PixelFormat format;
                switch (img.Format)
                {
                    case Pfim.ImageFormat.Rgb24:
                        format = PixelFormat.Format24bppRgb;
                        break;

                    case Pfim.ImageFormat.Rgba32:
                        format = PixelFormat.Format32bppArgb;
                        break;

                    default:
                        throw new Exception("Format not recognized");
                }
                unsafe
                {
                    fixed (byte* p = img.Data)
                    {
                        var bitmap = new Bitmap(img.Width, img.Height, img.Stride, format, (IntPtr)p);
                        image = bitmap;
                    }
                }
            }
            catch
            {
                using (var bmpTemp = Image.FromFile(path))
                {
                    image = new Bitmap(bmpTemp);
                }
            }
            return image;
        }

        protected void saveImage(Image image, string path)
        {
            string image_path = "../../../res/soGUI/" + path;
            //CSharpImageLibrary.DDS.DDSGeneral.
            //ImageEngineFormat.DDS_DXT1;
            if (!File.Exists(image_path))
                File.Delete(image_path);
            image.Save(image_path, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void loadText()
        {
            localXmlDocument = new XmlDocument();
            localXmlDocument.Load(LOCAL_XML_PATH);
        }

        public void saveData()
        {
            //Данные
            string json_string = JsonConvert.SerializeObject(helpData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(JSON_PATH, json_string);

            //Локализация
            localXmlDocument.Save(LOCAL_XML_PATH);

            imagesData.images.Clear();
            //Картинки
            foreach (KeyValuePair<string, ImageToken> value in imageData)
            {
                SOImage image = new SOImage();
                image.name = value.Key;
                image.path = value.Value.path;
                imagesData.images.Add(image);

                saveImage(value.Value.image, image.path);
            }

            json_string = JsonConvert.SerializeObject(imagesData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(PICTURES_JSON_PATH, json_string);
        }
    }
}
