using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StalkerOnlineQuesterEditor
{
    public class WordLocation
    {
        public int index;
        public int len;

        public WordLocation(int index, int len)
        {
            this.index = index; this.len = len;
        }
    }

    public static class TextUtils
    {

        static TextBox textbox = new TextBox();
        static bool inited = false;

        static void init()
        {
            inited = true;
            textbox.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ru-RU");
            textbox.SpellCheck.IsEnabled = true;
        }
            

        public static void findTextErrors(System.Windows.Forms.RichTextBox rtb)
        {
            if (!CSettings.hasErrorFinder())
            {
                int tmp = rtb.SelectionStart;
                rtb.Select(0, rtb.Text.Length);
                rtb.SelectionColor = Color.Black;
                rtb.Select(tmp, 0);
                
                return;
            }
            if (!inited) init();
            if (rtb == null) return;
            string text = rtb.Text;
            textbox.Text = text;
            //textbox.SpellCheck.CustomDictionaries.Add(new Uri(@"ru-RU.dic", UriKind.Relative));
            int index = 0;
            List<WordLocation> result = new List<WordLocation>();
            while (true)
            {
                //находим ошибку            
                index = textbox.GetNextSpellingErrorCharacterIndex(index, System.Windows.Documents.LogicalDirection.Forward);
                if (index > text.Length || index < 0) break;

                var error = textbox.GetSpellingError(index);
                int len = textbox.GetSpellingErrorLength(index);

                result.Add(new WordLocation(index, len));
                /*
                string word = textbox.Text.Substring(index, len);

                sb.AppendFormat("Ошибка в слове {0}, рекомендуется заменить на одно из следующих слов: ", word);
                */
                //переход к следующему слову
                index += len;
            }

            index = rtb.SelectionStart;

            rtb.Select(0, text.Length);
            rtb.SelectionColor = Color.Black;

            foreach (var i in result)
            {
                rtb.Select(i.index, i.len);
                rtb.SelectionColor = Color.DarkRed;
            }


            rtb.Select(index, 0);
            rtb.SelectionColor = Color.Black;
        }



    }
}
