using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Shield : ASkill
    {
        public Skill_Shield(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Shield(player, skill);
            }
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do(SkillRunType runType)
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //先行特效
            SkillPanel.RunBefore(this.SelfPlayer, null);
        }
    }
}
