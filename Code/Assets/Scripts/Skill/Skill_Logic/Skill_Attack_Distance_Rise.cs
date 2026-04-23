using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Distance_Rise : Skill_Attack_Area
    {

        public Skill_Attack_Distance_Rise(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel, isShow)
        {

        }

        public override void Do(SkillRunType runType)
        {
            DamageResult baseDr = null;

            List<Vector3Int> playCells = GetPlayCells();
            this.skillGraphic?.PlayAnimation(playCells);

            SkillState orbState = this.SelfPlayer.GetSkillByPriority(-100);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    if (DamageHelper.IsMiss(SelfPlayer, enemy, SkillPanel.Accuracy))
                    {
                        enemy.ShowMiss();
                        continue;
                    }

                    //先行特效
                    SkillPanel.RunBefore(this.SelfPlayer, enemy);

                    int distance = this.CalDistance(SelfPlayer.Cell, enemy.Cell);
                    //Debug.Log("distance:" + distance + " rise percent:" + percent);

                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, SkillPanel);

                    //Debug.Log("base damage:" + dr.Damage);

                    dr.Damage = dr.Damage;

                    dr.FromId = attackData.Tid;
                    enemy.OnHit(dr);

                    if (enemy.ID == SelfPlayer.Enemy.ID)
                    {
                        baseDr = dr;
                    }

                    //后行特效
                    SkillPanel.RunAfter(this.SelfPlayer, enemy, dr);
                }
            }

            if (orbState != null)
            {
                orbState.Do();
            }

            DoChediding(runType, baseDr);
        }

    }
}
