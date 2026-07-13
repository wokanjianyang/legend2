using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;
using Game.Data;
using SDD.Events;

namespace Game
{
    public class User
    {
        public Dictionary<int, int> ExclusiveDict = new Dictionary<int, int>();

        //public Dictionary<int, int> CardEquipDict = new Dictionary<int, int>();

        public Dictionary<int, int> CardRecord = new Dictionary<int, int>();

        public IDictionary<int, MagicData> GameRecord { get; set; } = new Dictionary<int, MagicData>();

        public Dictionary<int, bool> TaskLog = new Dictionary<int, bool>();

        public IDictionary<int, int> AchievementData { get; set; } = new Dictionary<int, int>();

        public List<Pet> PetList { get; set; } = new List<Pet>();

        public Dictionary<int, MagicData> FashionData { get; set; } = new Dictionary<int, MagicData>();

        public LegacyData LegacyData { get; set; } = new LegacyData();

        public int FashionUpId { get; set; } = 0;

        public Dictionary<int, int> OfflineLog { get; set; } = new Dictionary<int, int>();

        public long SecondExpTick { get; set; }

        public long SecondTotal { get; set; } = 0;

        public ADShowData ADShowData { get; set; } = new ADShowData();

        public RecordData Record { get; set; } = new RecordData();

        public AdData AdData { get; } = new AdData();

        public Babel_Data BabelData { get; set; } = new Babel_Data();

        //---------cal function
        public int GetExclusiveLevel(int id)
        {
            if (!this.ExclusiveDict.ContainsKey(id))
            {
                return 0;
            }

            return ExclusiveDict[id];
        }

        public int GetCardExp(int id)
        {
            if (!this.CardRecord.ContainsKey(id))
            {
                this.CardRecord[id] = 0;
            }

            return this.CardRecord[id];
        }

        public bool IsCardMax(int id)
        {
            int exp = GetCardExp(id);

            CardConfig config = CardConfigCategory.Instance.Get(id);

            int level = config.CalLevel(exp);

            return level >= config.MaxLevel;
        }


        public Item GetEquip(int position)
        {
            if (position >= 0 && position <= 10)
            {
                var ep = EquipPanelList[EquipPanelIndex];
                if (ep.ContainsKey(position))
                {
                    return ep[position];
                }
            }
            else if (position >= 1001 && position <= 1004)
            {
                if (EquipSpecialList.ContainsKey(position))
                {
                    return EquipSpecialList[position];
                }
            }

            return null;
        }

        //-----------------------old--------------------------

        public bool OldFile { get; set; } = false;
        public int Serial { get; set; } = 0;

        public string DeviceId { get; set; } = "";

        public string Account { get; set; } = "";
        public string Name { get; set; }

        public string AccountId { get; set; } = "";

        public long DataDate { get; set; } = 0;

        public int OffLineMapId { get; set; }

        public bool SpiritOfflineFlag { get; set; } = false;
        public Dictionary<int, int> SpiritOfflineLog { get; set; } = new Dictionary<int, int>();

        public MagicData Cycle { get; set; } = new MagicData();

        public MagicData MagicLevel { get; } = new MagicData();

        public MagicData MagicGold { get; } = new MagicData();

        public MagicData MagicExp { get; } = new MagicData();

        public MagicData RedRefreshCount { get; } = new MagicData();

        public IDictionary<int, double> KillRecord { get; } = new Dictionary<int, double>();

        public Dictionary<int, MagicData> RingData { get; } = new Dictionary<int, MagicData>();
        public Dictionary<int, int> RingSelect { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelGoldenList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, Equip_Special> EquipSpecialList { get; set; } = new Dictionary<int, Equip_Special>();

        public Dictionary<int, long> RecordMax = new Dictionary<int, long>();

        public IDictionary<int, Shengxiao> ShengxiaoList { get; set; } = new Dictionary<int, Shengxiao>();

        public int EquipPanelIndex { get; set; } = 0;
        public IDictionary<int, string> PlanNameList { get; set; } = new Dictionary<int, string>();

        public bool EquipGoldenSetting { get; set; } = false;

        public int EquipGoldenIndex { get; set; } = 0;

        public int SkillPanelIndex { get; set; } = 0;

        public IDictionary<int, MagicData> MagicEquipStrength { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> MagicEquipReform { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLevel { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLayer { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, StoneRecord> StoneData { get; set; } = new Dictionary<int, StoneRecord>();

        public IDictionary<int, MagicData> PetSpeicalLayerData { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> PetSpeicalLevelData { get; set; } = new Dictionary<int, MagicData>();

        public RecoverySetting RecoverySet { get; set; } = new RecoverySetting();

        public int InfoColor { get; set; } = 1;

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        public IDictionary<int, List<int>> SkillPanelList { get; set; } = new Dictionary<int, List<int>>();


        public IDictionary<int, int> RecordData { get; set; } = new Dictionary<int, int>();

        public Defend_Data DefendData { get; set; } = new Defend_Data();

        public InfiniteData InfiniteData { get; set; } = new InfiniteData();

        public HeroPhatomData HeroPhatomData { get; set; }

        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<string, bool> GiftListNew { get; set; } = new Dictionary<string, bool>();

        public Dictionary<int, long> VersionLog { get; } = new Dictionary<int, long>();

        public long GetLimitLevel()
        {
            long level = this.MagicLevel.Data;

            if (this.Cycle.Data > 0)
            {
                level = Math.Max(level, ConfigHelper.Max_Level + (this.Cycle.Data - 1) * ConfigHelper.Cycle_Level);
            }

            return (level) / 5000 + 1;
        }

        public int GetSkillLimit(SkillConfig skillConfig)
        {
            //double limit = (int)(skillConfig.MaxLevel + skillConfig.RiseMaxLevel * GetLimitLevel());
            //limit = limit * (100 + GetArtifactValue(ArtifactType.SkillLimit)) / 100;
            //return (int)limit;

            return 10;
        }

        public int GetReformLimit(int position)
        {
            long limit = (GetStrengthLevel(position) - 300000) / 1000;
            return (int)limit;
        }

        public long LastUploadTime { get; set; }

        public long LastSaveTime { get; set; }

        private bool isInLevelUp;

        public int MapId { get; set; } = 1;


        public MagicData LegacyTikerCount { get; } = new MagicData();

        public long SaveTicketTime { get; set; } = 0;

        public long SaveTickeTimeHand { get; set; } = 0;

        public long LoadTicketTime { get; set; } = 0;

        public long First_Create_Time { get; set; } = 0;

        //幻神记录
        public Dictionary<int, int> PhantomRecord { get; } = new Dictionary<int, int>();



        public long AdLastTime { get; set; } = 0;

        public Dictionary<int, MagicData> SoulRingData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> SoulBoneData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> RelicData { get; } = new Dictionary<int, MagicData>();

        public MagicData TalentExp { get; set; } = new MagicData();
        public Dictionary<int, MagicData> TalentData { get; } = new Dictionary<int, MagicData>();

        public int TalentPoint { get; set; } = 0;

        public MythData MythData { get; set; } = new MythData();

        public FestiveMapData FestiveMapData01 { get; set; } = new FestiveMapData();

        public Dictionary<int, MagicData> FestiveAttrData { get; } = new Dictionary<int, MagicData>();

        public WorldData WorldData { get; set; } = new WorldData();
        public MagicData WingData { get; set; } = new MagicData();

        public MagicData PillData { get; set; } = new MagicData();


        public PillTime PillTime { get; set; } = new PillTime();

        public Dictionary<int, MagicData> ItemMeterialData { get; } = new Dictionary<int, MagicData>();


        public Dictionary<int, MagicData> HalidomData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> ArtifactData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, int> PetCountData { get; } = new Dictionary<int, int>();

        public Dictionary<int, SpiritData> SpiritRecord { get; } = new Dictionary<int, SpiritData>();


        public FestiveWeekData WeekData = new FestiveWeekData();

        public IDictionary<int, int> FestiveData_0202 { get; set; } = new Dictionary<int, int>();
        public IDictionary<int, int> FestiveData_0302 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> FestiveData_1202 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> FestiveData_0102 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> SevenDayData { get; set; } = new Dictionary<int, int>();

        public int MinerSeed = 1;
        public long MinerTime { get; set; } = 0;

        public Dictionary<int, MagicData> MetalData { get; } = new Dictionary<int, MagicData>();

        public bool GameDoCheat211 { get; set; } = false;


        [JsonIgnore]
        public AttributeBonus AttributeBonus { get; set; }

        [JsonIgnore]
        public long TempUpExp { get; set; } = 0;
        public User()
        {

        }

        public void Init()
        {
            //设置各种属性值
            SetAttr();
        }


        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            long Level = MagicLevel.Data;

            //宠物属性
            int attrKey = 1;

            //基础属性，攻击10，防御0，生命1000，爆伤150，致命伤害150
            AttributeBonus.SetAttr(AttributeEnum.HP, attrKey++, 1000);
            AttributeBonus.SetAttr(AttributeEnum.Atk, attrKey++, 10);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, attrKey++, 50);
            AttributeBonus.SetAttr(AttributeEnum.DeadlyDamage, attrKey++, 50);

            //等级属性,攻击倍率，生命倍率，等级*1%
            AttributeBonus.SetAttr(AttributeEnum.RateAtk, attrKey++, Level * 1);
            AttributeBonus.SetAttr(AttributeEnum.RateDef, attrKey++, Level * 1);
            AttributeBonus.SetAttr(AttributeEnum.RateHp, attrKey++, Level * 1);

            //设置升级属性
            SetUpExp();

            //装备属性-普通装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelList[EquipPanelIndex])
            {
                Equip equip = kvp.Value;

                foreach (KeyValuePair<int, double> a in equip.GetTotalAttrList())
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, attrKey++, a.Value);
                }
            }

            //传奇装备套装
            List<EquipLegendSet> legs = this.GetActiveLegendSetList();
            foreach (var sp in legs)
            {
                Dictionary<int, double> atrList = sp.GetAtrList();

                foreach (var atr in atrList)
                {
                    AttributeBonus.SetAttr((AttributeEnum)(atr.Key), attrKey++, atr.Value);
                }
            }

            //套装属性
            for (int i = 1; i <= 3; i++)
            {
                EquipSetSuit suit = GetEquipSet(i, 1);
                foreach (EquipSetItem setItem in suit.List)
                {
                    if (setItem.IsActive() && setItem.Level > 0)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(setItem.Config.AttrId), attrKey++, setItem.GetAtrVue());
                    }
                }
            }

            //装备属性-四格装备
            foreach (KeyValuePair<int, Equip_Special> kvp in EquipSpecialList)
            {
                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList())
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, attrKey++, a.Value);
                }
            }

            //四格套装
            if (EquipSpecialList.Count >= 4)
            {
                EquipSetSuit suit = GetEquipSpecialSet(101);
                foreach (EquipSetItem setItem in suit.List)
                {
                    if (setItem.IsActive() && setItem.Level > 0)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(setItem.Config.AttrId), attrKey++, setItem.GetAtrVue());
                    }
                }
            }


            //强化属性
            foreach (var sp in this.MagicEquipStrength)
            {
                int position = sp.Key;
                EquipStrengthConfig strengthConfig = EquipStrengthConfigCategory.Instance.GetByPositioin(position);

                foreach (KeyValuePair<int, double> a in strengthConfig.GetTotalAtrList(sp.Value.Data))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, attrKey++, a.Value);
                }
            }

            //成就属性
            foreach (var sp in this.AchievementData)
            {
                int al = sp.Value;
                if (al > 0)
                {
                    AchievementConfig config = AchievementConfigCategory.Instance.Get((sp.Key));

                    for (int i = 0; i < config.AtrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)config.AtrIdList[i], attrKey++, config.GetAtrVue(i, al));
                    }
                }
            }

            foreach (var sp in this.PetList)
            {
                Dictionary<int, double> attrList = sp.GetTotalAttr();
                foreach (var al in attrList)
                {
                    AttributeBonus.SetAttr((AttributeEnum)(al.Key), attrKey++, al.Value);
                }
            }

            //专属属性
            foreach (var sp in this.ExclusiveDict)
            {
                int id = sp.Key;
                int level = sp.Value;

                if (level > 0)
                {
                    ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(id);

                    for (int i = 0; i < config.AtrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(config.AtrIdList[i]), attrKey++, config.AtrVueList[i]);
                    }
                }
            }

            //图鉴属性
            foreach (var sp in CardRecord)
            {
                if (sp.Key > 0)
                {
                    int exp = sp.Value;
                    CardConfig config = CardConfigCategory.Instance.Get(sp.Key);
                    int level = config.CalLevel(exp);
                    if (level > 0)
                    {
                        for (int i = 0; i < config.AtrIdList.Length; i++)
                        {
                            AttributeBonus.SetAttr((AttributeEnum)(config.AtrIdList[i]), attrKey++, config.AtrVueList[i] * level);
                        }

                        if (level >= config.SpeRequire)
                        {
                            AttributeBonus.SetAttr((AttributeEnum)(config.SpeAtrId), attrKey++, config.SpeAtrVue);
                        }
                    }
                }
            }

            //时装
            foreach (var sp in FashionData)
            {
                long fl = sp.Value.Data;
                if (fl > 0)
                {
                    FashionConfig config = FashionConfigCategory.Instance.Get(sp.Key);

                    for (int i = 0; i < config.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(config.AttrIdList[i]), attrKey++, config.AttrValueList[i]);
                    }

                    if (sp.Key == FashionUpId)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(config.UpAttrId), attrKey++, config.UpAttrValue);
                    }
                }
            }

            //传世
            for (int keyId = 1; keyId <= 24; keyId++)
            {
                int ly = GetLegacyLayer(keyId);
                if (ly > 0)
                {
                    LegacyConfig config = LegacyConfigCategory.Instance.Get(keyId);

                    Dictionary<int, double> attrList = config.GetTotalAtrList(ly);
                    foreach (var al in attrList)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(al.Key), attrKey++, al.Value);
                    }
                }

                int lv = GetLegacyLevel(keyId);
                if (lv > 0)
                {
                    LegacyGradeConfig gradeConfig = LegacyGradeConfigCategory.Instance.GetConfig(keyId, lv);

                    Dictionary<int, double> attrList = gradeConfig.GetTotalAtrList(lv);
                    foreach (var al in attrList)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(al.Key), attrKey++, al.Value);
                    }
                }
            }

            //传世套装
            for (int i = 1; i <= 3; i++)
            {
                int ls = GetLegacySetLayer(i);
                if (ls > 0)
                {
                    LegacySetConfig setConfig = LegacySetConfigCategory.Instance.GetByRole(i);

                    for (int k = 0; k < setConfig.AtrIdList.Length; k++)
                    {
                        int atrId = setConfig.AtrIdList[k];
                        long vue = setConfig.AtrVueList[k] * ls;

                        AttributeBonus.SetAttr((AttributeEnum)(atrId), attrKey++, vue);
                    }
                }
            }

            //net属性
            Loading_Data ldData = User_Data_Manager.NetData;
            if (ldData != null && ldData.AtrList.Count > 0)
            {
                for (int i = 0; i < ldData.AtrList.Count; i++)
                {
                    int atrId = ldData.AtrList[i].AtrId;
                    double vue = ldData.AtrList[i].AtrVue;
                    AttributeBonus.SetAttr((AttributeEnum)(atrId), attrKey++, vue);
                }
            }

            //商城属性
            Store_Data storeData = User_Data_Manager.StoreData;
            if (storeData != null && storeData.StoreList != null)
            {
                for (int i = 0; i < storeData.StoreList.Count; i++)
                {
                    Store_Data_Item data_Item = storeData.StoreList[i];

                    if (data_Item.Number > 0)
                    {
                        StoreConfig config = StoreConfigCategory.Instance.Get(data_Item.StoreId);

                        Dictionary<int, double> attrList = config.GetTotalAtrList(data_Item.Number);
                        foreach (var al in attrList)
                        {
                            AttributeBonus.SetAttr((AttributeEnum)(al.Key), attrKey++, al.Value);
                        }
                    }
                }
            }

            //技能属性
            foreach (var sp in User_Data.GetSkills())
            {
                for (int i = 0; i < sp.AttrIdList.Count; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)(sp.AttrIdList[i]), attrKey++, sp.AttrValueList[i]);
                }
            }

            //更新属性面版
            GameProcessor.Inst.EventCenter.Raise(new UpdateBagPanelUserAttr());
        }



        public int GetRuneCount(int runeId)
        {
            //计算装备的词条加成
            int count = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.RuneConfigId == runeId).Count();

            return count;
        }

        public List<SkillRune> GetRuneList(int skillId)
        {
            List<SkillRune> list = new List<SkillRune>();

            int skillLayer = SkillConfigCategory.Instance.Get(skillId).SkillLayer;

            //专属词条
            Dictionary<int, int> skillDict = new Dictionary<int, int>();

            //计算装备的词条加成
            List<int> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList();

            //list.AddRange(SkillRuneConfigCategory.Instance.GetAllRune(skillId, 2)); //测试代码，给技能加上所有词条 test

            //金装词条
            skillList.AddRange(this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList());

            foreach (int runeId in skillList)
            {
                if (!skillDict.ContainsKey(runeId))
                {
                    skillDict[runeId] = 0;
                }

                skillDict[runeId] += 1;
            }

            foreach (var kv in skillDict)
            {
                SkillRune skillRune = new SkillRune(kv.Key, kv.Value);
                list.Add(skillRune);
            }

            //if (skillId == 1002)
            //{
            //    Debug.Log(JsonConvert.SerializeObject(list));
            //}

            return list;
        }

        public int GetSuitMax()
        {
            return 4;
        }

        public List<SkillSuit> GetSuitList(int skillId)
        {
            List<SkillSuit> list = new List<SkillSuit>();

            int skillLayer = SkillConfigCategory.Instance.Get(skillId).SkillLayer;

            //计算装备的套装加成
            List<SkillSuitConfig> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList();

            //金装套装
            skillList.AddRange(this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList());

            var suitGroup = skillList.GroupBy(m => m.Id);

            foreach (var suitItem in suitGroup)
            {
                if (suitItem.Count() >= this.GetSuitMax())
                {  //SkillSuitHelper.SuitMax 件才成套,并且只能有一套能生效
                    SkillSuit suit = new SkillSuit(suitItem.Key);
                    list.Add(suit);
                }
            }

            //list.AddRange(SkillSuitConfigCategory.Instance.GetAllSuit(skillId, 10)); //测试代码，给技能加上所有词条 test

            return list;
        }

        public List<SkillTalent> GetTalentList(int skillId)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            foreach (var ex in this.PetList)
            {
                foreach (var sp in ex.Talents)
                {
                    SkillTalentConfig talentConfig = SkillTalentConfigCategory.Instance.Get(sp);
                    if (talentConfig.SkillId == skillId)
                    {
                        if (!dict.ContainsKey(talentConfig.Id))
                        {
                            dict[talentConfig.Id] = talentConfig.Id;
                        }
                    }
                }
            }

            foreach (var sp in this.ExclusiveDict)
            {
                if (sp.Value > 0)
                {
                    ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(sp.Key);

                    if (config.TalentId > 0)
                    {
                        SkillTalentConfig talentConfig = SkillTalentConfigCategory.Instance.Get(config.TalentId);
                        if (talentConfig.SkillId == skillId)
                        {
                            if (!dict.ContainsKey(talentConfig.Id))
                            {
                                dict[talentConfig.Id] = talentConfig.Id;
                            }
                        }
                    }
                }
            }

            List<SkillTalent> list = new List<SkillTalent>();
            foreach (var sp in dict)
            {
                SkillTalent talent = new SkillTalent(sp.Key);
                list.Add(talent);
            }

            return list;
        }

        public int GetSuitCount(int suitId)
        {
            int count = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            count += this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();

            return count;
        }

        public ShengxiaoGroup GetShengxiaoGroup()
        {
            List<Shengxiao> equips = this.ShengxiaoList.Select(m => m.Value).ToList();

            List<ShengxiaoGroupItem> redList = new List<ShengxiaoGroupItem>();

            for (int i = 3; i <= 12; i += 3)
            {
                List<ShengxiaoGroupConfig> list = ShengxiaoGroupConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Count == i).OrderByDescending(m => m.Quality).ToList();

                for (int j = 0; j < list.Count; j++)
                {
                    ShengxiaoGroupConfig config = list[j];

                    int count = equips.Where(m => m.GetQuality() >= config.Quality).Count();

                    if (count >= config.Count || config.Quality == 6) //如果激活了，则显示激活的颜色，如果没激活，则显示最低紫色的
                    {
                        ShengxiaoGroupItem redItem = new ShengxiaoGroupItem();
                        redItem.Count = count;
                        redItem.Config = config;
                        redList.Add(redItem);

                        break;
                    }
                }
            }

            ShengxiaoGroup red = new ShengxiaoGroup();
            red.List = redList;

            return red;
        }

        public EquipSetSuit GetEquipSet(int role, int cycle)
        {
            List<int> layers = null;

            if (cycle == 1)
            {
                List<Equip> equips = this.EquipPanelList[EquipPanelIndex].Select(m => m.Value).Where(m => m.Config.Role == role && m.GetQuality() == 5).ToList();

                layers = equips.Select(m => m.Config.LevelRequired).OrderByDescending(m => m).ToList();
            }

            List<EquipSetConfig> list = EquipSetConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Role == role && m.Cycle == cycle).ToList();

            List<EquipSetItem> redList = new List<EquipSetItem>();

            for (int i = 0; i < list.Count; i++)
            {
                EquipSetConfig config = list[i];

                int redLevel = layers.Count >= config.Count ? layers[config.Count - 1] : 0;
                int setLayer = redLevel / 10 + 1;

                EquipSetItem redItem = new EquipSetItem();
                redItem.Level = setLayer;
                redItem.Count = layers.Where(m => m >= redLevel).Count();
                redItem.Config = config;

                redList.Add(redItem);
            }

            EquipSetSuit red = new EquipSetSuit();
            red.List = redList;

            return red;
        }

        public EquipSetSuit GetEquipSpecialSet(int cycle)
        {
            List<int> layers = null;

            if (cycle == 101)
            {
                List<Equip_Special> equips = this.EquipSpecialList.Select(m => m.Value).Where(m => m.Config.Cycle == cycle).ToList();

                layers = equips.Select(m => m.Layer).OrderByDescending(m => m).ToList();
            }

            List<EquipSetConfig> list = EquipSetConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Cycle == cycle).ToList();

            List<EquipSetItem> redList = new List<EquipSetItem>();

            for (int i = 0; i < list.Count; i++)
            {
                EquipSetConfig config = list[i];

                int redLevel = layers.Count >= config.Count ? layers[config.Count - 1] : 0;

                EquipSetItem redItem = new EquipSetItem();
                redItem.Level = redLevel;
                redItem.Count = layers.Where(m => m >= redLevel).Count();
                redItem.Config = config;

                redList.Add(redItem);
            }

            EquipSetSuit red = new EquipSetSuit();
            red.List = redList;

            return red;
        }

        //---传奇套装
        public List<EquipLegendSet> GetActiveLegendSetList()
        {
            Dictionary<int, EquipLegendSet> dict = new Dictionary<int, EquipLegendSet>();

            foreach (var sp in this.EquipPanelList[EquipPanelIndex])
            {
                Equip equip = sp.Value;
                int lgId = equip.LegendData.Key;

                if (lgId > 0)
                {
                    int setId = equip.LegendConfig.SetId;
                    if (!dict.ContainsKey(setId))
                    {
                        EquipLegendSet set = new EquipLegendSet(setId);
                        dict[setId] = set;
                    }

                    dict[setId].Add(equip.LegendData.Value);
                }

            }

            return dict.Select(m => m.Value).Where(m => m.IsActive()).ToList();
        }

        public EquipLegendSet GetEquipLegendSet(int setId)
        {
            EquipLegendSet set = new EquipLegendSet(setId);

            foreach (var sp in this.EquipPanelList[EquipPanelIndex])
            {
                Equip equip = sp.Value;
                int lgId = equip.LegendData.Key;

                if (lgId > 0 && equip.LegendConfig.SetId == setId)
                {
                    set.Add(equip.LegendData.Value);
                }
            }

            return set;
        }

        public int GetRecordData(int type)
        {
            if (!RecordData.ContainsKey(type))
            {
                RecordData[type] = 0;
            }

            return RecordData[type];
        }

        public void SaveRecordData(int type, int data)
        {
            RecordData[type] = data;
        }

        public long GetAchievementProgeress(AchievementProType type)
        {
            long progress = 0;

            switch (type)
            {
                case AchievementProType.Advert:
                    progress = this.Record.Data.Data;
                    break;
                case AchievementProType.LegacyStrong:
                    progress = this.LegacyLevel.Select(m => m.Value.Data).Sum();
                    break;
                case AchievementProType.EquipStrong:
                    progress = this.MagicEquipStrength.Select(m => m.Value.Data).Sum();
                    break;
                case AchievementProType.EquipRefine:
                    progress = this.EquipPanelList[EquipPanelIndex].Select(m => m.Value.RefineLevel.Data).Sum();
                    break;
                case AchievementProType.EquipWear:
                    progress = this.EquipPanelList[EquipPanelIndex].Count;
                    break;
                case AchievementProType.SkillCount:
                    progress = this.SkillList.Count;
                    break;
                case AchievementProType.SkillLevel:
                    progress = this.SkillList.Select(m => m.MagicLevel.Data).Sum();
                    break;
                case AchievementProType.Level:
                    progress = this.MagicLevel.Data;
                    break;
                case AchievementProType.PetWear:
                    progress = this.PetList.Count;
                    break;
                case AchievementProType.PetBattle:
                    progress = this.PetList.Where(m => m.Status == 1).Count();
                    break;
                case AchievementProType.StageCount:
                    return this.MapId - 1;
                case AchievementProType.RecoverySet:
                    return this.RecoverySet.SetTotal;
                default:
                    {
                        int ct = (int)type;
                        if (!this.GameRecord.ContainsKey(ct))
                        {
                            this.GameRecord[ct] = new MagicData();
                        }
                        progress = this.GameRecord[ct].Data;
                    }
                    break;
            }

            return progress;
        }

        public void SetAchievementProgeress(AchievementProType type, long count)
        {
            int ct = (int)type;
            if (!this.GameRecord.ContainsKey(ct))
            {
                this.GameRecord[ct] = new MagicData();
            }

            this.GameRecord[ct].Data = count;
        }

        public void AddAchievementProgeress(AchievementProType type, long count)
        {
            int ct = (int)type;
            if (!this.GameRecord.ContainsKey(ct))
            {
                this.GameRecord[ct] = new MagicData();
            }

            this.GameRecord[ct].Data += count;
        }

        public int GetAchievementLevel(int id)
        {
            if (!this.AchievementData.ContainsKey(id))
            {
                AchievementData[id] = 0;
            }

            return AchievementData[id];
        }

        public void AddAchievementLevel(int id)
        {
            AchievementData[id]++;
        }


        public void KillMonsterEnvent(double rate, int quality, int count)
        {
            foreach (Pet sp in PetList)
            {
                sp.AddKillCount(rate);
                sp.AddExp(count);
            }

            AddAchievementProgeress(AchievementProType.MonsterKillTotal, count);
            AchievementProType mk = (AchievementProType)(301 + quality);
            AddAchievementProgeress(mk, count);
        }

        public void AddExpAndGold(double exp, double gold)
        {
            if (this.MagicGold.Data < 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }

            if (exp > 0)
            {
                if (this.MagicLevel.Data < GetMaxLevel())
                {
                    this.MagicExp.Data += (long)exp;
                }
                else
                {
                    this.MagicExp.Data = 0;
                }
            }

            if (gold > 0)
            {
                this.MagicGold.Data += (long)gold;
            }

            GameProcessor.Inst.EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI

            if (MagicExp.Data >= TempUpExp)
            {
                GameProcessor.Inst.StartCoroutine(LevelUp()); //升级
            }
        }

        public void SubExp(double exp)
        {
            if (exp <= 0 || this.MagicExp.Data < 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }
            this.MagicExp.Data -= (long)exp;

            GameProcessor.Inst.EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI
        }

        public void SubGold(double gold)
        {
            if (gold <= 0 || this.MagicGold.Data < 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }

            this.MagicGold.Data -= (long)gold;

            GameProcessor.Inst.EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI
        }

        IEnumerator LevelUp()
        {

            while (this.TempUpExp >= 1 && this.MagicExp.Data >= this.TempUpExp && this.MagicLevel.Data < GetMaxLevel())
            {
                MagicExp.Data -= TempUpExp;
                this.MagicLevel.Data++;

                SetUpExp();

                GameProcessor.Inst.EventCenter.Raise(new UserInfoUpdateEvent());
                GameProcessor.Inst.EventCenter.Raise(new SetPlayerLevelEvent { Cycle = this.Cycle.Data, Level = this.MagicLevel.Data });
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;

            if (this.MagicLevel.Data < 10000 && this.Cycle.Data <= 0)
            {
                GameProcessor.Inst.UpdateInfo();
            }

            //TaskHelper.CheckTask(TaskType.Cycle, this.Cycle.Data);
        }

        private void SetUpExp()
        {
            double levelAttr = LevelConfigCategory.GetLevelAttr(MagicLevel.Data);
            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= MagicLevel.Data && m.Value.EndLevel >= MagicLevel.Data).First().Value;

            double exp = StringHelper.StringToNumber(config.Exp);
            TempUpExp = (long)(levelAttr * exp);
        }

        public long GetBagItemCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.GetItemType() != ItemType.Equip && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }

        public long GetMaterialCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.GetItemType() == ItemType.Material && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }

        public long GetTicketCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.GetItemType() == ItemType.Ticket && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }

        public List<int> GetCurrentSkillList()
        {
            if (!SkillPanelList.ContainsKey(SkillPanelIndex))
            {
                SkillPanelList[SkillPanelIndex] = new List<int>();
            }
            return SkillPanelList[SkillPanelIndex];
        }

        public List<SkillData> GetCurrentSkill(List<int> existsList)
        {
            List<int> ids = GetCurrentSkillList();

            //Debug.Log(JsonConvert.SerializeObject(ids));

            List<SkillData> list = new List<SkillData>();

            for (int i = 0; i < ids.Count; i++)
            {
                SkillData skill = SkillList.Where(m => m.SkillId == ids[i] && !existsList.Contains(m.SkillId)).FirstOrDefault();
                if (skill != null)
                {
                    list.Add(skill);
                }
            }

            return list;
        }

        public int GetArtifactLevel(int artifactId)
        {
            if (!this.ArtifactData.ContainsKey(artifactId))
            {
                ArtifactData[artifactId] = new MagicData();
            }

            return (int)ArtifactData[artifactId].Data;
        }

        public int GetPetCount(int configId)
        {
            if (!this.PetCountData.ContainsKey(configId))
            {
                PetCountData[configId] = 1;
            }

            return PetCountData[configId];
        }

        public int GetPetSkillRate(int role)
        {
            return 0;
        }

        public void SetPetCount(int configId)
        {
            if (!this.PetCountData.ContainsKey(configId))
            {
                PetCountData[configId] = 1;
            }

            PetCountData[configId]++;
        }

        public int GetFestiveStep()
        {
            int currentStep = 99;

            List<FestiveConfig> list = FestiveConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Step > 0 && m.RequireCycle <= Cycle.Data).ToList();
            foreach (FestiveConfig config in list)
            {
                int max = this.GetFestiveCount(config.Id);
                if (max < config.Max && config.Step > 0 && config.Step < currentStep)
                {
                    currentStep = config.Step;
                }
            }

            return currentStep;
        }

        public int GetFestiveCount(int id)
        {
            if (!this.FestiveData_0102.ContainsKey(id))
            {
                this.FestiveData_0102[id] = 0;
            }

            return this.FestiveData_0102[id];
        }

        public void SaveFestiveCount(int configId, int count)
        {
            if (this.FestiveData_0102.ContainsKey(configId))
            {
                this.FestiveData_0102[configId] += count;
            }
            else
            {
                this.FestiveData_0102[configId] = count;
            }
        }

        public int GetSevenDayCount(int id)
        {
            if (!this.SevenDayData.ContainsKey(id))
            {
                this.SevenDayData[id] = 0;
            }

            return this.SevenDayData[id];
        }

        public void SaveSevenDayCount(int configId, int count)
        {
            if (this.SevenDayData.ContainsKey(configId))
            {
                this.SevenDayData[configId] += count;
            }
            else
            {
                this.SevenDayData[configId] = count;
            }
        }

        public double GetKillRecord(int dropId)
        {
            if (!KillRecord.ContainsKey(dropId))
            {
                KillRecord[dropId] = 0;
            }

            return KillRecord[dropId];
        }

        public void SaveKillRecord(int dropId, double kc)
        {
            if (!KillRecord.ContainsKey(dropId))
            {
                KillRecord[dropId] = 0;
            }

            KillRecord[dropId] += kc;
        }

        public long GetHideMaterialCount(int configId)
        {
            if (!ItemMeterialData.ContainsKey(configId))
            {
                ItemMeterialData[configId] = new MagicData();
            }

            return ItemMeterialData[configId].Data;
        }

        public void SaveHideMaterialCount(int configId, long count)
        {
            if (!ItemMeterialData.ContainsKey(configId))
            {
                ItemMeterialData[configId] = new MagicData();
            }

            ItemMeterialData[configId].Data += count;
        }

        public void UseHideMaterialCount(int configId, long count)
        {
            if (ItemMeterialData[configId].Data < count || count <= 0)
            {
                throw new Exception("数值错误");
            }

            ItemMeterialData[configId].Data -= count;
        }

        public long GetStrengthLevel(int position)
        {
            if (!MagicEquipStrength.ContainsKey(position))
            {
                MagicEquipStrength[position] = new MagicData();
            }

            return MagicEquipStrength[position].Data;
        }

        public void SaveStrengthLevel(int position, int level)
        {
            MagicEquipStrength[position].Data += level;
        }


        //public long GetRefineLevel(int position)
        //{
        //    if (!MagicEquipRefine.ContainsKey(position))
        //    {
        //        MagicEquipRefine[position] = new MagicData();
        //    }

        //    return MagicEquipRefine[position].Data;
        //}

        //public void SaveRefineLevel(int position, int level)
        //{
        //    MagicEquipRefine[position].Data += level;
        //}

        public long GetReformLevel(int position)
        {
            if (!MagicEquipReform.ContainsKey(position))
            {
                MagicEquipReform[position] = new MagicData();
            }

            return MagicEquipReform[position].Data;
        }

        public int GetLegacyLevel(int keyId)
        {
            if (!LegacyLevel.ContainsKey(keyId))
            {
                LegacyLevel[keyId] = new MagicData();
            }

            return (int)LegacyLevel[keyId].Data;
        }

        public void SaveLegacyLevel(int keyId, int level)
        {
            if (!LegacyLevel.ContainsKey(keyId))
            {
                LegacyLevel[keyId] = new MagicData();
            }

            LegacyLevel[keyId].Data += level;
        }

        public int GetLegacySetLayer(int role)
        {
            int startPart = (role - 1) * 8 + 1;
            int endPart = (role - 1) * 8 + 8;

            int count = LegacyLayer.Where(m => m.Key >= startPart && m.Key <= endPart).Count();
            if (count < 8)
            {
                return 0;
            }

            return (int)LegacyLayer.Where(m => m.Key >= startPart && m.Key <= endPart).Select(m => m.Value.Data).Min();
        }

        public int GetLegacyCurrentSet()
        {

            int r1 = GetLegacySetLayer(1);
            int r2 = GetLegacySetLayer(2);
            int r3 = GetLegacySetLayer(3);

            int currentLayer = MathHelper.GetMiddleNumber(r1, r2, r3) + 1;

            return currentLayer;
        }


        public int GetLegacyLayer(int keyId)
        {
            if (!LegacyLayer.ContainsKey(keyId))
            {
                LegacyLayer[keyId] = new MagicData();
            }

            return (int)LegacyLayer[keyId].Data;
        }

        public void SaveLegacyLayer(int keyId, int layer)
        {
            if (!LegacyLayer.ContainsKey(keyId))
            {
                LegacyLayer[keyId] = new MagicData();
            }

            LegacyLayer[keyId].Data = layer;
        }

        public long GetRingLevel(int ringId)
        {
            if (!RingData.ContainsKey(ringId))
            {
                RingData[ringId] = new MagicData();
            }

            return RingData[ringId].Data;
        }

        public void AddRingLevel(int ringId)
        {
            if (!RingData.ContainsKey(ringId))
            {
                RingData[ringId] = new MagicData();
            }

            RingData[ringId].Data++;
        }

        public long GetSoulRingLevel(int sid)
        {
            if (!SoulRingData.ContainsKey(sid))
            {
                SoulRingData[sid] = new MagicData();
            }
            return SoulRingData[sid].Data;
        }

        public long GetSoulBoneLevel(int sid)
        {
            if (!SoulBoneData.ContainsKey(sid))
            {
                SoulBoneData[sid] = new MagicData();
            }
            return SoulBoneData[sid].Data;
        }

        public void AddSoulBoneLevel(int sid)
        {
            if (!SoulBoneData.ContainsKey(sid))
            {
                SoulBoneData[sid] = new MagicData();
            }
            SoulBoneData[sid].Data++;
        }

        public int GetRelicGroupLevel(int gid)
        {
            int startId = 1 + (gid - 1) * 8;
            int endId = gid * 8;

            long groupLevel = RelicData.Where(m => m.Key >= startId && m.Key <= endId).Select(m => m.Value.Data).DefaultIfEmpty(0).Min();

            return (int)Math.Min(groupLevel, Cycle.Data) + this.GetRelicRise();
        }

        public int GetRelicRise()
        {
            if (this.Cycle.Data <= 30)
            {
                return 0;
            }

            return (int)Math.Min(this.Cycle.Data - 30, 10);
        }

        public int GetRelicLevel(int rid)
        {
            if (!RelicData.ContainsKey(rid))
            {
                RelicData[rid] = new MagicData();
            }
            return (int)RelicData[rid].Data;
        }

        public void AddRelicLevel(int rid)
        {
            if (!RelicData.ContainsKey(rid))
            {
                RelicData[rid] = new MagicData();
            }
            RelicData[rid].Data++;
        }

        public long GetTalentLevel(int tid)
        {
            if (!TalentData.ContainsKey(tid))
            {
                TalentData[tid] = new MagicData();
            }
            return TalentData[tid].Data;
        }

        public void AddTalentLevel(int tid, int fee)
        {
            if (!TalentData.ContainsKey(tid))
            {
                TalentData[tid] = new MagicData();
            }
            TalentData[tid].Data++;
            TalentPoint += fee;
        }

        public long GetHalidomLevel(int id)
        {
            if (!HalidomData.ContainsKey(id))
            {
                HalidomData[id] = new MagicData();
            }

            return HalidomData[id].Data;
        }


        public void SaveHalidom(int id)
        {
            if (!HalidomData.ContainsKey(id))
            {
                HalidomData[id] = new MagicData();
            }

            HalidomData[id].Data++;
        }

        public long GetFashionLevel(int id)
        {
            if (!FashionData.ContainsKey(id))
            {
                FashionData[id] = new MagicData();
            }

            return FashionData[id].Data;
        }

        public void SaveFashionLevel(int id)
        {
            if (!FashionData.ContainsKey(id))
            {
                FashionData[id] = new MagicData();
            }

            FashionData[id].Data++;
        }

        public long GetMetalLevel(int id)
        {
            if (!MetalData.ContainsKey(id))
            {
                MetalData[id] = new MagicData();
            }

            return MetalData[id].Data;
        }

        public long GetMetalQualityLevel(int quality)
        {
            MetalConfig config = MetalConfigCategory.Instance.GetQualityRiseConfig(quality);

            if (config == null)
            {
                return 0;
            }

            if (!MetalData.ContainsKey(config.Id))
            {
                return 0;
            }

            return MetalData[config.Id].Data;
        }

        public StoneRecord GetStoneRecord(int id)
        {
            if (!StoneData.ContainsKey(id))
            {
                StoneData[id] = new StoneRecord();
            }

            return StoneData[id];
        }

        internal long GetMaxLevel()
        {
            return Cycle.Data * ConfigHelper.Cycle_Level + ConfigHelper.Max_Level;
        }

        public int GetBagIdleCount(int index)
        {
            return ConfigHelper.BagCount[index] - this.Bags.Where(m => m.GetBagType() == index).Count();
        }

        public int GetLimitId()
        {
            int limitId = 0;

            if (this.First_Create_Time > 0)
            {
                limitId += (int)((TimeHelper.ClientNowSeconds() - this.First_Create_Time) / 86400);
            }
            else
            {
                limitId += 2000;
            }

            limitId += this.Account.Length * 1000;

            return limitId + 1020;
        }

        public List<Item> CheckRecovery(List<Item> items, out long gold, out int recoveryCount)
        {
            List<Item> newList = new List<Item>();
            gold = 0;

            List<Item> recoveryList = items.Where(m => RecoverySet.CheckRecovery(m, RecoveryType.Drop)).ToList();

            recoveryCount = recoveryList.Count;

            if (recoveryList.Count > 0)
            {
                Dictionary<int, long> recoveryDict = new Dictionary<int, long>();

                foreach (Item item in recoveryList)
                {
                    gold += item.ToRecoverDict(recoveryDict, 1);

                    if (item.GetItemType() == ItemType.Equip || item.GetItemType() == ItemType.EquipSpeical)
                    {
                        this.AddAchievementProgeress(AchievementProType.RecoveryTotal, 1);
                    }
                }

                foreach (var kvp in recoveryDict)
                {
                    if (kvp.Value > 0)
                    {
                        Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value);
                        newList.Add(recoveryItem);
                    }
                }

                items.RemoveAll(m => RecoverySet.CheckRecovery(m, RecoveryType.Drop));
                items.AddRange(newList);
            }



            return newList;
        }

        public List<Item> AutoToCard(List<Item> items)
        {

            List<Item> cardList = items.Where(m => RecoverySet.CheckAutuCard(m)).ToList();

            if (cardList.Count > 0)
            {
                items.RemoveAll(m => RecoverySet.CheckAutuCard(m));

                foreach (var sp in cardList)
                {
                    if (sp.GetItemType() == ItemType.Equip)
                    {
                        Equip equip = sp as Equip;

                        this.CardRecord[equip.Config.CardId] += 1;
                    }
                    else if (sp.GetItemType() == ItemType.Pet)
                    {

                        Pet pet = sp as Pet;

                        this.CardRecord[pet.Config.CardId] += 1;
                    }
                }
            }

            return cardList;
        }

        public bool CheckKeepSkill(int skillId, int skillLayer)
        {
            int c = this.SkillList.Where(m => (m.SkillId == skillId || m.SkillConfig.SkillLayer == skillLayer) && m.Recovery).Count();

            return c > 0;
        }

        private long GetRecordMax(int key)
        {
            if (RecordMax.ContainsKey(key))
            {
                return RecordMax[key];
            }
            return 0;
        }

        public void SaveRecordMax(int key, long v)
        {
            if (!RecordMax.ContainsKey(key))
            {
                RecordMax[key] = v + 3;
            }

            if (RecordMax[key] < v)
            {
                RecordMax[key] = v + 5;
            }
        }

        public int GetPetSpeicalLevel(int rid)
        {
            if (!PetSpeicalLevelData.ContainsKey(rid))
            {
                PetSpeicalLevelData[rid] = new MagicData();
            }
            return (int)PetSpeicalLevelData[rid].Data;
        }

        public void AddPetSpeicalLevel(int rid)
        {
            if (!PetSpeicalLevelData.ContainsKey(rid))
            {
                PetSpeicalLevelData[rid] = new MagicData();
            }
            PetSpeicalLevelData[rid].Data++;
        }
        public int GetPetSpeicalLayer(int rid)
        {
            if (!PetSpeicalLayerData.ContainsKey(rid))
            {
                PetSpeicalLayerData[rid] = new MagicData();
            }
            return (int)PetSpeicalLayerData[rid].Data;
        }

        public void AddPetSpeicalLayer(int rid)
        {
            if (!PetSpeicalLayerData.ContainsKey(rid))
            {
                PetSpeicalLayerData[rid] = new MagicData();
            }
            PetSpeicalLayerData[rid].Data++;
        }

        public int GetPetSpeicalGroupLevel()
        {
            if (PetSpeicalLayerData.Count < 3)
            {
                return 0;
            }

            return (int)PetSpeicalLayerData.Select(m => m.Value.Data).Min();
        }


        public long GetFestiveAttrLevel(int key)
        {
            if (!FestiveAttrData.ContainsKey(key))
            {
                FestiveAttrData[key] = new MagicData();
            }
            return FestiveAttrData[key].Data;
        }

        public void SaveFestiveAttrLevel(int key)
        {
            if (!FestiveAttrData.ContainsKey(key))
            {
                FestiveAttrData[key] = new MagicData();
            }
            FestiveAttrData[key].Data++;
        }


        public void SaveSpiritLevel(int cardId, long level)
        {
            if (!SpiritRecord.ContainsKey(cardId))
            {
                SpiritRecord[cardId] = new SpiritData();
            }

            SpiritRecord[cardId].Level.Data += level;
        }

        public int GetSpiritLevel(int cardId)
        {
            if (!SpiritRecord.ContainsKey(cardId))
            {
                SpiritRecord[cardId] = new SpiritData();
            }

            return (int)SpiritRecord[cardId].Level.Data;
        }
    }
}
