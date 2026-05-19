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

        public Gift_Pack(int configId) : base(configId, ItemType.GiftPack)
        {
            Config = GiftPackConfigCategory.Instance.Get(configId);
        }

        //------------------------------------------override----------------
        public override int GetQuality()
        {
            return this.Config.Quality;
        }

        public override string GetName()
        {
            return this.Config.Name;
        }

        public override string GetDes()
        {
            return this.Config.Des;
        }

        public override ShowType GetShowType()
        {
            if (Config.GiftType == 1)
            {
                return ShowType.Select;
            }
            else
            {
                return ShowType.Normal;
            }
        }
    }
}
