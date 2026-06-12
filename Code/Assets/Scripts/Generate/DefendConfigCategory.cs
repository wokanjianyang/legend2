using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public partial class MonsterDefendConfigCategory
    {
        public MonsterDefendConfig GetByLayerAndLevel(int layer, int level)
        {
            return this.list.Where(m => m.Layer == layer && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
        }
    }
}