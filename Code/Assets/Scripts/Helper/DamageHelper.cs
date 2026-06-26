using System;
using System.Text;
using UnityEngine;

namespace Game
{
    public class DamageHelper
    {
        private static AttributeEnum[] DamageLis = { AttributeEnum.CardDamage, AttributeEnum.FashionDamage, AttributeEnum.AchievementDamage, AttributeEnum.LegacyDamage, AttributeEnum.ExclusiveDamage, AttributeEnum.BabelDamage };

        public static DamageResult CalcDamage(AttributeBonus attcher, AttributeBonus enemy, SkillPanel skill)
        {
            //计算公式  ((攻击 - 防御) * 百分比系数 + 固定数值) * 暴击?.暴击倍率 * (伤害加成-伤害减免) * (幸运)

            DamageResult dr = new DamageResult();

            int role = skill.Config.Role;

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

                atk = atk * atk / (atk + def * ConfigHelper.Def_Rate);
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

            //致命
            int deadlyRate = (int)((attcher.CalBattleTotalAttr(AttributeEnum.DeadlyRate) + skill.DeadlyRate));
            if (RandomHelper.RandomCritRate(deadlyRate))
            {
                int deadlyDamage = (int)(attcher.CalBattleTotalAttr(AttributeEnum.DeadlyDamage) + skill.DeadlyDamage);
                atk *= (1 + deadlyDamage / 100.0);

                dr.IsDeadly = true;
            }

            //暴击
            int critRate = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.CalBattleTotalAttr(AttributeEnum.CritRateResist));
            if (RandomHelper.RandomCritRate(critRate))
            {
                long critDamage = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.CalBattleTotalAttr(AttributeEnum.CritDamageResist));
                atk *= (1 + critDamage / 100.0);

                dr.IsCrit = true;
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
            double extraDamage = enemy.CalBattleTotalAttr(AttributeEnum.DecreExtraDamage);
            if (extraDamage > 0)
            {
                atk *= 1 + extraDamage / 100.0;
            }

            //增伤倍率
            double mdi = attcher.CalBattleTotalAttr(AttributeEnum.MulDamageIncrea);
            if (mdi > 1)
            {
                atk *= mdi;
            }

            //减伤倍率
            double mdr = enemy.CalBattleTotalAttr(AttributeEnum.MulDamageResist);
            if (mdr > 1)
            {
                atk = atk / mdr;
            }

            //Debug.Log("attack:" + StringHelper.FormatNumber(attack));

            dr.Damage = Math.Max(1, atk);             //强制最少1点伤害
            dr.ExtendDamage = 0;
            dr.RoleType = (RoleType)role;
            dr.SkillId = skill.SkillId;
            dr.FromId = 0;
            dr.Type = (dr.IsCrit || dr.IsDeadly) ? MsgType.Crit : MsgType.Damage;

            DoBuff(attcher, enemy, dr);

            return dr;
        }

        public static void DoBuff(AttributeBonus attcher, AttributeBonus enemy, DamageResult dr)
        {
            int bf1_rate = (int)attcher.CalBattleSingleAdd(AttributeEnum.Buff1_Rate);
            if (bf1_rate > 0)
            {
                if (RandomHelper.RandomCritRate(bf1_rate))
                {
                    double bf1_vue = 1 + attcher.CalBattleSingleAdd(AttributeEnum.Buff1_Vue) / 100.0;
                    dr.Damage *= bf1_vue;
                }
            }

            if (dr.IsCrit)
            {
                double bf2_vue = attcher.CalBattleSingleAdd(AttributeEnum.Buff2_Vue);
                if (bf2_vue > 0)
                {
                    dr.Damage *= 1 + bf2_vue / 100.0;
                }
            }

            double bf3_vue1 = attcher.CalBattleSingleAdd(AttributeEnum.Buff3_Vue1);
            if (bf3_vue1 > 0)
            {
                double bf3_vue2 = attcher.CalBattleSingleAdd(AttributeEnum.Buff3_Vue2);
                double bf3_vue3 = attcher.CalBattleSingleAdd(AttributeEnum.Buff3_Vue3);
                double bf3_vue4 = attcher.CalBattleSingleAdd(AttributeEnum.Buff3_Vue4);
                double bf3_vue5 = attcher.CalBattleSingleAdd(AttributeEnum.Buff3_Vue5);

                int rd = RandomHelper.RandomNumber(0, 1000);
                if (rd < 1)
                {
                    dr.Damage *= 1 + (bf3_vue1) / 100.0;
                }
                else if (rd < 11)
                {
                    dr.Damage *= 1 + bf3_vue2 / 100.0;
                }
                else if (rd < 111)
                {
                    dr.Damage *= 1 + bf3_vue3 / 100.0;
                }
                else if (rd < 511)
                {
                    dr.Damage *= 1 + bf3_vue4 / 100.0;
                }
                else
                {
                    dr.Damage *= Math.Max(1 - bf3_vue5 / 100.0, 0);
                }
            }

            if (dr.IsDeadly)
            {
                double bf4_vue = attcher.CalBattleSingleAdd(AttributeEnum.Buff4_Vue);
                if (bf4_vue > 0)
                {
                    dr.Damage *= 1 + bf4_vue / 100.0;
                }
            }
        }

        public static double CalLuckyRate(int lucky)
        {
            if (lucky <= 0)
            {
                return 1;
            }
            if (lucky >= 15)
            {
                return 5;
            }

            int max = lucky * 400 / 15;
            int rd = RandomHelper.RandomNumber(0, max);

            return rd / 100.0 + 1;
        }

        public static double CalCurseRate(int curse)
        {
            if (curse <= 0)
            {
                return 1;
            }
            if (curse >= 15)
            {
                return 10;
            }

            int max = curse * 800 / 15;
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
        public DamageResult()
        {
        }

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

        public bool IsCrit { get; set; }

        public bool IsDeadly { get; set; }
    }
}