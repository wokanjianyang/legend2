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
        public override void Complete(Effect_State state)
        {
            double vue = 0;
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.TarAtrId;

            state.Owner.AttributeBonus.SetSkillAttr((AttributeEnum)atrId, fromId, vue);
        }

        public override void Run(Effect_State state)
        {
            double vue = state.CalVue();
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.TarAtrId;

            state.Owner.AttributeBonus.SetSkillAttr((AttributeEnum)atrId, fromId, vue);
        }
    }
}
