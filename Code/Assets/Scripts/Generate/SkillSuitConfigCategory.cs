using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillSuitConfigCategory
    {

        public SkillSuitConfig RandomSuit(int skillId, int quality, int seed)
        {
            List<SkillSuitConfig> tempList = this.list.Where(m => m.SkillId == skillId && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            if (tempList.Count == 1)
            {
                return tempList[0];
            }

            int maxRate = tempList.Select(m => m.EquipRate).Sum();
            int rd = RandomHelper.RandomNumber(seed, 1, maxRate + 1);

            int tempRate = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                tempRate += tempList[i].EquipRate;

                if (rd <= tempRate)
                {
                    return tempList[i];
                }
            }

            return null;
        }


        public List<SkillSuitConfig> GetSkillAllConfigs(int skillId, int skillLayer)
        {
            return this.list.Where(m => (m.SkillId == skillId)).ToList();
        }

        public List<SkillSuit> GetAllSuit(int skillId, int suitCount)
        {
            return GetAllSuit(skillId, suitCount, null);
        }

        public List<SkillSuit> GetAllSuit(int skillId, int suitCount, int[] excludeList)
        {
            List<SkillSuit> suitList = new List<SkillSuit>();

            List<SkillSuitConfig> suitConfigs = this.list.Where(m => m.SkillId == skillId).OrderBy(m => m.Id).ToList();

            if (excludeList != null)
            {
                suitConfigs = suitConfigs.Where(m => !excludeList.Contains(m.Id)).ToList();
            }

            foreach (SkillSuitConfig config in suitConfigs)
            {
                SkillSuit suit = new SkillSuit(config.Id);
                if (suitList.Count < suitCount)
                {
                    suitList.Add(suit);
                }
            }

            return suitList;
        }
    }
}
