using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 挂在在player单位上的
    /// </summary>
    public class Effect_Data
    {
        public int EffectId { get; }

        public int FromId { get; }

        public double Damage { get; set; }

        public double Percent { get; set; }

        public double Duration { get; set; }

        public double CD { get; set; }

        public int Max { get; set; }

        public EffectConfig Config;

        public Effect_Data(int effectId, int fromId, double damage, double percent, double duration, double cd, int max)
        {
            this.EffectId = effectId;
            this.FromId = fromId;
            this.Damage = damage;
            this.Percent = percent;
            this.Duration = duration;
            this.CD = cd;
            this.Max = max;

            this.Config = EffectConfigCategory.Instance.Get(EffectId);
        }

        public void MergeParam(double damage, double percent, double duration, double cd, int max)
        {
            this.Damage += damage;
            this.Percent += percent;
            this.Duration += duration;
            //this.CD = cd;
            this.Max += max;
        }

    }
}
