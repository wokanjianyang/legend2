using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class Store_Data
    {

        public List<Store_Data_Item> AtrList = new List<Store_Data_Item>();
    }


    public class Store_Data_Item
    {
        public int StoreId;
        public int Number;
    }

}
