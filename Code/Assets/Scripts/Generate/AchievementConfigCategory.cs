using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AchievementConfigCategory
    {
        public List<AchievementConfig> GetListByGid(int gid)
        {
            return this.list.Where(m => m.GroupId == gid).ToList();
        }
    }

    public enum AchievementRewardType
    {
        Attr = 1,
        Suit = 2,
        Stone = 3,
        SoulRing = 4,
        Tower = 5,
        Skill = 6,
    }

    public enum AchievementSourceType
    {
        Advert = 1,
        RealAdvert = 2,
        Strong = 3,
        Refine = 4,
        Level = 5,
        BossFamily = 6,
        EquipCopy = 7,
        Defend = 8,
        Infinite = 9,
        Legacy = 10,
    }
}
