using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Item_Com_Equip : Item_Component
    {
        public int ConfigId { get; set; }

        public int Layer { get; set; }

        public int RuneId { get; set; }

        public int SuitId { get; set; }

        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();


        public Item_Com_Equip(int configId, double qualityRise) : base()
        {
            this.ConfigId = configId;
        }

        public void BuildAttrEntryLis()
        {

        }
    }
}
