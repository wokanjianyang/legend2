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
        public APlayer Owner;
        public Effect_Data Data;

        public int EffectId { get; }

        public double Damage { get; set; }

        public double SrcVue { get; set; }

        public double CD { get; set; }

        public float TotalTime { get; set; } = 0;

        public float RunTime = 0f;

        public int Count = 0;

        private bool Active = true;

        public Effect_State(APlayer player, Effect_Data data, float cd, double damage,double srcVue)
        {
            this.Owner = player;
            this.Data = data;
            this.EffectId = data.EffectId;
            this.Data = data;
            this.CD = cd;
            this.Damage = damage;
            this.SrcVue = srcVue;
        }

        public void AddBuff()
        {
            this.TotalTime = 0; //持续时间刷新
            this.Active = true; //激活

            if (Count < Data.Max)
            {
                this.Count++;
            }
        }

        public void StartRun()  //第一次运行
        {

        }

        public void IntervalRun(float time)
        {
            TotalTime += time;

            if (Data.Config.RunCycle == "Interval") //循环类型才循环调用
            {
                RunTime += time;
            }

            if (RunTime >= CD && Active)
            {
                RunTime = 0;
                Effect_Compent_Manager.Instance.Run(this);
            }

            if (TotalTime >= Data.Duration)
            {
                this.Count = 0;
                this.Active = false;
                Effect_Compent_Manager.Instance.Complete(this);
            }
        }

        public double CalVue()
        {
            return Data.Percent * Count;
        }

        public bool isPause()
        {
            if (this.Data.Config.Type == (int)EffectCompentType.CrowdControl && Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool isIgnorePause()
        {
            if (this.Data.Config.Type == (int)EffectCompentType.ControlImmunity && Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
