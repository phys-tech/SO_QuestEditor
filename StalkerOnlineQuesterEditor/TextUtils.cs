using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Markup;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Collections.Generic;


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



    public static class SpellChecker
    {
        private static Thread wpfThread;
        private static Dispatcher wpfDispatcher;
        private static TextBox textbox;
        private static bool inited = false;
        private static readonly object lockObj = new object();

        private static bool check = false;

        public static void Init()
        {
            lock (lockObj)
            {
                if (inited) return;

                var dispatcherReady = new ManualResetEvent(false);

                wpfThread = new Thread(() =>
                {
                    wpfDispatcher = Dispatcher.CurrentDispatcher;
                    dispatcherReady.Set();
                    Dispatcher.Run(); // блокирующий вызов
                });

                wpfThread.SetApartmentState(ApartmentState.STA);
                wpfThread.IsBackground = true;
                wpfThread.Start();

                dispatcherReady.WaitOne(); // ждём, пока dispatcher готов

                var textboxReady = new ManualResetEvent(false);

                wpfDispatcher.Invoke(() =>
                {
                    textbox = new TextBox();
                    textbox.SpellCheck.IsEnabled = true;
                    textbox.Language = XmlLanguage.GetLanguage("ru-RU");

                    textboxReady.Set();
                });

                textboxReady.WaitOne();
                inited = true;
            }
        }

        public static List<WordLocation> GetSpellingErrors(string text)
        {
            if (!inited) Init();

            var result = new List<WordLocation>();

            wpfDispatcher.Invoke(() =>
            {
                textbox.Text = text ?? string.Empty;

                int index = 0;
                while (true)
                {
                    index = textbox.GetNextSpellingErrorCharacterIndex(index, LogicalDirection.Forward);
                    if (index < 0 || index >= textbox.Text.Length) break;

                    int len = textbox.GetSpellingErrorLength(index);
                    result.Add(new WordLocation(index, len));
                    index += len;
                }
            });

            return result;
        }

        public static async void CheckAndHighlightSpellingErrors(System.Windows.Forms.RichTextBox rtb)
        {
            if (rtb == null || rtb.IsDisposed || rtb.Disposing)
                return;

            check = true;

            // Получаем текст в UI-потоке
            string text = "";
            if (rtb.InvokeRequired)
            {
                try
                {
                    rtb.Invoke(new Action(() => text = rtb.Text));
                }
                catch { return; };
            }
            else
            {
                text = rtb.Text;
            }

            // Проверка орфографии в фоновом потоке
            var errors = await Task.Run(() =>
            {
                SpellChecker.Init(); // безопасно повторно
                return SpellChecker.GetSpellingErrors(text);
            });

            // Подсветка ошибок — только в UI потоке
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new Action(() => HighlightErrors(rtb, errors)));
            }
            else
            {
                HighlightErrors(rtb, errors);
            }
        }

        private static void HighlightErrors(System.Windows.Forms.RichTextBox rtb, List<WordLocation> errors)
        {
            if (rtb == null || rtb.IsDisposed || rtb.Disposing)
                return;

            if (!check) return;

            int selStart = rtb.SelectionStart;

            rtb.Select(0, rtb.Text.Length);
            rtb.SelectionColor = System.Drawing.Color.Black;

            foreach (var err in errors)
            {
                rtb.Select(err.index, err.len);
                rtb.SelectionColor = System.Drawing.Color.DarkRed;
            }

            rtb.Select(selStart, 0);
            rtb.SelectionColor = System.Drawing.Color.Black;
            check = false;
        }
    }






    public static class TextUtils
    {

       private async static void CheckSpellingAsync(System.Windows.Forms.RichTextBox rtb)
        {
            string text = rtb.Text;

            // Получаем ошибки в фоновом потоке
            var errors = await Task.Run(() => SpellChecker.GetSpellingErrors(text));

            // Назначаем подсветку — обязательно в UI-потоке
            ApplyTextErrors(rtb, errors);
        }

        private static void ApplyTextErrors(System.Windows.Forms.RichTextBox rtb, List<WordLocation> errors)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new Action(() => ApplyTextErrors(rtb, errors)));
                return;
            }

            int sel = rtb.SelectionStart;
            rtb.Select(0, rtb.Text.Length);
            rtb.SelectionColor = System.Drawing.Color.Black;

            foreach (var err in errors)
            {
                rtb.Select(err.index, err.len);
                rtb.SelectionColor = System.Drawing.Color.DarkRed;
            }

            rtb.Select(sel, 0);
            rtb.SelectionColor = System.Drawing.Color.Black;
        }

    }
}
