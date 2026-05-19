using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Equip_Special : Item
    {
        /// <summary>
        /// 词条属性列表
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();

        [JsonIgnore]
        public EquipSpeicalConfig Config { get; set; }


        public Equip_Special(int configId) : base(configId, ItemType.EquipSpeical)
        {
            this.Config = EquipSpeicalConfigCategory.Instance.Get(configId);
        }

        public IDictionary<int, double> GetBaseAttrList()
        {
            IDictionary<int, double> BaseAttrList = new Dictionary<int, double>();

            for (int i = 0; i < Config.AttrIdList.Length; i++)
            {
                int ai = Config.AttrIdList[i];
                double av = Config.AttrValueList[i];

                BaseAttrList.Add(ai, av + Config.AttrRiseList[i] * Layer);
            }

            return BaseAttrList;
        }

        private int GetLayerRate(int layer)
        {
            int b = 1;
            for (int i = 1; i < layer; i++)
            {
                b = b * 2;
            }
            return b;
        }


        public void Init(int seed)
        {
            //根据品质,生成随机属性

            //this.AttrEntryList.AddRange(AttrEntryConfigCategory.Instance.Build(this.Part, this.EquipConfig.Cycle, this.Quality, this.EquipConfig.Role, seed));
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, double> GetTotalAttrList()
        {
            return GetBaseAttrList();
        }

        public void Grade()
        {
            this.Layer++;
        }

        //------------------------------------------override----------------

        public override int GetQuality()
        {
            return this.Config.Quality;
        }

        public override string GetName()
        {
            return this.Config.Name;
        }

        public override int GetBagType()
        {
            return 3;
        }

        public override ShowType GetShowType()
        {
            return ShowType.Equip_Special;
        }

        public override long ToRecoverDict(Dictionary<int, long> dict, long number)
        {
            if (Config.Cycle == 101)
            {
                //四格的回收
                long rn = CalRecoveryNumber();
                int rid = this.Config.RecoveryItemId;

                if (!dict.ContainsKey(rid))
                {
                    dict[rid] = 0;
                }

                dict[rid] += rn;
            }

            return 1;
        }

        private long CalRecoveryNumber()
        {
            return this.Layer + 1;
        }
    }
}
