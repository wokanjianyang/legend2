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
                case AttributeEnum.CritRate:
                    total = CalBattleSingleAttr(attrType);
                    break;
                case AttributeEnum.Lucky:
                    total = CalBattleSingleAttr(attrType);
                    break;
                default:
                    Debug.LogError("not implete");
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
        public double CalBaseTotalAttr(AttributeEnum attrType)
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
                    Debug.LogError("not implete");
                    throw new Exception();
                    break;

            }

            return total;
        }

        //获取单项面板属性
        public double CalBaseSingleAttr(AttributeEnum type, params AttributeEnum[] increaTypes)
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
        public double CalBaseSingleMul(AttributeEnum type)
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

        public double CalBattleMaxAtk()
        {
            double atk = 0;

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.PhyAtk));

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.MagicAtt));

            atk = Math.Max(atk, CalBattleTotalAttr(AttributeEnum.SpiritAtt));

            return atk;
        }


        public long GetTotalAttr(AttributeEnum attrType)
        {
            return (long)GetTotalAttrDouble(attrType);
        }

        public long GetAttackAttr(AttributeEnum attrType)
        {
            return (long)GetTotalAttrDouble(attrType);
        }

        public double GetAttackDoubleAttr(AttributeEnum attrType)
        {
            return GetTotalAttrDouble(attrType);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType)
        {
            return GetTotalAttrDouble(attrType, true);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType, bool haveBuff)
        {
            double total = 0;
            double mr = 1;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, haveBuff, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulHp) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.PhyAtk:
                    total = CalTotal(AttributeEnum.PhyAtk, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrPhy) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrMagic) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrSpirit) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.Def:
                    total = CalTotal(AttributeEnum.Def, haveBuff, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulDef) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.Strong:
                    total = CalTotal(AttributeEnum.Strong, haveBuff);
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.StrongMul) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.PhyDamage:
                    total = 100 + CalTotal(AttributeEnum.PhyDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulPhyDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.CritRate:
                    total = CalTotal(attrType, haveBuff, AttributeEnum.RateCrit);
                    break;
                case AttributeEnum.Lucky:
                    total = CalTotal(attrType, haveBuff, AttributeEnum.RateLucky);
                    break;
                case AttributeEnum.MagicDamage:
                    total = 100 + CalTotal(AttributeEnum.MagicDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulMagicDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.SpiritDamage:
                    total = 100 + CalTotal(AttributeEnum.SpiritDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulSpiritDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.SecondExp:
                    total = CalTotal(AttributeEnum.SecondExp, haveBuff, AttributeEnum.ExpIncrea);
                    break;
                case AttributeEnum.SecondGold:
                    total = CalTotal(AttributeEnum.SecondGold, haveBuff, AttributeEnum.GoldIncrea);
                    break;
                case AttributeEnum.ExpIncrea:
                    total = CalTotal(AttributeEnum.ExpIncrea, haveBuff, AttributeEnum.RateExp);
                    break;
                case AttributeEnum.GoldIncrea:
                    total = CalTotal(AttributeEnum.GoldIncrea, haveBuff, AttributeEnum.RateGold);
                    break;
                case AttributeEnum.BurstIncrea:
                    total = CalTotal(AttributeEnum.BurstIncrea, haveBuff, AttributeEnum.RateBurst);
                    break;
                case AttributeEnum.QualityIncrea:
                    total = CalTotal(AttributeEnum.QualityIncrea, haveBuff, AttributeEnum.RateQuality);
                    break;
                default:
                    if ((int)attrType < 2001)
                    {
                        total = CalTotal(attrType, haveBuff);
                    }
                    else
                    {
                        total = CalMulTotal(haveBuff, attrType);
                    }
                    break;
            }

            return total;
        }

        public double GetPower()
        {
            double p1 = GetTotalAttrDouble(AttributeEnum.PhyAtk);
            double p2 = GetTotalAttrDouble(AttributeEnum.MagicAtt);
            double p3 = GetTotalAttrDouble(AttributeEnum.SpiritAtt);

            int role = 1;
            double powerDamage = p1;

            if (p2 > powerDamage)
            {
                role = 2;
                powerDamage = p2;
            }
            if (p3 > powerDamage)
            {
                role = 3;
                powerDamage = p3;
            }

            powerDamage *= CalPercent(AttributeEnum.AurasAttrIncrea);
            powerDamage *= CalPercent(AttributeEnum.DamageIncrea) * CalPercent(AttributeEnum.AurasDamageIncrea);
            powerDamage *= (1 + GetTotalAttrDouble(AttributeEnum.Lucky) * 0.1);
            powerDamage *= (1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRate), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamage) + 150) / 100);

            //增伤倍率
            double mdi = GetTotalAttrDouble(AttributeEnum.MulDamageIncrea);
            powerDamage *= (1 + mdi / 100);
            //破刃倍率
            double sdi = GetTotalAttrDouble(AttributeEnum.Shatter);
            powerDamage *= (1 + sdi);


            double powerDef = GetTotalAttrDouble(AttributeEnum.HP) / 10 + GetTotalAttrDouble(AttributeEnum.Def) * 3;
            powerDef *= (1 + CalPercent(AttributeEnum.DamageResist) * CalPercent(AttributeEnum.AurasDamageResist));
            powerDamage *= (1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRateResist), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamageResist) + 100) / 100);
            powerDef *= (1 + CalPercent(AttributeEnum.Miss));
            powerDef *= (1 + GetTotalAttrDouble(AttributeEnum.Strong));


            double newPower = (powerDamage + powerDef) / 20;
            return newPower;
        }

        public string GetPowerText()
        {
            return StringHelper.FormatNumber(GetPower());

            //return GetPowerNew().FormatUnit();
        }

        private double CalPercent(AttributeEnum type)
        {
            return (100 + GetTotalAttrDouble(type)) / 100;
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