using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class LegacyConfigCategory
    {
        public LegacyConfig GetByPosition(int part)
        {
            return this.list.Where(m => m.Part == part).FirstOrDefault();
        }



        public List<LegacyConfig> GetRoleList(int role)
        {
            return this.list.Where(m => m.Role == role).ToList();
        }

        public LegacyConfig GetDropItem(int role)
        {
            //List<LegacyConfig> dropList = this.list.Where(m => m.Role == role).ToList();

            //int total = dropList.Select(m => m.DropRate).Sum();
            //int rd = RandomHelper.RandomNumber(1, total + 1);

            //int endRate = 0;
            //for (int i = 0; i < dropList.Count; i++)
            //{
            //    endRate += dropList[i].DropRate;

            //    if (rd <= endRate)
            //    {
            //        return dropList[i];
            //    }
            //}

            return null;
        }


        public int GetDropLayer(int layer)
        {
            int result;

            int rd = RandomHelper.RandomNumber(1, 101);

            layer = Math.Min(layer * 5, 33);

            if (rd <= 40 - layer)
            {
                result = 3;
            }
            else if (rd <= 70)
            {
                result = 2;
            }
            else if (rd <= 90)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }
    }

    public partial class LegacyConfig
    {

        public long GetFee1(long level)
        {
            return this.Fee1 * level;
        }
        public long GetFee2(long level)
        {
            return this.Fee2 * level;
        }
    }
}