using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BabelConfigCategory
    {
        public BabelConfig GetByProgress(long progress)
        {
            var config = this.list.Where(m => m.Start <= progress && progress <= m.End).FirstOrDefault();
            return config;
        }
    }

    public partial class BabelConfig
    {
        public Item BuildItem(long progress)
        {
            for (int i = this.RewardLevelList.Length - 1; i >= 0; i--)
            {
                int rv = this.RewardLevelList[i];

                if (progress % rv == 0)
                {
                    return ItemHelper.BuildItem((ItemType)ItemTypeList[i], ItemIdList[i], 1, ItemCountList[i]);
                }
            }

            return null;

        }
    }


}
