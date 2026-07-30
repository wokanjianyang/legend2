using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BabelAtrConfigCategory
    {
        public List<BabelAtrConfig> GetNormalListByProgress(long progress)
        {
            return this.list.Where(m => m.StartLevel <= progress && m.Type == 1).ToList();
        }

        public List<BabelAtrConfig> GetSpeList()
        {
            return this.list.Where(m => m.Type == 2).ToList();
        }
    }



    public partial class BabelAtrConfig
    {
        public double GetAtrVue(int progress)
        {
            double vue = this.AtrValue * ((progress - this.StartLevel) / this.Rate);
            return vue;
        }
    }


}
