using System;
using System.Text;
using UnityEngine;

namespace Game
{
    public class DamageHelper
    {
        private static AttributeEnum[] DamageLis = { AttributeEnum.CardDamage, AttributeEnum.FashionDamage, AttributeEnum.AchievementDamage, AttributeEnum.LegacyDamage, AttributeEnum.ExclusiveDamage };

        public static DamageResult CalcDamage(AttributeBonus attcher, AttributeBonus enemy, SkillPanel skill)
        {
            //计算公式  ((攻击 - 防御) * 百分比系数 + 固定数值) * 暴击?.暴击倍率 * (伤害加成-伤害减免) * (幸运)

            int role = skill.SkillData.SkillConfig.Role;

            double atk = 0;

            if (role == 0)
            {
                //职业攻击，取对应攻击
                atk = attcher.CalBattleMaxAtk();
            }
            else
            {
                //普攻,取最大3系最大攻击
                atk = attcher.CalBattleRoleAtk(role);
            }

            //技能百分比
            atk = atk * skill.Percent / 100.0;

            //技能攻击加成
            atk *= (1 + skill.AttrIncrea / 100.0);

            //lucky
            int lucky = (int)(attcher.CalBattleTotalAttr(AttributeEnum.Lucky) - enemy.CalBattleTotalAttr(AttributeEnum.Curse));

            //防御减伤为 攻击/防御*0.75
            double def = enemy.CalBattleTotalAttr(AttributeEnum.Def);

            if (def >= 1)
            {
                //诅咒对防御的影响
                if (lucky < 0)
                {
                    double curseRate = CalCurseRate(-lucky);
                    def *= curseRate;
                }

                atk = atk * atk / (atk + def);
            }


            //技能固伤不受防御影响
            atk += skill.Damage;
            if (lucky > 0)
            {
                double luckyRate = CalLuckyRate(lucky);

                atk *= luckyRate;
            }

            //技能终伤
            atk *= (1 + skill.FinalIncrea / 100.0);

            MsgType type = MsgType.Damage;

            //致命
            int deadlyRate = (int)((attcher.CalBattleTotalAttr(AttributeEnum.DeadlyRate) + skill.DeadlyRate));
            if (RandomHelper.RandomCritRate(deadlyRate))
            {
                int deadlyDamage = (int)(attcher.CalBattleTotalAttr(AttributeEnum.DeadlyDamage) + skill.DeadlyDamage);
                atk *= (1 + deadlyDamage / 100.0);

                type = MsgType.Crit;
            }

            //暴击
            int critRate = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.CalBattleTotalAttr(AttributeEnum.CritRateResist));
            if (RandomHelper.RandomCritRate(critRate))
            {
                long critDamage = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.CalBattleTotalAttr(AttributeEnum.CritDamageResist));
                atk *= (1 + critDamage / 100.0);

                type = MsgType.Crit;
            }

            //特殊增伤
            foreach (var sp in DamageLis)
            {
                double dm = attcher.CalBattleTotalAttr(sp);
                if (dm > 0)
                {
                    atk *= (1 + dm / 100.0);
                }
            }

            //伤害加成-伤害减免
            double dma = attcher.CalBattleTotalAttr(AttributeEnum.DamageIncrea) - enemy.CalBattleTotalAttr(AttributeEnum.DamageResist);
            if (dma >= 0)
            {
                atk *= (1 + dma / 100.0);
            }
            else
            {
                atk = atk / (1 - (dma / 100.0));
            }

            //承受者的易伤
            double extraDamage = enemy.CalBattleTotalAttr(AttributeEnum.ExtraDamage);
            if (extraDamage > 0)
            {
                atk *= 1 + extraDamage / 100.0;
            }

            //Debug.Log("attack:" + StringHelper.FormatNumber(attack));

            //强制最少1点伤害
            return new DamageResult(Math.Max(1, atk), 0, type, (RoleType)role, skill.SkillId); //
        }

        public static double CalLuckyRate(int lucky)
        {
            if (lucky <= 0)
            {
                return 1;
            }
            if (lucky >= 9)
            {
                return 5;
            }

            int max = lucky * 400 / 9;
            int rd = RandomHelper.RandomNumber(0, max);

            return rd / 100.0 + 1;
        }

        public static double CalCurseRate(int curse)
        {
            if (curse <= 0)
            {
                return 1;
            }
            if (curse >= 9)
            {
                return 50;
            }

            int max = curse * 4000 / 9;
            int rd = RandomHelper.RandomNumber(0, max);

            return rd / 100.0 + 1;
        }

        public static bool IsMiss(APlayer self, APlayer enemy, double skillAccuracy)
        {
            double accuracy = self.AttributeBonus.CalBattleTotalAttr(AttributeEnum.Accuracy) + skillAccuracy;
            double miss = enemy.AttributeBonus.CalBattleTotalAttr(AttributeEnum.Miss);

            double rate = 100 + accuracy - miss;

            rate = Math.Max(rate, 20); //闪避最高80%

            //Debug.Log("miss rate:" + rate);

            return !RandomHelper.RandomCritRate((int)rate);
        }

        public static bool IsMiss2(APlayer self, APlayer enemy)
        {
            double miss = enemy.AttributeBonus.CalBattleTotalAttr(AttributeEnum.Miss2);

            double rate = Math.Min(miss, 95); //闪避最高95%

            //Debug.Log("miss2 rate:" + rate);

            return RandomHelper.RandomCritRate((int)rate);
        }
    }

    public class DamageResult
    {
        public DamageResult(double damage, double extendDamage, MsgType type, RoleType roleType, int skillId)
        {
            this.Damage = damage;
            this.ExtendDamage = extendDamage;
            this.Type = type;
            this.RoleType = roleType;
            this.SkillId = skillId;
        }

        public DamageResult(int formId, double damage, MsgType type, RoleType roleType)
        {
            this.FromId = formId;
            this.Damage = damage;
            this.Type = type;
            this.RoleType = roleType;
        }

        public MsgType Type { get; set; }

        public RoleType RoleType { get; set; }
        public double Damage { get; set; }
        public double ExtendDamage { get; set; }
        public int FromId { get; set; }

        public int SkillId { get; set; }
    }
}