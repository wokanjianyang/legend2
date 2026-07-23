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

        public long GetFee1(long level)
        {
            EquipStrengthFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return MathHelper.GetSeqByType(config.RiseType1, level, config.Fee1);
        }

        public long GetFee2(long level)
        {
            EquipStrengthFeeConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).FirstOrDefault();
            long riseLevel = level - config.StartLevel;

            return MathHelper.GetSeqByType(config.RiseType2, level, config.Fee2);
        }


        public long GetMaxLevel()
        {
            return this.list.Select(m => m.EndLevel).Max();
        }
    }

}
