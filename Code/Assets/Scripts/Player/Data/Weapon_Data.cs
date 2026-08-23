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

        public bool isMaxLevel()
        {
            if (this.Level.Data >= 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long GetNeedExp()
        {
            WeaponConfig config = WeaponConfigCategory.Instance.Get(Id);
            long layer = this.Level.Data / 10;
            return config.Exp * (100 + layer * 20) / 100;
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

        private int[] fs = { 5, 4, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        public long GetFee()
        {
            WeaponConfig config = WeaponConfigCategory.Instance.Get(Id);

            int layer = (int)(this.Level.Data / 10);
            return fs[layer] * config.Fee;
        }

        public int GetFeeId()
        {
            int layer = (int)(this.Level.Data / 10);
            return 5015 + layer;
        }

        public void Grade()
        {
            if (isMaxLevel())
            {
                return;
            }

            this.Level.Data++;
            this.Exp.Data = 0;
        }
    }
}
