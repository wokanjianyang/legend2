using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetTraitConfigCategory
    {
        private int[] rates = { 1, 4, 10, 25, 45 };

        public List<PetTrait> BuildTraitList(int petId, int type, int role, int quality, int offline)
        {
            List<PetTraitConfig> configs = this.list.Where(m => (m.Role == 0 || m.Role == role) && m.StartPetId <= petId && petId <= m.EndPetId && m.Type == type
            && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            List<PetTrait> traits = new List<PetTrait>();

            List<int> excludeList = new List<int>();

            for (int i = 1; i <= 1; i++)
            {
                List<PetTraitConfig> temps = configs.Where(m => !excludeList.Contains(m.Id)).ToList();
                List<int> rates = temps.Select(m => m.Rate).ToList();
                int rd = RandomHelper.RandomListRateIndex(rates);
                PetTraitConfig config = temps[rd];

                int maxLevel = (petId - config.StartPetId) / config.LevelRate + 1;

                int level = RandomHelper.RandomNumber(1, maxLevel + 1);

                if (type == 1)
                {
                    level = maxLevel;  //固定天赋，等级固定
                }

                int tt = 1;
                if (offline == 0 && RandomHelper.RandomDropRate(100)) //在线天赋可能变异
                {
                    tt = 2;
                }


                PetTrait trait = new PetTrait();
                trait.Id = config.Id;
                trait.Level = level;
                trait.Type = tt;
                traits.Add(trait);

                excludeList.Add(config.Id);
            }

            return traits;
        }
    }
    public partial class PetTraitConfig
    {
        public long GetVue(int i, int level, int type)
        {
            int bv = type == 1 ? this.AtrVueList[i] : this.AtrVueList1[i];
            long vue = MathHelper.GetSeqByType(this.RiseType[i], level, bv);
            return vue;
        }
    }
}
