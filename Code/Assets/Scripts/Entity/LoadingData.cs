using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class LoadingData
    {

        public List<NetAtrItem> AtrList = new List<NetAtrItem>();
    }


    public class NetAtrItem
    {
        public int AtrId;
        public double AtrVue;
    }
}
