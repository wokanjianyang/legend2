using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Expert : ASkill
    {
        //����ר������
        public Skill_Expert(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel)
        {
            this.skillGraphic = null;
        }

        public override void Do()
        {

        }

        public override bool IsCanUse()
        {
            return false;
        }
    }
}
