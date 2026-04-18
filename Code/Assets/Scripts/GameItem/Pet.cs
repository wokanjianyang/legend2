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

        public MagicData KillCount { get; set; } = new MagicData();

        //public MagicData LayerExp { get; set; } = new MagicData();
        public List<KeyValuePair<int, MagicData>> Flairs { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<KeyValuePair<int, MagicData>> Skills { get; set; } = new List<KeyValuePair<int, MagicData>>();

        public List<int> Talents { get; set; } = new List<int>();

        public int Role { get; set; }

        public override int GetQuality()
        {
            return Flairs.Count + 4;
        }

        public Pet(int role)
        {
            this.Type = ItemType.Pet;
            this.Role = role;
            this.Name = "≤‚ ‘≥ËŒÔ";
        }

        public Dictionary<int, double> GetBaseAttr()
        {
            Dictionary<int, double> attrs = new Dictionary<int, double>();

            long level = PetLevel.Data;
            long riseRate = level / 10;

            foreach (var sp in this.Flairs)
            {
                PetConfig config = PetConfigCategory.Instance.Get(sp.Key);
                int attrId = config.AttrId;

                double attrValue = (sp.Value.Data * KillCount.Data / ConfigHelper.PetKillPercent) * riseRate;
                attrs[attrId] = attrValue;
            }

            return attrs;
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
    }
}
