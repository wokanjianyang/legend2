using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class DropData
    {
        public int DropId { get; set; }

        public int DropIndex { get; set; } = 0;

        public DropData(int dropId, int dropIndex)
        {
            this.DropId = dropId;
            this.DropIndex = dropIndex;
        }

        public Item BuildItem(int number)
        {
            DropBaseConfig config = DropBaseConfigCategory.Instance.Get(DropId);

            return ItemHelper.BuildItemNew((ItemType)config.ItemType, config.ItemIdList[DropIndex], 0, number, 0);
        }

    }
}
