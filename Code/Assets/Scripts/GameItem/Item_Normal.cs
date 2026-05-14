using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Item_Normal : Item
    {

        ItemConfig Config;

        public Item_Normal(int configId) : base(configId, ItemType.Normal)
        {
            Config = ItemConfigCategory.Instance.Get(configId);
        }

        public override ItemType GetItemType()
        {
            return (ItemType)this.Config.Type;
        }

        public override string GetDes()
        {
            return this.Config.Des;
        }

        public override int GetQuality()
        {
            return this.Config.Quality;
        }

        public override string GetName()
        {
            return this.Config.Name;
        }


        public override int GetRequired()
        {
            return this.Config.LevelRequired;
        }

        public override int GetRecoveryId()
        {
            return this.Config.RecoveryItemId;
        }

        public override long GetMaxNum()
        {
            return this.Config.MaxNum;
        }


    }
}
