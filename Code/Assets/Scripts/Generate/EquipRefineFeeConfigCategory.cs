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

        public long GetFee1(long level, long baseFee)
        {
            EquipRefineFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return (config.Fee1 + config.RiseFee1 * riseLevel) * baseFee;
        }

        public long GetFee2(long level, long baseFee)
        {
            EquipRefineFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return (config.Fee2 + config.RiseFee2 * riseLevel) * baseFee;
        }

        public long GetTotalFee1(long level, long baseFee)
        {
            long total = 0;
            for (int i = 1; i <= level; i++)
            {
                total += GetFee1(i, baseFee);
            }

            return total;
        }

        public long GetTotalFee2(long level, long baseFee)
        {
            long total = 0;
            for (int i = 1; i <= level; i++)
            {
                total += GetFee2(i, baseFee);
            }

            return total;
        }
    }

    public partial class EquipRefineFeeNewConfigCategory
    {
        public int GetMaxLevel()
        {
            return this.list.Select(m => m.Id).Max();
        }
    }
}
