using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Pet : Item
    {
        public MagicData PetLevel { get; set; } = new MagicData();

        public MagicData PetLayer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        public MagicDouble KillCount { get; set; } = new MagicDouble();

        public MagicDouble InheritCount { get; set; } = new MagicDouble();

        //public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<KeyValuePair<int, MagicData>> Skills { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<PetTrait> TraitList { get; set; } = new List<PetTrait>();

        public List<int> Talents { get; set; } = new List<int>();

        public int Role { get; set; }

        public int Status { get; set; } = 0;

        private string Name;

        [JsonIgnore]
        public PetConfig Config { get; set; }

        public Pet(int configId, int role) : base(configId, ItemType.Pet)
        {
            this.Role = role;

            this.Config = PetConfigCategory.Instance.Get(configId);
            this.Name = Config.Name;
        }

        public Dictionary<int, double> GetTotalAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            foreach (var sp in this.Flairs)
            {
                PetAtrConfig config = PetAtrConfigCategory.Instance.Get(sp.Key);
                int attrId = config.AtrId;

                if (!attrs.ContainsKey(attrId))
                {
                    attrs[attrId] = 0;
                }

                long attrValue = GetTotalKillCount() * config.AtrVue / sp.Value.Data;
                attrs[attrId] += attrValue;
            }

            //if (Config.TraitId > 0)
            //{
            //    PetTraitConfig trait = PetTraitConfigCategory.Instance.Get(Config.TraitId);
            //    for (int i = 0; i < trait.AtrIdList.Length; i++)
            //    {
            //        int attrId = trait.AtrIdList[i];
            //        if (!attrs.ContainsKey(attrId))
            //        {
            //            attrs[attrId] = 0;
            //        }

            //        long atrVue = trait.GetVue(i, Config.TraitLevel, this.TraitType);
            //        attrs[attrId] += atrVue;
            //    }
            //}

            for (int t = 0; t < this.TraitList.Count; t++)
            {
                PetTraitConfig trait = PetTraitConfigCategory.Instance.Get(this.TraitList[t].Id);
                for (int i = 0; i < trait.AtrIdList.Length; i++)
                {
                    int attrId = trait.AtrIdList[i];
                    if (!attrs.ContainsKey(attrId))
                    {
                        attrs[attrId] = 0;
                    }

                    long atrVue = trait.GetVue(i, this.TraitList[t].Level, this.TraitList[t].Type);
                    attrs[attrId] += atrVue;
                }
            }

            return attrs;
        }

        public long GetTotalKillCount()
        {
            return (long)(this.KillCount.Data + InheritCount.Data * 0.8);
        }

        public void AddExp(long exp)
        {
            this.LevelExp.Data += exp;

            long fee = PetAtrConfigCategory.Instance.GetPetFee(PetLevel.Data);

            while (this.LevelExp.Data >= fee)
            {
                this.LevelExp.Data -= fee;
                this.PetLevel.Data++;

                fee = PetAtrConfigCategory.Instance.GetPetFee(PetLevel.Data);
            }
        }

        public long GetTotalExp()
        {
            long total = this.LevelExp.Data;
            for (int i = 1; i < this.PetLevel.Data; i++)
            {
                total += PetAtrConfigCategory.Instance.GetPetFee(i);
            }

            return total;
        }

        public void AddKillCount(double rate)
        {
            this.KillCount.Data += rate;
        }

        public bool IsImportant()
        {
            bool important = false;

            if (TraitList.Where(m => m.Type > 1).Count() >= 1)
            {
                important = true;
            }

            return important;
        }

        //--------------ovveride
        public override int GetQuality()
        {
            return this.Quality;
        }

        public override string GetName()
        {
            return this.Name;
        }

        public override int GetBagType()
        {
            return 3;
        }

        public override ShowType GetShowType()
        {
            return ShowType.Pet;
        }

        public override long ToRecoverDict(Dictionary<int, long> dict, long number)
        {
            //long rn = CalRecoveryNumber();
            //int rid = ItemHelper.Pet_Exp;

            //if (!dict.ContainsKey(rid))
            //{
            //    dict[rid] = 0;
            //}

            //dict[rid] += rn;

            return this.GetQuality() * 10000;
        }

        //private long CalRecoveryNumber()
        //{
        //    return this.GetQuality() * 100;
        //}
    }

    public class PetTrait
    {
        public int Id { get; set; }
        public int Level { get; set; }

        public int Type { get; set; }

        public PetTrait()
        {

        }
    }
}
