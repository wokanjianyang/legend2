using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CardConfigCategory
    {

    }



    public partial class CardConfig
    {
        //private int[] exps = { 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047 };

        public int CalLevel(int exp)
        {
            if (exp <= 0)
            {
                return 0;
            }

            int level = (int)(Math.Sqrt(exp / this.BaseFee));
            return Math.Min(level, this.MaxLevel);
        }

        public int CalNextExp(int exp)
        {
            int level = CalLevel(exp);

            int nextExp = level * level * this.BaseFee;

            if (nextExp > exp)
            {
                return nextExp;
            }
            else
            {
                return (level + 1) * (level + 1) * this.BaseFee;
            }
        }
    }

}
