using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 挂在在player单位上的
    /// </summary>
    public class Effect_Buff
    {
        public int EffectId { get; }

        public double Damage { get; set; }

        public double Percent { get; set; }

        public double Duratioin { get; set; }

        public double CD { get; set; }

        private float TotalTime = 0;

        private float RunTime = 0f;

        private int RunCount = 0;

        public bool Active = true;

        public Effect_Buff(int effectId, double damage, double percent, double duration, double cd)
        {
            this.EffectId = effectId;
            this.Damage = damage;
            this.Percent = percent;
            this.Duratioin = duration;
            this.CD = cd;

            this.Active = true;
        }

        public void RunCD(float time)
        {
            TotalTime += time;
            RunTime += time;
        }

        public bool IsRun(float time)
        {
            if (!Active)
            {
                return false; //已经结束的
            }

            TotalTime += time;
            RunTime += time;

            if (RunTime < CD)
            {
                return false; //还没到触发时间
            }

            //this.Run  运行Buff

            RunTime = 0;
            RunCount++;

            if (TotalTime >= Duratioin)
            {
                this.Active = false;
            }

            return true;
        }
    }
}
