using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class EquipSpeicalConfigCategory
    {
        public Item BuildEquip(int configId, int layer)
        {
            Item item = new Equip_Special(configId);

            return item;
        }

        //public Equip BuildByPack(int configId)
        //{
        //    GiftPackEquipConfig config = GiftPackEquipConfigCategory.Instance.Get(configId);

        //    Equip item = new Equip(config.EquipId, config.RuneId, config.SuitId, config.Quality);

        //    List<KeyValuePair<int, long>> AttrEntryList = new List<KeyValuePair<int, long>>();

        //    for (int i = 0; i < config.AttrIdList.Length; i++)
        //    {
        //        int attrId = config.AttrIdList[i];
        //        AttrEntryConfig entryConfig = AttrEntryConfigCategory.Instance.GetRedConfig(attrId, config.Cycle);
        //        AttrEntryList.Add(new KeyValuePair<int, long>(attrId, entryConfig.MaxValue));
        //    }

        //    item.AttrEntryList = AttrEntryList;

        //    return item;
        //}

        public EquipSpeicalConfig GetConfig(int sid, int layer)
        {
            return this.list.Where(m => m.Sid == sid && m.StartLayer <= layer && layer <= m.EndLayer).FirstOrDefault();
        }
    }
}