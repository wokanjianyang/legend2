using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Phantom : ABattleRule
{
    private bool PhanStart = false;

    private int PhanId = 0;
    private int Layer = 0;

    private const int MaxTime = 180 * 5;

    private int Time = 180 * 5;

    private long MapTime = 0;

    protected override RuleType ruleType => RuleType.Phantom;

    private APlayer RealBoss = null;

    public BattleRule_Phantom(Dictionary<string, object> param)
    {
        param.TryGetValue("PhantomId", out object pid);
        param.TryGetValue("MapTime", out object mapTime);

        this.PhanId = (int)pid;
        this.MapTime = (long)mapTime;

        PhanStart = true;
        RealBoss = null;
        Time = MaxTime;

        GameProcessor.Inst.User.PhantomRecord.TryGetValue(PhanId, out int lv);
        Layer = lv;
    }

    public override void DoMapLogic(int roundNum)
    {
        if (!PhanStart)
        {
            return;
        }

        if (Time <= 0)
            return;
        Time--;

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Phantom, Message = RealBoss.Name + "����û��ͨ����ս��" });
            PhanStart = false;
        }

        if (RealBoss == null)
        {
            //Debug.Log("PhanId:" + PhanId + " Layer:" + Layer);
            RealBoss = new Monster_Phantom(PhanId, Layer, true, 10);  //ˢ�±���,10������Ѫ
            GameProcessor.Inst.PlayerManager.LoadMonster(RealBoss);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Phantom, Message = RealBoss.Name + "����ʿ,����Ҫ����ս����?" });
        }

        if (RealBoss.HP <= 0 && Time > 0)
        {
            GameProcessor.Inst.User.PhantomRecord[PhanId] = Layer + 1;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Phantom, Message = RealBoss.Name + "��ǿ�����ʿ,���Ѿ�ͨ���˿��飡" });

            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

            GameProcessor.Inst.HeroDie(RuleType.Phantom, MapTime);

            PhanStart = false;
            return;
        }

        if (Time <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Phantom, Message = RealBoss.Name + "����û��ͨ����ս..." });
            GameProcessor.Inst.HeroDie(RuleType.Phantom, MapTime);
            PhanStart = false;
            return;
        }

        if (Time % 5 == 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowPhantomInfoEvent() { Time = Time / 5 });
        }
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Phantom, MapTime);
        }
    }

    //public override void DoHeroLogic()
    //{
    //    if (!PhanStart)
    //    {
    //        return;
    //    }

    //    if (Time <= 0)
    //        return;
    //    Time--;

    //    var hero = GameProcessor.Inst.PlayerManager.GetHero();
    //    hero.DoEvent();

    //    if (hero.HP <= 0)
    //    {
    //        GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = RealBoss.Name + "����û��ͨ����ս��" });
    //        PhanStart = false;
    //    }

    //    GameProcessor.Inst.EventCenter.Raise(new ShowPhantomInfoEvent() { Time = Time });
    //}

    //public override void DoValetLogic()
    //{
    //    if (!PhanStart)
    //    {
    //        return;
    //    }

    //    base.DoValetLogic();
    //}

    //public override void DoMonsterLogic()
    //{
    //    if (!PhanStart)
    //    {
    //        return;
    //    }

    //    if (Time <= 0)
    //        return;
    //    Time--;

    //    GameProcessor.Inst.EventCenter.Raise(new ShowPhantomInfoEvent() { Time = Time });

    //    if (RealBoss == null)
    //    {
    //        Debug.Log("PhanId:" + PhanId + " Layer:" + Layer);
    //        RealBoss = new Monster_Phantom(PhanId, Layer, true, 10);  //ˢ�±���,10������Ѫ
    //        GameProcessor.Inst.PlayerManager.LoadMonster(RealBoss);

    //        GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = RealBoss.Name + "����ʿ,����Ҫ����ս����?" });
    //    }

    //    var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
    //    foreach (var enemy in enemys)
    //    {
    //        enemy.DoEvent();
    //    }

    //    if (RealBoss.HP <= 0 && Time > 0)
    //    {
    //        GameProcessor.Inst.User.PhantomRecord[PhanId] = Layer + 1;

    //        GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = RealBoss.Name + "��ǿ�����ʿ,���Ѿ�ͨ���˿��飡" });

    //        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

    //        GameProcessor.Inst.HeroDie(RuleType.Phantom, MapTime);

    //        PhanStart = false;
    //        return;
    //    }

    //    if (Time <= 0) {
    //        GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = RealBoss.Name + "����û��ͨ����ս..." });
    //        PhanStart = false;
    //        return;
    //    }
    //}


}
