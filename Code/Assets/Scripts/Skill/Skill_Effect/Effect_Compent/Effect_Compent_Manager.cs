using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent_Manager
    {
        public Dictionary<int, Effect_Compent> compents = new Dictionary<int, Effect_Compent>();

        private static Effect_Compent_Manager instance = null;

        public static Effect_Compent_Manager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Effect_Compent_Manager();
                }

                return instance;
            }

            private set { }
        }

        public Effect_Compent_Manager()
        {
            compents.Add((int)EffectCompentType.Attr, new Effect_Rise_Attr());
            compents.Add((int)EffectCompentType.Lifesteal‌, new Effect_Lifesteal());
            compents.Add((int)EffectCompentType.CrowdControl, new Effect_Crowd_Control());
            compents.Add((int)EffectCompentType.ControlImmunity‌, new Effect_Control_Immunity());
            compents.Add((int)EffectCompentType.Dot, new Effect_Dot());
            compents.Add((int)EffectCompentType.RestorePercent, new Effect_Restore_Percent());
        }

        public Effect_State Excute(APlayer attcher, APlayer enemy, SkillPanel sp, Effect_Data data, float cd, double damage)
        {
            EffectConfig config = EffectConfigCategory.Instance.Get(data.EffectId);

            APlayer target;
            Effect_Manager manger;

            if (config.TargetType == (int)EffectCompentTarget.Enemy)
            {
                //挂载到敌人身上
                target = enemy;
                manger = enemy.EffectManager;
            }
            else
            {
                //挂载到自己身上
                target = attcher;
                manger = attcher.EffectManager;
            }

            int key = data.FromId;

            if (!manger.StateDict.ContainsKey(key))
            {
                manger.StateDict[key] = new Effect_State(target, data, cd, damage);
            }

            Effect_State state = manger.StateDict[key];
            state.AddBuff();

            //单次的直接运行
            if (state.Data.Config.RunCycle == "Single")
            {
                state.RunTime = 0;
                Run(state);
            }

            Debug.Log("Effect Excute：" + sp.SkillData.SkillConfig.Name + " -" + data.EffectId + " -" + data.FromId);

            return state;
        }

        public void Run(Effect_State state)
        {
            int type = state.Data.Config.Type;

            Effect_Compent compent = compents[type];
            compent.Run(state);
        }

        public void Complete(Effect_State state)
        {
            int type = state.Data.Config.Type;

            Effect_Compent compent = compents[type];
            compent.Complete(state);


        }
    }


    public enum EffectCompentType
    {
        Attr = 1,  //增减属性
        Lifesteal = 2, //伤害回复
        CrowdControl = 3, //控制
        ControlImmunity‌ = 4, //免疫控制
        Dot = 5, //按伤害百分比持续掉血
        RestorePercent = 6, //按生命上限比例回血

    }

    public enum EffectCompentTarget
    {
        Self = 1,
        Enemy = 2,
        Skill = 3,
        Valet = 4,
    }

}
