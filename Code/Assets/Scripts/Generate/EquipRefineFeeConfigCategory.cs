using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public partial class EquipRefineFeeNewConfigCategory
    {
        public int GetMaxLevel()
        {
            return this.list.Select(m => m.Id).Max();
        }
    }
}
