using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Weapon_Data
    {
        public int Id { get; set; } = 0;

        public MagicData Level { get; set; } = new MagicData();

        public MagicData Layer { get; set; } = new MagicData();

        public MagicData Exp { get; set; } = new MagicData();

        public int Status { get; set; } = 0;

        public bool isExpFull()
        {
            long needExp = GetNeedExp();

            if (Exp.Data >= needExp)
            {
                return true;
            }

            return false;
        }

        public long GetNeedExp()
        {
            return 1000 * this.Level.Data;
        }

        public void AddExp(long e)
        {
            long ne = GetNeedExp();
            if (this.Exp.Data < ne)
            {
                e = Math.Min(e, ne - this.Exp.Data);
                this.Exp.Data += e;
            }
        }

        public long GetFee()
        {
            return this.Level.Data - this.Layer.Data * 10;
        }

        public int GetFeeId()
        {
            return 5015 + (int)this.Layer.Data;
        }
    }
}
