using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

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

        public Dictionary<int, int> HoneList { get; set; } = new Dictionary<int, int>();

        public EquipData Data { get; set; } = new EquipData();

        [JsonIgnore]
        public SkillRuneConfig SkillRuneConfig { get; set; }

        [JsonIgnore]
        public SkillSuitConfig SkillSuitConfig { get; set; }

        [JsonIgnore]
        public EquipConfig Config { get; set; }

        [JsonIgnore]
        public int[] Position { get; set; }

        [JsonIgnore]
        public int Part { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 品质属性
        /// </summary>
        public IDictionary<int, long> QualityAttrList { get; set; }


        [JsonIgnore]
        /// <summary>
        /// 套装属性名称
        /// </summary>
        public IDictionary<int, string> GroupNameList { get; set; }

        public Equip(int configId, int runeConfigId, int suitConfigId, int quality) : base(configId, ItemType.Equip)
        {
            this.ConfigId = configId;
            this.RuneConfigId = runeConfigId;
            this.SuitConfigId = suitConfigId;

            Config = EquipConfigCategory.Instance.Get(configId);

            Part = Config.Part;
            Position = Config.Position;
            Quality = quality;

            QualityAttrList = new Dictionary<int, long>();
            if (Quality > 0 && (Part <= 10 || Part >= 21))
            {
                EquipQualityConfig qualityConfig = EquipQualityConfigCategory.Instance.Get(quality);

                if (qualityConfig.AttrIdList != null)
                {
                    int qualityRate = Level / 10 + 1;
                    for (int i = 0; i < qualityConfig.AttrIdList.Length; i++)
                    {
                        QualityAttrList.Add(qualityConfig.AttrIdList[i], qualityConfig.AttrValueList[i] * qualityRate);
                    }
                }
            }

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
                    AttributeBase = AttributeBase * (Quality * 10 + 90) / 100;
                }

                BaseAttrList.Add(Config.AttrIdList[i], AttributeBase);
            }

            return BaseAttrList;
        }

        //private int GetLayerRate(int layer)
        //{
        //    int b = 1;
        //    for (int i = 1; i < layer; i++)
        //    {
        //        b = b * 2;
        //    }
        //    return 1;
        //}

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
        public IDictionary<int, double> GetTotalAttrList(long level)
        {
            long basePercent = 100;
            long qualityPercent = 100;

            if (level > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(level);
                basePercent += refineConfig.GetBaseAttrPercent(level);
                qualityPercent += refineConfig.GetQualityAttrPercent(level);
            }
            else if (Part >= 21 && Quality >= 7)
            {
                basePercent = 100;
                qualityPercent = 100 * Layer;
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

            //计算随机属性
            for (int i = 0; i < AttrEntryList.Count; i++)
            {
                int attrId = AttrEntryList[i].Key;
                long attrTotalValue = AttrEntryList[i].Value + GetHoneValue(i);
                attrTotalValue = attrTotalValue * qualityPercent / 100;

                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += attrTotalValue;
            }

            //计算品质属性
            foreach (int attrId in QualityAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += QualityAttrList[attrId];
            }

            return AttrList;
        }

        public void Grade()
        {
            this.Layer++;
        }

        public void Hone(int index)
        {
            if (!HoneList.ContainsKey(index))
            {
                HoneList[index] = 0;
            }

            HoneList[index]++;
        }

        public int GetHoneLevel(int index)
        {
            if (HoneList.ContainsKey(index))
            {
                return HoneList[index];
            }
            return 0;
        }

        public int GetHoneValue(int index)
        {
            int level = GetHoneLevel(index);
            int attrId = this.AttrEntryList[index].Key;
            int attrValue = (int)this.AttrEntryList[index].Value;

            if (level > 0)
            {
                EquipHoneConfig config = EquipHoneConfigCategory.Instance.GetByAttrId(attrId);
                return Math.Min(level * config.AttrValue, config.StartValue + Layer * config.AttrValue - attrValue);
            }

            return 0;
        }

        public void GetRestoreItems(Dictionary<int, int> mlist)
        {
            int quanlity = this.GetQuality();
            int layer = Layer;

            for (int l = 1; l < layer; l++)
            {
                EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == Part && m.Layer == l && m.Quanlity == quanlity).FirstOrDefault();

                if (!mlist.ContainsKey(config.MetailId))
                {
                    mlist[config.MetailId] = 0;
                }

                mlist[config.MetailId] += config.MetailCount;


                if (!mlist.ContainsKey(config.MetailId1))
                {
                    mlist[config.MetailId1] = 0;
                }

                mlist[config.MetailId1] += config.MetailCount1;
            }

            foreach (var kv in HoneList)
            {
                int honeLevel = kv.Value;
                int redNumber = EquipHoneConfigCategory.Instance.GetTotalNeedNumber(honeLevel);

                if (!mlist.ContainsKey(ItemHelper.SpecialId_Red_Stone))
                {
                    mlist[ItemHelper.SpecialId_Red_Stone] = 0;
                }
                mlist[ItemHelper.SpecialId_Red_Stone] += redNumber;
            }
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


        //--------------ovveride
        public override int GetQuality()
        {
            return this.Quality;
        }

        public override string GetName()
        {
            return this.Config.Name;
        }

        public override int LevelRequired()
        {
            return this.Config.LevelRequired;
        }

    }
}
