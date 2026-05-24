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
            return this.Fee1 * level;
        }
        public long GetFee2(long level)
        {
            return this.Fee2 * level;
        }

        public Dictionary<int, double> GetTotalAtrList(int level)
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();

            for (int i = 0; i < AtrIdList.Length; i++)
            {
                int rl = RequireList[i];

                if (level > rl)
                {
                    int riseLevel = level - rl;
                    int attrId = AtrIdList[i];
                    double attrValue = AtrVueList[i] * riseLevel;

                    dict[attrId] = attrValue;
                }
            }

            for (int i = 0; i < SpeIdList.Length; i++)
            {
                if (level >= SpeRequireList[i])
                {
                    dict[SpeIdList[i]] = SpeVueList[i];
                }
            }

            return dict;
        }
    }
}