using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Material : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    //private long Progress = 1;

    private int MaxProgress = 0; //
    private const int SkipTime = 15;
    private const int SkipCount = 10;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };


    private int MapType = 0;
    private AchievementProType at;
    MaterialCopyConfig config;

    protected override RuleType ruleType => RuleType.Materail;

    private AchievementProType[] TypeList = { AchievementProType.Material1, AchievementProType.Material2, AchievementProType.Material3 };

    public BattleRule_Material(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        param.TryGetValue("MapId", out object mapId);

        this.MapType = (int)mapId;

        at = TypeList[this.MapType - 1];

        //this.Progress = (long)progress;
        User user = User_Data_Manager.Data;
        Materail_Record record = user.MaterailData.GetRecordType(MapType);

        config = MaterialCopyConfigCategory.Instance.GetByProgress(this.MapType, record.Progress);

        this.MaxProgress = config.EndLevel;

        string msg = config.MapName + "-进图次数" + record.Count + "次";
        GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Title = msg });
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!this.Over)
        {
            GameProcessor.Inst.CloseBattle(RuleType.Materail, 0);
            return;
        }

        if (!Start)
        {
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        User user = User_Data_Manager.Data;
        Materail_Record record = user.MaterailData.GetRecordType(MapType);

        long currentProgres = record.Progress;

        if (enemys.Count <= 0 && currentProgres <= MaxProgress && this.Start)
        {
            string msg = "第" + currentProgres + "关";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            //Load All
            for (int i = 0; i < 1; i++)
            {
                var enemy = new Monster_Material(this.MapType, currentProgres);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            this.Start = false;

            return;
        }

        if (enemys.Count <= 0 && !this.Start)
        {
            long progess = user.GetAchievementProgeress(at);

            if (progess < currentProgres)
            {
                user.SetAchievementProgeress(at, currentProgres);
            }

            record.Progress++;

            BuildReward(currentProgres);

            this.Start = true;
            return;
        }

        if (currentProgres > MaxProgress && this.Over)
        {
            this.Over = false;
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = config.MapName + "本阶段通关，您就是神！！！" });
            return;
        }
    }

    private void BuildReward(long level)
    {
        User user = User_Data_Manager.Data;

        long gold = 0;
        List<Item> items = new List<Item>();

        if (config.RewardId == 0)
        {
            //金币副本
            gold = config.RewardCount;

        }
        else
        {
            Item item = ItemHelper.BuildMaterial(config.RewardId, config.RewardCount);
            items.Add(item);
        }

        if (gold > 0)
        {
            //增加经验,金币
            user.AddExpAndGold(0, gold);
        }
        if (items.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Materail,
            Message = BattleMsgHelper.BuildRewardMessage(config.MapName + "第" + level + "关奖励:", 0, gold, items),
        });
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero != null && hero.HP <= 0)
        {
            User user = User_Data_Manager.Data;
            InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

            if (record == null)
            {
                return;
            }

            record.Count.Data--;
            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Count = record.Progress.Data, PauseCount = record.Count.Data });

            if (record.Count.Data > 0)
            {
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
            else
            {
                this.Over = false;
                user.InfiniteData.Complete();
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = "无尽闯关失败，请明天再来" });
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
        }
    }
}
