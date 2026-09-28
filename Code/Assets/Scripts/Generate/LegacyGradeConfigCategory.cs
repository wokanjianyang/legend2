using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class LegacyGradeConfigCategory
    {
        public LegacyGradeConfig GetConfig(int keyId, int level)
        {
            return this.list.Where(m => m.KeyId == keyId && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
        }
    }

    public partial class LegacyGradeConfig
    {
        public long GetFee1(long level)
        {
            long rate = 1;
            if (level > 30)
            {
                rate += (level - 21) / 10;
            }
            return this.Fee1 * level * rate;
        }
        public long GetFee2(long level)
        {
            double rate = 1;
            if (level > 30)
            {
                long rise = (level - 21) / 10;
                rate += rise / 5.0;
            }
            return (long)(this.Fee2 * level * rate);
        }

        public List<KeyValuePair<int, double>> GetTotalAtrList(int level)
        {
            List<KeyValuePair<int, double>> list = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < AtrIdList.Length; i++)
            {
                int rl = RequireList[i];

                if (level > rl)
                {
                    int riseLevel = level - rl;
                    int attrId = AtrIdList[i];
                    double attrValue = AtrVueList[i] * riseLevel;

                    list.Add(new KeyValuePair<int, double>(attrId, attrValue));
                }
            }

            for (int i = 0; i < SpeIdList.Length; i++)
            {
                if (level >= SpeRequireList[i])
                {
                    list.Add(new KeyValuePair<int, double>(SpeIdList[i], SpeVueList[i]));
                }
            }

            return list;
        }
    }
}