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
                case AttributeEnum.DamageResist:
                case AttributeEnum.DecreExtraDamage:
                case AttributeEnum.CritRate:
                case AttributeEnum.CritDamage:
                case AttributeEnum.CritRateResist:
                case AttributeEnum.CritDamageResist:
                case AttributeEnum.DeadlyRate:
                case AttributeEnum.DeadlyDamage:
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
                case AttributeEnum.BabelDamage:
                case AttributeEnum.Speed:
                case AttributeEnum.DecreRestore:
                case AttributeEnum.RestoreIncrea:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.MulDamageIncrea:
                case AttributeEnum.MulDamageResist:
                    total = CalBattleSingleMul(attrType);
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

        //最终面板属性
        public double CalPanelTotalAttr(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalPanelAtr(AttributeEnum.HP);
                    total *= (1 + CalPanelAtr(AttributeEnum.IncreaHp) / 100.0);
                    total *= (1 + CalPanelAtr(AttributeEnum.RateHp) / 100.0);
                    total *= CalPanelAtr(AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalPanelAtr(AttributeEnum.PhyAtk) + CalPanelAtr(AttributeEnum.Atk);
                    total *= (1 + (CalPanelAtr(AttributeEnum.IncreaAtk) + CalPanelAtr(AttributeEnum.IncreaPhyAtk)) / 100.0);
                    total *= (1 + (CalPanelAtr(AttributeEnum.RateAtk) + CalPanelAtr(AttributeEnum.RatePhyAtk)) / 100.0);
                    total *= CalPanelAtr(AttributeEnum.MulAtk);
                    total *= CalPanelAtr(AttributeEnum.MulPhyAtk);
                    break;
                case AttributeEnum.MagicAtk:
                    total = CalPanelAtr(AttributeEnum.MagicAtk) + CalPanelAtr(AttributeEnum.Atk);
                    total *= (1 + (CalPanelAtr(AttributeEnum.IncreaAtk) + CalPanelAtr(AttributeEnum.IncreaMagicAtk)) / 100.0);
                    total *= (1 + (CalPanelAtr(AttributeEnum.RateAtk) + CalPanelAtr(AttributeEnum.RateMagicAtk)) / 100.0);
                    total *= CalPanelAtr(AttributeEnum.MulAtk);
                    total *= CalPanelAtr(AttributeEnum.MulMagicAtk);
                    break;
                case AttributeEnum.SpiritAtk:
                    total = CalPanelAtr(AttributeEnum.SpiritAtk) + CalPanelAtr(AttributeEnum.Atk);
                    total *= (1 + (CalPanelAtr(AttributeEnum.IncreaAtk) + CalPanelAtr(AttributeEnum.IncreaSpiritAtk)) / 100.0);
                    total *= (1 + (CalPanelAtr(AttributeEnum.RateAtk) + CalPanelAtr(AttributeEnum.RateSpiritAtk)) / 100.0);
                    total *= CalPanelAtr(AttributeEnum.MulAtk);
                    total *= CalPanelAtr(AttributeEnum.MulSpiritAtk);
                    break;
                case AttributeEnum.Def:
                    total = CalPanelAtr(AttributeEnum.Def);
                    total *= (1 + CalPanelAtr(AttributeEnum.IncreaDef) / 100.0);
                    total *= (1 + CalPanelAtr(AttributeEnum.RateDef) / 100.0);
                    total *= CalPanelAtr(AttributeEnum.MulDef);
                    break;
                case AttributeEnum.Strong:
                    total = CalPanelAtr(AttributeEnum.Strong);
                    total *= CalPanelAtr(AttributeEnum.MulStrong);
                    break;
                case AttributeEnum.PhyDamage:
                    total = CalPanelAtr(AttributeEnum.PhyDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RatePhyDamage) / 100.0);
                    break;
                case AttributeEnum.MagicDamage:
                    total = CalPanelAtr(AttributeEnum.MagicDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateMagicDamage) / 100.0);
                    break;
                case AttributeEnum.SpiritDamage:
                    total = CalPanelAtr(AttributeEnum.SpiritDamage);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateSpiritDamage) / 100.0);
                    break;
                default:
                    total = CalPanelAtr(attrType);
                    break;

            }

            return total;
        }

        //计算最终面板的中间方法
        public double CalPanelAtr(AttributeEnum type)
        {
            int t = (int)type;

            if (t <= -10000)
            {
                return 1 + CalPanelSingleMul(type) / 100.0;
            }
            else if (t > -10000 && t < 0)
            {
                return CalPanelSingleAdd(type);
            }
            else if (t >= 0 && t < 10000)
            {
                return CalPanelSingleAdd(type);
            }
            else if (t >= 10000 && t < 20000)
            {
                return 1 + CalPanelSingleMul(type) / 100.0;
            }
            else if (t >= 20000)
            {
                return CalPanelSingleAdd(type);
            }

            return 0;
        }

        //高级面板属性
        public double CalPanelSingleAtr(AttributeEnum type)
        {
            int t = (int)type;

            if (t <= -10000)
            {
                return CalPanelSingleMul(type);
            }
            else if (t < 0)
            {
                return CalPanelSingleAdd(type);
            }
            else if (t < 10000)
            {
                return CalPanelSingleAdd(type);
            }
            else if (t >= 10000)
            {
                return CalPanelSingleMul(type);
            }

            return 0;
        }

        //获取单项面板加法属性
        public double CalPanelSingleAdd(AttributeEnum type)
        {
            double total = 0;

            if (AllAttrDict.ContainsKey(type))
            {
                foreach (var item in AllAttrDict[type])
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

            AttributeEnum[] DamageLis = { AttributeEnum.CardDamage, AttributeEnum.FashionDamage, AttributeEnum.AchievementDamage, AttributeEnum.LegacyDamage, AttributeEnum.ExclusiveDamage, AttributeEnum.BabelDamage };
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