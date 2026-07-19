using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipSetConfigCategory
    {

    }

    public class EquipSetSuit
    {
        public List<EquipSetItem> List;
    }

    public class EquipSetItem
    {
        public int Level;

        public int Count;

        public EquipSetConfig Config;

        public int GetAtrVue()
        {
            int atrVue = (int)MathHelper.GetSeqByType(Config.RiseType, Level, Config.AtrValue);
            return atrVue;
        }

        public bool IsActive()
        {
            return this.Count >= this.Config.Count;
        }
    }
}