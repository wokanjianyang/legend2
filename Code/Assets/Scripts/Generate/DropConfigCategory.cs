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
                    int layer = 0;
                    if (mapConfig.BaseLayerList != null && mapConfig.BaseLayerList.Length > i)
                    {
                        layer = mapConfig.BaseLayerList[i];
                    }
                    if (RandomHelper.RandomDropRate(realRate))
                    {
                        list.Add(BuildByDropBaseId(mapConfig.BaseIdList[i], qualityRise, 0, layer, 0, 0));
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
                        int layer = 0;
                        if (mapConfig.DropLayerList != null && mapConfig.DropLayerList.Length > i)
                        {
                            layer = mapConfig.DropLayerList[i];
                        }
                        list.Add(BuildByDropId(mapConfig.DropIdList[i], qualityRise, 0, layer, 0, 0));
                    }
                }
            }

            return list;
        }

        public List<Item> BuildBossDropItem(int groupId, double burstRise, double qualityRise, double equipRise, double petRise)
        {
            MapGroupConfig groupConfig = MapGroupConfigCategory.Instance.Get(groupId);

            List<Item> list = new List<Item>();

            if (groupConfig.BaseIdList != null)
            {
                for (int i = 0; i < groupConfig.BaseIdList.Length; i++)
                {
                    int realRate = (int)(groupConfig.BaseRateList[i] / burstRise);
                    if (RandomHelper.RandomDropRate(realRate))
                    {
                        int layer = 0;
                        if (groupConfig.BaseLayerList != null && groupConfig.BaseLayerList.Length > i)
                        {
                            layer = groupConfig.BaseLayerList[i];
                        }

                        list.Add(BuildByDropBaseId(groupConfig.BaseIdList[i], qualityRise, 0, layer, equipRise, petRise));
                    }
                }
            }

            if (groupConfig.DropIdList != null)
            {
                for (int i = 0; i < groupConfig.DropIdList.Length; i++)
                {
                    int realRate = (int)(groupConfig.DropRateList[i] / burstRise);
                    if (RandomHelper.RandomDropRate(realRate))
                    {
                        int layer = 0;
                        if (groupConfig.DropLayerList != null && groupConfig.DropLayerList.Length > i)
                        {
                            layer = groupConfig.DropLayerList[i];
                        }

                        list.Add(BuildByDropId(groupConfig.DropIdList[i], qualityRise, 0, layer, equipRise, petRise));
                    }
                }
            }

            return list;
        }

        public Item BuildByDropBaseId(int baseId)
        {
            return BuildByDropBaseId(baseId, 1, 0, 0, 0, 0);
        }

        public Item BuildByDropBaseId(int baseId, double qualityRise, int seed, int layer, double equipRise, double petRise)
        {
            DropBaseConfig config = DropBaseConfigCategory.Instance.Get(baseId);

            //if (config.ItemIdList.Length <= 0)
            //{
            //    Debug.LogError(" drop base id£º" + baseId + " size=0");
            //}

            int itemIndex = RandomHelper.RandomNumber(seed, 0, config.ItemIdList.Length);


            if (config.ItemType == (int)ItemType.Equip)
            {
                qualityRise += equipRise;
            }
            else if (config.ItemType == (int)ItemType.Pet)
            {
                qualityRise += petRise;
            }

            return ItemHelper.BuildItemNew((ItemType)config.ItemType, config.ItemIdList[itemIndex], (int)qualityRise, 1, seed, layer);
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

                list.Add(ItemHelper.BuildItemNew((ItemType)config.ItemType, config.ItemIdList[itemIndex], (int)qualityRise, sp.Value, seed));

            }

            return list;
        }

        public Item BuildByDropId(int dropId, double qualityRise, int seed, int layer, double equipRise, double petRise)
        {
            DropConfig config = DropConfigCategory.Instance.Get(dropId);

            int index = RandomDropIndex(config, 0);

            int baseId = config.BaseIdList[index];

            return BuildByDropBaseId(baseId, qualityRise, seed, layer, equipRise, petRise);
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