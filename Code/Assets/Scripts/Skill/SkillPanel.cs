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
        public int SkillId { get; }

        public SkillConfig Config { get; }

        public double Damage { get; }
        public double Percent { get; set; }
        public int Dis { get; }
        public int EnemyMax { get; }
        public float CD { get; }

        public int Rate { get; } //触发概率

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

        public Dictionary<int, Effect_Data> EffectList { get; } = new Dictionary<int, Effect_Data>(); //特殊效果 

        public string Desc { get; set; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, List<SkillTalent> talentList, bool isPlayer)  //怪物默认满CD
            : this(skillData, runeList, suitList, talentList, 0, 0, 500, isPlayer)
        {

        }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList, List<SkillTalent> talentList, int risePercent, int riseLevel, double cd, bool isPlayer)
        {
            this.Config = skillData.SkillConfig;
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

                //User user = User_Data_Manager.Data;
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

            int levelDis = 0;
            int runeDis = baseRuneList.Select(m => m.Dis).Sum();
            int suitDis = baseSuitList.Select(m => m.Dis).Sum();
            int talentDis = talentList.Select(m => m.Dis).Sum();

            int levelEnemyMax = 0;
            int runeEnemyMax = baseRuneList.Select(m => m.EnemyMax).Sum();
            int suitEnemyMax = baseSuitList.Select(m => m.EnemyMax).Sum();
            int talentEnemyMax = talentList.Select(m => m.EnemyMax).Sum();

            int levelCD = 0;
            int runeCD = baseRuneList.Select(m => m.CD).Sum();
            int suitCD = baseSuitList.Select(m => m.CD).Sum();
            int talentCD = talentList.Select(m => m.CD).Sum();

            int levelDuration = 0;
            int runeDuration = baseRuneList.Select(m => m.Duration).Sum();
            int suitDuration = baseSuitList.Select(m => m.Duration).Sum();
            int talentDuration = talentList.Select(m => m.Duration).Sum();

            int levelCritRate = 0;
            int runeCritRate = baseRuneList.Select(m => m.CritRate).Sum();
            int suitCritRate = baseSuitList.Select(m => m.CritRate).Sum();
            int talentCritRate = talentList.Select(m => m.CritRate).Sum();

            int levelCritDamage = 0;
            int runeCritDamage = baseRuneList.Select(m => m.CritDamage).Sum();
            int suitCritDamage = baseSuitList.Select(m => m.CritDamage).Sum();
            int talentCritDamage = talentList.Select(m => m.CritDamage).Sum();

            int levelAttrIncrea = 0;
            int runeAttrIncrea = baseRuneList.Select(m => m.AttrIncrea).Sum();
            int suitAttrIncrea = baseSuitList.Select(m => m.AttrIncrea).Sum();
            int talentAttrIncrea = talentList.Select(m => m.AttrIncrea).Sum();

            int levelFinalIncrea = 0;
            int runeFinalIncrea = baseRuneList.Select(m => m.FinalIncrea).Sum();
            int suitFinalIncrea = baseSuitList.Select(m => m.FinalIncrea).Sum();
            int talentFinalIncrea = talentList.Select(m => m.FinalIncrea).Sum();

            int levelRow = 0;
            int runeRow = baseRuneList.Select(m => m.Row).Sum();
            int suitRow = baseSuitList.Select(m => m.Row).Sum();
            int talentRow = talentList.Select(m => m.Row).Sum();

            int levelColumn = 0;
            int runeColumn = baseRuneList.Select(m => m.Column).Sum();
            int suitColumn = baseSuitList.Select(m => m.Column).Sum();
            int talentColumn = talentList.Select(m => m.Column).Sum();

            int levelAc = 0;
            int runeAc = baseRuneList.Select(m => m.Accuracy).Sum();
            int suitAc = baseSuitList.Select(m => m.Accuracy).Sum();
            int talentAc = talentList.Select(m => m.Accuracy).Sum();

            int levelSpeed = 0;
            int runeSpeed = baseRuneList.Select(m => m.Speed).Sum();
            int suitSpeed = baseSuitList.Select(m => m.Speed).Sum();
            int talentSpeed = talentList.Select(m => m.Speed).Sum();

            int levelDeadlyRate = 0;
            int runeDeadlyRate = baseRuneList.Select(m => m.DeadlyRate).Sum();
            int suitDeadlyRate = baseSuitList.Select(m => m.DeadlyRate).Sum();
            int talentDeadlyRate = talentList.Select(m => m.DeadlyRate).Sum();

            int levelDeadlyDamage = 0;
            int runeDeadlyDamage = baseRuneList.Select(m => m.DeadlyDamage).Sum();
            int suitDeadlyDamage = baseSuitList.Select(m => m.DeadlyDamage).Sum();
            int talentDeadlyDamage = talentList.Select(m => m.DeadlyDamage).Sum();

            //等级加成
            if (Config.RiseId != null)
            {
                for (int i = 0; i < Config.RiseId.Length; i++)
                {
                    int rv = Config.RiseRequireLevel[i];

                    int riseId = Config.RiseId[i];
                    int riseVue = Config.RiseVue[i];

                    if (Level >= rv)
                    {
                        switch (riseId)
                        {
                            case 1:
                                levelPercent += riseVue;
                                break;
                            case 2:
                                levelDamage += riseVue;
                                break;
                            case 3:
                                levelDuration += riseVue;
                                break;
                            case 4:
                                levelEnemyMax += riseVue;
                                break;
                            case 5:
                                levelRow += riseVue;
                                break;
                            case 6:
                                levelColumn += riseVue;
                                break;
                            case 7:
                                levelSpeed += riseVue;
                                break;
                            case 8:
                                levelCD += riseVue;
                                break;
                            case 9:
                                levelDis += riseVue;
                                break;
                            case 10:
                                levelAttrIncrea += riseVue;
                                break;
                            case 11:
                                levelFinalIncrea += riseVue;
                                break;
                            case 12:
                                levelCritRate += riseVue;
                                break;
                            case 13:
                                levelDeadlyRate += riseVue;
                                break;
                        }
                    }
                }
            }

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage + talentDamage + levelDamage;
            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent + talentPercent + levelPercent;

            this.IgnoreDef += skillData.SkillConfig.IgnoreDef + runeIgnoreDef + suitIgnoreDef + talentIgnoreDef;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis + talentDis + levelDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax + talentEnemyMax + levelEnemyMax;

            cd = (cd + runeCD + suitCD + talentCD + levelCD) / 100.0 + 1;
            this.CD = (float)(Math.Round(skillData.SkillConfig.CD / cd, 2));
            this.Rate = 1;
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration + talentDuration + levelDuration;

            this.Row = skillData.SkillConfig.Row + runeRow + suitRow + talentRow + levelRow;
            this.Column = skillData.SkillConfig.Column + runeColumn + suitColumn + talentColumn + levelColumn;

            this.CritRate = skillData.SkillConfig.CritRate + runeCritRate + suitCritRate + talentCritRate + levelCritRate;
            this.CritDamage = skillData.SkillConfig.CritDamage + runeCritDamage + suitCritDamage + talentCritDamage + levelCritDamage;

            this.Accuracy = runeAc + suitAc + talentAc + levelAc;
            this.Speed = runeSpeed + suitSpeed + talentSpeed + levelSpeed;

            this.AttrIncrea = 0 + runeAttrIncrea + suitAttrIncrea + talentAttrIncrea + levelAttrIncrea;
            this.FinalIncrea = 0 + runeFinalIncrea + suitFinalIncrea + talentFinalIncrea + levelFinalIncrea;

            this.DeadlyRate = runeDeadlyRate + suitDeadlyRate + talentDeadlyRate + levelDeadlyRate;
            this.DeadlyDamage = runeDeadlyDamage + suitDeadlyDamage + talentDeadlyDamage + levelDeadlyDamage;

            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);

            //精通的额外加成
            this.Damage = (int)((Damage * (100 + risePercent)) / 100);
            this.Percent = (int)((Percent * (100 + risePercent)) / 100);

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

            foreach (var sp in talentList)
            {
                if (sp.AttrId > 0)
                {
                    AttrIdList.Add(sp.AttrId);
                    AttrValueList.Add(sp.AttrValue);
                }
            }

            if (isPlayer)
            {
                Desc = string.Format(Config.Des, (int)Percent, (int)Damage, Duration, EnemyMax, Row, Column);
            }

            //技能的特效
            string[] skilEffectList = skillData.SkillConfig.EffectList;
            if (skilEffectList != null && skilEffectList.Length > 0)
            {
                foreach (string skillEffect in skilEffectList)
                {
                    int[] effectParams = StringHelper.ConvertSkillParams(skillEffect);

                    int effectId = effectParams[0];
                    double vue = effectParams[1];
                    int duration = effectParams[2];
                    int max = effectParams[3];

                    if (vue < 0)
                    {
                        vue = Percent;
                    }

                    AddEffect(effectId, vue, duration, max);
                }
            }

            //suit effect
            foreach (SkillSuit suit in effectSuitList)
            {
                if (suit.EffectId > 0)
                {
                    AddEffect(suit.EffectId, suit.EffectVue, suit.EffectDuration, suit.EffectMax);
                }
            }

            //talent effect
            foreach (SkillTalent sp in talentList)
            {
                if (sp.EffectId > 0)
                {
                    AddEffect(sp.EffectId, sp.EffectVue, sp.EffectDuration, sp.EffectMax);
                }
            }

            //rune effect 暂无
            foreach (var effect in EffectList)
            {
                int effectId = effect.Key;
                EffectConfig effectConfig = EffectConfigCategory.Instance.Get(effectId);
                Effect_Data data = effect.Value;


                if (effectConfig.Des != "")
                {
                    Desc += "," + data.GetDesc();
                }
            }


        }



        private void AddEffect(int effectId, double vue, int duration, int max)
        {
            if (!EffectList.ContainsKey(effectId)) //如果已经存在，就叠加其他参数
            {
                int fromId = GetFromId(effectId);
                Effect_Data data = new Effect_Data(effectId, fromId, vue, duration, max);

                EffectList.Add(effectId, data);
            }
            else
            {
                EffectList[effectId].MergeParam(vue, duration, max);
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
                Effect_Data data = sp.Value;

                if (data.Config.StartType == "Before")
                {
                    float cd = self.CalAtkInterval(this.Speed);

                    Effect_State state = Effect_Compent_Manager.Instance.Excute(self, enemy, this, data, cd, 0);
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
                Effect_Data data = sp.Value;

                if (data.Config.StartType == "After")
                {
                    float cd = self.CalAtkInterval(this.Speed);

                    Effect_State state = Effect_Compent_Manager.Instance.Excute(self, enemy, this, data, cd, res.Damage);
                }
            }
        }
    }


}
