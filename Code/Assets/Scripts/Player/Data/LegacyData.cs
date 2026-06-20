using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class LegacyData
    {
        public long Ticket { get; set; }

        public MagicDouble Time { get; set; } = new MagicDouble();

        public void Check(long level)
        {
            long nt = DateTime.Today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;

                if (Time.Data < ConfigHelper.LegacyDefaultTime * 7)
                {
                    Time.Data += ConfigHelper.LegacyDefaultTime;
                }
            }
        }
    }
}
