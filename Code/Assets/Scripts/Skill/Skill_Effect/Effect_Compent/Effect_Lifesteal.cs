using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// ÎüÑª
    /// </summary>
    public class Effect_Lifesteal : Effect_Compent
    {
        //public Effect_Lifesteal(SkillPanel sp, Effect_Data data) : base(sp, data)
        //{


        //}


        //public override void Do(APlayer self, APlayer target, double damage)
        //{
        //    double restoreHp = damage * Vue / 100;

        //    self.OnRestore(FromId, restoreHp);
        //}
        public override void Complete(Effect_State state)
        {
            //
        }

        public override void Run(Effect_State state)
        {
            double restoreHp = 0;
            int fromId = state.Data.FromId;
            int atrId = state.Data.Config.SrcAtrId;

            double vue = state.CalVue();

            if (atrId == 0)
            {
                restoreHp = state.Damage * vue / 100.0;
            }
            else
            {
                double atrVue = state.Owner.AttributeBonus.CalBattleTotalAttr((AttributeEnum)atrId);
                restoreHp = atrVue * vue / 100.0;
            }

            state.Owner.OnRestore(fromId, restoreHp);
        }
    }
}
