using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    abstract public class Effect_Compent
    {
        public abstract void Run(Effect_State state);

        public abstract void Complete(Effect_State state);
    }

}
