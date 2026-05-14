using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Item
    {
        public int ConfigId
        {
            get;
            set;
        }
        public long Count { get; set; }
        public int Quality { get; set; }

        public int Layer { get; set; }

        public int Level { get; set; }

        public long UUID { get; set; }

        public bool IsLock { get; set; }
        public bool IsNew { get; set; } = true;
        public bool IsKeep { get; set; } = false;

        public int Seed { get; set; } = -1;

        public ItemType Type { get; set; }

        private Item()
        {

        }

        public Item(int configId, ItemType type)
        {
            this.ConfigId = configId;
            this.Type = type;
        }

        public virtual int GetQuality()
        {
            return 1;
        }

        public virtual string GetName()
        {
            return "not name" + Type.ToString();
        }

        public virtual string GetDes()
        {
            return "not description" + Type.ToString();

        }


        public virtual ItemType GetItemType()
        {
            return this.Type;
        }

        public virtual int GetBagType()
        {
            return 4;
        }

        public virtual ShowType GetShowType()
        {
            return ShowType.Normal;
        }

        public virtual int GetRequired()
        {
            return 1;
        }

        public virtual int GetRecoveryId()
        {
            return 0;
        }

        /// <summary>
        /// 堆叠数量
        /// </summary>
        public virtual long GetMaxNum()
        {
            return 1;
        }

        public virtual void ToRecoverDict(Dictionary<int, long> dict)
        {

        }

        //[JsonIgnore]
        //public int BoxId { get; set; } = -1;

        //public void AddComponent(Item_Component component)
        //{
        //    //Components.Add(component);
        //}
    }

    public enum ItemType
    {
        Gold = 0,
        Normal = 1,
        Equip = 2,
        EquipSpeical = 3,
        SkillBox = 4,
        Material = 5, //进入包裹的材料
        Material_Hide = 6, //不进入包裹的材料
        GiftPack = 7,
        Buff = 8,
        GoldPack = 9,
        ExpPack = 10,
        Ticket = 11,
        Exclusive = 12,
        Card = 13,
        GiftPackExclusive = 14,
        Fashion = 15,
        Halidom = 16,
        Material_Usable = 17,
        Pet = 18,
        Shengxiao = 19,

        Ring = 19,
        GiftPackEquip = 20,
        GiftPackPet = 21,
        GiftPackShengxiao = 22,

        Spirit = 30,

        Metal = 98,
        Ad = 99,
    }

    public enum ShowType
    {
        Normal = 1,
        Equip,
        Equip_Special,
        Pet,
        Select,
        Drop,
        Shengxiao
    }
}
