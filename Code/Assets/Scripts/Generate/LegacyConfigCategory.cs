using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class LegacyConfigCategory
    {
        public LegacyConfig GetByPart(int role, int part)
        {
            return this.list.Where(m => m.Role == role && m.Part == part).FirstOrDefault();
        }

        public List<LegacyConfig> GetRoleList(int role)
        {
            return this.list.Where(m => m.Role == role).ToList();
        }

    }


    public partial class LegacyConfig
    {
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