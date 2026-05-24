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
            {nameof(AttributeEnum.GoldKillIncrea), "杀敌金币" },
            {nameof(AttributeEnum.ExpKillIncrea), "杀敌经验" },
            {nameof(AttributeEnum.SecondExp), "经验收益" },
            {nameof(AttributeEnum.SecondGold), "金币收益" },


            {nameof(AttributeEnum.RestoreHp), "固定回血" },
            {nameof(AttributeEnum.RestoreHpPercent), "比例回血" },
            {nameof(AttributeEnum.QualityIncrea), "品质加成" },
            {nameof(AttributeEnum.Miss), "闪避" },
            {nameof(AttributeEnum.Accuracy), "命中" },
            {nameof(AttributeEnum.Strong), "韧性" },
            {nameof(AttributeEnum.Protect), "绝对减伤" },
            {nameof(AttributeEnum.BurstMul), "多次掉落" },
            {nameof(AttributeEnum.RateExp), "经验增幅" },
            {nameof(AttributeEnum.RateGold), "金币增幅" },
            {nameof(AttributeEnum.RateBurst), "爆率增幅" },
            {nameof(AttributeEnum.RateQuality), "品质增幅" },
            {nameof(AttributeEnum.RateCrit), "暴击增幅" },
            {nameof(AttributeEnum.RateLucky), "幸运增幅" },
            //{nameof(AttributeEnum.MetailFinal), "挖矿速度" },
            //{nameof(AttributeEnum.RelicRise), "神器掌控" },
            {nameof(AttributeEnum.RateCritDamage), "爆伤增幅" },
            {nameof(AttributeEnum.Shatter), "破韧倍率" },

            {nameof(AttributeEnum.CardDamage), "图鉴增伤" },
            {nameof(AttributeEnum.FashionDamage), "时装增伤" },
            {nameof(AttributeEnum.AchievementDamage), "成就增伤" },
            {nameof(AttributeEnum.LegacyDamage), "传世增伤" },
            {nameof(AttributeEnum.ExclusiveDamage), "珍品增伤" },

            {nameof(AttributeEnum.PetOnLimit), "宠物备战位" },
            {nameof(AttributeEnum.PetBattleLimit), "宠物出战位" },
            {nameof(AttributeEnum.SkillLevelRise), "全技能等级" },

            //{nameof(AttributeEnum.SpRate), "护盾固防" },
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
