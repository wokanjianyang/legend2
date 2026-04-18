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
            return this.list.Where(m => ids.Contains(m.SkillId)).ToList();

        }
    }


}
