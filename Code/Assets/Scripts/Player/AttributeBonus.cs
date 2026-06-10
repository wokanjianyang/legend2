using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Game
{
    public class AttributeBonus
    {
        private Dictionary<AttributeEnum, Dictionary<int, double>> AllAttrDict = new Dictionary<AttributeEnum, Dictionary<int, double>>();

        private Dictionary<AttributeEnum, Dictionary<int, double>> SkillDict = new Dictionary<AttributeEnum, Dictionary<int, double>>();

        public Dictionary<AttributeEnum, Dictionary<int, double>> BuffDict = new Dictionary<AttributeEnum, Dictionary<int, double>>();

        public AttributeBonus()
        {
            foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
            {
                AllAttrDict.Add(item, new Dictionary<int, double>());
                SkillDict.Add(item, new Dictionary<int, double>());
            }
        }

        public void SetSkillAttr(AttributeEnum attrType, int attrKey, double attrValue)
        {
            SkillDict[attrType][attrKey] = attrValue;
        }

        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, double attrValue)
        {
            int key = (int)attrKey;
            AllAttrDict[attrType][key] = attrValue;
        }

        public void SetAttr(AttributeEnum attrType, int attrKey, double attrValue)
        {
            AllAttrDict[attrType][attrKey] = attrValue;
        }


        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, int Position, double attrValue)
        {
            int key = ((int)attrKey) * 99999 + Position;
            AllAttrDict[attrType][key] = attrValue;
        }

        //获取最终战斗属性
        public double CalBattleTotalAttr(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalBattleSingleAttr(AttributeEnum.HP);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaHp) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateHp) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaPhyAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RatePhyAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulPhyAtk);
                    break;
                case AttributeEnum.MagicAtk:
                    total = CalBattleSingleAttr(AttributeEnum.MagicAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaMagicAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RateMagicAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulMagicAtk);
                    break;
                case AttributeEnum.SpiritAtk:
                    total = CalBattleSingleAttr(AttributeEnum.SpiritAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaSpiritAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RateSpiritAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulSpiritAtk);
                    break;
                case AttributeEnum.Def:
                    total = CalBattleSingleAttr(AttributeEnum.Def);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaDef) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateDef) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulDef);

                    double decreDef = CalBattleSingleDiv(AttributeEnum.decreDivDef);
                    total = total / decreDef;
                    break;
                case AttributeEnum.Strong:
                    total = CalBattleSingleAttr(AttributeEnum.Strong);
                    total *= CalBattleSingleMul(AttributeEnum.MulStrong);
                    break;
                case AttributeEnum.PhyDamage:
                    total = CalBattleSingleAttr(AttributeEnum.PhyDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RatePhyDamage) / 100.0);
                    break;
                case AttributeEnum.MagicDamage:
                    total = CalBattleSingleAttr(AttributeEnum.MagicDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateMagicDamage) / 100.0);
                    break;
                case AttributeEnum.SpiritDamage:
                    total = CalBattleSingleAttr(AttributeEnum.SpiritDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateSpiritDamage) / 100.0);
                    break;
                case AttributeEnum.DamageIncrea:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.DamageResist:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.DecreExtraDamage:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.CritRate:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.CritDamage:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.CritRateResist:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.CritDamageResist:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.DeadlyRate:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.DeadlyDamage:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.Lucky:
                case AttributeEnum.Curse:
                case AttributeEnum.Accuracy:
                case AttributeEnum.Miss:
                case AttributeEnum.Miss2:
                case AttributeEnum.CardDamage:
                case AttributeEnum.FashionDamage:
                case AttributeEnum.LegacyDamage:
                case AttributeEnum.AchievementDamage:
                case AttributeEnum.ExclusiveDamage:
                case AttributeEnum.Speed:
                case AttributeEnum.DecreRestore:
                case AttributeEnum.RestoreIncrea:
                    total = CalBattleSingleAttr(attrType);
                    break;
                default:
                    Debug.LogError("not implete type:" + attrType.ToString());
                    throw new Exception();
                    break;

            }

            return total;
        }

        //获取单项战斗属性
        private double CalBattleSingleAttr(AttributeEnum type, params AttributeEnum[] increaTypes)
        {
            double total = 0;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
                {
                    total += item.Value;
                }
            }

            if (SkillDict.ContainsKey(type))
            {
                foreach (var item in SkillDict[type])
                {
                    total += item.Value;
                }
            }

            if (BuffDict.ContainsKey(type))
            {
                foreach (var item in BuffDict[type])
                {
                    total += item.Value;
                }
            }


            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum increaType = increaTypes[i];

                foreach (var item in AllAttrDict[increaType])
                {
                    total += item.Value;
                }

                if (SkillDict.ContainsKey(increaType))
                {
                    foreach (var item in SkillDict[increaType])
                    {
                        total += item.Value;
                    }
                }

                if (BuffDict.ContainsKey(increaType))
                {
                    foreach (var item in BuffDict[increaType])
                    {
                        total += item.Value;
                    }
                }
            }

            return total;
        }

        //获取单项战斗倍率
        public double CalBattleSingleMul(AttributeEnum type)
        {
            double total = 1;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
                {
                    total *= (1 + item.Value / 100.0);
                }
            }

            if (SkillDict.ContainsKey(type))
            {
                foreach (var item in SkillDict[type])
                {
                    total *= (1 + item.Value / 100.0);
                }
            }

            if (BuffDict.ContainsKey(type))
            {
                foreach (var item in BuffDict[type])
                {
                    total *= (1 + item.Value / 100.0);
                }
            }

            return total;
        }

        public double CalBattleSingleDiv(AttributeEnum type)
        {
            double total = 1;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
                {
                    total = total / (1 - item.Value / 100.0);
                }
            }

            if (SkillDict.ContainsKey(type))
            {
                foreach (var item in SkillDict[type])
                {
                    total = total / (1 - item.Value / 100.0);
                }
            }

            if (BuffDict.ContainsKey(type))
            {
                foreach (var item in BuffDict[type])
                {
                    total = total / (1 - item.Value / 100.0);
                }
            }

            return total;
        }

        //获取最终面板属性
        public double CalPanelTotalAttr(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalBattleSingleAttr(AttributeEnum.HP);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaHp) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateHp) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaPhyAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RatePhyAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulPhyAtk);
                    break;
                case AttributeEnum.MagicAtk:
                    total = CalBattleSingleAttr(AttributeEnum.MagicAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaMagicAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RateMagicAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulMagicAtk);
                    break;
                case AttributeEnum.SpiritAtk:
                    total = CalBattleSingleAttr(AttributeEnum.SpiritAtk) + CalBattleSingleAttr(AttributeEnum.Atk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaAtk, AttributeEnum.IncreaSpiritAtk) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateAtk, AttributeEnum.RateSpiritAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAtk);
                    total *= CalBattleSingleMul(AttributeEnum.MulSpiritAtk);
                    break;
                case AttributeEnum.Def:
                    total = CalBattleSingleAttr(AttributeEnum.Def);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.IncreaDef) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateDef) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulDef);
                    break;
                case AttributeEnum.Strong:
                    total = CalBattleSingleAttr(AttributeEnum.Strong);
                    total *= CalBattleSingleMul(AttributeEnum.MulStrong);
                    break;
                case AttributeEnum.PhyDamage:
                    total = CalBattleSingleAttr(AttributeEnum.PhyDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RatePhyDamage) / 100.0);
                    break;
                case AttributeEnum.MagicDamage:
                    total = CalBattleSingleAttr(AttributeEnum.MagicDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateMagicDamage) / 100.0);
                    break;
                case AttributeEnum.SpiritDamage:
                    total = CalBattleSingleAttr(AttributeEnum.SpiritDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateSpiritDamage) / 100.0);
                    break;
                case AttributeEnum.CritRate:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.Lucky:
                    total = CalBattleSingleAttr(attrType);
                    break;
                default:
                    total = CalPanelSingleAttr(attrType);
                    break;

            }

            return total;
        }

        //获取单项面板属性
        public double CalPanelSingleAttr(AttributeEnum type, params AttributeEnum[] increaTypes)
        {
            double total = 0;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
                {
                    total += item.Value;
                }
            }

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum increaType = increaTypes[i];

                foreach (var item in AllAttrDict[increaType])
                {
                    total += item.Value;
                }
            }

            return total;
        }

        //获取单项面板倍率
        public double CalPanelSingleMul(AttributeEnum type)
        {
            double total = 100;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
                {
                    total *= (1 + item.Value / 100.0);
                }
            }

            return total - 100;
        }

        //获取（物攻，魔法，道术）的战斗属性，技能伤害用
        public double CalBattleRoleAtk(int role)
        {
            double attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = CalBattleTotalAttr(AttributeEnum.PhyAtk);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = CalBattleTotalAttr(AttributeEnum.MagicAtk);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = CalBattleTotalAttr(AttributeEnum.SpiritAtk);
                        break;
                    }
            }

            return attack;
        }

        //获取（物攻，魔法，道术）的面本属性，召唤用
        public double CalBaseRoleAtk(int role)
        {
            double attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = CalPanelTotalAttr(AttributeEnum.PhyAtk);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = CalPanelTotalAttr(AttributeEnum.MagicAtk);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = CalPanelTotalAttr(AttributeEnum.SpiritAtk);
                        break;
                    }
            }

            return attack;
        }

        //获取（物攻，魔法，道术）的战斗属性中的最大值，普攻技能用
        public double CalBattleMaxAtk()
        {
            double atk = 0;

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.PhyAtk));

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.MagicAtk));

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.SpiritAtk));

            return atk;
        }

        //获取（物攻，魔法，道术）的战斗属性中的最大值，计算战力用
        public double CalBaseMaxAtk()
        {
            double atk = 0;

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.PhyAtk));

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.MagicAtk));

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.SpiritAtk));

            return atk;
        }

        public double GetPower()
        {
            double power = 0;

            double atk = CalBaseMaxAtk();
            power += atk;

            AttributeEnum[] DamageLis = { AttributeEnum.CardDamage, AttributeEnum.FashionDamage, AttributeEnum.AchievementDamage, AttributeEnum.LegacyDamage, AttributeEnum.ExclusiveDamage };
            foreach (var sp in DamageLis)
            {
                double dm = CalPanelTotalAttr(sp);
                if (dm > 0)
                {
                    power *= (1 + dm / 100.0);
                }
            }
            double def = CalPanelTotalAttr(AttributeEnum.Def);

            power += def * 5;

            double hp = CalPanelTotalAttr(AttributeEnum.HP);

            power += hp / 100;

            return power;
        }

        public string GetPowerText()
        {
            return StringHelper.FormatNumber(GetPower());

            //return GetPowerNew().FormatUnit();
        }


        private double CalTotal(AttributeEnum type, bool haveBuff, params AttributeEnum[] increaTypes)
        {
            double total = 0;

            foreach (double hp in AllAttrDict[type].Values)
            {
                total += hp;
            }

            if (haveBuff && SkillDict.ContainsKey(type))
            {
                foreach (var item in SkillDict[type])
                {
                    total += item.Value;
                }
            }

            double percent = 0;

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum percentType = increaTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    percent += pc;
                }

                if (haveBuff && SkillDict.ContainsKey(percentType))
                {
                    foreach (var item in SkillDict[type])
                    {
                        total += item.Value;
                    }
                }
            }
            return total * (100.0 + percent) / 100.0;
        }

        public double CalMulTotal(bool haveBuff, params AttributeEnum[] mulTypes)
        {
            double total = 100;

            for (int i = 0; i < mulTypes.Length; i++)
            {
                AttributeEnum percentType = mulTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    total *= (100.0 + pc) / 100.0;
                }

                if (haveBuff && SkillDict.ContainsKey(percentType))
                {
                    foreach (var item in SkillDict[percentType])
                    {
                        total *= (100.0 + item.Value) / 100.0;
                    }
                }
            }

            return total - 100;
        }
    }
}