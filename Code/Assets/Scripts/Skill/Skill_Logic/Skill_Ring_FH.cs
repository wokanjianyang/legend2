using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Ring_FH : ASkill
    {
        public Skill_Ring_FH(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do(SkillRunType runType)
        {
            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
            {
                Type = MsgType.Ring,
                Content = SkillPanel.SkillData.SkillConfig.Name
            });

            //先行特效
            SkillPanel.RunBefore(this.SelfPlayer, null);

            double percent = this.SkillPanel.Percent;
            double maxHp = this.SelfPlayer.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
            double hp = maxHp * percent / 100.0;

            this.SelfPlayer.SetHP(hp);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }
    }
}
