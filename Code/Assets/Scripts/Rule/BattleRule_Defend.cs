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

    private const int MaxProgress = 100; //

    private int[] MonsterList = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1, };
    private int[] MonsterList1 = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };

    protected override RuleType ruleType => RuleType.Defend;

    private DefendRecord CurrentRecord = null;

    public Battle_Defend(Dictionary<string, object> param)
    {
        User user = User_Data_Manager.Data;
        this.CurrentRecord = user.DefendData.GetCurrentRecord(AppHelper.DefendLevel);

        this.Level = AppHelper.DefendLevel;

        this.LoadDefend(CurrentRecord.Hp);
    }

    private Defend defendPlayer = null;

    private void LoadDefend(int hp)
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

        if (enemys.Count <= 0 && this.CurrentRecord.Progress <= MaxProgress && this.Start)
        {
            int si = (int)(this.CurrentRecord.Progress - 1) / 10 + 1;

            if (!User_Data_Manager.Data.DefendData.GetCurrentRecord(this.Level).BuffDict.ContainsKey(si))
            {
                GameProcessor.Inst.EventCenter.Raise(new DefendBuffSelectEvent() { Index = si, Level = this.Level });
            }

            if (AppHelper.SetData.InfoColor <= 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "第" + this.CurrentRecord.Progress + "波发起了进攻" });
            }


            //Load All
            int[] ml = this.CurrentRecord.Progress % 10 == 0 ? MonsterList1 : MonsterList;

            for (int i = 0; i < ml.Length; i++)
            {
                var enemy = new Monster_Defend(this.Level, this.CurrentRecord.Progress, ml[i]);
                GameProcessor.Inst.PlayerManager.LoadMonsterDefend(enemy);
            }

            string msg = "第" + this.CurrentRecord.Progress + "波，剩余" + CurrentRecord.Count + "次挑战机会";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            this.Start = false;

            return;
        }

        User user = User_Data_Manager.Data;

        if (enemys.Count <= 0 && !this.Start)
        {
            //check 
            long progess = user.GetAchievementProgeress(AchievementProType.Defend);
            long cp = (this.Level - 1) * 100 + this.CurrentRecord.Progress;
            if (progess < cp)
            {
                user.SetAchievementProgeress(AchievementProType.Defend, cp);
            }

            this.BuildReward();

            this.Start = true;
            this.CurrentRecord.Progress++;
            this.CurrentRecord.Hp = (int)defendPlayer.HP;

            return;
        }

        if (this.CurrentRecord.Progress > MaxProgress && this.Over)
        {
            this.Over = false;

            CurrentRecord.Complete();

            //BuildReward();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫成功" });

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);

            return;
        }
    }

    private void BuildReward()
    {
        MonsterDefendConfig rewardConfig = MonsterDefendConfigCategory.Instance.GetByLayerAndLevel(this.Level, this.CurrentRecord.Progress);

        User user = User_Data_Manager.Data;

        long exp = (long)(rewardConfig.Exp + (this.CurrentRecord.Progress - 1) * rewardConfig.RiseExp);
        long gold = exp;

        //增加经验,金币
        user.AddExpAndGold(exp, gold);

        List<KeyValuePair<double, DropConfig>> dropList = new List<KeyValuePair<double, DropConfig>>();

        //掉落道具
        int dropId = user.DefendData.GetDropId(this.Level, this.CurrentRecord.Progress);

        //int seed = AppHelper.GetDeviceIdentifier().GetHashCode();
        //if (user != null && user.Account != null)
        //{
        //    seed = user.Account.GetHashCode();
        //}
        //seed += TimeHelper.TodaySeed() + this.CurrentRecord.Progress;

        List<Item> items = new List<Item>();
        items.Add(DropConfigCategory.Instance.BuildByDropBaseId(dropId, 1, 0));

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

        if (QualityConfigHelper.GetMaxColor(items) >= AppHelper.SetData.InfoColor)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Normal,
                Message = BattleMsgHelper.BuildRewardMessage("守卫沙城" + this.CurrentRecord.Progress + "奖励:", exp, gold, items)
            });
        }
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();

        if (hero.HP <= 0)
        {
            CurrentRecord.Count--;
            string msg = "第" + this.CurrentRecord.Progress + "波，剩余" + CurrentRecord.Count + "次挑战机会";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            if (CurrentRecord.Count <= 0)
            {
                GameOver();
            }
            else
            {
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);  //停止地图逻辑
            }
        }

        var defend = GameProcessor.Inst.PlayerManager.GetDefend();
        if (defend.HP <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        this.Over = false;
        CurrentRecord.Complete();
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "守卫龙城失败，请明天再来" });
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);
    }
}
