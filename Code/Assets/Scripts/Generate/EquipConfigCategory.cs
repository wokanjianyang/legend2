using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class EquipConfigCategory
    {
        private int[] rates = { 1, 5, 40, 200, 1000 };

        public Item BuildEquip(int configId, double qualityRise, int seed)
        {
            int quality = MathHelper.RandomArrayIndex(rates, qualityRise);

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

            return item;
        }

        public Item BuildOfflineEquip(int configId, int quality)
        {
            EquipConfig config = this.Get(configId);

            int runeId = 0;
            int suitId = 0;

            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.RandomEquipRuneId(quality, config.Role, 0);

            runeId = runeConfig.Id;

            suitId = SkillSuitConfigCategory.Instance.RandomSuit(runeConfig.SkillId, quality, 0).Id;

            Equip item = new Equip(configId, runeId, suitId, quality);
            item.Init(0);

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

            return item;
        }

        public List<EquipConfig> GetCardList(int cardId)
        {
            return this.list.Where(m => m.CardGroupId == cardId).ToList();
        }

        public int GetOfflineKeepCount(int count)
        {
            return count / rates[rates.Length - 1];
        }
    }
}