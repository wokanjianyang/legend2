using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SoulRingConfigCategory
    {

    }

    public partial class SoulRingConfig
    {
        public Dictionary<int, double> GetTotalAtrList(int  level)
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

    public partial class SoulBoneConfigCategory
    {
        public SoulBoneConfig GetConfig(int sid, long level)
        {
            var config = this.list.Where(m => m.Sid == sid && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
        }
    }
}
