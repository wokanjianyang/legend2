using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipRefineConfigCategory
    {
        public EquipRefineConfig GetByPart(int position)
        {
            return this.list.Where(m => m.Position == position).FirstOrDefault();
        }
    }

    public partial class EquipRefineConfig
    {
        public long GetRisePercent(long level, int type)
        {
            long riseLevel = level - this.RequireLevel[type - 1];
            if (riseLevel <= 0)
            {
                return 0;
            }
            else
            {
                return riseLevel * this.AtrVueList[type - 1];
            }
        }

        public long GetAtrVue(int p, long level)
        {
            long riseLevel = level - this.RequireLevel[p];
            if (riseLevel <= 0)
            {
                return 0;
            }
            else
            {
                return this.AtrVueList[p] * riseLevel;
            }
        }
    }

}
