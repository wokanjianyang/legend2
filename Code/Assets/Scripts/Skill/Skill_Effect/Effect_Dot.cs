using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Effect_Dot : Effect_Compent
    {
        public Effect_Dot(int configId, int fromId, double percent, long damage, int duration, int max) : base(configId, fromId, percent, damage, duration, max)
        {

        }

        public override void Do(APlayer self, APlayer target, double damage)
        {
            if (this.Duration > 1)
            {
                //效果持续
                Effect_Buff buff = new Effect_Buff(this.Config.Id, this.Damage, this.Percent, this.Duration, 1);

                target.AddEffect(this, buff);
            }
            else
            {
                //效果就只一个回合
                double lossHp = damage * Percent / 100;

                DamageResult dr = new DamageResult(FromId, lossHp, MsgType.Effect, RoleType.All);

                self.OnHit(dr);
            }
        }
    }
}
