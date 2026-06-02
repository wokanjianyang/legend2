using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Compent_Factory
    {
        public static Effect_Compent Create(SkillPanel sp, Effect_Data data)
        {
            Effect_Compent compent;

            EffectConfig config = EffectConfigCategory.Instance.Get(data.EffectId);

            if (config.Type == (int)EffectCompentType.Attr)
            {
                compent = new Effect_Rise_Attr(sp, data);
            }
            else if (config.Type == (int)EffectCompentType.Lifesteal‌)
            {
                compent = new Effect_Lifesteal(sp, data);
            }
            else if (config.Type == (int)EffectCompentType.CrowdControl)
            {
                compent = new Effect_Crowd_Control(sp, data);
            }
            else if (config.Type == (int)EffectCompentType.ControlImmunity‌)
            {
                compent = new Effect_Control_Immunity(sp, data);
            }
            else if (config.Type == (int)EffectCompentType.Dot)
            {
                compent = new Effect_Dot(sp, data);
            }
            else if (config.Type == (int)EffectCompentType.RestorePercent)
            {
                compent = new Effect_Restore_Percent(sp, data);
            }
            else
            {
                compent = new Effect_Compent(sp, data);
            }

            return compent;
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
