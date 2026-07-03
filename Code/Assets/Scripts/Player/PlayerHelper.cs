using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PlayerHelper
    {
        public static Dictionary<string, string> PlayerAttributeMap = new Dictionary<string, string>()
        {

            {nameof(AttributeEnum.HP), "生命值" },
            {nameof(AttributeEnum.Def), "防御" },
            {nameof(AttributeEnum.Atk), "攻击" },
            {nameof(AttributeEnum.PhyAtk), "物理攻击" },
            {nameof(AttributeEnum.MagicAtk),"魔法攻击" },
            {nameof(AttributeEnum.SpiritAtk), "道术攻击" },

            {nameof(AttributeEnum.DefIgnore), "忽视防御" },
            {nameof(AttributeEnum.Cd), "冷却" },
            {nameof(AttributeEnum.Speed), "攻速" },
            {nameof(AttributeEnum.MoveSpeed), "移动速度" },
            {nameof(AttributeEnum.Lucky), "幸运" },
            {nameof(AttributeEnum.Curse), "诅咒" },
            {nameof(AttributeEnum.CritRate), "暴击概率" },
            {nameof(AttributeEnum.CritDamage), "暴击伤害" },
            {nameof(AttributeEnum.DeadlyRate), "致命概率" },
            {nameof(AttributeEnum.DeadlyDamage), "致命伤害" },
            {nameof(AttributeEnum.CritRateResist), "抗暴率" },
            {nameof(AttributeEnum.CritDamageResist), "爆伤减免" },
            {nameof(AttributeEnum.DamageIncrea), "伤害加成" },
            {nameof(AttributeEnum.DamageResist), "伤害减免" },
            {nameof(AttributeEnum.IncreaAtk), "攻击加成" },
            {nameof(AttributeEnum.IncreaHp), "生命加成" },
            {nameof(AttributeEnum.IncreaDef), "防御加成" },
            {nameof(AttributeEnum.ExpIncrea), "经验加成" },
            {nameof(AttributeEnum.BurstIncrea), "爆率加成" },
            {nameof(AttributeEnum.GoldIncrea), "金币加成" },
            {nameof(AttributeEnum.QualityIncrea), "品质加成" },
            {nameof(AttributeEnum.GoldKillIncrea), "杀敌金币" },
            {nameof(AttributeEnum.ExpKillIncrea), "杀敌经验" },



            {nameof(AttributeEnum.Accuracy), "命中" },
            {nameof(AttributeEnum.Miss), "闪避" },
            {nameof(AttributeEnum.RestoreIncrea), "回复加成" },

            {nameof(AttributeEnum.Strong), "韧性" },
            {nameof(AttributeEnum.Protect), "绝对减伤" },
            {nameof(AttributeEnum.BurstMul), "多次掉落" },
            {nameof(AttributeEnum.RateExp), "经验增幅" },
            {nameof(AttributeEnum.RateGold), "金币增幅" },
            {nameof(AttributeEnum.RateBurst), "爆率增幅" },
            {nameof(AttributeEnum.RateQuality), "品质增幅" },
            //{nameof(AttributeEnum.RateCrit), "暴击增幅" },
            //{nameof(AttributeEnum.RateLucky), "幸运增幅" },
            //{nameof(AttributeEnum.MetailFinal), "挖矿速度" },
            //{nameof(AttributeEnum.RelicRise), "神器掌控" },
            //{nameof(AttributeEnum.RateCritDamage), "爆伤增幅" },
            {nameof(AttributeEnum.Shatter), "破韧倍率" },

            {nameof(AttributeEnum.CardDamage), "图鉴增伤" },
            {nameof(AttributeEnum.FashionDamage), "时装增伤" },
            {nameof(AttributeEnum.AchievementDamage), "成就增伤" },
            {nameof(AttributeEnum.LegacyDamage), "传世增伤" },
            {nameof(AttributeEnum.ExclusiveDamage), "珍宝增伤" },
            {nameof(AttributeEnum.BabelDamage), "通天增伤" },

            {nameof(AttributeEnum.PetOnLimit), "宠物备战位" },
            {nameof(AttributeEnum.PetBattleLimit), "宠物出战位" },
            {nameof(AttributeEnum.SkillBattleNumber), "技能出战栏" },
            {nameof(AttributeEnum.SkillSuitCount), "词条需求减少" },
            {nameof(AttributeEnum.PetInherit), "宠物继承损耗减少" },
            {nameof(AttributeEnum.EquipStrongLimit), "强化上限" },
            {nameof(AttributeEnum.EquipRefineLimit), "精炼上限" },

            {nameof(AttributeEnum.SkillLevelRise), "全技能等级" },
            {nameof(AttributeEnum.SkillLevelRiseP1), "基础心法" },
            {nameof(AttributeEnum.SkillLevelRiseP2), "流光剑诀" },
            {nameof(AttributeEnum.SkillLevelRiseP3), "月华剑阵" },
            {nameof(AttributeEnum.SkillLevelRiseP4), "蛮牛冲撞" },
            {nameof(AttributeEnum.SkillLevelRiseP5), "龙魂护体" },
            {nameof(AttributeEnum.SkillLevelRiseP6), "无求易决" },
            {nameof(AttributeEnum.SkillLevelRiseP7), "火麟剑诀" },

            {nameof(AttributeEnum.SkillLevelRiseM1), "冥想心法" },
            {nameof(AttributeEnum.SkillLevelRiseM2), "紫电神雷" },
            {nameof(AttributeEnum.SkillLevelRiseM3), "火狱阵法" },
            {nameof(AttributeEnum.SkillLevelRiseM4), "烈焰焚天" },
            {nameof(AttributeEnum.SkillLevelRiseM5), "炎凰法盾" },
            {nameof(AttributeEnum.SkillLevelRiseM6), "万法归宗" },
            {nameof(AttributeEnum.SkillLevelRiseM7), "极寒冰暴" },

            {nameof(AttributeEnum.SkillLevelRiseS1), "清心咒" },
            {nameof(AttributeEnum.SkillLevelRiseS2), "破魔符咒" },
            {nameof(AttributeEnum.SkillLevelRiseS3), "毒咒术" },
            {nameof(AttributeEnum.SkillLevelRiseS4), "圣疗术" },
            {nameof(AttributeEnum.SkillLevelRiseS5), "太虚麟盾" },
            {nameof(AttributeEnum.SkillLevelRiseS6), "道法自然" },
            {nameof(AttributeEnum.SkillLevelRiseS7), "召唤英灵" },
            //{nameof(AttributeEnum.RealMulDamageResist), "完全减伤" },
            //{nameof(AttributeEnum.RealHpDamage), "血量真伤" },
            //{nameof(AttributeEnum.RealCritRate), "弱点暴击" },

            {nameof(AttributeEnum.PhyDamage), "物伤加成" },
            {nameof(AttributeEnum.MagicDamage),"魔伤加成" },
            {nameof(AttributeEnum.SpiritDamage), "道伤加成" },

            {nameof(AttributeEnum.IncreaPhyAtk), "物攻加成" },
            {nameof(AttributeEnum.IncreaMagicAtk),"魔法加成" },
            {nameof(AttributeEnum.IncreaSpiritAtk), "道术加成" },

            {nameof(AttributeEnum.EquipBaseIncrea), "装备基础属性" },
            {nameof(AttributeEnum.EquipRandomIncrea), "装备随机属性" },
            {nameof(AttributeEnum.EquipQualityIncrea), "装备品质属性" },
            {nameof(AttributeEnum.EquipSetIncrea), "装备套装属性" },



            {nameof(AttributeEnum.RateHp), "生命增幅" },
            {nameof(AttributeEnum.RateDef), "防御增幅" },
            {nameof(AttributeEnum.RateAtk), "攻击增幅" },
            {nameof(AttributeEnum.RatePhyAtk), "物攻增幅" },
            {nameof(AttributeEnum.RateMagicAtk), "魔击增幅" },
            {nameof(AttributeEnum.RateSpiritAtk), "道攻增幅" },
            {nameof(AttributeEnum.RatePhyDamage), "物伤增幅" },
            {nameof(AttributeEnum.RateMagicDamage), "魔伤增幅" },
            {nameof(AttributeEnum.RateSpiritDamage), "道伤增幅" },

            //{nameof(AttributeEnum.PanelAtt), "最终攻击" },
            //{nameof(AttributeEnum.PanelDef),"最终防御" },
            //{nameof(AttributeEnum.PanelHp), "最终生命" },
            //{nameof(AttributeEnum.PanelPhyAtt), "最终物攻" },
            //{nameof(AttributeEnum.PanelMagicAtt),"最终魔法" },
            //{nameof(AttributeEnum.SkillValetSpeed), "宠物攻速" },

            {nameof(AttributeEnum.MulAtk), "攻击倍率" },
            {nameof(AttributeEnum.MulDef),"防御倍率" },
            {nameof(AttributeEnum.MulHp), "生命倍率" },
            {nameof(AttributeEnum.MulPhyAtk), "物攻倍率" },
            {nameof(AttributeEnum.MulMagicAtk),"魔法倍率" },
            {nameof(AttributeEnum.MulSpiritAtk), "道术倍率" },
            {nameof(AttributeEnum.MulPhyDamageRise), "物伤倍率" },
            {nameof(AttributeEnum.MulMagicDamageRise),"魔伤倍率" },
            {nameof(AttributeEnum.MulSpiritDamageRise), "道伤倍率" },
            {nameof(AttributeEnum.MulDamageIncrea), "增伤倍率" },
            {nameof(AttributeEnum.MulDamageResist),"减伤倍率" },
            {nameof(AttributeEnum.MulStrong),"韧性倍率" },
        };
    }
}
