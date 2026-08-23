using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class EquipLegendSet
    {
        public int SetId;

        public int Total_Fliar;

        public int Count;

        public EquipLegendSetConfig Config;

        public EquipLegendSet(int setId)
        {
            this.SetId = setId;
            this.Total_Fliar = 0;
            this.Count = 0;

            this.Config = EquipLegendSetConfigCategory.Instance.Get(setId);
        }

        public void Add(int fliar)
        {
            this.Count++;
            this.Total_Fliar += fliar;
        }

        public bool IsActive()
        {
            return this.Count >= Config.Count;
        }

        public Dictionary<int, double> GetAtrList()
        {
            Dictionary<int, double> dict = new Dictionary<int, double>();

            if (IsActive() && Config.AtrIdList != null)  //激活了之后才有属性
            {
                for (int i = 0; i < Config.AtrIdList.Length; i++)
                {
                    int atrId = Config.AtrIdList[i];
                    int atrVue = Config.AtrVueList[i] + (int)(this.Total_Fliar * Config.RiseList[i] / 5);

                    dict[atrId] = atrVue;
                }
            }

            return dict;
        }

        public string FormatDesc()
        {
            List<int> vues = new List<int>();

            if (Config.AtrIdList != null)
            {
                for (int i = 0; i < Config.AtrIdList.Length; i++)
                {
                    int atrVue = Config.AtrVueList[i];

                    if (IsActive())
                    {

                        atrVue += (int)(this.Total_Fliar * Config.RiseList[i] / 5);
                    }

                    vues.Add(atrVue);
                }
            }

            if (Config.AtrIdList == null)
            {
                return Config.Desc;
            }
            else if (Config.AtrIdList.Length == 1)
            {
                return string.Format(Config.Desc, vues[0]);
            }
            else if (Config.AtrIdList.Length == 2)
            {
                return string.Format(Config.Desc, vues[0], vues[1]);
            }
            else if (Config.AtrIdList.Length == 5)
            {
                return string.Format(Config.Desc, vues[0], vues[1], vues[2], vues[3], vues[4]);
            }

            return Config.Desc;
        }
    }
}
