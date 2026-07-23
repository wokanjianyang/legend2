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

        public Dictionary<int, long> GetTotalFee(int part, int layer)
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();

            for (int i = 1; i <= layer; i++)
            {
                EquipGradeConfig config = this.GetConfig(part, i);

                for (int k = 0; k < config.MidList.Length; k++)
                {
                    int mid = config.MidList[k];
                    long fee = config.GetFee(k, i);

                    if (!dict.ContainsKey(mid))
                    {
                        dict[mid] = 0;
                    }

                    dict[mid] += fee;
                }
            }

            return dict;
        }
    }


    public partial class EquipGradeConfig
    {
        public long GetFee(int index, int level)
        {
            long fee =  MathHelper.GetSeqByType(RiseTypeList[index], level - StartLayer + 1, McList[index]);

            return fee;
        }
    }
}
