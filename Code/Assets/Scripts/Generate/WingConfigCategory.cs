using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WingConfigCategory
    {
        public List<WingConfig> GetAllByType(int type)
        {
            var configs = this.list.Where(m => m.Type == type).ToList();
            return configs;
        }
    }

    public partial class WingConfig
    {
        public long GetFee(long level)
        {
            return 5 * level;
        }

        public long GetAttr(long level)
        {
            if (level >= this.RequireLevel)
            {
                if (this.RiseType <= 0)
                {
                    return this.AtrVue;
                }
                else
                {
                    return MathHelper.GetSeqByType(this.RiseType, level, this.AtrVue);
                }
            }

            return 0;
        }
    }
}
