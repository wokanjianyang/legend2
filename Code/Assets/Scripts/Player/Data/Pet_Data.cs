using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Pet_Data
    {
        public MagicDouble Kill { get; set; } = new MagicDouble();

        public MagicData Exp { get; set; } = new MagicData();

        public int Status { get; set; } = 0;

        public void Add(long exp, double kill)
        {
            this.Kill.Data += kill;
            this.Exp.Data += exp;
        }

        public int GetLevel()
        {
            int level = 1;
            long fee = PetAtrConfigCategory.Instance.GetPetFee(level);
            long exp = Exp.Data;

            while (exp > fee)
            {
                level++;
                exp -= fee;

                fee = PetAtrConfigCategory.Instance.GetPetFee(level);
            }

            return level;
        }

        public string GetExp()
        {
            int level = 1;
            long fee = PetAtrConfigCategory.Instance.GetPetFee(level);
            long exp = Exp.Data;

            while (exp > fee)
            {
                level++;
                exp -= fee;

                fee = PetAtrConfigCategory.Instance.GetPetFee(level);
            }

            return exp + "/" + fee;
        }
    }
}
