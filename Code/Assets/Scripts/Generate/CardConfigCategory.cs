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
        private int[] exps = { 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047 };

        public int CalLevel(int exp)
        {
            if (exp <= 0)
            {
                return 0;
            }

            for (int i = exps.Length - 1; i >= 0; i--)
            {
                if (exp >= exps[i] * this.BaseFee)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        public int CalNextExp(int exp)
        {
            foreach (int e in exps)
            {
                int nx = e * this.BaseFee;
                if (nx > exp)
                {
                    return nx;
                }
            }

            return int.MaxValue;
        }
    }

}
