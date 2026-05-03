using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class EquipConfigCategory
    {
        public Item BuildEquip(int configId, double qualityRise, int seed)
        {
            int quality = RandomQuanlity(qualityRise);

            EquipConfig config = this.Get(configId);

            int runeId = 0;
            int suitId = 0;

            if (quality >= 3)
            {
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.RandomEquipRuneId(quality, config.Role, seed);

                if (runeConfig == null)
                {

                    Debug.LogError("erro config equip id£º" + configId);
                }

                runeId = runeConfig.Id;

                if (quality >= 4)
                {
                    suitId = SkillSuitConfigCategory.Instance.RandomSuit(runeConfig.SkillId, quality, seed).Id;
                }
            }

            Equip item = new Equip(configId, runeId, suitId, quality);
            item.Init(seed);

            Item_Component component = new Item_Com_Equip(configId, qualityRise);

            item.AddComponent(component);

            return item;
        }

        public Equip BuildByPack(int configId)
        {
            GiftPackEquipConfig config = GiftPackEquipConfigCategory.Instance.Get(configId);

            Equip item = new Equip(config.EquipId, config.RuneId, config.SuitId, config.Quality);

            List<KeyValuePair<int, long>> AttrEntryList = new List<KeyValuePair<int, long>>();

            for (int i = 0; i < config.AttrIdList.Length; i++)
            {
                int attrId = config.AttrIdList[i];
                AttrEntryConfig entryConfig = AttrEntryConfigCategory.Instance.GetRedConfig(attrId, config.Cycle);
                AttrEntryList.Add(new KeyValuePair<int, long>(attrId, entryConfig.MaxValue));
            }

            item.AttrEntryList = AttrEntryList;

            item.Count = 1;
            return item;
        }

        private static int RandomQuanlity(double qualityRise)
        {
            int start = 0;

            int[] rates = { 1, 10, 100, 500, 2500 };

            int r = RandomHelper.RandomNumber(0, rates[rates.Length - 1]);

            r = (int)(r / qualityRise);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    return 5 - i - start;
                }
            }

            return 1;
        }


        public List<EquipConfig> GetCardList(int cardId)
        {
            return this.list.Where(m => m.CardGroupId == cardId).ToList();
        }
    }
}