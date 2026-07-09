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
            EquipConfig config = this.Get(configId);

            if (config.Cycle == 1)
            {
                return BuildCycle1(config, qualityRise, seed);
            }
            else if (config.Cycle == 10)
            {
                return BuildCycle10(config, seed);
            }

            return null;
        }

        public Equip BuildCycle1(EquipConfig config, double qualityRise, int seed)
        {
            int quality = MathHelper.RandomArrayIndex(rates, qualityRise);

            int runeId = 0;
            int suitId = 0;

            if (quality >= 3)
            {
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.RandomEquipRuneId(quality, config.Role, config.LevelRequired);

                if (runeConfig == null)
                {

                    Debug.LogError("erro config equip id£º" + config.Id);
                }

                runeId = runeConfig.Id;

                if (quality >= 4)
                {
                    suitId = SkillSuitConfigCategory.Instance.RandomSuit(runeConfig.SkillId, quality, seed).Id;
                }
            }

            Equip item = new Equip(config.Id, runeId, suitId, quality);
            item.Init(seed);

            return item;
        }

        public Equip BuildCycle10(EquipConfig config, int seed)
        {
            Equip item = new Equip(config.Id, 0, 0, config.Quality);

            int lgId = item.Config.LegendId;
            int lgFlair = seed > 0 ? 40 : RandomHelper.RandomNumber(20, 100 + 1);

            item.LegendData = new KeyValuePair<int, int>(lgId, lgFlair);

            return item;
        }

        private int RandonFlair(int seed)
        {
            if (seed > 0)
            {
                return 40;
            }

            if (RandomHelper.RandomCritRate(5))
            {
                return RandomHelper.RandomNumber(90, 101);
            }
            else
            {
                return RandomHelper.RandomNumber(20, 91);
            }
        }

        public Item BuildOfflineEquip(int configId, int quality)
        {
            EquipConfig config = this.Get(configId);

            int runeId = 0;
            int suitId = 0;

            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.RandomEquipRuneId(quality, config.Role, config.LevelRequired);

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


        public int GetOfflineKeepCount(int count)
        {
            return count / rates[rates.Length - 1];
        }

        public int GetOfflineKeepCount1(int count)
        {
            return count / rates[rates.Length - 2];
        }
    }
}