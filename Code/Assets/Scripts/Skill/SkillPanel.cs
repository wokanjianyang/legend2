using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillPanel
    {
        public SkillData SkillData { get; set; }
        public int SkillId { get; }

        public double Damage { get; }
        public double Percent { get; set; }
        public int Dis { get; }
        public int EnemyMax { get; }
        public int CD { get; }

        public int Rate { get; }

        public int Row { get; }
        public int Column { get; }

        public int Duration { get; }

        public int Accuracy { get; }

        public int Miss { get; }

        public int IgnoreDef { get; set; }  //无视防御

        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率

        public int DeadlyRate { get; } //致命率
        public int DeadlyDamage { get; } //致命倍率

        public int Speed { get; }


        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成

        public int FinalIncrea { get; } //最终伤害加成

        public int InheritIncrea { get; } //召唤物高级属性继承

        public bool DefinitelyCrit { get; } //必定暴击

        public int Level { get; } //面板等级

        public Dictionary<int, EffectData> EffectIdList { get; } = new Dictionary<int, EffectData>(); //特殊效果 

        public AttackGeometryType Area { get; }

        public AttackCastType CastType { get; }

        public string CenterType { get; }

        public List<KeyValuePair<int, int>> RuneTextList { get; } = new List<KeyValuePair<int, int>>();
        public List<KeyValuePair<int, int>> SuitTextList { get; } = new List<KeyValuePair<int, int>>();

        public List<int> AttrIdList = new List<int>();

        public List<double> AttrValueList = new List<double>();

        public string Desc { get; set; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, bool isPlayer) : this(skillData, runeList, suitList, isPlayer, RuleType.Normal, 0)
        {

        }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, bool isPlayer, RuleType ruleType, int petRate)
        {
            this.SkillData = skillData;
            this.SkillId = skillData.SkillId;

            if (runeList == null)
            {
                runeList = new List<SkillRune>();
            }
            if (suitList == null)
            {
                suitList = new List<SkillSuit>();
            }

            long riseLevel = 0;
            if (isPlayer)
            {
                List<SkillRuneConfig> skillRuneConfigs = SkillRuneConfigCategory.Instance.GetSkillAllConfigs(SkillId, skillData.SkillConfig.SkillLayer);

                foreach (SkillRuneConfig config in skillRuneConfigs)
                {
                    int count = runeList.Where(m => m.SkillRuneConfig.Id == config.Id).Select(m => m.AvailableQuantity).Sum();
                    RuneTextList.Add(new KeyValuePair<int, int>(config.Id, count));
                }

                List<SkillSuitConfig> skillSuitConfigs = SkillSuitConfigCategory.Instance.GetSkillAllConfigs(SkillId, skillData.SkillConfig.SkillLayer);
                foreach (SkillSuitConfig config in skillSuitConfigs)
                {
                    int count = suitList.Where(m => m.SkillSuitConfig.Id == config.Id).Count();
                    SuitTextList.Add(new KeyValuePair<int, int>(config.Id, count));
                }

                User user = GameProcessor.Inst.User;
                RingConfig ringConfig = RingConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == SkillId).FirstOrDefault();
                if (ringConfig != null)
                {
                    long ringLevel = user.GetRingLevel(ringConfig.Id);
                    riseLevel = ringLevel * ringConfig.RiseSkillLevel;
                }
            }

            List<SkillRune> baseRuneList = runeList.Where(m => m.EffectId == 0).ToList();
            List<SkillSuit> baseSuitList = suitList.Where(m => m.EffectId == 0).ToList();

            List<SkillRune> effectRuneList = runeList.Where(m => m.EffectId > 0).ToList();
            List<SkillSuit> effectSuitList = suitList.Where(m => m.EffectId > 0).ToList();

            this.Level = (int)(skillData.MagicLevel.Data + riseLevel);

            int levelPercent = (Level - 1) * skillData.SkillConfig.LevelPercent;
            long levelDamage = (Level - 1) * skillData.SkillConfig.LevelDamage;

            long runeDamage = baseRuneList.Select(m => m.Damage).Sum() * skillData.MagicLevel.Data;
            long suitDamage = baseSuitList.Select(m => m.Damage).Sum() * skillData.MagicLevel.Data;

            int runePercent = baseRuneList.Select(m => m.Percent).Sum();
            int suitPercent = baseSuitList.Select(m => m.Percent).Sum();

            int runePercentRate = baseRuneList.Select(m => m.PercentRate).Sum();
            int suitPercentRate = baseSuitList.Select(m => m.PercentRate).Sum();

            int runeIgnoreDef = baseRuneList.Select(m => m.IgnoreDef).Sum();
            int suitIgnoreDef = baseSuitList.Select(m => m.IgnoreDef).Sum();

            int runeDis = baseRuneList.Select(m => m.Dis).Sum();
            int suitDis = baseSuitList.Select(m => m.Dis).Sum();

            int runeEnemyMax = baseRuneList.Select(m => m.EnemyMax).Sum();
            int suitEnemyMax = baseSuitList.Select(m => m.EnemyMax).Sum();

            int runeCD = baseRuneList.Select(m => m.CD).Sum();
            int suitCD = baseSuitList.Select(m => m.CD).Sum();

            int runeDuration = baseRuneList.Select(m => m.Duration).Sum();
            int suitDuration = baseSuitList.Select(m => m.Duration).Sum();

            int runeCritRate = baseRuneList.Select(m => m.CritRate).Sum();
            int suitCritRate = baseSuitList.Select(m => m.CritRate).Sum();

            int runeCritDamage = baseRuneList.Select(m => m.CritDamage).Sum();
            int suitCritDamage = baseSuitList.Select(m => m.CritDamage).Sum();

            int runeDamageIncrea = baseRuneList.Select(m => m.DamageIncrea).Sum();
            int suitDamageIncrea = baseSuitList.Select(m => m.DamageIncrea).Sum();

            int runeAttrIncrea = baseRuneList.Select(m => m.AttrIncrea).Sum();
            int suitAttrIncrea = baseSuitList.Select(m => m.AttrIncrea).Sum();

            int runeFinalIncrea = baseRuneList.Select(m => m.FinalIncrea).Sum();
            int suitFinalIncrea = baseSuitList.Select(m => m.FinalIncrea).Sum();

            int runeRow = baseRuneList.Select(m => m.Row).Sum();
            int suitRow = baseSuitList.Select(m => m.Row).Sum();

            int runeColumn = baseRuneList.Select(m => m.Column).Sum();
            int suitColumn = baseSuitList.Select(m => m.Column).Sum();

            int runeInheritIncrea = baseRuneList.Select(m => m.InheritIncrea).Sum();
            int suitInheritIncrea = baseSuitList.Select(m => m.InheritIncrea).Sum();

            int runeAc = baseRuneList.Select(m => m.Accuracy).Sum();
            int suitAc = baseSuitList.Select(m => m.Accuracy).Sum();

            int runeMiss = baseRuneList.Select(m => m.Miss).Sum();
            int suitMiss = baseSuitList.Select(m => m.Miss).Sum();

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage + levelDamage;

            if (SkillData.SkillConfig.SkillLayer >= 11)
            {
                this.Damage = this.Damage * (100 + runePercentRate + suitPercentRate + petRate) / 100;
            }

            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent + levelPercent;
            //系数倍率
            this.Percent = this.Percent * (100 + runePercentRate + suitPercentRate + petRate) / 100;

            this.IgnoreDef += skillData.SkillConfig.IgnoreDef + runeIgnoreDef + suitIgnoreDef;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax;
            this.CD += Math.Max(skillData.SkillConfig.CD - runeCD - suitCD, 0);
            this.Rate = 1;
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration;

            this.Row = skillData.SkillConfig.Row + runeRow + suitRow;
            this.Column = skillData.SkillConfig.Column + runeColumn + suitColumn;

            this.CritRate = skillData.SkillConfig.CritRate + runeCritRate + suitCritRate;
            this.CritDamage = skillData.SkillConfig.CritDamage + runeCritDamage + suitCritDamage;
            this.DamageIncrea = skillData.SkillConfig.DamageIncrea + runeDamageIncrea + suitDamageIncrea;

            this.Accuracy = runeAc + suitAc;
            this.Miss = runeMiss + suitMiss;

            this.AttrIncrea = 0 + runeAttrIncrea + suitAttrIncrea;
            this.FinalIncrea = 0 + runeFinalIncrea + suitFinalIncrea;

            this.InheritIncrea = runeInheritIncrea + suitInheritIncrea;

            //施法范围
            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);
            this.CastType = (AttackCastType)skillData.SkillConfig.CastType;

            //foreach(SkillSuit suit in suitList) {
            //    if (suit.Center != "") {
            //        this.CenterType = suit.Center;
            //    }
            //}
            //技能属性
            if (SkillId == 1001)
            {
                AttrIdList.Add((int)AttributeEnum.PhyAtk);
                AttrValueList.Add(Damage);

                AttrIdList.Add((int)AttributeEnum.IncreaPhyAtk);
                AttrValueList.Add(Percent);
            }
            else if (SkillId == 2001)
            {
                AttrIdList.Add((int)AttributeEnum.MagicAtk);
                AttrValueList.Add(Damage);

                AttrIdList.Add((int)AttributeEnum.IncreaMagicAtk);
                AttrValueList.Add(Percent);
            }
            else if (SkillId == 3001)
            {
                AttrIdList.Add((int)AttributeEnum.SpiritAtk);
                AttrValueList.Add(Damage);

                AttrIdList.Add((int)AttributeEnum.IncreaSpiritAtk);
                AttrValueList.Add(Percent);
            }

            if (isPlayer)
            {
                Desc = string.Format(SkillData.SkillConfig.Des, (int)Percent, (int)Damage, Duration, EnemyMax, Row, Column);
            }

            //技能的特效
            string[] skilEffectList = skillData.SkillConfig.EffectList;
            if (skilEffectList != null && skilEffectList.Length > 0)
            {
                foreach (string skillEffect in skilEffectList)
                {
                    int[] effectParams = StringHelper.ConvertSkillParams(skillEffect);
                    int effectId = effectParams[0];
                    int duration = effectParams[1];
                    int max = effectParams[2];
                    double percent = effectParams[3];

                    List<SkillSuit> itemSuitList = effectSuitList.Where(m => m.EffectId == effectId).ToList();

                    if (itemSuitList.Count > 0)
                    {
                        duration += itemSuitList.Select(m => m.Duration).Sum();
                        max += itemSuitList.Select(m => m.EnemyMax).Sum();
                        percent += itemSuitList.Select(m => m.Percent).Sum();
                    }

                    List<SkillRune> itemRuneList = effectRuneList.Where(m => m.EffectId == effectId).ToList();
                    if (itemRuneList.Count > 0)
                    {
                        duration += itemRuneList.Select(m => m.Duration).Sum();
                        max += itemRuneList.Select(m => m.EnemyMax).Sum();
                        percent += itemRuneList.Select(m => m.Percent).Sum();
                    }

                    EffectConfig effectConfig = EffectConfigCategory.Instance.Get(effectId);



                    if (effectId > 0 && !EffectIdList.ContainsKey(effectId)) //不能叠加
                    {
                        int fromId = GetFromId(effectId);
                        EffectIdList[effectId] = new EffectData(effectId, fromId, percent, 0, duration, max);
                    }

                    //Remove
                    effectSuitList.RemoveAll(m => m.EffectId == effectId);
                    effectRuneList.RemoveAll(m => m.EffectId == effectId);

                    if (effectConfig.Des != "")
                    {
                        Desc += "," + string.Format(effectConfig.Des, percent, max, duration);
                    }
                }
            }

            //rune effect 暂无

            //suit effect
            foreach (SkillSuit suit in effectSuitList)
            {
                if (suit.EffectId > 0 && !EffectIdList.ContainsKey(suit.EffectId))
                {
                    int fromId = GetFromId(suit.EffectId);
                    EffectIdList[suit.EffectId] = new EffectData(suit.EffectId, fromId, suit.Percent, suit.Damage, suit.Duration, suit.EnemyMax);
                }
            }

            //special
            if (EffectIdList.ContainsKey((int)EffectSpecialId.DefinitelyCrit))
            {
                this.DefinitelyCrit = true;
                EffectIdList.Remove((int)EffectSpecialId.DefinitelyCrit);
            }
            else
            {
                this.DefinitelyCrit = false;
            }


            //技能附加的属性


            //TEST skill
            //this.CD = 0;
            //this.Row = 2;
            //this.Column = 2;
            //this.Duration = 3;
        }

        private int GetFromId(int effectId)
        {
            return (int)AttributeFrom.Skill * 100000 + effectId * 10;
        }


    }

    public enum DivineType
    {
        SingleRepeat = 1,
        DistanceRise = 2,
        SingleEjection = 3,
    }
}
