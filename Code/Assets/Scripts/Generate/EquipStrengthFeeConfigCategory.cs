using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipStrengthFeeConfigCategory
    {

        public EquipStrengthFeeConfig GetByLevel(long level)
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

        public long GetFee(long level)
        {
            EquipStrengthFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return config.Fee + config.RiseFee * riseLevel;
        }


        public long GetMaxLevel()
        {
            return this.list.Select(m => m.EndLevel).Max();
        }
    }

}
