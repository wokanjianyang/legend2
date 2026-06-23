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
            int atrVue = (int)(Config.AttrValue + (Level - 1) * Config.AttrRise);
            return atrVue;
        }
    }
}