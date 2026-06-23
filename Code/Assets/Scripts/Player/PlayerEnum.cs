using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {


        //------------------减益------------------------------
        decreMulHp = -10001, //生命减少倍率
        decreDivDef = -10002, //防御减少倍率
        decreMulAtk = -10003, //攻击减少倍率

        DecreRestore = -15, //禁疗
        DecreExtraDamage = -28, //额外承伤倍率

        decreHp = -1, //灼烧流血伤害
        //------------------基础------------------------------

        HP = 1, //生命值
        Def = 2, //防御

        Atk = 3, //全系攻
        PhyAtk = 4, //物理攻击
        MagicAtk = 5,//魔法攻击
        SpiritAtk = 6, //道术攻击
        Lucky = 7, //幸运
        Curse‌ = 8, //诅咒

        Cd = 10, //技能冷却
        Speed = 11, //攻速
        MoveSpeed = 12, //移动速度
        Accuracy = 13, //命中
        Miss = 14, //闪避
        RestoreIncrea = 15, //回复加成-对吸血和治疗都有效

        CritRate = 21, //暴击率
        CritDamage = 22, //暴害增加
        DeadlyRate = 23, //致命率加成
        DeadlyDamage = 24, //致命伤害加成
        CritRateResist = 25, //抗暴
        CritDamageResist = 26, //爆伤减免
        DamageIncrea = 27, //伤害增加
        DamageResist = 28, //伤害减少

        Strong = 41,//韧性
        Shatter = 42, //破韧
        Parry = 43,//格挡

        DefIgnore = 51,//无视防御
        Protect = 52,//免疫
        BurstMul = 53,//连爆
        Miss2 = 54,//二次闪避


        GoldIncrea = 81, //金币加成
        ExpIncrea = 82, //经验加成
        QualityIncrea = 83,//品质加成
        BurstIncrea = 84, //爆率加成
        GoldKillIncrea = 85, //杀敌金币基础
        ExpKillIncrea = 86, //杀敌经验基础


        CardDamage = 91,//图鉴增伤
        FashionDamage = 92,//时装增伤
        AchievementDamage = 93,//成就增伤
        LegacyDamage = 94, //传世增伤
        ExclusiveDamage = 95, //珍宝增伤
        BabelDamage = 96,//通天增伤

        PetOnLimit = 111,  //宠物备战位
        PetBattleLimit = 112, //宠物出战位
        SkillBattleNumber = 113, //技能栏
        SkillSuitCount = 114, //词条套装数量
        fk = 222, //词条套装数量

        //------------------加成------------------------------
        IncreaHp = 1001, //生命加成
        IncreaDef = 1002,//防御加成
        IncreaAtk = 1003, //攻击加成
        IncreaPhyAtk = 1004, //物理攻击加成
        IncreaMagicAtk = 1005,//魔法攻击加成
        IncreaSpiritAtk = 1006, //道术攻击加成

        PhyDamage = 1007, //物伤加成
        MagicDamage = 1008,//魔伤加成
        SpiritDamage = 1009, //道伤加成

        IncreaLucky = 1013,//幸运加成
        //IncreaAccuracy = 1014, //命中加成
        //IncreaMiss = 1015, //闪避加成

        IncreaCrit = 1021,//暴击加成
        IncreaDamage = 1022, //爆伤加成
        IncreaDeadly = 1023, //致命率加成
        IncreaDeadlyDamage = 1024, //致命伤害加成

        //------------------增幅------------------------------
        RateHp = 2001, //生命增幅
        RateDef = 2002,//防御增幅
        RateAtk = 2003, //攻击增幅

        RatePhyAtk = 2004, //物理攻击增幅
        RateMagicAtk = 2005,//魔法攻击增幅
        RateSpiritAtk = 2006, //道术攻击增幅

        RatePhyDamage = 2007, //物伤增幅
        RateMagicDamage = 2008,//魔伤增幅
        RateSpiritDamage = 2009, //道伤增幅

        //RateLucky = 2013, //幸运增幅
        //RateAccuracy = 2014, //命中增幅
        //RateMiss = 2015, //闪避增幅

        //RateCrit = 2021,//暴击增幅
        //RateCritDamage = 2022, //爆伤增幅
        //RateDeadly = 2023, //致命率增幅
        //RateDeadlyDamage = 2024, //致命伤害增幅

        //RateDamageIncrea = 27, //伤害增加增幅
        //RateDamageResist = 28, //伤害减少增幅

        RateExp = 2081, //经验增幅
        RateGold = 2082, //金币增幅
        RateBurst = 2083, //爆率增幅
        RateQuality = 2084, //品质增幅


        //------------------倍率------------------------------
        MulHp = 10001,  //生命倍率
        MulDef = 10002,  //防御倍率
        MulAtk = 10003,  //攻击倍率

        MulPhyAtk = 10004, //物攻倍率
        MulMagicAtk = 10005,  //法功倍率
        MulSpiritAtk = 10006,  //道术倍率

        MulPhyDamageRise = 10007, //物伤倍率
        MulMagicDamageRise = 10008, //法伤倍率
        MulSpiritDamageRise = 10009, //魔伤倍率

        MulDamageIncrea = 10027,  //增伤倍率
        MulDamageResist = 10028, //减伤倍率

        MulStrong = 10041,//韧性倍率
                          //ShatterMul = 10042,//破韧倍率

        //----------特殊-------------
        EquipBaseIncrea = 20001, //装备基础属性百分比
        EquipSetIncrea = 20002, //装备套装属性百分比
        EquipRandomIncrea = 20003, //装备随机属性百分比
        EquipQualityIncrea = 20004, //传奇属性百分比

        //-----------技能加成
        SkillLevelRise = 21000, //全技能等级
        SkillLevelRiseP1 = 21001, //战士1技能+1
        SkillLevelRiseP2 = 21002, //战士2技能+1
        SkillLevelRiseP3 = 21003, //战士3技能+1
        SkillLevelRiseP4 = 21004, //战士4技能+1
        SkillLevelRiseP5 = 21005, //战士5技能+1
        SkillLevelRiseP6 = 21006, //战士6技能+1
        SkillLevelRiseP7 = 21007, //战士7技能+1
        SkillLevelRiseM1 = 22001, //法师1技能+1
        SkillLevelRiseM2 = 22002, //法师2技能+1
        SkillLevelRiseM3 = 22003, //法师3技能+1
        SkillLevelRiseM4 = 22004, //法师4技能+1
        SkillLevelRiseM5 = 22005, //法师5技能+1
        SkillLevelRiseM6 = 22006, //法师6技能+1
        SkillLevelRiseM7 = 22007, //法师7技能+1
        SkillLevelRiseS1 = 23001, //道士1技能+1
        SkillLevelRiseS2 = 23002, //道士2技能+1
        SkillLevelRiseS3 = 23003, //道士3技能+1
        SkillLevelRiseS4 = 23004, //道士4技能+1
        SkillLevelRiseS5 = 23005, //道士5技能+1
        SkillLevelRiseS6 = 23006, //道士6技能+1
        SkillLevelRiseS7 = 23007, //道士7技能+1

        //--废弃
        //MonsterFaster = 107,//副刷新速度加快
        //MetailFinal = 109, //挖矿
        //Talent = 111, //天赋点
        //DefendRate = 112, //防御系数
        //SpRate = 113,//护盾固防
        //RealHpDamage = 114,//真实血量伤害
        //RealMulDamageResist = 115, //溢出减伤迭代计算
        //RealCritRate = 116,//溢出暴击转为伤害加成
        //LuckyHit = 117, //青龙之力-幸运一击
        //Relic2 = 118, //神器2
        //Relic3 = 119, //神器2
        //Relic4 = 120, //神器2
        //Relic5 = 121, //神器2
        //RelicRise = 122, //所有神器等级+1

        //AurasDamageResist = 201, //光环减伤
        //AurasDamageIncrea = 202, //光环增伤
        //AurasAttrIncrea = 203,//

        //SkillUpCount = 301, //技能栏出战数量
        //SkillPhyDamage = 302, //物理伤害
        //SkillMagicDamage = 303,//魔法伤害
        //SkillSpiritDamage = 304, //道术伤害
        //SkillAllDamage = 305, //所有伤害加成
        //SkillValetCount = 306, //召唤数量+1
        //SkillValetSpeed = 307, //攻击速度
        //SkillValetHp = 308, //生命加成

        //SkillDivine2010 = 12010, //分身神技
        //SkillDivine3010 = 13010, //无极神技


        //PanelHp = 11001, //面板生命
        //PanelPhyAtt = 11002, //面板物攻
        //PanelMagicAtt = 11003, //面板魔法
        //PanelSpiritAtt = 11004, //面板道术
        //PanelDef = 11005, //面板防御
        //PanelAtt = 11006,//面板攻击


    }

    /// <summary>
    /// 属性来源
    /// </summary>
    public enum AttributeFrom
    {
        UserBase = 0, //人物面板总属性
        ConfigBase = 3, //人物升级属性
        Skill = 5,//技能增幅
    }

    public enum PlayerType
    {
        Hero = 0,
        Hero_Pet,
        Enemy,
        Valet,
        Defend,
        HeroPhatom,
        Duplication,
    }

    public enum ProgressType
    {
        [LabelText("角色经验")]
        PlayerExp = 0,

        [LabelText("技能经验")]
        SkillExp = 1,

        [LabelText("角色经验")]
        PlayerHP = 2,
    }

    public enum RoleType
    {
        Warrior = 1, //战士
        Mage = 2, //法师
        Warlock = 3, //道士
        All = 99,
    }
}
