using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent_Factory
    {
        public static Effect_State Excute(APlayer attcher, APlayer enemy, SkillPanel sp, Effect_Data data, float cd, double damage)
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

        public static void Run(Effect_State state)
        {
            int type = state.Data.Config.Type;

            if (type == (int)EffectCompentType.Attr)
            {
                Rise_Attr_Run(state);
            }
            else if (type == (int)EffectCompentType.Lifesteal‌)
            {
                Lifesteal_Run(state);
            }
            //else if (type == (int)EffectCompentType.CrowdControl)
            //{
            //    compent = new Effect_Crowd_Control(sp, data);
            //}
            //else if (type == (int)EffectCompentType.ControlImmunity‌)
            //{
            //    compent = new Effect_Control_Immunity(sp, data);
            //}
            //else if (type == (int)EffectCompentType.Dot)
            //{
            //    compent = new Effect_Dot(sp, data);
            //}
            //else if (type == (int)EffectCompentType.RestorePercent)
            //{
            //    compent = new Effect_Restore_Percent(sp, data);
            //}
            //else
            //{
            //    compent = new Effect_Compent(sp, data);
            //}
        }

        private static void Rise_Attr_Run(Effect_State state)
        {
            double vue = state.CalVue();
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.TargetAttr;

            state.Attacher.AttributeBonus.SetSkillAttr((AttributeEnum)atrId, fromId, vue);
        }

        private static void Lifesteal_Run(Effect_State state)
        {

        }

        public static void Complete(Effect_State state)
        {
            int type = state.Data.Config.Type;

            if (type == (int)EffectCompentType.Attr)
            {
                Rise_Attr_Complete(state);
            }
            else if (type == (int)EffectCompentType.Lifesteal‌)
            {
                Lifesteal_Complete(state);
            }
        }

        private static void Rise_Attr_Complete(Effect_State state)
        {
            int vue = 0;
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.TargetAttr;

            state.Attacher.AttributeBonus.SetSkillAttr((AttributeEnum)atrId, fromId, vue);
        }

        private static void Lifesteal_Complete(Effect_State state)
        {

        }


        //public static Effect_Compent Create(int Type)
        //{
        //    Effect_Compent compent;

        //    if (Type == (int)EffectCompentType.Attr)
        //    {
        //        compent = new Effect_Rise_Attr(sp, data);
        //    }
        //    else if (Type == (int)EffectCompentType.Lifesteal‌)
        //    {
        //        compent = new Effect_Lifesteal(sp, data);
        //    }
        //    else if (Type == (int)EffectCompentType.CrowdControl)
        //    {
        //        compent = new Effect_Crowd_Control(sp, data);
        //    }
        //    else if (Type == (int)EffectCompentType.ControlImmunity‌)
        //    {
        //        compent = new Effect_Control_Immunity(sp, data);
        //    }
        //    else if (Type == (int)EffectCompentType.Dot)
        //    {
        //        compent = new Effect_Dot(sp, data);
        //    }
        //    else if (Type == (int)EffectCompentType.RestorePercent)
        //    {
        //        compent = new Effect_Restore_Percent(sp, data);
        //    }
        //    else
        //    {
        //        compent = new Effect_Compent(sp, data);
        //    }

        //    return compent;
        //}


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
