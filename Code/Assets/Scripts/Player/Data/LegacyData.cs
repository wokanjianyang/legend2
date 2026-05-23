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

        public MagicDouble Time { get; set; }

        private const int DefaultTime = 3600;

        public void Check(long level)
        {
            if (Time == null)
            {
                Time = new MagicDouble();
                Time.Data = DefaultTime;
            }

            long nt = DateTime.Today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;

                if (level > 30 && Time.Data < DefaultTime * 7)
                {
                    Time.Data += DefaultTime;
                }
            }
        }
    }
}
