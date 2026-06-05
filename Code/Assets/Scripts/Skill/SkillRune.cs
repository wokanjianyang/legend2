using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillRune
    {
        //生效数量
        public int AvailableQuantity { get; private set; }

        public SkillRuneConfig Config { get; }

        public long Damage { get; }
        public int Percent { get; }
        public int Dis { get; }

        public int Duration { get; }
        public int EnemyMax { get; }
        public int CD { get; }
        public int Row { get; }
        public int Column { get; }
        public int IgnoreDef { get; } //无视防御
        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率

        public int DeadlyRate { get; } //暴击率
        public int DeadlyDamage { get; } //暴击倍率

        public int RateDamage { get; } //增伤倍率
        public int AttrIncrea { get; } //攻击加成
        public int FinalIncrea { get; } //最终伤害加成

        public int Speed { get; }

        public int Accuracy { get; }

        public int EffectId { get; } //

        public double EffectValue { get; }

        public int EffectMax { get; } //

        public int AttrId { get; }

        public double AttrValue { get; }

        public SkillRune(int runeId, int quantity)
        {
            this.Config = SkillRuneConfigCategory.Instance.Get(runeId);
            this.AvailableQuantity = Math.Min(quantity, Config.Max);

            this.CD = Config.CD * AvailableQuantity;
            this.Duration = Config.Duration * AvailableQuantity;
            this.Dis = Config.Dis * AvailableQuantity;
            this.EnemyMax = Config.EnemyMax * AvailableQuantity;
            this.Row = Config.Row * AvailableQuantity;
            this.Column = Config.Column * AvailableQuantity;

            this.Damage = Config.Damage * AvailableQuantity;
            this.Percent = Config.Percent * AvailableQuantity;
            this.IgnoreDef = Config.IgnoreDef * AvailableQuantity;

            this.CritRate = Config.CritRate * AvailableQuantity;
            this.CritDamage = Config.CritDamage * AvailableQuantity;
            this.DeadlyRate = Config.DeadlyRate * AvailableQuantity;
            this.DeadlyDamage = Config.DeadlyDamage * AvailableQuantity;

            this.RateDamage = Config.RateDamage * AvailableQuantity;
            this.AttrIncrea = Config.AttrIncrea * AvailableQuantity;
            this.FinalIncrea = Config.FinalIncrea * AvailableQuantity;
            this.Speed = Config.Speed * AvailableQuantity;
            this.Accuracy = Config.Accuracy * AvailableQuantity;

            if (Config.EffectId > 0)
            {
                this.EffectId = Config.EffectId;
                this.EffectValue = Config.EffectValue;
                this.EffectMax = Config.EffectMax;
            }

            if (Config.AttrId > 0)
            {
                this.AttrId = Config.AttrId;
                this.AttrValue = Config.AttrValue;
            }
        }

        public void AddCount(int count)
        {
            this.AvailableQuantity = Math.Min(AvailableQuantity + count, Config.Max);
        }

    }
}
