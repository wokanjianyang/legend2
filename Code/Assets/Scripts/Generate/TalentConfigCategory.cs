using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TalentConfigCategory
    {

    }

    public partial class TalentConfig
    {
        public double GetAttrValue(long level)
        {
            if (level <= 0)
            {
                return 0;
            }

            double bv = this.AtrVue + (level - 1) * this.RiseVue;

            return bv;
        }
    }
}
