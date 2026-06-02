using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 发球效果
    /// </summary>
    public class Effect_Dot : Effect_Compent
    {
        public Effect_Dot(SkillPanel sp, Effect_Data data) : base(sp, data)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {
            if (this.Duration > 1)
            {
                //效果持续
                target.AddEffect(this);
            }
            else
            {
                //效果就只一个回合
                double lossHp = damage * Percent / 100.0;

                DamageResult dr = new DamageResult(FromId, lossHp, MsgType.Effect, RoleType.All);

                self.OnHit(dr);
            }
        }
    }
}
