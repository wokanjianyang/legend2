using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class Store_Data
    {
        public int Lottery { get; set; }
        public int Points‌ { get; set; }

        public List<Store_Data_Item> StoreList = new List<Store_Data_Item>();
    }


    public class Store_Data_Item
    {
        public int StoreId;
        public int Number;
    }

}
