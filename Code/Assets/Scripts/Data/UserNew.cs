using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;
using Game.Data;
using SDD.Events;

namespace Game
{
    public class UserNew
    {
        public string Name { get; set; }

        public LgData Level { get; set; }
        public LgData Exp { get; set; }
        public LgData Gold { get; set; }
        public Dictionary<int, Equip> EquipData { get; set; } = new Dictionary<int, Equip>();

        public Dictionary<int, Pet> PetData { get; set; }

        public UserNew()
        {
            //this.EventCenter = new EventManager();
        }

        public void Init(PlayerData data)
        {
            Name = data.Name;

            Level = new LgData();
            Level.Data = data.Level;

            Exp = new LgData();
            Exp.Data = data.Exp;

            Gold = new LgData();
            Gold.Data = data.Gold;
        }

        public void Init()
        {
            //设置各种属性值
            //SetAttr();
        }


    }
}
