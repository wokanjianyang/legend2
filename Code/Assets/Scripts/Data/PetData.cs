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
    public class PetData
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public double Exp { get; set; }


        public void Init(Pet pet)
        {
            Name = pet.Name;
            Level = (int)pet.Level.Data;
            Exp = pet.Exp.Data;
        }

    }
}
