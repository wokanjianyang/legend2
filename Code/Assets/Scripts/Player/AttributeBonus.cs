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
            int key = (int)attrKey;
            SkillDict[attrType][key] = attrValue;
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
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.HpIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateHp) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RatePhyAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrPhy);
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalBattleSingleAttr(AttributeEnum.MagicAtt);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RateMagicAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrMagic);
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RateSpiritAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrSpirit);
                    break;
                case AttributeEnum.Def:
                    total = CalBattleSingleAttr(AttributeEnum.Def);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.DefIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateDef) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulDef);
                    break;
                case AttributeEnum.Strong:
                    total = CalBattleSingleAttr(AttributeEnum.Strong);
                    total *= CalBattleSingleMul(AttributeEnum.StrongMul);
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
                case AttributeEnum.ExtraDamage:
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

        //获取最终面板属性
        public double CalPanelTotalAttr(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalBattleSingleAttr(AttributeEnum.HP);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.HpIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateHp) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RatePhyAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrPhy);
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalBattleSingleAttr(AttributeEnum.MagicAtt);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RateMagicAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrMagic);
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalBattleSingleAttr(AttributeEnum.PhyAtk);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateaAtk, AttributeEnum.RateSpiritAtk) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttr);
                    total *= CalBattleSingleMul(AttributeEnum.MulAttrSpirit);
                    break;
                case AttributeEnum.Def:
                    total = CalBattleSingleAttr(AttributeEnum.Def);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.DefIncrea) / 100.0);
                    total *= (1 + CalBattleSingleAttr(AttributeEnum.RateDef) / 100.0);
                    total *= CalBattleSingleMul(AttributeEnum.MulDef);
                    break;
                case AttributeEnum.Strong:
                    total = CalBattleSingleAttr(AttributeEnum.Strong);
                    total *= CalBattleSingleMul(AttributeEnum.StrongMul);
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
                        attack = CalBattleTotalAttr(AttributeEnum.MagicAtt);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = CalBattleTotalAttr(AttributeEnum.SpiritAtt);
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
                        attack = CalPanelTotalAttr(AttributeEnum.MagicAtt);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = CalPanelTotalAttr(AttributeEnum.SpiritAtt);
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

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.MagicAtt));

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.SpiritAtt));

            return atk;
        }

        //获取（物攻，魔法，道术）的战斗属性中的最大值，计算战力用
        public double CalBaseMaxAtk()
        {
            double atk = 0;

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.PhyAtk));

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.MagicAtt));

            atk = Math.Max(atk, CalPanelTotalAttr(AttributeEnum.SpiritAtt));

            return atk;
        }

        public double GetPower()
        {
            double atk = CalBaseMaxAtk();



            return atk;
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