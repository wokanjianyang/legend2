using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Lifesteal : Effect_Compent
    {
        public Effect_Lifesteal(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {
            

        }
        

        public override void Do(APlayer self, APlayer target, double damage)
        {
            double restoreHp = damage * Percent / 100;

            self.OnRestore(FromId, restoreHp);
        }

    }
}
