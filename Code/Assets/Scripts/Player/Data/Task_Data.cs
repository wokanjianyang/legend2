using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Task_Data
    {

        public Dictionary<int, Task_Item_Data> Data = new Dictionary<int, Task_Item_Data>();

        public Task_Item_Data GetItem(int taskId)
        {
            if (!Data.ContainsKey(taskId))
            {
                Task_Item_Data item = new Task_Item_Data();
                item.Reset();
                Data[taskId] = item;
            }

            return Data[taskId];
        }

        public void Check()
        {
            long ticket = DateTime.Now.Ticks;

            foreach (var sp in Data)
            {
                Task_Item_Data item = sp.Value;

                if (item.TaskDay < ticket)
                {
                    sp.Value.Reset();
                }
            }
        }
    }

    public class Task_Item_Data
    {
        public int TaskId { get; set; } = 0;

        public long TaskDay { get; set; } = 0;

        public int Progress { get; set; } = 0;

        public int TaskStatus { get; set; } = 0;

        public void Reset()
        {
            this.TaskDay = DateTime.Now.Ticks;
            this.Progress = 0;
            this.TaskStatus = 0;
        }
    }
}
