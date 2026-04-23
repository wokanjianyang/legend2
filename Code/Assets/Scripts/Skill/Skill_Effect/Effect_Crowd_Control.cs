using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Crowd_Control : Effect_Compent
    {
        public Effect_Crowd_Control(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {

        }

    }
}
