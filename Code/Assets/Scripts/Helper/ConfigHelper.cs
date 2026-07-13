using System.Text;

namespace Game
{
    public class ConfigHelper
    {
#if IS_TAPTAP
        public const int Channel = 1; //Tap 1 QQ 2
#else
        public const int Channel = 2; //Tap 1 QQ 2
#endif

#if IS_TAPTAP
        public const int AC = 1; //Tap 1 QQ 2
#else
        public const int AC = 2; //Tap 1 QQ 2
#endif

#if IS_Test
        public const int TestRate = 5;
        public const int SrvId = 98; //测试服99
#else
        public const int TestRate = 1;
        public const int SrvId = 1; //正式1
#endif

        public const int Channel_Tap = 1;

        public const int Version = 1;

        public const long PackTime = 1759807046; //打包时间，防止作弊

        public const long PackEndTime = 1805727046; //超过此时间,游戏不能使用，需要更新

        public const long Max_Level = 120000; //最大人物等级和强化等级

        public const long Cycle_Level = 10000; //每次轮回增加等级

        public const long Cycle_Max = 32;


        public const long RestoreGold = 10000;

        public const double Def_Rate = 3; //防御系数


        public const long MaxOfflineTime = 3600 * 24;  //最长离线时间

        //public const int MaxBagCount = 210;  // 包裹数量
        public static int[] BagCount = new int[] { 200, 200, 200, 450, 550 };

        public const int LegacyDefaultTime = 1200; //20分钟 


        //public static int[] PercentAttrIdList = { 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 41, 43, 45, 50, 51, 52, 53, 54, 55, 60, 91, 92, 93, 94, 101, 102, 103, 108, 109, 110, 201, 202, 203, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013 };

        public static int[] BaseAttrIdList = { 1, 2, 3, 4, 5, 6, 7, 8, 85, 86, 111, 112, 113, 114, 222 };


        public static int[] RateAttrIdList = { 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009 };

        public static string[] LayerNameList = { "黄", "玄", "地", "天", "荒", "洪", "宙", "宇", "极", "道", "虚", "始" };
        public static string[] LayerChinaList = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八","十九", "二十",
         "二一", "二二", "二三", "二四", "二五", "二六", "二七", "二八","二九", "三十",};

        public static string[] LayerAlbList = { "Ⅰ", "Ⅱ", "Ⅲ", "Ⅳ", "Ⅴ", "Ⅵ", "Ⅶ", "Ⅷ", };

        public static string[] CycleList = { "零转", "一转", "二转", "三转", "四转", "五转", "六转", "七转", "八转", "九转", "十转",
            "练气", "筑基", "金丹", "元婴", "化神", "练虚", "合体", "大乘","渡劫", "散仙初期",
         "散仙中期", "‌散仙后期", "人仙初期", "人仙中期", "人仙后期", "地仙初期", "地仙中期", "地仙后期","真仙初期", "‌真仙中期",
          "真仙后期",  "天仙初期", "天仙中期", "天仙后期", "玄仙初期", "玄仙中期", "玄仙后期","金仙初期", "金仙中期",  "金仙后期",
        };

        public static string[] UnitList = { "万", "亿", "兆", "京", "垓", "秭", "穰", "沟", "涧", "正", "载", "极", "恒", "河", "沙", "阿", "僧", "祇"
                , "那", "由", "他", "不", "可", "思" , "议", "无", "量", "大", "数", "古", "戈", "尔" , "频", "波", "罗"
                , "天", "地", "玄", "黄","宇","宙","洪","荒","日","月","盈","昃" ,"辰","宿","列","张","寒","来","暑","往"
                ,"秋","收","冬","藏","闰","余","成","岁","律","吕","调","阳","云","腾","致","雨"}; // 露结为霜金生丽水

        public const int MapStartId = 1;

        public const int DefendHp = 300; //防守塔默认血量
        public const int DefendMaxLevel = 8; //守沙难度

        public const int SkillSuitMax = 4;
        public const int SkillSuitMin = 2;

        public const int SkillNumber = 5;

        public const float DelayShowTime = 0.75f;
        //public const float SkillAnimaTime = 0.75f;
        //public const float SkillAnimaTime1 = 0.75f;

        public const float PvpRate = 200;
        public const float ValetPvpRate = 30;

        public const int EquipRefreshCount = 10;

        public const int AutoExitMapTime = 2;
        public const int AutoStartMapTime = 2;
        public const int AutoResurrectionTime = 10;

        public const int PillDefaultTime = 60;
        public const int BabelCount = 100;
        public const int BabelMax = 600;

        public const int PillMax = 20;
        public const int PillMax2 = 10;

        public const int Mine_Time = 60;

        public const int Infinit_Max = 6000;

        public static string[] RoleName = { "战士", "法师", "道士" };
        public static string[] RoleName1 = { "战", "法", "道" };
        public const int PetMax = 2;

        public const int MaxWorld = 800;

        public const int PetSpeicalMaxLayer = 3;

        public const double MaxNumber = 1E300;

        public const double PetKillPercent = 100.0;  //为了符合小数值，杀怪加属性削弱十倍

        public const int EnvTest = 0;  //0 不测试 ，1 测试伤害

        public const int OfflineTime = 60 * 2;

        public const int SkillBoxExp = 500;
    }
}