using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Data;

public class BattleRule_Infinite : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    //private long Progress = 1;

    private int MaxProgress = ConfigHelper.Infinit_Max; //
    private const int SkipTime = 15;
    private const int SkipCount = 10;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };
    private long AttckTime = 0;
    private long UseTime = 0;
    private double OverTime = 0;

    protected override RuleType ruleType => RuleType.Infinite;

    public BattleRule_Infinite(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        param.TryGetValue("count", out object count);

        //this.Progress = (long)progress;
        User user = User_Data_Manager.Data;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";
        if (ac)
        {
            this.MaxProgress = MaxProgress / 2;
        }
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!this.Over)
        {
            OverTime += currentRoundTime;
            if (OverTime > 30)
            {
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
            return;
        }

        if (!Start)
        {
            //倒计时
            //this.UseTime = this.AttckTime + SkipTime - TimeHelper.ClientNowSeconds();
            //GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = "剩余时间：" + this.UseTime + "秒" });
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        User user = User_Data_Manager.Data;
        InfiniteRecord record = user.InfinData.GetCurrentRecord();

        long currentProgres = record.Progress.Data;

        if (enemys.Count <= 0 && currentProgres <= MaxProgress && this.Start)
        {
            if (AppHelper.SetData.InfoColor <= 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "第" + currentProgres + "波发起了进攻" });
            }

            this.AttckTime = TimeHelper.ClientNowSeconds();

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Infinite(currentProgres, MonsterList[i]);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            string msg = "第" + currentProgres + "波，剩余" + record.Count + "次挑战机会";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });


            this.Start = false;

            return;
        }

        if (enemys.Count <= 0 && !this.Start)
        {
            long progess = user.GetAchievementProgeress(AchievementProType.Infinite);
            long ap = 1;
            if (UseTime >= 0 && currentProgres < progess)
            {
                long ar = progess / 1000;
                ap = Math.Min(progess - currentProgres, 5 + ar * 5);
                if (MaxProgress > currentProgres)
                {
                    ap = Math.Min(ap, MaxProgress - currentProgres);
                }
            }

            if (progess < currentProgres)
            {
                user.SetAchievementProgeress(AchievementProType.Infinite, currentProgres);
            }

            record.Progress.Data += ap;

            for (int i = 0; i < ap; i++)
            {
                BuildReward(currentProgres + i);
            }

            //

            this.Start = true;
            return;
        }

        if (currentProgres > MaxProgress && this.Over)
        {
            this.Over = false;
            user.InfinData.Complete();
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "无尽闯关成功，您就是神！！！" });
            return;
        }
    }

    private void BuildReward(long level)
    {
        InfiniteConfig rewardConfig = InfiniteConfigCategory.Instance.GetByLevel(level);
        if (rewardConfig == null)
        {
            return;
        }

        User user = User_Data_Manager.Data;

        long exp = (long)rewardConfig.Exp;
        long gold = (long)rewardConfig.Gold;

        //增加经验,金币
        user.AddExpAndGold(exp, gold);

        //掉落道具
        DropData dd = user.InfinData.GetDropId((int)level);
        int number = 1;

        InfiniteDropConfig infiniteDropConfig = InfiniteDropConfigCategory.Instance.GetConfig(dd.DropId);
        if (infiniteDropConfig != null)
        {
            int nr = 1;
            if (infiniteDropConfig.RateNumber > 0)
            {
                nr = ((int)level / infiniteDropConfig.RateNumber + 1);
            }
            number = infiniteDropConfig.Number * nr;
        }

        List<Item> items = new List<Item>();
        if (dd.DropId > 0)
        {
            items.Add(dd.BuildItem(number));
        }

        if (items.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        if (QualityConfigHelper.GetMaxColor(items) >= AppHelper.SetData.InfoColor)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Normal,
                Message = BattleMsgHelper.BuildRewardMessage("无尽闯关" + level + "奖励:", exp, gold, items),
            });
        }
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero != null && hero.HP <= 0)
        {
            User user = User_Data_Manager.Data;
            InfiniteRecord record = user.InfinData.GetCurrentRecord();

            if (record == null)
            {
                return;
            }

            record.Count--;

            string msg = "第" + record.Progress.Data + "波，剩余" + record.Count + "次挑战机会";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            if (record.Count > 0)
            {
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);  //停止地图逻辑
            }
            else
            {
                this.Over = false;
                user.InfinData.Complete();
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "无尽闯关失败，请明天再来" });
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
        }
    }
}
