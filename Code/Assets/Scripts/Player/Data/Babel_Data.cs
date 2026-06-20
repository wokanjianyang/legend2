using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Babel_Data
    {
        public long Ticket { get; set; }

        public MagicData Progress { get; set; } = new MagicData();

        public int Count { get; set; } = 0;


        public void Check()
        {
            long nt = DateTime.Today.Ticks;

            if (nt > Ticket)
            {
                //Debug.Log("nt:" + nt + "  Ticket:" + Ticket);

                Ticket = nt;
                Count = ConfigHelper.BabelCount;
            }
        }
    }
}
