using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Legacy : ABattleRule
{
    private bool Start = false;

    private int Layer = 0;

    private double MapTime = 0;

    private const int MaxQuanlity = 30;
    protected override RuleType ruleType => RuleType.Legacy;

    public BattleRule_Legacy(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("Layer", out object layer);

        this.MapTime = (long)mapTime;
        this.Layer = (int)layer;

        Start = true;

        User user = GameProcessor.Inst.User;
        user.LegacyData.Time.Data -= 3;

        MapTime = 0;
    }


    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            return;
        }
        //Debug.Log("create pill currentRoundTime:" + currentRoundTime);

        MapTime += currentRoundTime;
        //Debug.Log("create pill MapTime:" + MapTime);

        User user = GameProcessor.Inst.User;

        if (user.LegacyData.Time.Data <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "您已经没有了挑战时间！" });

            GameOver();

            Start = false;
            return;
        }

        user.LegacyData.Time.Data -= currentRoundTime;
        if (user.LegacyData.Time.Data < -60)
        {
            user.LegacyData.Time.Data = -60; //如果切后台，导致currentRoundTime特别大
        }

        string msg = "剩余时间：" + (int)user.LegacyData.Time.Data + "S";
        GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        if (enemys.Count >= MaxQuanlity)
        {
            return;
        }

        var enemy = new Monster_Legacy(Layer);
        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.CloseBattle(RuleType.Legacy, 0);
    }
}
