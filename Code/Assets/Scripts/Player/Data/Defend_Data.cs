using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Defend_Data
    {

        public long Ticket { get; set; }

        public Dictionary<int, MagicData> CountDict { get; set; } = new Dictionary<int, MagicData>();

        public Dictionary<int, DefendRecord> CurrentDict = new Dictionary<int, DefendRecord>();

        public Dictionary<int, List<List<int>>> DropDict = new Dictionary<int, List<List<int>>>();

        public void Check()
        {
            long nt = DateTime.Today.Ticks;

            if (nt > Ticket)
            {
                //Debug.Log("nt:" + nt + "  Ticket:" + Ticket);

                Ticket = nt;

                CurrentDict.Clear();

                for (int i = 1; i <= ConfigHelper.DefendMaxLevel; i++)
                {
                    BuildNewData(i);
                }
            }
        }

        public DefendRecord GetCurrentRecord(int level)
        {
            if (!CurrentDict.ContainsKey(level))
            {
                BuildNewData(level);
            }

            return CurrentDict[level];
        }


        private void BuildNewData(int level)
        {
            DefendRecord record = new DefendRecord();
            record.Init();

            CurrentDict[level] = record;

            if (DropDict.TryGetValue(level, out List<List<int>> dropList))
            {
                if (dropList.Count > 0)
                {
                    dropList.RemoveAt(0);
                }
            }
        }

        public void Complete()
        {
            DefendRecord record = this.CurrentDict[AppHelper.DefendLevel];
            record.Count = 0;
        }

        public int GetDropId(int layer, int progress)
        {
            return GetDropIdList(layer)[progress - 1];
        }

        public List<int> GetDropIdList(int layer)
        {
            if (!DropDict.ContainsKey(layer))
            {
                DropDict[layer] = new List<List<int>>();
            }

            List<List<int>> DropList = DropDict[layer];

            if (DropList.Count < 2)
            {
                for (int i = DropList.Count; i < 2; i++)
                {
                    var list = DefendDropConfigCategory.Instance.GetAllDropIdList(layer);
                    DropList.Add(list);
                }
            }

            //Debug.Log("infinite drop1-100:" + DropList[0][99]);
            //Debug.Log("drop:" + DropList[0][99] + "," + DropList[0][199] + "," + DropList[0][299]);

            return DropList[0];
        }

        public List<DefendBuffConfig> GetBuffList()
        {
            int level = AppHelper.DefendLevel;
            CurrentDict.TryGetValue(level, out DefendRecord Current);

            List<DefendBuffConfig> list = new List<DefendBuffConfig>();

            if (Current != null)
            {
                foreach (var kv in Current.BuffDict)
                {
                    DefendBuffConfig config = DefendBuffConfigCategory.Instance.Get(kv.Value);

                    list.Add(config);
                }
            }
            return list;
        }

        public List<int> GetExcludeList()
        {
            int level = AppHelper.DefendLevel;
            CurrentDict.TryGetValue(level, out DefendRecord Current);

            List<int> list = new List<int>();

            if (Current != null)
            {
                var gp = Current.BuffDict.Select(m => m.Value).GroupBy(m => m);
                foreach (var g in gp)
                {
                    int buffId = g.Key;
                    int count = g.Count();
                    DefendBuffConfig config = DefendBuffConfigCategory.Instance.Get(buffId);

                    if (config.MaxCount <= count)
                    {
                        list.Add(config.Id);

                        //Debug.Log("Exclued buff:" + config.Name + " count:" + count);
                    }
                }
            }
            return list;
        }

    }

    public class DefendRecord
    {
        public int Progress { get; set; } = 0;

        public int Hp { get; set; } = 0;

        public int Count { get; set; } = 0;

        public Dictionary<int, int> BuffDict = new Dictionary<int, int>();

        public void Init()
        {
            this.Progress = 1;
            this.Hp = ConfigHelper.DefendHp;
            this.Count = 10;
        }
    }
}
