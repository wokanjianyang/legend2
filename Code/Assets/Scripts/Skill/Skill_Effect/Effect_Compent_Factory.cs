using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent_Factory
    {



        public static Effect_Compent Create(int configId, int fromId, double percent, long damage, int duration, int max)
        {
            EffectConfig config = EffectConfigCategory.Instance.Get(configId);

            if (config.Type == (int)EffectCompentType.Attr)
            {
                return new Effect_Rise_Attr(configId, fromId, percent, damage, duration, max);
            }
            else if (config.Type == (int)EffectCompentType.Lifesteal‌)
            {
                return new Effect_Lifesteal(configId, fromId, percent, damage, duration, max);
            }
            else if (config.Type == (int)EffectCompentType.CrowdControl)
            {
                return new Effect_Crowd_Control(configId, fromId, percent, damage, duration, max);
            }
            else if (config.Type == (int)EffectCompentType.ControlImmunity‌)
            {
                return new Effect_Control_Immunity(configId, fromId, percent, damage, duration, max);
            }
            else if (config.Type == (int)EffectCompentType.Dot)
            {
                return new Effect_Dot(configId, fromId, percent, damage, duration, max);
            }
            else if (config.Type == (int)EffectCompentType.RestorePercent)
            {
                return new Effect_Restore_Percent(configId, fromId, percent, damage, duration, max);
            }
            else
            {
                return new Effect_Compent(configId, fromId, percent, damage, duration, max);
            }
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
