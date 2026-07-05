using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class Lottery_Result
    {
        public List<Lottery_Result_Item> List = new List<Lottery_Result_Item>();
    }


    public class Lottery_Result_Item
    {
        public int Id;
        public int Type;
        public int Points;
    }

}
