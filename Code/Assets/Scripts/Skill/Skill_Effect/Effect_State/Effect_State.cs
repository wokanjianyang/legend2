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
        public APlayer SelfPlayer { get; }

        public List<Effect_Buff> BuffList = new List<Effect_Buff>();

        public int EffectId { get; }
        public int Max { get; } //可叠加层数

        private EffectConfig Config;



        public Effect_State(int effectId, int max)
        {
            this.EffectId = effectId;
            this.Max = max;
            this.Config = EffectConfigCategory.Instance.Get(effectId);
        }

        public void AddBuff(Effect_Buff buff)
        {
            while (BuffList.Count >= Max) //移除旧的，增加新的
            {
                BuffList.RemoveAt(0);
            }

            BuffList.Add(buff);
        }

        public void RunCD(float time)
        {
            foreach (Effect_Buff buff in BuffList)
            {
                if (buff.IsRun(time))
                {
                    //需要运行了
                    buff.RunCD(time);
                }
                else
                {
                    //无需运行

                }
            }
        }


        public bool isPause()
        {
            if (this.Config.Type == (int)EffectCompentType.CrowdControl)
            {
                int ac = BuffList.Where(m => m.Active).Count();
                if (ac >= 1)
                {
                    return true;
                }
            }

            return false;

        }

        public bool isIgnorePause()
        {
            if (this.Config.Type == (int)EffectCompentType.ControlImmunity)
            {
                int ac = BuffList.Where(m => m.Active).Count();
                if (ac >= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
