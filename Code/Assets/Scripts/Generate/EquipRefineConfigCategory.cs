using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipRefineConfigCategory
    {

        public EquipRefineConfig GetByLevel(long level)
        {
            try
            {
                return this.list.First();
            }
            catch
            {

            }

            return null;
        }

        public EquipRefineConfig GetByPositioin(int position)
        {
            return this.list.Where(m => m.Position == position).FirstOrDefault();
        }
    }

    public partial class EquipRefineConfig
    {
        public long GetRisePercent(long level, int type)
        {
            long riseLevel = level - this.RequireLevel[type - 1];
            return riseLevel * this.AtrVueList[type - 1];
        }
    }

}
