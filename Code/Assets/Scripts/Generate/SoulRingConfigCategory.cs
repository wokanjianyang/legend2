using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SoulRingConfigCategory
    {
        public SoulRingConfig GetConfig(int sid, long level)
        {
            var config = this.list.Where(m => m.Sid == sid && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
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
