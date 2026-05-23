using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetConfigCategory
    {

        public List<PetConfig> GetCardList(int cardId)
        {
            return this.list.Where(m => m.CardGroupId == cardId).ToList();
        }
    }



    public partial class PetConfig
    {
        public long GetAttr(long layer)
        {
            return 0;
        }
    }
}
