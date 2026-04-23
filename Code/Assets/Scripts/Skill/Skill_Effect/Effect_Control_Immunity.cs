using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Control_Immunity : Effect_Compent
    {
        public Effect_Control_Immunity(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {

        }

    }
}
