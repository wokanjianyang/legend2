using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// ∏ƒ±‰ Ù–‘
    /// </summary>
    public class Effect_Rise_Attr : Effect_Compent
    {
        public Effect_Rise_Attr(SkillPanel sp, Effect_Data data) : base(sp, data)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {
            if (this.Config.TargetType == (int)EffectCompentTarget.Enemy)
            {
                Effect_State state = target.AddEffect(this);

                double vue = state.Percent * state.Count;

                target.AttributeBonus.SetSkillAttr((AttributeEnum)Config.TargetAttr, this.FromId, vue);
            }
            else if (this.Config.TargetType == (int)EffectCompentTarget.Self)
            {
                Effect_State state = self.AddEffect(this);

                double vue = state.Percent * state.Count;

                self.AttributeBonus.SetSkillAttr((AttributeEnum)Config.TargetAttr, this.FromId, vue);
            }
        }
    }
}
