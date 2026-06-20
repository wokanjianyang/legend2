using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{
    public enum ADTypeEnum
    {
        GoldCount = 1,
        StoneCount = 2,
        Stone1Count = 3,
        //ExpAdd,
        //ExpTime,
        //GoldAdd,
        //GoldTime,
        //ErrorCount = 10,
    }
    [Serializable]
    public class ADData
    {
        public int ADType;
        public int CurrentShowCount;
        public int MaxShowCount;
    }
    [Serializable]
    public class ADShowData
    {
        public long LastTicket = 0;

        public List<ADData> ADDatas;

        public ADShowData()
        {

        }

        public void Reset()
        {
            this.LastTicket = DateTime.Today.Ticks;


            ADDatas = new List<ADData>();
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.GoldCount,
                CurrentShowCount = 0,
                MaxShowCount = 3
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.StoneCount,
                CurrentShowCount = 0,
                MaxShowCount = 3
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.Stone1Count,
                CurrentShowCount = 0,
                MaxShowCount = 3
            });
        }

        public bool CheckDate()
        {
            return this.LastTicket < DateTime.Today.Ticks;
        }

        public ADData GetADShowStatus(ADTypeEnum adType)
        {
            if (this.CheckDate())
            {
                this.Reset();
            }

            if (this.ADDatas == null)
            {
                this.Reset();
            }

            //ADData ret = null;
            var data = this.ADDatas.FirstOrDefault(d => d.ADType == (int)adType);
            return data;
        }
    }
}
