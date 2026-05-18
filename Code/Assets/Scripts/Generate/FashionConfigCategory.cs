using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class FashionConfigCategory
    {
        public List<FashionConfig> GetList(int cycle)
        {
            return this.list.Where(m => m.Cycle == cycle).ToList();
        }
    }

}
