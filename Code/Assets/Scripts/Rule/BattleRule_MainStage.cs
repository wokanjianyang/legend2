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

        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(1);
        }
        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(2);
        }
        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(3);
        }
        for (int i = 0; i < 5; i++)
        {
            QualityList.Add(4);
        }
        for (int i = 0; i < 1; i++)
        {
            QualityList.Add(5);
        }
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);

        int mc1 = QualityList.Where(m => m == 1).Count() + enemys.Where(m => m.Quality == 1).Count();
        int mc2 = QualityList.Where(m => m == 2).Count() + enemys.Where(m => m.Quality == 2).Count();
        int mc3 = QualityList.Where(m => m == 3).Count() + enemys.Where(m => m.Quality == 3).Count();
        int mc4 = QualityList.Where(m => m == 4).Count() + enemys.Where(m => m.Quality == 4).Count();
        int mc5 = QualityList.Where(m => m == 5).Count() + enemys.Where(m => m.Quality == 5).Count();

        GameProcessor.Inst.EventCenter.Raise(new ShowStageInfoEvent() { Mc1 = mc1, Mc2 = mc2, Mc3 = mc3, Mc4 = mc4, Mc5 = mc5 });

        if (enemys.Count < MaxQuanlity && QualityList.Count > 0)
        {
            int count = Math.Min(MaxFreshQuanlity, MaxQuanlity - enemys.Count);

            for (int i = 0; i < count; i++)
            {
                if (QualityList.Count > 0)
                {
                    if (QualityList[0] < 5)
                    {
                        var enemy = MonsterBaseCategory.Instance.BuildMonster(mapConfig, QualityList[0], RuleType.MainStage);
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

        if (Start && mc5 <= 0 && mc4 <= 0 && mc3 <= 0 && mc2 <= 0 && mc1 <= 0)
        {
            Start = false;

            User user = GameProcessor.Inst.User;
            if (user.MapId == mapConfig.Id)
            {
                //闯关成功
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = $"<color=white>挑战主线关卡成功,已自动解锁下一个地图</color>",
                    Type = RuleType.MainStage
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
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.MainStage, MapTime);
        }
    }
}
