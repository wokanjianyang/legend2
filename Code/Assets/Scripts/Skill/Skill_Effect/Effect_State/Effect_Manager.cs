using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class Effect_Manager
    {
        public APlayer SelfPlayer { get; }

        protected Dictionary<int, Effect_State> StateDict = new Dictionary<int, Effect_State>();

        public int Duration { get; set; } //持续时长

        public int Max { get; } //可叠加层数

        public int Layer { get; set; }  //当前叠加层数

        private float TotalTime = 0;

        private float Interval = 1f;

        private float RunTime = 0f;

        private int RunCount = 0;

        public bool Active = true;

        public Effect_Manager(APlayer player)
        {
            this.SelfPlayer = player;
        }

        public void RunCD(float time)
        {

            foreach (Effect_State sp in StateDict.Values)
            {
                sp.RunCD(time);
            }
        }

        public void AddEffect(Effect_Compent compent, Effect_Buff buff)
        {
            if (StateDict.ContainsKey(buff.EffectId))
            {
                StateDict[buff.EffectId] = new Effect_State(compent.Config.Id, compent.Max);
            }
            else
            {
                StateDict[buff.EffectId].AddBuff(buff);
            }



            //if (!EffectMap.TryGetValue(effectData.FromId, out List<Effect> list))
            //{
            //    list = new List<Effect>();
            //    EffectMap[effectData.FromId] = list;
            //}

            //if (list.Count > 0 && list.Count >= effectData.Max)
            //{
            //    //移除旧的
            //    int RemoveCount = list.Count - Math.Max(0, effectData.Max - 1);

            //    for (int i = 0; i < RemoveCount; i++)
            //    {
            //        //effectData.Layer = list[i].Data.Layer; //使用旧的FromId

            //        list[i].Clear();
            //    }
            //    list.RemoveRange(0, RemoveCount);
            //}

            //int layer = 0;
            //if (list.Count > 0)
            //{
            //    layer = (list[list.Count - 1].Layer + 1) % effectData.Max; //每叠加一层，FromId+1
            //}

            //Effect effect = new Effect(this, effectData, damage, rolePercent, layer);
            //list.Add(effect);

            // 立即运行类型，立即使用
            //if (effect.Data.Config.RunType == 0)
            //{
            //    effect.Do(1f);
            //}
        }

        public bool isPause()
        {
            foreach (KeyValuePair<int, Effect_State> sp in StateDict)
            {
                if (sp.Value.isPause())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
