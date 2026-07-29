using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MaterialCopyConfigCategory
    {
        public MaterialCopyConfig GetByProgress(int type, long progress)
        {
            MaterialCopyConfig config = this.list.Where(m => m.Type == type && m.StartLevel <= progress && progress <= m.EndLevel).FirstOrDefault();

            return config;
        }

        public int GetMaxProgress(int type)
        {
            return this.list.Where(m => m.Type == type).Select(m => m.EndLevel).Max();
        }
    }

}