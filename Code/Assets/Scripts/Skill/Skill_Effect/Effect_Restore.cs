using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Restore : Effect_Compent
    {
        public Effect_Restore(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {

        }


        public override void Do(APlayer self, APlayer target, double damage)
        {
            double baseHp = self.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
            double restoreHp = baseHp * Percent / 100;

            self.OnRestore(FromId, restoreHp);
        }
    }
}
