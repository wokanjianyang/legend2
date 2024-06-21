using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    public class PetEgg : Item
    {
        public int Role { get; set; }

        public int Rarity { get; set; }

        public int Potential { get; set; }

        public PetEgg(int configId)
        {
            this.Type = ItemType.PetEgg;
            this.ConfigId = configId;

            Name = "";
            Des = "";
            Level = 0;
            Gold = 0;

        }
    }
}
