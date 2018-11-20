﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace StalkerOnlineQuesterEditor
{
    //! @todo: переделывать все нахер. 
    //! Класс констант квеста - типов. Лучше бы переделывать.
    public class СQuestConstants
    {
        List<СQuestType> ierarchyQuestsType = new List<СQuestType>();
        List<СQuestType> simpleQuestsType = new List<СQuestType>();

        public static int TYPE_FARM = 0; // собрать предметы
        public static int TYPE_FARM_AUTO = 16; // собранные предметы автоматически исчезают
        public static int TYPE_TALK = 1; // поговорить
        public static int TYPE_KILLMOBS_WITH_ONTEST = 2; // убить мобов со здачей НИП
        public static int TYPE_KILLMOBS = 3; // убить мобов
        public static int TYPE_AREA_DISCOVER = 4; // войти в зону
        public static int TYPE_MONEYBACK = 5; // заплатить НИП
        public static int TYPE_TRIGGER_ACTION = 6; // взаимодействвать с триггером
        public static int TYPE_QITEM_USE = 7; // использовать квестовый предмет
        public static int TYPE_AREA_LEAVE = 8; // покинуть зону
        public static int TYPE_TIMER = 9; // таймер
        public static int TYPE_DIE = 17; // умереть (отправиться на респ)
        public static int TYPE_GAME_EXIT = 18; // выйти из игры
        public static int TYPE_ITEM_EQIP = 19; // экипировать предмет
        public static int TYPE_ITEM_ADD = 20; // получить предмет
        public static int TYPE_GIVE_EFFECT = 21; // получить эффект
        public static int TYPE_REPUTATION = 22; // собрать количество репутации (со сдачей)
        public static int TYPE_REPUTATION_AUTO = 23; // автоматически закрывается при количестве репутации
        public static int TYPE_KILL = 24; // убить

        public static int TYPE_ITEM_CATEGORY = 25; // Собрать количество предметов категории.
        public static int TYPE_ITEM_CATEGORY_AUTO = 26; // Собрать количество предметов категории АВТО.
        public static int TYPE_HAVE_EFFECT = 27; // Находиться под действием эффекта
        public static int TYPE_IN_AREA = 28; // Находиться в зоне

        public static int TYPE_CREATE_NPC = 51; // создать бегающего НИП
        public static int TYPE_CREATE_MOB = 52; // создать моба
        public static int TYPE_KILLNPC = 53; // убить НИП созданного квестом
        public static int TYPE_KILLNPC_WITH_ONTEST = 54; // убить НИП со сдачей

        public СQuestConstants()
        {
            simpleQuestsType.Add(new СQuestType(TYPE_FARM, "0  Собрать необходимое количество предметов."));
            simpleQuestsType.Add(new СQuestType(TYPE_FARM_AUTO, "16 Собрать необходимое количество предметов авто."));
            simpleQuestsType.Add(new СQuestType(TYPE_TALK, "1  Поговорить."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLMOBS_WITH_ONTEST, "2  Убийство мобов с обязательной сдачей евента."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLMOBS, "3  Убийство мобов."));
            simpleQuestsType.Add(new СQuestType(TYPE_AREA_DISCOVER, "4  Посещение зоны."));
            simpleQuestsType.Add(new СQuestType(TYPE_MONEYBACK, "5  Сдача денег."));
            simpleQuestsType.Add(new СQuestType(TYPE_TRIGGER_ACTION, "6  Действие над триггером."));
            simpleQuestsType.Add(new СQuestType(TYPE_QITEM_USE, "7  Использовать предмет."));
            simpleQuestsType.Add(new СQuestType(TYPE_AREA_LEAVE, "8  Покинуть зону."));
            simpleQuestsType.Add(new СQuestType(TYPE_TIMER, "9  Таймер."));
            simpleQuestsType.Add(new СQuestType(TYPE_DIE, "17 Умереть."));
            simpleQuestsType.Add(new СQuestType(TYPE_GAME_EXIT, "18 Выйти из игры."));
            simpleQuestsType.Add(new СQuestType(TYPE_ITEM_EQIP, "19 Экипировка предмета."));
            simpleQuestsType.Add(new СQuestType(TYPE_ITEM_ADD, "20 Добавление предмета."));
            simpleQuestsType.Add(new СQuestType(TYPE_GIVE_EFFECT, "21 Получение эффекта."));
            simpleQuestsType.Add(new СQuestType(TYPE_REPUTATION, "22 Необходимое количество репутации."));
            simpleQuestsType.Add(new СQuestType(TYPE_REPUTATION_AUTO, "23 Необходимое количество репутации АВТО."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILL, "24 Убийство."));
            simpleQuestsType.Add(new СQuestType(TYPE_ITEM_CATEGORY, "25 Собрать количество предметов категории."));
            simpleQuestsType.Add(new СQuestType(TYPE_ITEM_CATEGORY_AUTO, "26 Собрать количество предметов категории АВТО."));
            simpleQuestsType.Add(new СQuestType(TYPE_HAVE_EFFECT, "27 Находиться под действием эффекта."));
            simpleQuestsType.Add(new СQuestType(TYPE_IN_AREA, "28 Находиться в зоне."));

            ierarchyQuestsType.Add(new СQuestType(50, "50 Игра против режиссера."));
            simpleQuestsType.Add(new СQuestType(TYPE_CREATE_NPC, "51 Создать NPC."));
            simpleQuestsType.Add(new СQuestType(TYPE_CREATE_MOB, "52 Создать Моба."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLNPC, "53 Убийство NPC."));
            simpleQuestsType.Add(new СQuestType(TYPE_KILLNPC_WITH_ONTEST, "54 Убийство NPC с обязательной сдачей евента."));

            ierarchyQuestsType.Add(new СQuestType(10,"10 Выполнение всех в любом порядке."));
            ierarchyQuestsType.Add(new СQuestType(11,"11 Выполнение всех по порядку."));
            ierarchyQuestsType.Add(new СQuestType(12,"12 Выполнение на выбор."));
            ierarchyQuestsType.Add(new СQuestType(13,"13 Выполнение всех в любом порядке со сдачей."));
            ierarchyQuestsType.Add(new СQuestType(14,"14 Выполнение всех по порядку со сдачей."));
            ierarchyQuestsType.Add(new СQuestType(15,"15 Выполнение на выбор со сдачей."));

            //Туториал
            //simpleQuestsType.Add(new СQuestType(200,"200 _экипировка предмета."));
            simpleQuestsType.Add(new СQuestType(201,"201 _выстрел."));
            simpleQuestsType.Add(new СQuestType(202,"202 _лечение."));
            simpleQuestsType.Add(new СQuestType(203,"203 _восстановление предмета."));
            simpleQuestsType.Add(new СQuestType(204,"204 _фонарик."));
            //simpleQuestsType.Add(new СQuestType(205,"205 _добавление предмета."));
            simpleQuestsType.Add(new СQuestType(206,"206 _событие ГУИ."));



        }
        public bool isSimple(int questType)
        {
            foreach(СQuestType quest in simpleQuestsType)
                if (quest.getType().Equals(questType))
                    return true;
            return false;
        }

        public string getDescription(int questType)
        {
            foreach (СQuestType quest in simpleQuestsType)
                if (quest.getType().Equals(questType))
                    return quest.getDescription();

            foreach (СQuestType quest in ierarchyQuestsType)
                if (quest.getType().Equals(questType))
                    return quest.getDescription();
            return "";
        }

        public List<СQuestType> getListQuests()
        {
            List<СQuestType> ret = new List<СQuestType>();
            ret.AddRange(ierarchyQuestsType);
            ret.AddRange(simpleQuestsType);
            return ret;
        }

        public int getQuestTypeOnDescription(string description)
        {
            foreach (СQuestType type in simpleQuestsType)
                if (type.getDescription().Equals(description))
                    return type.getType();
            foreach (СQuestType type in ierarchyQuestsType)
                if (type.getDescription().Equals(description))
                    return type.getType();
            return 0;
        }
    }

    public class СQuestType
    {
        int QuestType;
        string Description;


        public СQuestType(int typeID, string Description)
        {
            this.QuestType = typeID;
            this.Description = Description;
        }

        public string getDescription()
        {
            return Description;
        }

        public int getType()
        {
            return QuestType;
        }

    }

    public class BillboardQuests
    {
        protected List<int> _constants;

        public BillboardQuests()
        {
            _constants = new List<int>();
            System.Xml.Linq.XDocument doc;
            try
            {
                doc = System.Xml.Linq.XDocument.Load("source/QuestsInBoard.xml");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось загрузить файл:" + System.IO.Path.GetFullPath("source/QuestsInBoard.xml"), "Ошибка");
                return;
            }
            foreach (System.Xml.Linq.XElement item in doc.Root.Elements())
            {
                int quest_id = 0;
                if (int.TryParse(item.Value, out quest_id))
                    _constants.Add(quest_id);
            }
        }

        public List<int> getKeys()
        {
            return _constants;
        }
    }
}
