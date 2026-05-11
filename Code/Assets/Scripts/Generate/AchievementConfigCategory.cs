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

        public long CalRequire(AchievementConfig config, int level)
        {
            if (config.ConRiseType == 1)
            {
                return config.Condition + (level - 1) * config.CondRiseVue;
            }
            else if (config.ConRiseType == 2) //sq
            {
                return (long)(config.Condition * Math.Pow(2, level));
            }
            else if (config.ConRiseType == 3) //sq
            {
                return config.Condition * MathHelper.GetSequence1(level);
            }


            return long.MaxValue;
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

    public enum AchievementProType
    {
        Level = 1,
        DayCount = 2,
        Advert = 3,
        EquipWear = 4,
        RecoverySet = 5,
        BossFamily = 6,
        EquipCopy = 7,
        Defend = 8,
        Infinite = 9,
        SkillCount = 10,
        SkillLevel = 11,

        PetWear = 100,
        PetTotal = 101,
        EquipTotal = 201,
        EquipRefine = 202,
        EquipStrong = 203,
        StageCount = 300,
        MonsterKillTotal = 301,
        MonsterKill1 = 302,
        MonsterKill2 = 303,
        MonsterKill3 = 304,
        MonsterKill4 = 305,
        MonsterKill5 = 306,
        MonsterKill6 = 307,

    }
}
