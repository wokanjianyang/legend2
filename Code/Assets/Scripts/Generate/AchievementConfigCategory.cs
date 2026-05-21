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
                return config.Condition * MathHelper.GetSequence2(level);
            }


            return long.MaxValue;
        }
    }
    public partial class AchievementConfig
    {
        public long GetAtrVue(int i, int level)
        {
            return AtrVueList[i] + (level - 1) * AtrVueRiseList[i];
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
        Level = 1,  //等级
        DayCount = 2,  //登录天数
        Advert = 3,  //广告数量
        EquipWear = 4,  //穿戴装备数量
        RecoverySet = 5,  //回收设定
        RecoveryTotal = 6,
        EquipCopy = 7,
        Defend = 8,
        Infinite = 9, //
        SkillCount = 10, //技能总数量
        SkillLevel = 11, //技能总等级

        PetWear = 100,  //上阵宠物
        PetTotal = 101,  //累计获取宠物数量
        EquipTotal = 201, //累计获取装备数量
        EquipRefine = 202, //精炼等级
        EquipStrong = 203, //强化等级
        StageCount = 300, //闯关数
        MonsterKillTotal = 301, //累计杀敌数量
        MonsterKill1 = 302, //累计白怪数量
        MonsterKill2 = 303, //累计绿怪数量
        MonsterKill3 = 304, //累计蓝怪数量
        MonsterKill4 = 305, //累计紫怪数量
        MonsterKill5 = 306, //累计boss数量
        MonsterKill6 = 307, //累计区域boss数量

    }
}
