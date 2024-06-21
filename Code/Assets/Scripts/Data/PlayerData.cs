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
    public class PlayerData
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public double Exp { get; set; }

        public double Gold { get; set; }

        public Dictionary<int, Equip> EquipData { get; set; } = new Dictionary<int, Equip>();

        public Dictionary<int, Pet> PetData { get; set; }


        public void Init(UserNew user)
        {
            Name = user.Name;
            Level = (int)user.Level.Data;
            Exp = user.Exp.Data;
            Gold = user.Gold.Data;

            PetData = new Dictionary<int, Pet>();
        }

    }
}
