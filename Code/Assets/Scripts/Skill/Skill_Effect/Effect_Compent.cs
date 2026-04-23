using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent
    {
        public EffectConfig Config { get; set; }

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
        public int Duration { get; set; }
        /// <summary>
        /// 叠加层数
        /// </summary>
        public int Max { get; set; }

        public Effect_Compent(int configId, int fromId, double percent, long damage, int duration, int max)
        {
            this.Config = EffectConfigCategory.Instance.Get(configId);
            this.FromId = fromId;

            this.Duration = duration;
            this.Max = max;
            this.Percent = percent;
            this.Damage = damage;
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
