using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AchievementGroupConfigCategory
    {
        public List<AchievementGroupConfig> GetListByPid(int pid)
        {
            return this.list.Where(m => m.Pid == pid).ToList();
        }
    }
}
