using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class DropConfigCategory
    {
        public List<KeyValuePair<double, DropConfig>> GetByMapLevel(int mapId, double rate)
        {
            List<KeyValuePair<double, DropConfig>> list = new List<KeyValuePair<double, DropConfig>>();

            MapConfig map = MapConfigCategory.Instance.Get(mapId);

            if (map != null)
            {
                for (int i = 0; i < map.DropIdList.Length; i++)
                {
                    DropConfig dropConfig = this.Get(map.DropIdList[i]);
                    list.Add(new KeyValuePair<double, DropConfig>(map.DropRateList[i] / rate, dropConfig));

                    //Debug.Log("fd:" + (map.DropRateList[i] / rate));
                }
            }

            return list;
        }

        public List<Item> BuildDropItem(int mapId, double burstRise, double qualityRise)
        {
            MapConfig mapConfig = MapConfigCategory.Instance.Get(mapId);

            List<Item> list = new List<Item>();

            if (mapConfig.BaseIdList != null)
            {
                for (int i = 0; i < mapConfig.BaseIdList.Length; i++)
                {
                    int realRate = (int)(mapConfig.BaseRateList[i] / burstRise);
                    if (RandomHelper.RandomDropRate(realRate))
                    {
                        list.Add(BuildByDropBaseId(mapConfig.BaseIdList[i], (int)qualityRise, 0));
                    }
                }
            }

            if (mapConfig.DropIdList != null)
            {
                for (int i = 0; i < mapConfig.DropIdList.Length; i++)
                {
                    int realRate = (int)(mapConfig.DropRateList[i] / burstRise);
                    if (RandomHelper.RandomDropRate(realRate))
                    {
                        list.Add(BuildByDropId(mapConfig.DropIdList[i], (int)qualityRise));
                    }
                }
            }

            return list;
        }

        public Item BuildByDropBaseId(int baseId, int qualityRise, int seed)
        {
            DropBaseConfig config = DropBaseConfigCategory.Instance.Get(baseId);

            //if (config.ItemIdList.Length <= 0)
            //{
            //    Debug.LogError(" drop base id£º" + baseId + " size=0");
            //}

            int itemIndex = RandomHelper.RandomNumber(seed,0, config.ItemIdList.Length);

            return ItemHelper.BuildItem((ItemType)config.ItemType, config.ItemIdList[itemIndex], (int)qualityRise, 1);
        }
        public List<Item> BuildByDropBaseIdList(List<int> idList, int qualityRise, int seed)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < idList.Count; i++)
            {
                int baseId = idList[i];
                if (!dict.ContainsKey(baseId))
                {
                    dict[baseId] = 0;
                }

                dict[baseId]++;
            }

            List<Item> list = new List<Item>();

            foreach (var sp in dict)
            {
                int baseId = sp.Key;

                DropBaseConfig config = DropBaseConfigCategory.Instance.Get(baseId);
                int itemIndex = RandomHelper.RandomNumber(0, config.ItemIdList.Length, seed);

                list.Add(ItemHelper.BuildItem((ItemType)config.ItemType, config.ItemIdList[itemIndex], (int)qualityRise, sp.Value));

            }

            return list;
        }

        public Item BuildByDropId(int dropId, int qualityRise)
        {
            DropConfig config = DropConfigCategory.Instance.Get(dropId);

            int index = RandomDropIndex(config, 0);

            int baseId = config.BaseIdList[index];

            return BuildByDropBaseId(baseId, qualityRise, 0);
        }


        public int RandomDropIndex(DropConfig config, int seed)
        {

            if (config.BaseRateList != null && config.BaseRateList.Length > 0)
            {
                int[] RateList = config.BaseRateList;

                int total = RateList.Select(m => m).Sum();

                int rd = RandomHelper.RandomNumber(seed, 1, total + 1);

                int endRate = 0;
                for (int i = 0; i < RateList.Length; i++)
                {
                    endRate += RateList[i];

                    if (rd <= endRate)
                    {
                        return i;
                    }
                }

                return 0;
            }
            else
            {
                int index = RandomHelper.RandomNumber(0, config.BaseIdList.Length);
                return index;
            }
        }
    }
}