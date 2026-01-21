using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBaseCategory
    {
        public Monster BuildMonster(MapConfig mapConfig, int quality,  RuleType ruleType)
        {
            Monster enemy = new Monster(mapConfig.Id, quality, ruleType);
            return enemy;
        }

        public MonsterBase GetByMapId(int MapId)
        {
            return this.list.Where(m => m.MapId == MapId).First();
        }
    }

}