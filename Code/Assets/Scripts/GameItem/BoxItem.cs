using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Game.Data;
using System;

namespace Game
{
    public class BoxItem
    {
        public BoxItem()
        {

        }

        public Item Item { get; set; }

        public MagicData MagicNubmer { get; } = new MagicData();

        public int BoxId { get; set; }

        public void AddStack(long quantity)
        {
            this.MagicNubmer.Data += quantity;
        }

        public void RemoveStack(long quantity)
        {
            this.MagicNubmer.Data -= Math.Abs(quantity);

            if (quantity <= 0)
            {
                this.MagicNubmer.Data = 0;
            }
        }

        public bool IsFull()
        {
            if (MagicNubmer.Data < Item.GetMaxNum())
            {
                return false;
            }

            return true;
        }

        public int GetBagType()
        {
            return this.Item.GetBagType();
        }

        public int GetBagSort()
        {
            if (this.Item.GetItemType() == ItemType.Equip)
            {
                Equip equip = this.Item as Equip;
                var config = equip.Config;
                return config.Part * 10000 + config.LevelRequired + equip.GetQuality();
            }

            return this.Item.ConfigId;
        }
    }
}
