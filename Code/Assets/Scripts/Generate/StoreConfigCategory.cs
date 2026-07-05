using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class StoreConfigCategory
    {

    }

    public partial class StoreConfig
    {


        public Dictionary<int, double> GetTotalAtrList(int level)
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();

            for (int i = 0; i < AtrIdList.Length; i++)
            {
                int riseLevel = Math.Min(level, this.Max);
                int attrId = AtrIdList[i];
                double attrValue = AtrVueList[i] * level;


                dict[attrId] = attrValue;
            }

            if (SpeId > 0 && SpeLevel > 0)
            {
                int riseLevel = level / SpeLevel;
                if (riseLevel > 0)
                {
                    dict[SpeId] = SpeVue * level;
                }
            }

            return dict;
        }
    }
}
