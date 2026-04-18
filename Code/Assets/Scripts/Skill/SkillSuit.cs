using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillSuit
    {
        public SkillSuitConfig Config { get; }

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

        public SkillSuit(int suitId)
        {
            this.Config = SkillSuitConfigCategory.Instance.Get(suitId);

            this.CD = Config.CD;
            this.Duration = Config.Duration;
            this.Dis = Config.Dis;
            this.EnemyMax = Config.EnemyMax;
            this.Row = Config.Row;
            this.Column = Config.Column;

            this.Damage = Config.Damage;
            this.Percent = Config.Percent;
            this.IgnoreDef = Config.IgnoreDef;

            this.CritRate = Config.CritRate;
            this.CritDamage = Config.CritDamage;
            this.DeadlyRate = Config.DeadlyRate;
            this.DeadlyDamage = Config.DeadlyDamage;

            this.RateDamage = Config.RateDamage;
            this.AttrIncrea = Config.AttrIncrea;
            this.FinalIncrea = Config.FinalIncrea;
            this.Speed = Config.Speed;
            this.Accuracy = Config.Accuracy;

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
    }
}
