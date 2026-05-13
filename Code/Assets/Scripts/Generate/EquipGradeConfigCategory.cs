using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipGradeConfigCategory
    {
        public EquipGradeConfig GetConfig(int position, int level)
        {
            return this.list.Where(m => m.Part == position && m.StartLayer <= level && level <= m.EndLayer).FirstOrDefault();
        }
    }


    public partial class EquipGradeConfig
    {
        public long GetFee(int index, int level)
        {
            long fee = this.McList[index] * MathHelper.GetSeqByType(RiseTypeList[index], level - StartLayer + 1, RiseMcList[index]);

            return fee;
        }
    }
}
