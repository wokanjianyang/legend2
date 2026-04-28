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
            if (this.Item.GetItemType() == ItemType.Equip)
            {
                int type = (this.Item as Equip).Config.Role;

                return type <= 0 ? 3 : type - 1; //四格等全职业装备放战士包裹
            }
            if (this.Item.GetItemType() == ItemType.Exclusive || this.Item.GetItemType() == ItemType.EquipSpeical || this.Item.GetItemType() == ItemType.Pet || this.Item.GetItemType() == ItemType.Shengxiao)
            {
                return 3;
            }

            return 4;
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
