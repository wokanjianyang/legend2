using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterConfigCategory
    {
        public MonsterConfig GetByMapId(int MapId)
        {
            return this.list.Where(m => m.MapId == MapId).First();
        }
    }

}