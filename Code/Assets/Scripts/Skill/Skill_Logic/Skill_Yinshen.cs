using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Yinshen : ASkill
    {
        public Skill_Yinshen(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            this.skillGraphic = new SkillGraphic_Hide(player, skill);
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do(SkillRunType runType)
        {
            ToHide();
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //先行特效
            SkillPanel.RunBefore(this.SelfPlayer, null);
        }

        private void ToHide()
        {
            this.SelfPlayer.IsHide = true;
        }
    }
}
