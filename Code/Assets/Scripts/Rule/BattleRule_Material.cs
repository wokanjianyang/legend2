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

    private int MaxProgress = 0; //

    private int MapType = 0;
    private string MapName = "";
    private AchievementProType at;

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

        if (!record.SkipReward)
        {
            int mp = (int)user.GetAchievementProgeress(at);
            mp = Math.Max(0, mp - 10);
            record.SkipProgress = mp;  //计算跳过的关卡
            record.Progress = mp + 1;

            //发送跳关奖励
            this.BuildSkipReward(record, mp);
        }

        this.MaxProgress = MaterialCopyConfigCategory.Instance.GetMaxProgress(this.MapType);

        MaterialCopyConfig config = MaterialCopyConfigCategory.Instance.GetByProgress(this.MapType, 1);
        this.MapName = config.MapName;

        string msg = MapName + "-进图次数" + record.Count + "次";
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
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Materail, Message = this.MapName + "通关，您就是神！！！" });
            return;
        }
    }

    private void BuildReward(long level)
    {
        MaterialCopyConfig config = MaterialCopyConfigCategory.Instance.GetByProgress(this.MapType, level);

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

    private void BuildSkipReward(Materail_Record record, int sp)
    {
        record.SkipReward = true;

        if (sp <= 0)
        {
            return;
        }

        long gold = 0;
        List<Item> items = new List<Item>();

        MaterialCopyConfig config = MaterialCopyConfigCategory.Instance.GetByProgress(this.MapType, sp);

        long rc = (sp - config.StartLevel + 1) * config.RewardCount;
        rc += config.SkipReward;

        if (config.RewardId == 0)
        {
            //金币副本
            gold = rc;
        }
        else
        {
            Item item = ItemHelper.BuildMaterial(config.RewardId, rc);
            items.Add(item);
        }

        if (gold > 0)
        {
            User user = User_Data_Manager.Data;
            user.AddExpAndGold(0, gold);
        }
        if (items.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Materail,
            Message = BattleMsgHelper.BuildRewardMessage(MapName + "跳过" + sp + "关获得奖励:", 0, gold, items),
        });
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero != null && hero.HP <= 0)
        {

        }
    }
}
