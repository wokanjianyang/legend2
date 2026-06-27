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

        private List<BossLog> logs = new List<BossLog>();

        public BattleRule_Normal()
        {
            mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);
            AppHelper.Boss = false;

            if (mapConfig.BossId > 0)
            {
                List<BossConfig> configs = BossConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id <= mapConfig.BossId).ToList();
                foreach (BossConfig config in configs)
                {
                    BossLog log = new BossLog(config.Id, config.Rate);

                    logs.Add(log);
                }
            }
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

            var enemy = MonsterConfigCategory.Instance.BuildMonster(mapConfig, quality, RuleType.Normal);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);


            if (roundNum % BossLog.TimeNumber == 1)
            { //每刷新100个怪，判定一次刷新boss

                foreach (var sp in logs)
                {
                    if (sp.RandomRefresh())
                    {
                        GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(sp.BossId, RuleType.Normal));
                    }
                }
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
            if (mapConfig.Id <= 1)  //第一张新手地图，不刷新高级品质怪
            {
                return 1;
            }

            if (this.Total < 100)  //前100个怪只有白色
            {
                return 1;
            }

            int maxRate = 10000 + mapConfig.GroupId * 1000;
            if (mapConfig.BossId > 0 && !AppHelper.Boss && RandomHelper.RandomDropRate(maxRate))  //40000，区域boss刷新概率
            {
                AppHelper.Boss = true;
                return 6;
            }

            int rd = RandomHelper.RandomNumber(1, 5000);
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

    public class BossLog
    {
        public static int TimeNumber = 100;

        public BossLog(int bossId, int rate)
        {
            this.BossId = bossId;
            this.Rate = rate;
            this.MinRate = rate / 2;
            this.LimitRate = (int)(rate * 1.5);
            this.Count = 0;
        }

        //bossid
        public int BossId { get; set; }

        //刷新概率
        public int Rate { get; set; }

        public int MinRate { get; set; }

        //保底概率
        public int LimitRate { get; set; }

        //判定次数
        public int Count { get; set; }


        public bool RandomRefresh()
        {
            this.Count += TimeNumber;

            Debug.Log("boss count：" + Count);

            if (AppHelper.Boss) //已经刷新了，不再刷新
            {
                return false;
            }

            if (this.Count < MinRate) //不足最低，不刷新
            {
                return false;
            }

            if (this.Count > LimitRate) //大于保底，必定刷新
            {
                AppHelper.Boss = true;
                this.Count = 0;
                return true;
            }

            if (RandomHelper.RandomNumber(0, this.Rate) <= 0)
            {
                AppHelper.Boss = true;
                this.Count = 0;
                return true;
            }

            return false;
        }
    }
}
