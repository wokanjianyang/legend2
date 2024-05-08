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
    }

    public class DropHelper
    {
        public static List<Item> TowerEquip(long fl, int equipLevel)
        {
            List<Item> items = new List<Item>();

            if (fl % 20000 == 0)
            {
                //2w����䵱ǰ�ȼ���װ
                items.AddRange(DropHelper.RandomLevelEquip((int)fl, equipLevel, 5));
            }
            if (fl % 1500 == 0)
            {
                //2000����䵱ǰ�ȼ���װ
                items.AddRange(DropHelper.RandomLevelEquip((int)fl, equipLevel, 4));
            }
            if (fl % 1000 == 0)
            {
                int fourLevel = 1;
                if (fl > 500000)  //50w���ϣ�����2���ĸ�,���µ���3��4��
                {
                    fourLevel = 2;
                }
                int rd = RandomHelper.RandomNumber(1, 5);
                items.Add(ItemHelper.BuildEquip(rd * 100 + fourLevel, 1, 1, -1));
            }

            if (fl % 30 == 0)
            {
                //ÿ30����䵱ǰ�ȼ����װ��
                items.AddRange(DropHelper.RandomLevelEquip((int)fl, equipLevel, 0));
            }
            return items;
        }

        public static List<Item> RandomLevelEquip(int seed, int equipLevel, int quality)
        {
            List<Item> list = new List<Item>();

            List<DropConfig> drops = DropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Level == equipLevel && m.ItemType == 2).ToList(); //��ȡ��ǰ�ȼ���װ��
            if (drops.Count > 0)
            {
                int[] ids = drops[0].ItemIdList;
                if (ids != null && ids.Length > 0)
                {
                    int index = RandomHelper.RandomNumber(0, ids.Length);

                    Item item = ItemHelper.BuildEquip(ids[index], quality, 1, AppHelper.RefreshSeed(seed));  //�̶�����
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<Item> BuildDropItem(List<KeyValuePair<double, DropConfig>> dropList, int qualityRate)
        {
            List<Item> list = new List<Item>();

            for (int i = 0; i < dropList.Count; i++)
            {
                double rate = dropList[i].Key;
                DropConfig config = dropList[i].Value;

                if (RandomHelper.RandomResult(rate))
                {
                    int index = RandomHelper.RandomNumber(0, config.ItemIdList.Length);
                    int configId = config.ItemIdList[index];

                    Item item = ItemHelper.BuildItem((ItemType)config.ItemType, configId, qualityRate, config.Quantity);
                    list.Add(item);
                }
            }
            return list;
        }
    }
}