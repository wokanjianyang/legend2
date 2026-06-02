using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent
    {
        public SkillPanel Skill;

        public EffectConfig Config { get; set; }

        public int ConfigId { get; }

        public int FromId { get; }
        /// <summary>
        /// 伤害比例加成
        /// </summary>
        public double Percent { get; set; }
        /// <summary>
        /// 伤害固定加成
        /// </summary>
        public double Damage { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double Duration { get; set; }

        public float CD = 1;

        /// <summary>
        /// 叠加层数
        /// </summary>
        public int Max { get; set; }



        public Effect_Compent(SkillPanel sp, Effect_Data data)
        {
            this.Skill = sp;
            this.ConfigId = data.EffectId;
            this.FromId = data.FromId;
            this.Damage = data.Damage;
            this.Percent = data.Percent;
            this.Duration = data.Duration;
            this.CD = (float)data.CD;
            this.Max = data.Max;

            this.Config = EffectConfigCategory.Instance.Get(ConfigId);
        }



        public void Add(double percent, double damage, int duration, int max)
        {
            this.Percent += percent;
            this.Damage += damage;
            this.Duration += duration;
            this.Max += max;
        }

        public virtual void Do(APlayer self, APlayer target, double damage)
        {

        }
    }


}
