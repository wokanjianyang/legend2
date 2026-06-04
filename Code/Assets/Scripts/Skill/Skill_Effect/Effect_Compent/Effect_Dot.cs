using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// ·¢ÇòÐ§¹û
    /// </summary>
    public class Effect_Dot : Effect_Compent
    {
        public override void Complete(Effect_State state)
        {

        }

        public override void Run(Effect_State state)
        {
            double damage = 0;
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.SrcAtrId;

            double vue = state.CalVue();

            if (atrId == 0)
            {
                damage = state.Damage * vue / 100.0;
            }
            else
            {
                double atrVue = state.Owner.AttributeBonus.CalBattleTotalAttr((AttributeEnum)atrId);
                damage = atrVue * vue / 100.0;
            }

            DamageResult dr = new DamageResult(fromId, damage, MsgType.Dot, 0);

            state.Owner.OnHit(dr);
        }
    }
}
