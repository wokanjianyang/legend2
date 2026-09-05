using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteDropConfigCategory
    {
        public InfiniteDropConfig GetConfig(int dropId)
        {
            return this.list.Where(m => m.DropBaseId == dropId).FirstOrDefault();
        }

        public List<DropData> GetAllDropIdList()
        {
            int maxLevel = ConfigHelper.Infinit_Max;

            List<DropData> rates = new List<DropData>();

            Dictionary<int, int> dict = new Dictionary<int, int>();

            List<InfiniteDropConfig> dropConfigs = new List<InfiniteDropConfig>();

            foreach (InfiniteDropConfig config in this.list)
            {

                dropConfigs.Add(config);
            }


            for (int level = 1; level <= maxLevel; level++)
            {
                dropConfigs.RemoveAll(m => m.EndLevel < level);

                List<InfiniteDropConfig> tempConfigs = dropConfigs.Where(m => m.StartLevel <= level && m.EndLevel >= level && (level - m.StartLevel) % m.RateLevel == 0).ToList();

                InfiniteDropConfig config = RandomId(tempConfigs);

                if (config == null)
                {
                    rates.Add(new DropData(0, 0));
                    continue;
                }

                if (!dict.ContainsKey(config.Id))
                {
                    dict[config.Id] = 0;
                }

                dict[config.Id]++;

                if (dict[config.Id] >= config.Max)
                {
                    dropConfigs.Remove(config); //掉落上限的，去掉
                }

                DropBaseConfig dropBaseConfig = DropBaseConfigCategory.Instance.Get(config.DropBaseId);
                int di = RandomHelper.RandomNumber(0, dropBaseConfig.ItemIdList.Length);

                rates.Add(new DropData(config.DropBaseId, di));
            }

            return rates;
        }

        private InfiniteDropConfig RandomId(List<InfiniteDropConfig> dropConfigs)
        {
            int total = dropConfigs.Select(m => m.Rate).Sum();
            int rd = RandomHelper.RandomNumber(1, total + 1);

            int endRate = 0;
            for (int i = 0; i < dropConfigs.Count; i++)
            {
                endRate += dropConfigs[i].Rate;

                if (rd <= endRate)
                {
                    return dropConfigs[i];
                }
            }

            return null;
        }

        //private int RandomDropId(List<InfiniteDropConfig> dropConfigs)
        //{
        //    int total = dropConfigs.Select(m => m.Rate).Sum();
        //    int rd = RandomHelper.RandomNumber(1, total + 1);

        //    int endRate = 0;
        //    for (int i = 0; i < dropConfigs.Count; i++)
        //    {
        //        endRate += dropConfigs[i].Rate;

        //        if (rd <= endRate)
        //        {
        //            return dropConfigs[i].DropId;
        //        }
        //    }

        //    return -1;
        //}

        //private List<InfiniteDropConfig> GetLevelList(long level, List<int> excludeList)
        //{
        //    List<InfiniteDropConfig> configs = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level && (level - m.StartLevel) % m.RateLevel == 0).ToList();

        //    List<InfiniteDropConfig> list = new List<InfiniteDropConfig>();

        //    foreach (InfiniteDropConfig config in configs)
        //    {
        //        int total = excludeList.Where(m => m == config.DropId).Count();

        //        if (config.DropId >= 180001 && config.DropId <= 180100) //神器
        //        {
        //            ArtifactConfig artifactConfig = ArtifactConfigCategory.Instance.GetByItemId(config.DropId);
        //            int atLevel = User_Data_Manager.Data.GetArtifactLevel(artifactConfig.Id);

        //            if (total + atLevel >= artifactConfig.MaxCount)
        //            {
        //                continue;
        //            }
        //        }

        //        if (config.Max > total)
        //        {
        //            list.Add(config);
        //        }
        //    }
        //    return list;
        //}
    }

}
