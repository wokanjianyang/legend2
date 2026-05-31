using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BattleRule_Normal : ABattleRule
    {
        private double MapTime = 0;
        private int Total = 0;

        protected override RuleType ruleType => RuleType.Normal;

        private MapConfig mapConfig;

        public BattleRule_Normal()
        {
            mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);
        }

        public override void DoMapLogic(int roundNum, double currentRoundTime)
        {
            MapTime += currentRoundTime;

            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

            string msg = (int)MapTime + "S击杀" + Math.Max(0, Total - enemys.Count) + "个";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            if (enemys.Count >= 20)
            {
                return;
            }

            int quality = BuildQuality();

            if (quality <= 5)
            {
                var enemy = MonsterConfigCategory.Instance.BuildMonster(mapConfig, quality, RuleType.Normal);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
            else
            {
                GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.BossId, RuleType.Normal));
            }


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

        private int BuildQuality()
        {
            if (mapConfig.BossId > 0 && RandomHelper.RandomDropRate(1000))  //40000
            {
                return 6;
            }

            int rd = RandomHelper.RandomNumber(1, 3000);
            if (rd < 1)
            {
                return 5;
            }
            else if (rd < 10)
            {
                return 4;
            }
            else if (rd < 100)
            {
                return 3;
            }
            else if (rd < 500)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}
