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
        private int Count = 1;
        private bool start = false;

        protected override RuleType ruleType => RuleType.Normal;

        public override void DoMapLogic(int roundNum, double currentRoundTime)
        {
            if (roundNum % 2 != 0)
            {
                return;
            }

            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
            if (enemys.Count >= 20)
            {
                return;
            }

            MapConfig mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);

            if (Count % 500 != 0)
            {
                int quality = 1;
                if (Count % 100 == 0)
                {
                    quality = 4;
                }
                else if (Count % 30 == 0)
                {
                    quality = 3;
                }
                else if (Count % 10 == 0)
                {
                    quality = 2;
                }

                var enemy = MonsterBaseCategory.Instance.BuildMonster(mapConfig, quality, RuleType.Normal);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
            else
            {
                GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.Id, RuleType.Normal));
            }

            //Specail
            List<MonsterSpecialConfig> configs = MonsterSpecialConfigCategory.Instance.GetAll().Values.Where(m => m.BuildRate < Count).ToList();
            foreach (MonsterSpecialConfig config in configs)
            {
                if (RandomHelper.RandomNumber(1, config.BuildRate) <= 1)
                {
                    GameProcessor.Inst.PlayerManager.LoadMonster(new Monster_Specail(config.Id, 1, RuleType.Normal));
                }
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Count = Count });
            Count++;
        }
    }
}
