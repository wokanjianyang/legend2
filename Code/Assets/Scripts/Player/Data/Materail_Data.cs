using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Materail_Data
    {

        public long Ticket { get; set; }

        public Dictionary<int, Materail_Record> CurrentDict = new Dictionary<int, Materail_Record>();


        public void Check()
        {
            long nt = DateTime.Today.Ticks;

            if (nt > Ticket)
            {
                //Debug.Log("nt:" + nt + "  Ticket:" + Ticket);

                Ticket = nt;

                CurrentDict.Clear();

                for (int i = 1; i <= 3; i++)
                {
                    BuildNewData(i);
                }
            }
        }

        public Materail_Record GetRecordType(int type)
        {
            if (!CurrentDict.ContainsKey(type))
            {
                BuildNewData(type);

            }
            return CurrentDict[type];
        }

        private void BuildNewData(int type)
        {
            Materail_Record record = new Materail_Record();
            record.Init();

            CurrentDict[type] = record;
        }
    }

    public class Materail_Record
    {
        public int Progress { get; set; } = 0;

        public int Count { get; set; } = 0;

        public int Type { get; set; } = 0;

        public bool SkipReward { get; set; } = false;

        public int SkipProgress { get; set; } = 0;

        public void Init()
        {
            this.Progress = 1;
            this.Count = 10;
            this.SkipReward = false;
            this.SkipProgress = 0;
        }
    }
}
