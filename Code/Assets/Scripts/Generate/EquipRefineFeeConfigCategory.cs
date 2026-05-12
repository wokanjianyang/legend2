using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipRefineFeeConfigCategory
    {

        public EquipRefineFeeConfig GetByLevel(long level)
        {
            try
            {
                return this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            }
            catch
            {

            }

            return null;
        }

        public long GetFee1(long level)
        {
            EquipRefineFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return config.Fee1 + config.RiseFee1 * riseLevel;
        }

        public long GetFee2(long level)
        {
            EquipRefineFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return config.Fee2 + config.RiseFee2 * riseLevel;
        }


        public long GetMaxLevel()
        {
            return this.list.Select(m => m.EndLevel).Max();
        }
    }

}
