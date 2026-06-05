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

            //增加护盾
            double percent = this.SkillPanel.Percent * (1 + this.SkillPanel.AttrIncrea / 100.0);

            double maxHp = this.SelfPlayer.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
            double sp = maxHp * percent / 100.0;

            //Debug.Log("maxHp:" + maxHp + " sp:" + sp);

            this.SelfPlayer.AddSP(sp);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }
    }
}
