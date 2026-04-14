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

        public override void DoMapLogic(int roundNum, double currentRoundTime)
        {
            if (roundNum % 2 != 0)
            {
                return;
            }

            MapTime += currentRoundTime;

            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Time = (int)MapTime, Count = Math.Max(0, Total - enemys.Count) });

            if (enemys.Count >= 20)
            {
                return;
            }

            MapConfig mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);

            int quality = BuildQuality();

            if (quality <= 4)
            {
                var enemy = MonsterBaseCategory.Instance.BuildMonster(mapConfig, quality, RuleType.Normal);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
            else
            {
                GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.Id, RuleType.Normal));
            }


            //Specail
            List<MonsterSpecialConfig> configs = MonsterSpecialConfigCategory.Instance.GetAll().Values.Where(m => m.BuildRate < Total).ToList();
            foreach (MonsterSpecialConfig config in configs)
            {
                if (RandomHelper.RandomNumber(1, config.BuildRate) <= 1)
                {
                    GameProcessor.Inst.PlayerManager.LoadMonster(new Monster_Specail(config.Id, 1, RuleType.Normal));
                }
            }

            Total++;
        }

        private int BuildQuality()
        {
            int rd = RandomHelper.RandomNumber(1, 1801);
            if (rd < 1)
            {
                return 5;
            }
            else if (rd < 6)
            {
                return 4;
            }
            else if (rd < 30)
            {
                return 3;
            }
            else if (rd < 180)
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
