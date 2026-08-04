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

        private int MaxBossId = 0;  //当前可以刷新的boss

        public BattleRule_Normal()
        {
            mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);
            AppHelper.Boss = false;

            User user = User_Data_Manager.Data;
            int k1 = user.GetExclusiveLevel(106);

            List<BossConfig> configs;

            if (k1 >= 1)  //有勇气号角，刷新低于此图的所有boss
            {
                MaxBossId = Math.Max(mapConfig.BossId, mapConfig.GroupId - 1);
                configs = BossConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id <= mapConfig.BossId || m.Id < mapConfig.GroupId).ToList();
            }
            else  //没有勇气号角，只有boss图刷新固定boss
            {
                MaxBossId = mapConfig.BossId;
                configs = BossConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id == mapConfig.BossId).ToList();
            }

            List<BossLog> temps = AppHelper.BossLogs;
            AppHelper.BossLogs = new List<BossLog>();

            foreach (BossConfig config in configs)
            {
                int rate = config.Id == mapConfig.BossId ? 100 : 150;  //非本图，boss刷新概率降低50%

                BossLog log = temps.Where(m => m.BossId == config.Id).FirstOrDefault();
                if (log == null)
                {
                    log = new BossLog(config.Id);
                }

                rate = config.Rate * rate / 100;

                log.InitRate(rate);

                AppHelper.BossLogs.Add(log);
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


            if (roundNum % BossLog.TimeNumber <= 1)
            { //每刷新100个怪，判定一次刷新boss

                foreach (var sp in AppHelper.BossLogs)
                {
                    if (sp.BossId <= MaxBossId)
                    {
                        if (sp.RandomRefresh())
                        {
                            GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(sp.BossId, RuleType.Normal));
                        }
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

            if (!AppHelper.Boss) //击杀boss过程中，不增加保底
            {
                foreach (var sp in AppHelper.BossLogs)
                {
                    if (sp.BossId <= MaxBossId)
                    {
                        sp.Count += ConfigHelper.TestRate; //20倍测试
                    }
                }
            }

            int rd = RandomHelper.RandomNumber(1, 5000);
            if (rd <= 2)
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
        public static int TimeNumber = 20;

        public BossLog(int bossId)
        {
            this.BossId = bossId;
            this.Count = 0;
        }

        public void InitRate(int rate)
        {
            this.Rate = rate;
            this.MinRate = rate / 2;
            this.LimitRate = (int)(rate * 1.5);
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
            //Debug.Log("boss " + BossId + " count：" + Count + " minRate:" + MinRate + " maxRate:" + LimitRate);

            if (AppHelper.Boss) //已经刷新了，不再刷新
            {
                return false;
            }

            if (Rate <= 0)
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

            if (RandomHelper.RandomNumber(0, this.Rate) < TimeNumber)
            {
                AppHelper.Boss = true;
                this.Count = 0;
                return true;
            }

            return false;
        }
    }
}
