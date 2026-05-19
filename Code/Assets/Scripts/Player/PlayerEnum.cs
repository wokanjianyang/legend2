using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {


        //------------------减益------------------------------
        decreMulHp = -10001, //生命减少倍率
        decreMulDef = -10002, //防御减少倍率
        decreMulAtk = -10003, //攻击减少倍率

        decreExtraDamage = -10029, //额外承伤倍率

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

        Speed = 11, //攻速
        MoveSpeed = 12, //移动速度
        Accuracy = 13, //命中
        Miss = 14, //闪避

        CritRate = 21, //暴击率
        CritDamage = 22, //暴害增加
        DeadlyRate = 23, //致命率加成
        DeadlyDamage = 24, //致命伤害加成
        CritRateResist = 25, //抗暴
        CritDamageResist = 26, //爆伤减免
        DamageIncrea = 27, //伤害增加
        DamageResist = 28, //伤害减少
        ExtraDamage = 29,//易伤额外伤害

        Strong = 41,//韧性
        Shatter = 42, //破韧
        Parry = 43,//格挡

        DefIgnore = 51,//无视防御
        Protect = 52,//免疫
        BurstMul = 53,//连爆
        Miss2 = 54,//二次闪避


        SecondExp = 61, //每秒经验收益
        SecondGold = 62, //每秒金币收益
        RestoreHp = 63, //固定回血数值
        RestoreHpPercent = 64,//百分比回血数值


        WarriorSkillPercent = 71, //战士技能百分比系数
        WarriorSkillDamage = 72, //战士技能固定系数
        MageSkillPercent = 73, //法师技能百分比系数
        MageSkillDamage = 74, //法师技能固定系数
        WarlockSkillPercent = 75, //道士技能百分比系数
        WarlockSkillDamage = 76, //道士技能固定系数
        SkillDeadlyRate = 77,  //技能致命率
        SkillDeadlyDamage = 78, //技能致命伤害
        SkillFinalDamage = 79, //技能终伤

        GoldIncrea = 81, //金币加成
        ExpIncrea = 82, //经验加成
        QualityIncrea = 83,//品质加成
        BurstIncrea = 84, //爆率加成


        CardDamage = 91,//图鉴增伤
        FashionDamage = 92,//时装增伤
        AchievementDamage = 93,//成就增伤
        LegacyDamage = 94,//传世增伤

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

        RateLucky = 2013, //幸运增幅
        //RateAccuracy = 2014, //命中增幅
        //RateMiss = 2015, //闪避增幅

        RateCrit = 2021,//暴击增幅
        RateCritDamage = 2022, //爆伤增幅
        RateDeadly = 2023, //致命率增幅
        RateDeadlyDamage = 2024, //致命伤害增幅

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
        EquipRandomIncrea = 20002, //装备随机属性百分比
        EquipQualityIncrea = 20003, //装备品质属性百分比
        EquipSetIncrea = 20004, //装备套装属性百分比

        //--废弃
        MonsterFaster = 107,//副刷新速度加快
        MetailFinal = 109, //挖矿
        Talent = 111, //天赋点
        DefendRate = 112, //防御系数
        SpRate = 113,//护盾固防
        RealHpDamage = 114,//真实血量伤害
        RealMulDamageResist = 115, //溢出减伤迭代计算
        RealCritRate = 116,//溢出暴击转为伤害加成
        LuckyHit = 117, //青龙之力-幸运一击
        Relic2 = 118, //神器2
        Relic3 = 119, //神器2
        Relic4 = 120, //神器2
        Relic5 = 121, //神器2
        RelicRise = 122, //所有神器等级+1

        AurasDamageResist = 201, //光环减伤
        AurasDamageIncrea = 202, //光环增伤
        AurasAttrIncrea = 203,//

        SkillUpCount = 301, //技能栏出战数量
        SkillPhyDamage = 302, //物理伤害
        SkillMagicDamage = 303,//魔法伤害
        SkillSpiritDamage = 304, //道术伤害
        SkillAllDamage = 305, //所有伤害加成
        SkillValetCount = 306, //召唤数量+1
        SkillValetSpeed = 307, //攻击速度
        SkillValetHp = 308, //生命加成

        SkillDivine2010 = 12010, //分身神技
        SkillDivine3010 = 13010, //无极神技


        PanelHp = 11001, //面板生命
        PanelPhyAtt = 11002, //面板物攻
        PanelMagicAtt = 11003, //面板魔法
        PanelSpiritAtt = 11004, //面板道术
        PanelDef = 11005, //面板防御
        PanelAtt = 11006,//面板攻击


    }

    /// <summary>
    /// 属性来源
    /// </summary>
    public enum AttributeFrom
    {
        HeroPanel = 0, //人物面板总属性
        HeroBase = 1, //人物升级属性
        EquipBase = 2, //装备基础属性
        EquiStrong = 3, //装备强化属性
        Skill = 4,//技能增幅
        Tower = 5,//无尽塔
        Phantom = 6,//幻神
        EquipSuit = 7, //装备套装
        SoulRing = 8, //魂环
        Auras = 9, //光环
        Achivement = 10, //成就
        Exclusive = 11, //专属
        Card = 12,//图鉴
        Wing = 13, //翅膀
        Fashion = 14, //时装
        EquipRed = 15, //装备套装
        Halidom = 16, //遗物
        Metal = 17,//矿石
        Legacy = 18,//传世
        Ring = 19,//特戒
        Cycle = 20,//转生
        Pill = 21,//修炼
        SoulBone = 22, //魂骨
        Talent = 23, //天赋
        EquipReform = 24, //改造
        Pet = 25,//宠物
        Relic = 26,//神器
        Stone = 27,//宝石
        Pill2 = 28,
        CardSpeical = 29,//暗金图鉴
        FashionSpeical = 30,//暗金时装
        Pill3 = 31,
        PetSpeical = 32,
        Shengxiao = 33,
        Festive = 34,
        Spirit = 35,

        Dingzhi = 98,
        /// <summary>
        /// 测试属性
        /// </summary>
        Test = 99,
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

    public enum MondelType
    {
        Nomal = 1,
        Boss = 2,
        YueLing = 5,
    }

    public enum SlotType
    {
        [LabelText("武器")]
        武器 = 1,
        [LabelText("衣服")]
        衣服 = 2,
        [LabelText("项链")]
        项链 = 3,
        [LabelText("头盔")]
        头盔 = 4,
        [LabelText("左手镯")]
        左手镯 = 5,
        [LabelText("右手镯")]
        右手镯 = 6,
        [LabelText("左戒指")]
        左戒指 = 7,
        [LabelText("右戒指")]
        右戒指 = 8,
        [LabelText("腰带")]
        腰带 = 9,
        [LabelText("鞋子")]
        鞋子 = 10,
        [LabelText("斗笠")]
        斗笠 = 11,
        [LabelText("护盾")]
        护盾 = 12,
        [LabelText("神符")]
        神符 = 13,
        [LabelText("魔石")]
        魔石 = 14,

        [LabelText("专属1")]
        神圣怒斩 = 15,
        [LabelText("专属2")]
        神圣噬魂 = 16,
        [LabelText("专属3")]
        神圣血饮 = 17,
        [LabelText("专属4")]
        神圣屠龙 = 18,
        [LabelText("专属5")]
        神圣倚天 = 19,
        [LabelText("专属6")]
        神圣命运 = 20,

        [LabelText("金武器")]
        金武器 = 21,
        [LabelText("金衣服")]
        金衣服 = 22,
        [LabelText("金项链")]
        金项链 = 23,
        [LabelText("金头盔")]
        金头盔 = 24,
        [LabelText("金左镯")]
        金左镯 = 25,
        [LabelText("金右镯")]
        金右镯 = 26,
        [LabelText("金左戒")]
        金左戒 = 27,
        [LabelText("金右戒")]
        金右戒 = 28,
        [LabelText("金腰带")]
        金腰带 = 29,
        [LabelText("金鞋子")]
        金鞋子 = 30,

        [LabelText("暗金武器")]
        暗金武器 = 31,
        [LabelText("暗金衣服")]
        暗金衣服 = 32,
        [LabelText("暗金项链")]
        暗金项链 = 33,
        [LabelText("暗金头盔")]
        暗金头盔 = 34,
        [LabelText("暗金左镯")]
        暗金左镯 = 35,
        [LabelText("暗金右镯")]
        暗金右镯 = 36,
        [LabelText("暗金左戒")]
        暗金左戒 = 37,
        [LabelText("暗金右戒")]
        暗金右戒 = 38,
        [LabelText("暗金腰带")]
        暗金腰带 = 39,
        [LabelText("暗金鞋子")]
        暗金鞋子 = 40,

        混沌武器 = 41,
        混沌衣服 = 42,
        混沌项链 = 43,
        混沌头盔 = 44,
        混沌左镯 = 45,
        混沌右镯 = 46,
        混沌左戒 = 47,
        混沌右戒 = 48,
        混沌腰带 = 49,
        混沌鞋子 = 50,

        [LabelText("主专属")]
        主专属 = 101,
        [LabelText("副专属")]
        副专属 = 102,
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

    public enum CopyType
    {
        [LabelText("装备副本")]
        装备副本 = 1,
        [LabelText("幻影挑战")]
        幻影挑战 = 2,
        [LabelText("BOSS之家")]
        BossFamily = 3,
        [LabelText("未知暗殿")]
        AnDian = 4,
        [LabelText("守卫沙城")]
        Defend = 5,
        HeorPhantom = 6,
        Mine = 7,
        Infinite = 8,
        Legacy = 9,
        Pill,
        Babel,
        Myth,
    }

    public enum RoleType
    {
        Warrior = 1, //战士
        Mage = 2, //法师
        Warlock = 3, //道士
        All = 99,
    }
}
