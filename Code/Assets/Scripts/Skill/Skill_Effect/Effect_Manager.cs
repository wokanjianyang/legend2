using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class Effect_Manager
    {
        public APlayer SelfPlayer { get; }

        public Dictionary<int, Effect_State> StateDict = new Dictionary<int, Effect_State>();

        public Effect_Manager(APlayer player)
        {
            this.SelfPlayer = player;
        }

        public void RunCD(float time)
        {
            foreach (Effect_State sp in StateDict.Values)
            {
                sp.IntervalRun(time);
            }
        }

        public bool isPause()
        {
            foreach (KeyValuePair<int, Effect_State> sp in StateDict)
            {
                if (sp.Value.isPause())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
