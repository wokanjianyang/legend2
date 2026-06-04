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
        public APlayer Attacher;
        public Effect_Data Data;

        public int EffectId { get; }

        public double Damage { get; set; }

        public double CD { get; set; }

        public float TotalTime { get; set; } = 0;

        public float RunTime = 0f;

        public int Count = 0;

        public Effect_State(APlayer player, Effect_Data data, float cd, double damage)
        {
            this.Attacher = player;
            this.Data = data;
            this.EffectId = data.EffectId;
            this.Data = data;
            this.CD = cd;
            this.Damage = damage;
        }

        public void AddBuff()
        {
            this.TotalTime = 0; //持续时间刷新

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

            if (Data.Config.StartType == "Interval") //循环类型才循环调用
            {
                RunTime += time;
            }

            if (TotalTime >= Data.Duration)
            {
                this.Count = 0;
                Effect_Compent_Manager.Instance.Complete(this);
            }

            if (RunTime >= CD)
            {
                RunTime = 0;
                Effect_Compent_Manager.Instance.Run(this);
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
