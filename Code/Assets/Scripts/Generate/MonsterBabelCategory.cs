using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBabelConfigCategory
    {
        public MonsterBabelConfig GetByProgress(long progress)
        {
            MonsterBabelConfig config = this.list.Where(m => m.StartLevel <= progress && progress <= m.EndLevel).FirstOrDefault();

            return config;
        }


    }

}