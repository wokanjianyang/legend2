using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_Defend : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    private int Level = 0;
    private long Progress = 0;

    private const int MaxProgress = 100; //

    private long PauseCount = 0;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };

    protected override RuleType ruleType => RuleType.Defend;

    public Battle_Defend(Dictionary<string, object> param)
    {
        param.TryGetValue("progress", out object progress);
        param.TryGetValue("hp", out object hp);
        param.TryGetValue("count", out object count);

        this.Progress = (long)progress;
        this.PauseCount = (long)count;
        this.Level = AppHelper.DefendLevel;

        this.LoadDefend((long)hp);
    }

    private Defend defendPlayer = null;

    private void LoadDefend(long hp)
    {
        this.defendPlayer = new Defend(hp);
        GameProcessor.Inst.PlayerManager.LoadDefend(this.defendPlayer);

        this.Start = true;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!this.Over)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        if (enemys.Count <= 0 && this.Progress <= MaxProgress && this.Start)
        {
            int si = (int)(this.Progress - 1) / 10 + 1;

            if (!GameProcessor.Inst.User.DefendData.GetCurrentRecord(this.Level).BuffDict.ContainsKey(si))
            {
                GameProcessor.Inst.EventCenter.Raise(new DefendBuffSelectEvent() { Index = si, Level = this.Level });
            }

            if (GameProcessor.Inst.User.InfoColor <= 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "第" + this.Progress + "波发起了进攻" });
            }

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Defend(this.Level, this.Progress, MonsterList[i]);
                GameProcessor.Inst.PlayerManager.LoadMonsterDefend(enemy);
            }

            string msg = "第" + Progress + "波，剩余" + PauseCount + "次复活";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            this.Start = false;

            return;
        }

        User user = GameProcessor.Inst.User;

        if (enemys.Count <= 0 && !this.Start)
        {
            //check 
            long progess = user.GetAchievementProgeress(AchievementProType.Defend);
            long cp = (this.Level - 1) * 100 + this.Progress;
            if (progess < cp)
            {
                user.SetAchievementProgeress(AchievementProType.Defend, cp);
            }

            this.BuildReward();

            this.Start = true;
            this.Progress++;

            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);

            record.Progress.Data = this.Progress;
            record.Hp.Data = (long)defendPlayer.HP;

            return;
        }

        if (this.Progress > MaxProgress && this.Over)
        {
            this.Over = false;

            user.DefendData.Complete();

            //BuildReward();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫成功" });

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);

            return;
        }
    }

    private void BuildReward()
    {
        DefendConfig rewardConfig = DefendConfigCategory.Instance.GetByLayerAndLevel(this.Level, (int)this.Progress);

        User user = GameProcessor.Inst.User;

        long exp = (long)(rewardConfig.Exp + (this.Progress - 1) * rewardConfig.RiseExp);
        long gold = exp;

        //增加经验,金币
        user.AddExpAndGold(exp, gold);

        List<KeyValuePair<double, DropConfig>> dropList = new List<KeyValuePair<double, DropConfig>>();

        //掉落道具
        int dropId = user.DefendData.GetDropId(this.Level, (int)this.Progress);

        int seed = AppHelper.GetDeviceIdentifier().GetHashCode();
        if (user != null && user.Account != null)
        {
            seed = user.Account.GetHashCode();
        }
        seed += TimeHelper.TodaySeed() + (int)this.Progress;

        List<Item> items = new List<Item>();
        items.Add(DropConfigCategory.Instance.BuildByDropBaseId(dropId, 1, seed));

        DefendDropConfig defendDropConfig = DefendDropConfigCategory.Instance.GetConfig(this.Level, dropId);
        if (defendDropConfig != null && defendDropConfig.Number > 1)
        {
            foreach (Item item in items)
            {
                item.Temp_Number = defendDropConfig.Number;
            }
        }

        if (items.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        if (QualityConfigHelper.GetMaxColor(items) >= user.InfoColor)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Normal,
                Message = BattleMsgHelper.BuildRewardMessage("守卫沙城" + this.Progress + "奖励:", exp, gold, items)
            });
        }
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Defend, 0);
        }

        var defend = GameProcessor.Inst.PlayerManager.GetDefend();
        if (defend.HP <= 0)
        {
            this.Over = false;
            GameProcessor.Inst.User.DefendData.Complete();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "守卫龙城失败" });

            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);
        }
    }
}
