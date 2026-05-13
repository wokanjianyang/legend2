using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Gift_Pack : Item
    {
        [JsonIgnore]
        public GiftPackConfig Config { get; set; }

        public GiftPack(int configId) : base(configId, ItemType.GiftPack)
        {
            this.ConfigId = configId;

            Config = GiftPackConfigCategory.Instance.Get(configId);

        }

        public override int GetQuality()
        {
            return 4;
        }
    }
}
