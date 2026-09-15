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
        public List<KeyValuePair<int, double>> GetTotalAtrList(long level)
        {
            List<KeyValuePair<int, double>> list = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < this.AtrList.Length; i++)
            {
                if (level >= RequireLevel[i])
                {
                    list.Add(new KeyValuePair<int, double>(AtrList[i], GetCurrentAtr(i, level)));
                }
            }

            for (int i = 0; i < this.SpeAtrList.Length; i++)
            {
                if (level >= this.SpeLevel[i])
                {
                    list.Add(new KeyValuePair<int, double>(SpeAtrList[i], SpeVueList[i]));
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
