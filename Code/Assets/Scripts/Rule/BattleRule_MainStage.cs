using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_MainStage : ABattleRule
{
    private bool Start = false;

    private int MapId = 0;
    private long MapTime = 0;

    private List<int> QualityList;

    private const int MaxQuanlity = 30; //最多数量
    private int MaxFreshQuanlity = 5; //最多刷新数量
    protected override RuleType ruleType => RuleType.MainStage;

    public BattleRule_MainStage(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);
        param.TryGetValue("MapTime", out object mapTime);

        this.MapId = (int)mapId;
        this.MapTime = (long)mapTime;
        this.Start = true;

        QualityList = new List<int>();

        for (int i = 0; i < 2; i++)
        {
            QualityList.Add(5);
        }
        for (int i = 0; i < 30; i++)
        {
            QualityList.Add(1);
        }
        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(4);
        }
        for (int i = 0; i < 20; i++)
        {
            QualityList.Add(2);
        }
        for (int i = 0; i < 10; i++)
        {
            QualityList.Add(3);
        }

    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);

        string msg = "击杀所有怪物通关，剩余：" + (QualityList.Count() + enemys.Count);
        GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

        if (enemys.Count < MaxQuanlity && QualityList.Count > 0)
        {
            int count = Math.Min(MaxFreshQuanlity, MaxQuanlity - enemys.Count);

            for (int i = 0; i < count; i++)
            {
                if (QualityList.Count > 0)
                {
                    if (QualityList[0] <= 5)
                    {
                        var enemy = MonsterConfigCategory.Instance.BuildMonster(mapConfig, QualityList[0], RuleType.MainStage);
                        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                    }
                    else
                    {
                        BossConfig bossConfig = BossConfigCategory.Instance.Get(this.MapId);
                        GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(this.MapId, RuleType.MainStage));
                    }
                    QualityList.RemoveAt(0);
                }
            }
        }

        if (Start && QualityList.Count <= 0 && enemys.Count <= 0)
        {
            Start = false;

            User user = GameProcessor.Inst.User;
            if (user.MapId == mapConfig.Id)
            {
                //闯关成功
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = $"<color=#00FF00>挑战主线关卡成功,已自动解锁下一个地图</color>",
                    Type = RuleType.Normal
                });

                user.MapId = mapConfig.Id + 1;
            }

            GameProcessor.Inst.HeroDie(RuleType.MainStage, MapTime);
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = $"<color=#FF0000>人物死亡，主线闯关失败</color>",
                Type = RuleType.Normal
            });

            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.MainStage, MapTime);
        }
    }
}
