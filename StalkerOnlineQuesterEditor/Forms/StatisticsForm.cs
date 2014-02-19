﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StalkerOnlineQuesterEditor
{
    //! Форма отображения статистики по квестам и персонажам
    public partial class StatisticsForm : Form
    {
        //! Ссылка на родительскую главную форму
        MainForm parent;
        //! Общее число NPC 
        int NPCCount = 0;
        //! Экземпляр класса CQuests, хранящий всю инфу по квестам
        CQuests quests;
        //! Экземпляр класса CDialogs, хранящий всю инфу по диалогам
        CDialogs dialogs;

        //! Конструктор, получает элементы от главной формы
        public StatisticsForm(MainForm parent, int _NPCCount, CQuests _quests, CDialogs _dialogs)
        {
            InitializeComponent();
            this.parent = parent;
            NPCCount = _NPCCount;
            quests = _quests;
            dialogs = _dialogs;
            calcStatistic();
        }

        //! Заполняет статистику для формы
        void calcStatistic()
        {
            string str = "";

            float Credits = new float();
            int countOfQuests = 0;
            int countOfAmountGold = 0;
            int countOfDialogs = 0;
            int countOfTitleLetters = 0;
            int countOfTextLetters = 0;
            int countOfTitleNoSpaces = 0;
            int countOfTextNoSpaces = 0;
            int NpcDialogs = NPCCount;//parent.NPCBox.Items.Count;
            const int NumExp = 3;
            int[] countOfExQuests = { 0, 0, 0 };
            List<int> lExperience = new List<int>(3);
            float[] averageExp = { 0, 0, 0 };
            lExperience.Add(0); lExperience.Add(0); lExperience.Add(0);
            foreach (CQuest quest in quests.quest.Values)
            {
                if (quest.Additional.IsSubQuest == 0)
                {
                    countOfQuests++;
                    if (quest.Reward.Expirience.Any())
                    {
                        for (int i = 0; i < NumExp; i++)
                            if (quest.Reward.Expirience[i] != 0)
                                countOfExQuests[i]++;
                    }
                    if (quest.Reward.Credits != 0)
                        countOfAmountGold++;
                }
                Credits += quest.Reward.Credits;

                if (quest.Reward.Expirience.Any())
                    for (int i = 0; i < NumExp; i++)
                        lExperience[i] += quest.Reward.Expirience[i];
            }
            for (int i = 0; i < NumExp; i++)
                averageExp[i] = lExperience[i] / countOfExQuests[i];

            foreach (string npc in dialogs.dialogs.Keys)
            {
                foreach (int id in dialogs.dialogs[npc].Keys)
                {
                    countOfDialogs++;
                    countOfTextLetters += dialogs.dialogs[npc][id].Text.Length;
                    countOfTitleLetters += dialogs.dialogs[npc][id].Title.Length;
                    countOfTextNoSpaces += dialogs.dialogs[npc][id].Text.Replace(" ", "").Length;
                    countOfTitleNoSpaces += dialogs.dialogs[npc][id].Title.Replace(" ", "").Length;
                }
            }

            str += "Общее количество NPC:        " + NpcDialogs.ToString() + "\n";
            str += "Общее количество квестов:    " + countOfQuests.ToString() + "\n";
            str += "Общее количество диалогов:   " + countOfDialogs.ToString() + "\n";
            str += "Общее количество знаков в словах NPC:   " + countOfTextLetters.ToString() + 
                                            ", без пробелов: " + countOfTextNoSpaces.ToString() + "\n";
            str += "Общее количество знаков в словах ГГ:   " + countOfTitleLetters.ToString() +
                                            ", без пробелов: " + countOfTitleNoSpaces.ToString() + "\n\n";
            str += "По наградам:\n";
            str += "Общее количество денег:         ";
            str += (Credits.ToString() + " руб." + "   Среднее: " + (Credits / countOfAmountGold).ToString() + " руб.\n");
            str += "   Общее количество опыта:\n";
            str += "\aБоевого:         ";
            if (countOfExQuests[0] != 0 || lExperience[0] != 0)
                str += (lExperience[0].ToString() + "   Среднее: " + averageExp[0].ToString() + "\n");
            str += "\aВыживания:         ";
            if (countOfExQuests[1] != 0 || lExperience[1] != 0)
                str += (lExperience[1].ToString() + "   Среднее: " + averageExp[1].ToString() + "\n");
            str += "\aПоддержка:         ";
            if (countOfExQuests[2] != 0 || lExperience[2] != 0)
                str += (lExperience[2].ToString() + "   Среднее: " + averageExp[2].ToString() + "\n");
            labelInfo.Text = str;
        }

        //! Нажатие ОК - закрытие формы
        private void bOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
