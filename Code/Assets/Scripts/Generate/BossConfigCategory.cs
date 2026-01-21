using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BossConfigCategory
    {

    }

    public class BossHelper
    {
        public static Boss BuildBoss(int mapId, RuleType ruleType)
        {
            Boss boss = new Boss(mapId, ruleType);
            return boss;
        }
    }
}