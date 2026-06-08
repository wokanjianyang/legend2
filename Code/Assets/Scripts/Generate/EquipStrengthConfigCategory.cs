using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipStrengthConfigCategory
    {
        public EquipStrengthConfig GetByPositioin(int position)
        {
            return this.list.Where(m => m.Position == position).FirstOrDefault();
        }
    }


    public partial class EquipStrengthConfig
    {
        public Dictionary<int, double> GetTotalAtrList(long level)
        {
            Dictionary<int, double> list = new Dictionary<int, double>();

            for (int i = 0; i < this.AtrList.Length; i++)
            {
                if (level >= RequireLevel[i])
                {
                    list[AtrList[i]] = GetCurrentAtr(i, level);
                }
            }

            for (int i = 0; i < this.SpeAtrList.Length; i++)
            {
                if (level >= this.SpeLevel[i])
                {
                    list[SpeAtrList[i]] = SpeVueList[i];
                }
            }


            return list;
        }

        public long GetCurrentAtr(int i, long level)
        {
            long riseLevel = level - this.RequireLevel[i] + 1;
            long vue = MathHelper.GetSeqByType(AtrTypeList[i], riseLevel, AtrVueList[i]);

            return vue;
        }

        public long GetRiseAtr(int i, long level)
        {
            long riseLevel = level - this.RequireLevel[i] + 1;
            long vue = MathHelper.GetRiseByType(AtrTypeList[i], riseLevel, AtrVueList[i]);

            return vue;
        }
    }
}
