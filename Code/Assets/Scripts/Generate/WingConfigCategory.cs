using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WingConfigCategory
    {
        public List<WingConfig> GetAllByPart(int part)
        {
            var configs = this.list.Where(m => m.Part == part).ToList();
            return configs;
        }

        public List<WingConfig> GetAllByPartAndType(int part, int type)
        {
            var configs = this.list.Where(m => m.Part == part && m.Type == type).ToList();
            return configs;

        }
        public List<KeyValuePair<int, double>> GetTotalAtrList(int part, int level, int percent)
        {
            List<KeyValuePair<int, double>> totals = new List<KeyValuePair<int, double>>();

            List<WingConfig> list = GetAllByPart(part);

            foreach (var sp in list)
            {
                double vue = sp.GetAttr(level) * (100 + percent) / 100;
                if (vue > 0)
                {
                    KeyValuePair<int, double> item = new KeyValuePair<int, double>(sp.AtrId, vue);
                    totals.Add(item);
                }
            }

            return totals;
        }
    }

    public partial class WingConfig
    {

        public double GetAttr(long level)
        {
            if (level >= this.RequireLevel)
            {
                if (this.RiseType <= 0)
                {
                    return this.AtrVue;
                }
                else
                {
                    long riselevel = level - this.RequireLevel + 1;
                    if (this.MaxLevel > 0)
                    {
                        riselevel = Math.Min(this.MaxLevel, riselevel);
                    }
                    return MathHelper.GetSeqByType(this.RiseType, riselevel, 1) * this.AtrVue;
                }
            }

            return 0;
        }
    }
}
