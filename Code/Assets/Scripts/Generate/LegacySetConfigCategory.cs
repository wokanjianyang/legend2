using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class LegacySetConfigCategory
    {
        public LegacySetConfig GetByRole(int role)
        {
            return this.list.Where(m => m.Role == role).FirstOrDefault();
        }
    }
}