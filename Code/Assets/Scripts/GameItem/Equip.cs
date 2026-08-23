using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Equip : Item
    {
        /// <summary>
        /// 词条属性列表
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();

        public int RuneConfigId { get; set; }

        public int SuitConfigId { get; set; }

        //public int Quality { get; set; }
        public int RuneSeed { get; set; }
        public int SuitSeed { get; set; }
        public int RefreshCount { get; set; }
        public long RefreshDate { get; set; }

        public MagicData RefineLevel { get; set; } = new MagicData();

        public KeyValuePair<int, int> LegendData { get; set; } = new KeyValuePair<int, int>();

        public int ReformExp { get; set; } = 0;

        public EquipData Data { get; set; } = new EquipData();

        [JsonIgnore]
        public SkillRuneConfig SkillRuneConfig { get; set; }

        [JsonIgnore]
        public SkillSuitConfig SkillSuitConfig { get; set; }

        [JsonIgnore]
        private EquipLegendConfig _LegendConfig;
        [JsonIgnore]
        public EquipLegendConfig LegendConfig
        {
            get
            {
                if (_LegendConfig == null && LegendData.Key > 0)
                {
                    _LegendConfig = EquipLegendConfigCategory.Instance.Get(LegendData.Key);
                }

                return _LegendConfig;
            }
        }

        [JsonIgnore]
        public EquipConfig Config { get; set; }

        [JsonIgnore]
        public int[] Position { get; set; }

        [JsonIgnore]
        public int Part { get; set; }

        private int[] QualityRate = { 100, 110, 120, 150, 200 };


        public Equip(int configId, int runeConfigId, int suitConfigId, int quality) : base(configId, ItemType.Equip)
        {
            this.RuneConfigId = runeConfigId;
            this.SuitConfigId = suitConfigId;

            Config = EquipConfigCategory.Instance.Get(configId);

            Part = Config.Part;
            Position = Config.Position;
            Quality = quality;
            Level = Config.LevelRequired;

            if (RuneConfigId > 0 && (Config.Cycle > 0))
            {
                SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
            }

            if (SuitConfigId > 0 && (Config.Cycle > 0))
            {
                SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SuitConfigId);
            }
        }

        public IDictionary<int, double> GetBaseAttrList()
        {
            IDictionary<int, double> BaseAttrList = new Dictionary<int, double>();
            for (int i = 0; i < Config.AttrIdList.Length; i++)
            {
                long AttributeBase = Config.AttrValueList[i];

                if (Config.Cycle == 1)
                {
                    AttributeBase = AttributeBase * QualityRate[Quality - 1] / 100;

                    int rl = GetReformLevel();
                    if (rl > 0)
                    {
                        AttributeBase = AttributeBase * (100 + rl * 10) / 100; //改造属性
                    }
                }

                BaseAttrList.Add(Config.AttrIdList[i], AttributeBase);
            }

            return BaseAttrList;
        }

        public IDictionary<int, double> GetRefineSpeAtrList(int position)
        {
            IDictionary<int, double> RefineSpeAtrList = new Dictionary<int, double>();

            User user = User_Data_Manager.Data;

            long level = user.GetRefineLevel(position);
            if (level > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByPart(Config.Part);

                for (int i = 0; i < refineConfig.SpeAtrList.Length; i++)
                {
                    if (level >= refineConfig.SpeLevel[i])
                    {
                        RefineSpeAtrList.Add(refineConfig.SpeAtrList[i], refineConfig.SpeVueList[i]);
                    }
                }
            }

            return RefineSpeAtrList;
        }

        public void CheckReFreshCount()
        {
            long tk = DateTime.Today.Ticks;
            if (this.RefreshDate < tk)
            {
                this.RefreshDate = tk;
                this.RefreshCount = ConfigHelper.EquipRefreshCount;
            }

            if (this.RuneSeed <= 0)
            {
                this.RuneSeed = AppHelper.InitSeed();
            }
            if (this.SuitSeed <= 0)
            {
                this.SuitSeed = AppHelper.InitSeed();
            }

            if (this.Data == null || this.Data.RuneIdList.Count == 0)
            {
                this.Data = new EquipData();
                this.Data.Refresh(this.Part, this.Config.Cycle, this.Quality, this.Config.Role);
            }
        }

        public void Refesh(bool save)
        {
            if (save)
            {
                this.AttrEntryList.Clear();
                this.AttrEntryList.AddRange(Data.GetAttrList());

                this.RuneConfigId = Data.GetRuneId();
                this.SuitConfigId = Data.GetSuitId();

                this.SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
                this.SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SuitConfigId);
            }

            Data.Refresh(this.Part, this.Config.Cycle, this.Quality, this.Config.Role);
        }

        public void Init(int seed)
        {
            //根据品质,生成随机属性

            this.AttrEntryList.AddRange(AttrEntryConfigCategory.Instance.Build(this.Part, this.Config.Cycle, this.Config.LevelRequired, this.Quality, this.Config.Role, seed));

            //if (this.Part <= 10 && this.Quality >= 6)
            //{
            //    this.Data = new EquipData();
            //    this.Data.Refresh(this.Part, this.Config.Cycle, this.Quality, this.Config.Role);
            //}
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, double> GetTotalAttrList(int position)
        {
            long basePercent = 100;
            long randomPercent = 100;

            User user = User_Data_Manager.Data;

            long level = user.GetRefineLevel(position);
            if (level > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByPart(Config.Part);
                basePercent += refineConfig.GetRisePercent(level, 1);
                basePercent += (int)user.AttributeBonus.CalPanelAtr(AttributeEnum.EquipBaseIncrea);
                randomPercent += refineConfig.GetRisePercent(level, 2);
            }

            //根据基础属性和词条属性，计算总属性
            IDictionary<int, double> BaseAttrList = this.GetBaseAttrList();

            IDictionary<int, double> AttrList = new Dictionary<int, double>();
            foreach (int attrId in BaseAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += BaseAttrList[attrId] * basePercent / 100;
            }

            Dictionary<int, int> rs = new Dictionary<int, int>();

            //计算随机属性
            for (int i = 0; i < AttrEntryList.Count; i++)
            {
                int attrId = AttrEntryList[i].Key;
                long attrTotalValue = AttrEntryList[i].Value;

                AttrEntryConfig config = AttrEntryConfigCategory.Instance.GetConfig(this.Config.Cycle, attrId, this.Config.LevelRequired);

                if (!rs.ContainsKey(config.Id))
                {
                    rs[config.Id] = 0;
                }
                rs[config.Id]++;

                if (attrTotalValue > config.MaxValue)
                {
                    attrTotalValue = 0;  //如果数值修改了，则不计算数值
                }

                attrTotalValue = attrTotalValue * randomPercent / 100;

                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += attrTotalValue;

                if (rs[config.Id] > config.MaxCount)
                {
                    AttrList[attrId] = 0; //如果修改了数量
                }
            }


            IDictionary<int, double> RefineAttrList = this.GetRefineSpeAtrList(position);
            foreach (int attrId in RefineAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += RefineAttrList[attrId];
            }


            //计算传奇属性
            if (LegendData.Key > 0)
            {
                EquipLegendConfig legendConfig = EquipLegendConfigCategory.Instance.Get(LegendData.Key);
                for (int i = 0; i < legendConfig.AtrIdList.Length; i++)
                {
                    int atrId = legendConfig.AtrIdList[i];
                    double atrVue = legendConfig.AtrVueList[i];
                    if (!AttrList.ContainsKey(atrId))
                    {
                        AttrList[atrId] = 0;
                    }

                    AttrList[atrId] += atrVue;
                }
            }

            return AttrList;
        }

        public override void Grade()
        {
            this.Layer++;
        }

        public void Refine()
        {
            this.RefineLevel.Data++;
        }

        public void ToLegend(int lgId, int lgFliar)
        {
            this.LegendData = new KeyValuePair<int, int>(lgId, lgFliar);

            _LegendConfig = EquipLegendConfigCategory.Instance.Get(LegendData.Key);
        }



        public long GetAttrRateCount()
        {
            return AttrEntryList.Where(m => (m.Key == 2001 || m.Key == 2004 || m.Key == 2005 || m.Key == 2006 || m.Key == 2010)).Count();
        }

        public int GetFull()
        {
            int full = 0;

            foreach (var sp in this.AttrEntryList)
            {
                AttrEntryConfig config = AttrEntryConfigCategory.Instance.GetRedConfig(sp.Key, this.Config.Cycle);
                if (config == null)
                {
                    return 1;
                }

                if (sp.Value == config.MaxValue)
                {
                    full++;
                }

            }

            if (full == this.AttrEntryList.Count)
            {
                return 1;
            }

            return 0;
        }

        public void AddReformExp(int exp)
        {
            this.ReformExp += exp;
        }

        public int GetReformLevel()
        {
            if (ReformExp <= 0)
            {
                return 0;
            }
            else
            {
                int r = (int)Math.Sqrt(8 * ReformExp + 1);
                r = (r - 1) / 2;
                return r;
            }
        }

        public int GetReformNeedExp()
        {
            int nl = GetReformLevel() + 1;

            return nl * (nl + 1) / 2;
        }
        //--------------ovveride
        public override int GetQuality()
        {
            return this.Quality;
        }

        public override string GetName()
        {
            return this.Config.Name;
        }

        public override int GetRequired()
        {
            return this.Config.LevelRequired;
        }

        public override int GetBagType()
        {
            if (Config.Cycle >= 10)
            {
                return 3;
            }

            return Config.Role - 1;
        }

        public override ShowType GetShowType()
        {
            return ShowType.Equip;
        }

        public override long ToRecoverDict(Dictionary<int, long> dict, long number)
        {
            if (Config.Cycle == 1)
            {
                if (!dict.ContainsKey(ItemHelper.Equip_Strong))
                {
                    dict[ItemHelper.Equip_Strong] = 0;
                }

                dict[ItemHelper.Equip_Strong] += CalStone() * number;

                if (this.GetQuality() >= 5)
                {
                    if (!dict.ContainsKey(ItemHelper.Equip_Refine))
                    {
                        dict[ItemHelper.Equip_Refine] = 0;
                    }

                    int bc = this.Config.LevelRequired / 20 + 1;

                    dict[ItemHelper.Equip_Refine] += bc * number;
                }
            }
            else if (Config.Cycle == 10)
            {
                if (!dict.ContainsKey(ItemHelper.Equip_Legend))
                {
                    dict[ItemHelper.Equip_Legend] = 0;
                }

                int count = Config.LevelRequired / 10;
                dict[ItemHelper.Equip_Legend] += count * number;
            }

            return (long)(Config.Price * Prices[GetQuality() - 1] * number);
        }

        private double[] Prices = { 1, 1.1, 1.2, 1.5, 2, 3, 4 };

        private int CalStone()
        {
            int count = Config.LevelRequired / 10 + this.GetQuality();
            return count;
        }

    }
}
