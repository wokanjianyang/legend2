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

        public double Percent { get; set; }

        public double Duration { get; set; }

        public int Max { get; set; }

        public EffectConfig Config;

        public Effect_Data(int effectId, int fromId, double percent, double duration, int max)
        {
            this.EffectId = effectId;
            this.FromId = fromId;
            this.Percent = percent;
            this.Duration = duration;
            this.Max = max;

            this.Config = EffectConfigCategory.Instance.Get(EffectId);
        }

        public void MergeParam(double percent, double duration, int max)
        {
            this.Percent += percent;
            this.Duration += duration;
            this.Max += max;
        }

    }
}
