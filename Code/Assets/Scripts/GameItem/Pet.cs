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
        public int Mid { get; set; } = 1;
        public MagicData PetLevel { get; set; } = new MagicData();

        public MagicData PetLayer { get; set; } = new MagicData();

        public MagicData LevelExp { get; set; } = new MagicData();

        public MagicData KillCount { get; set; } = new MagicData();

        public MagicData InheritCount { get; set; } = new MagicData();

        //public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<KeyValuePair<int, MagicData>> Skills { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<int> Talents { get; set; } = new List<int>();

        public int Role { get; set; }

        public int Status { get; set; } = 0;

        public Pet(int role) : base(role, ItemType.Pet)
        {
            this.Role = role;
        }

        public Dictionary<int, double> GetBaseAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            long level = PetLevel.Data;
            long riseRate = 1 + level / 10;

            foreach (var sp in this.Flairs)
            {
                PetConfig config = PetConfigCategory.Instance.Get(sp.Key);
                int attrId = config.AttrId;

                if (!attrs.ContainsKey(attrId))
                {
                    attrs[attrId] = 0;
                }

                double attrValue = (sp.Value.Data * GetTotalKillCount() / ConfigHelper.PetKillPercent) * riseRate;
                attrs[attrId] += attrValue;
            }

            return attrs;
        }

        public long GetTotalKillCount()
        {
            return this.KillCount.Data + (int)(InheritCount.Data * 0.8);
        }

        public void AddExp(long exp)
        {
            this.LevelExp.Data += exp;

            long fee = PetConfigCategory.Instance.GetPetFee(PetLevel.Data);

            if (this.LevelExp.Data >= fee)
            {
                this.LevelExp.Data -= fee;
                this.PetLevel.Data++;
            }
        }

        public long GetSkillPercent()
        {
            return 1;
        }

        public void AddKillCount(int rate)
        {
            this.KillCount.Data += rate;
        }


        //--------------ovveride
        public override int GetQuality()
        {
            return this.Quality;
        }

        public override string GetName()
        {
            return "≤‚ ‘≥ËŒÔ";
        }

        public override int GetBagType()
        {
            return 4;
        }

        public override ShowType GetShowType()
        {
            return ShowType.Pet;
        }
    }
}
