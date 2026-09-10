using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipLegendConfigCategory
    {
        public List<EquipLegendConfig> GetList(int setId)
        {

            return this.list.Where(m => m.SetId == setId).ToList();
        }
    }

}