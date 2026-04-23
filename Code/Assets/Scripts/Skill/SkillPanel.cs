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

        public int Level { get; } //面板等级

        public AttackGeometryType Area { get; set; }

        public List<KeyValuePair<int, int>> TalentTextList { get; } = new List<KeyValuePair<int, int>>();
        public List<KeyValuePair<int, int>> RuneTextList { get; } = new List<KeyValuePair<int, int>>();
        public List<KeyValuePair<int, int>> SuitTextList { get; } = new List<KeyValuePair<int, int>>();

        public List<int> AttrIdList = new List<int>();

        public List<double> AttrValueList = new List<double>();

        public Dictionary<int, Effect_Compent> EffectList { get; } = new Dictionary<int, Effect_Compent>(); //特殊效果 

        public string Desc { get; set; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, List<SkillTalent> talentList, bool isPlayer)
            : this(skillData, runeList, suitList, talentList, isPlayer, RuleType.Normal, 0)
        {

        }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, List<SkillTalent> talentList, bool isPlayer, RuleType ruleType, int petRate)
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

            if (talentList == null)
            {
                talentList = new List<SkillTalent>();
            }

            long riseLevel = 0;
            if (isPlayer)
            {

                List<SkillTalentConfig> skillTalentConfigs = SkillTalentConfigCategory.Instance.GetSkillAllConfigs(SkillId);
                foreach (SkillTalentConfig config in skillTalentConfigs)
                {
                    int count = talentList.Where(m => m.Config.Id == config.Id).Count();
                    TalentTextList.Add(new KeyValuePair<int, int>(config.Id, count));
                }

                List<SkillRuneConfig> skillRuneConfigs = SkillRuneConfigCategory.Instance.GetSkillAllConfigs(SkillId, skillData.SkillConfig.SkillLayer);

                foreach (SkillRuneConfig config in skillRuneConfigs)
                {
                    int count = runeList.Where(m => m.Config.Id == config.Id).Select(m => m.AvailableQuantity).Sum();
                    RuneTextList.Add(new KeyValuePair<int, int>(config.Id, count));
                }

                List<SkillSuitConfig> skillSuitConfigs = SkillSuitConfigCategory.Instance.GetSkillAllConfigs(SkillId, skillData.SkillConfig.SkillLayer);
                foreach (SkillSuitConfig config in skillSuitConfigs)
                {
                    int count = suitList.Where(m => m.Config.Id == config.Id).Count();
                    SuitTextList.Add(new KeyValuePair<int, int>(config.Id, count));
                }

                //User user = GameProcessor.Inst.User;
                //RingConfig ringConfig = RingConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == SkillId).FirstOrDefault();
                //if (ringConfig != null)
                //{
                //    long ringLevel = user.GetRingLevel(ringConfig.Id);
                //    riseLevel = ringLevel * ringConfig.RiseSkillLevel;
                //}
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
            long talentDamage = talentList.Select(m => m.Damage).Sum();

            int runePercent = baseRuneList.Select(m => m.Percent).Sum();
            int suitPercent = baseSuitList.Select(m => m.Percent).Sum();
            int talentPercent = talentList.Select(m => m.Percent).Sum();

            int runeIgnoreDef = baseRuneList.Select(m => m.IgnoreDef).Sum();
            int suitIgnoreDef = baseSuitList.Select(m => m.IgnoreDef).Sum();
            int talentIgnoreDef = talentList.Select(m => m.IgnoreDef).Sum();

            int runeDis = baseRuneList.Select(m => m.Dis).Sum();
            int suitDis = baseSuitList.Select(m => m.Dis).Sum();
            int talentDis = talentList.Select(m => m.Dis).Sum();

            int runeEnemyMax = baseRuneList.Select(m => m.EnemyMax).Sum();
            int suitEnemyMax = baseSuitList.Select(m => m.EnemyMax).Sum();
            int talentEnemyMax = talentList.Select(m => m.EnemyMax).Sum();

            int runeCD = baseRuneList.Select(m => m.CD).Sum();
            int suitCD = baseSuitList.Select(m => m.CD).Sum();
            int talentCD = talentList.Select(m => m.CD).Sum();

            int runeDuration = baseRuneList.Select(m => m.Duration).Sum();
            int suitDuration = baseSuitList.Select(m => m.Duration).Sum();
            int talentDuration = talentList.Select(m => m.Duration).Sum();

            int runeCritRate = baseRuneList.Select(m => m.CritRate).Sum();
            int suitCritRate = baseSuitList.Select(m => m.CritRate).Sum();
            int talentCritRate = talentList.Select(m => m.CritRate).Sum();

            int runeCritDamage = baseRuneList.Select(m => m.CritDamage).Sum();
            int suitCritDamage = baseSuitList.Select(m => m.CritDamage).Sum();
            int talentCritDamage = talentList.Select(m => m.CritDamage).Sum();

            int runeAttrIncrea = baseRuneList.Select(m => m.AttrIncrea).Sum();
            int suitAttrIncrea = baseSuitList.Select(m => m.AttrIncrea).Sum();
            int talentAttrIncrea = talentList.Select(m => m.AttrIncrea).Sum();

            int runeFinalIncrea = baseRuneList.Select(m => m.FinalIncrea).Sum();
            int suitFinalIncrea = baseSuitList.Select(m => m.FinalIncrea).Sum();
            int talentFinalIncrea = talentList.Select(m => m.FinalIncrea).Sum();

            int runeRow = baseRuneList.Select(m => m.Row).Sum();
            int suitRow = baseSuitList.Select(m => m.Row).Sum();
            int talentRow = talentList.Select(m => m.Row).Sum();

            int runeColumn = baseRuneList.Select(m => m.Column).Sum();
            int suitColumn = baseSuitList.Select(m => m.Column).Sum();
            int talentColumn = talentList.Select(m => m.Column).Sum();

            int runeAc = baseRuneList.Select(m => m.Accuracy).Sum();
            int suitAc = baseSuitList.Select(m => m.Accuracy).Sum();
            int talentAc = talentList.Select(m => m.Accuracy).Sum();

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage + talentDamage + levelDamage;

            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent + talentPercent + levelPercent;

            this.IgnoreDef += skillData.SkillConfig.IgnoreDef + runeIgnoreDef + suitIgnoreDef + talentIgnoreDef;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis + talentDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax + talentEnemyMax;
            this.CD += Math.Max(skillData.SkillConfig.CD - runeCD - suitCD - talentCD, 0);
            this.Rate = 1;
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration + talentDuration;

            this.Row = skillData.SkillConfig.Row + runeRow + suitRow + talentRow;
            this.Column = skillData.SkillConfig.Column + runeColumn + suitColumn + talentColumn;

            this.CritRate = skillData.SkillConfig.CritRate + runeCritRate + suitCritRate + talentCritRate;
            this.CritDamage = skillData.SkillConfig.CritDamage + runeCritDamage + suitCritDamage + talentCritDamage;

            this.Accuracy = runeAc + suitAc + talentAc;

            this.AttrIncrea = 0 + runeAttrIncrea + suitAttrIncrea + talentAttrIncrea;
            this.FinalIncrea = 0 + runeFinalIncrea + suitFinalIncrea + talentFinalIncrea;

            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);

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

                    AddEffect(effectId, percent, duration, max);
                }
            }

            //suit effect
            foreach (SkillSuit suit in effectSuitList)
            {
                if (suit.EffectId > 0)
                {
                    AddEffect(suit.EffectId, suit.Percent, suit.Duration, suit.EnemyMax);
                }
            }

            //talent effect
            foreach (SkillTalent sp in talentList)
            {
                if (sp.EffectId > 0)
                {
                    AddEffect(sp.EffectId, sp.EffectValue, 0, sp.EffectMax);
                }
            }

            //rune effect 暂无
            foreach (var effect in EffectList)
            {
                int effectId = effect.Key;
                EffectConfig effectConfig = EffectConfigCategory.Instance.Get(effectId);
                Effect_Compent compent = effect.Value;


                if (effectConfig.Des != "")
                {
                    Desc += "," + string.Format(effectConfig.Des, compent.Percent, compent.Max, compent.Duration);
                }
            }


        }

        private void AddEffect(int effectId, double percent, int duration, int max)
        {
            if (!EffectList.ContainsKey(effectId)) //如果已经存在，就叠加其他参数
            {
                int fromId = GetFromId(effectId);
                Effect_Compent compent = new Effect_Compent(effectId, fromId, percent, 0, duration, max);
                EffectList.Add(effectId, compent);
            }
            else
            {
                EffectList[effectId].Add(percent, 0, duration, max);
            }

        }

        private int GetFromId(int effectId)
        {
            return (int)AttributeFrom.Skill * 100000 + this.SkillId * 1000 + effectId * 10;
        }

        /// <summary>
        /// 运行先行效果
        /// </summary>
        public void RunBefore(APlayer self, APlayer enemy)
        {
            foreach (var sp in EffectList)
            {
                Effect_Compent com = sp.Value;

                if (com.Config.RunType == "Before")
                {
                    com.Do(self, enemy, 0);
                }
            }
        }

        /// <summary>
        /// 运行后行效果
        /// </summary>
        public void RunAfter(APlayer self, APlayer enemy, DamageResult res)
        {
            foreach (var sp in EffectList)
            {
                Effect_Compent com = sp.Value;

                if (com.Config.RunType == "Afer")
                {
                    com.Do(self, enemy, res.Damage);
                }
            }
        }

    }


}
