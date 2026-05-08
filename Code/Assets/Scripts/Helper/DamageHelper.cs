using System;
using System.Text;
using UnityEngine;

namespace Game
{
    public class DamageHelper
    {
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

            //防御减伤为 攻击/防御*0.75
            double def = enemy.CalBattleTotalAttr(AttributeEnum.Def);

            if (def >= 1)
            {
                //诅咒对防御的影响
                double curse = attcher.CalBattleTotalAttr(AttributeEnum.Curse) - enemy.CalBattleTotalAttr(AttributeEnum.Lucky);
                if (curse > 0)
                {
                    double curseRate = CalLuckyRate((int)curse);
                    def *= curseRate;
                }

                atk = Math.Max(1, atk - def * ConfigHelper.Def_Rate);
            }


            //技能固伤不受防御影响
            atk += skill.Damage;

            //lucky
            double lucky = attcher.CalBattleTotalAttr(AttributeEnum.Lucky) - enemy.CalBattleTotalAttr(AttributeEnum.Curse);
            if (lucky > 0)
            {
                double luckyRate = CalLuckyRate((int)lucky);

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
            }

            //暴击
            int critRate = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.CalBattleTotalAttr(AttributeEnum.CritRateResist));
            if (RandomHelper.RandomCritRate(critRate))
            {
                long critDamage = (int)(attcher.CalBattleTotalAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.CalBattleTotalAttr(AttributeEnum.CritDamageResist));
                atk *= (1 + critDamage / 100.0);
            }

            //增伤
            atk *= (1 + attcher.CalBattleTotalAttr(AttributeEnum.DamageIncrea) / 100.0);

            //减伤
            atk = atk / (1 + attcher.CalBattleTotalAttr(AttributeEnum.DamageResist) / 100.0);

            //承受者的易伤
            double extraDamage = enemy.CalBattleTotalAttr(AttributeEnum.ExtraDamage);
            if (extraDamage > 0)
            {
                atk *= 1 + extraDamage / 100.0;
            }

            //Debug.Log("attack:" + StringHelper.FormatNumber(attack));

            //强制最少1点伤害
            return new DamageResult(Math.Max(1, atk), 0, MsgType.Damage, (RoleType)role, skill.SkillId); //
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