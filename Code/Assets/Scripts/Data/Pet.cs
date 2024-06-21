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
    public class Pet
    {
        public string Name { get; set; }

        public LgData Level { get; set; }
        public LgData Exp { get; set; }

        public Pet()
        {
            //this.EventCenter = new EventManager();
        }

        public void Init(PetData data)
        {
            Name = data.Name;

            Level = new LgData();
            Level.Data = data.Level;

            Exp = new LgData();
            Exp.Data = data.Exp;
        }

        public void Init()
        {
            //设置各种属性值
            //SetAttr();
        }


    }
}
