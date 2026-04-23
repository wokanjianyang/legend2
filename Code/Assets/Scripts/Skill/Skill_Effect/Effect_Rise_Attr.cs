using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Rise_Attr : Effect_Compent
    {
        public Effect_Rise_Attr(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {
            if (this.Config.TargetType == (int)EffectCompentTarget.Enemy)
            {

            }
            else if (this.Config.TargetType == (int)EffectCompentTarget.Self)
            {

            }
        }

    }
}
