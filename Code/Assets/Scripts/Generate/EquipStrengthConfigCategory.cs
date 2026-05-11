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
                    double vue = AtrVueList[i] * level;

                    list[AtrList[i]] = vue;
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
    }
}
