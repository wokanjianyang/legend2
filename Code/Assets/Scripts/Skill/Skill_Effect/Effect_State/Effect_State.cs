using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
    /// <summary>
    /// 多个BUFF组合成一个State
    /// </summary>
    public class Effect_State
    {
        public int EffectId { get; }

        public double Damage { get; set; }

        public double Percent { get; set; }

        public double Duratioin { get; set; }

        public double CD { get; set; }

        private float TotalTime = 0;

        private float RunTime = 0f;

        public int Count = 0;

        public int Max { get; } //可叠加层数

        private EffectConfig Config;



        public Effect_State(Effect_Compent compent)
        {
            this.EffectId = compent.ConfigId;
            this.Max = compent.Max;

            this.Damage = compent.Damage;
            this.Percent = compent.Percent;
            this.Duratioin = compent.Duration;
            this.CD = compent.CD;

            this.Config = EffectConfigCategory.Instance.Get(EffectId);
        }

        public void AddBuff()
        {
            if (Count < Max)
            {
                Count++;
            }
        }

        public void RunCD(float time)
        {
            TotalTime += time;
            RunTime += time;
        }


        public bool isPause()
        {
            if (this.Config.Type == (int)EffectCompentType.CrowdControl && Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool isIgnorePause()
        {
            if (this.Config.Type == (int)EffectCompentType.ControlImmunity && Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
