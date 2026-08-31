using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class Setting_Data
    {
        public bool ShowPlayerEffect { get; set; } = true; //是否显示技能效果
        public bool ShowMonsterDamage { get; set; } = true; //是否显示怪物伤害
        public bool ShowMonsterSkill { get; set; } = true; //是否显示怪物技能
        public int InfoColor { get; set; } = 1; //掉落信息显示颜色

        public bool Babel_Auto { get; set; } = false;

        public IDictionary<int, bool> BossOpen { get; set; } = new Dictionary<int, bool>();

        public Setting_Data()
        {

        }

    }
}
