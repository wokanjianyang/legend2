using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {
        public SkillRuneConfig RandomEquipRuneId(int quality, int role, int seed)
        {
            List<SkillRuneConfig> tempList = this.list.Where(m => (m.Role == role) && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            if (tempList.Count == 0)
            {
                return null;
            }

            if (tempList.Count == 1)
            {
                return tempList[0];
            }

            List<int> rates = tempList.Select(m => m.EquipRate).ToList();
            int rd = RandomHelper.RandomListRateIndex(rates);

            return tempList[rd];
        }


        public List<SkillRune> GetAllRune(int skillId, int runeCount)
        {
            List<SkillRune> runeList = new List<SkillRune>();

            List<SkillRuneConfig> runeConfigs = this.list.Where(m => m.SkillId == skillId).OrderBy(m => m.Id).ToList();

            foreach (SkillRuneConfig config in runeConfigs)
            {
                SkillRune skillRune = new SkillRune(config.Id, runeCount);
                runeList.Add(skillRune);
            }
            return runeList;
        }

        public List<SkillRuneConfig> GetSkillAllConfigs(int skillId, int skillLayer)
        {
            return this.list.Where(m => (m.SkillId == skillId)).ToList();
        }


    }


}
