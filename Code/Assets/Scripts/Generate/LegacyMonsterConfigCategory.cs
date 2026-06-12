using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterLegacyConfigCategory
    {
        public MonsterLegacyConfig GetByRole(int role)
        {
            return this.list.Where(m => m.Role == role).FirstOrDefault();
        }
    }

}