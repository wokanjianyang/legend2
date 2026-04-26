using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BattleRule_Test : ABattleRule
    {
        private double MapTime = 0;
        private int Total = 0;

        protected override RuleType ruleType => RuleType.Test;

        public override void DoMapLogic(int roundNum, double currentRoundTime)
        {
            if (roundNum % 2 != 0)
            {
                return;
            }

            MapTime += currentRoundTime;

            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Time = (int)MapTime, Count = Math.Max(0, Total - enemys.Count) });

            if (enemys.Count >= 5)
            {
                return;
            }

            var enemy = new Monster_Test();
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);



            //Specail
            //List<MonsterSpecialConfig> configs = MonsterSpecialConfigCategory.Instance.GetAll().Values.Where(m => m.BuildRate < Total).ToList();
            //foreach (MonsterSpecialConfig config in configs)
            //{
            //    if (RandomHelper.RandomNumber(1, config.BuildRate) <= 1)
            //    {
            //        GameProcessor.Inst.PlayerManager.LoadMonster(new Monster_Specail(config.Id, 1, RuleType.Normal));
            //    }
            //}

            Total++;
        }

    }
}
