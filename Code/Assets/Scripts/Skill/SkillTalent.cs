using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillTalent
    {
        public SkillTalentConfig Config { get; }

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
        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成
        public int FinalIncrea { get; } //最终伤害加成

        public int PercentRate { get; }

        public int InheritIncrea { get; }
        public int EffectId { get; } //

        public int Accuracy { get; }
        public int Miss { get; }
        //public string Center { get; }

        public SkillTalent(int tid)
        {
            this.Config = SkillTalentConfigCategory.Instance.Get(tid);

            this.Damage = Config.Damage;
            this.Percent = Config.Percent;
            this.Dis = Config.Dis;
            this.Duration = Config.Duration;
            this.EnemyMax = Config.EnemyMax;
            this.CD = Config.CD;
            this.Row = Config.Row;
            this.Column = Config.Column;

            //this.IgnoreDef = SkillSuitConfig.IgnoreDef;
            //this.CritRate = SkillSuitConfig.CritRate;
            //this.CritDamage = SkillSuitConfig.CritDamage;
            //this.DamageIncrea = SkillSuitConfig.DamageIncrea;

            //this.AttrIncrea = SkillSuitConfig.AttrIncrea;
            //this.FinalIncrea = SkillSuitConfig.FinalIncrea;
            //this.InheritIncrea = SkillSuitConfig.InheritIncrea;

            //this.PercentRate = SkillSuitConfig.PercentRate;
            ////this.Center = SkillSuitConfig.Center;

            //this.EffectId = SkillSuitConfig.EffectId;
            //this.Accuracy = SkillSuitConfig.Accuracy;
            //this.Miss = SkillSuitConfig.Miss;
        }
    }
}
