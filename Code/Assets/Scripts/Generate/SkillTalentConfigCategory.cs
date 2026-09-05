using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class SkillTalentConfigCategory
    {


        public List<SkillTalentConfig> GetSkillAllConfigs(int skillId)
        {
            return this.list.Where(m => (m.SkillId == skillId)).ToList();

        }

        public List<SkillTalentConfig> GetSkillAllConfigs(List<int> ids)
        {
            return this.list.Where(m => ids.Contains(m.SkillId) && m.Role > 0).ToList();

        }

        public List<SkillTalent> GetAllTalent(int skillId, int count)
        {
            List<SkillTalent> talentList = new List<SkillTalent>();

            List<SkillTalentConfig> suitConfigs = this.list.Where(m => m.SkillId == skillId).OrderBy(m => m.Id).ToList();


            foreach (SkillTalentConfig config in suitConfigs)
            {
                SkillTalent talent = new SkillTalent(config.Id);
                if (talentList.Count < count)
                {
                    talentList.Add(talent);
                }
            }

            return talentList;

        }
    }


}
