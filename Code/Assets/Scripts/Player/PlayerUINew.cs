using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Dialog;
using SDD.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUINew : MonoBehaviour, IPlayer, IPointerClickHandler
{
    [LabelText("血条")]
    public Com_Progress com_Progress;

    private float doTime = 0;

    private float effectTime = 0;

    private float damageTime = 0;

    private float restoreTime = 0;

    public APlayer SelfPlayer { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.doTime -= Time.unscaledDeltaTime;
        this.effectTime += Time.unscaledDeltaTime;
        this.damageTime += Time.unscaledDeltaTime;
        this.restoreTime += Time.unscaledDeltaTime;

        if (this.SelfPlayer == null)
        {
            return;
        }

        if (doTime <= 0)
        {
            this.doTime = this.SelfPlayer.DoEvent();
        }

        if (effectTime > 0.2f)
        {
            this.SelfPlayer.DoEffect(effectTime);
            effectTime = 0;
        }

        if (damageTime > 0.1f)
        {
            this.damageTime = 0;
        }

        if (this.restoreTime >= 1f)
        {
            this.restoreTime = 0;
            this.SelfPlayer.AutoRestore();
        }
    }

    public void SetParent(APlayer player)
    {
        this.SelfPlayer = player;

        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);

    }

    public void OnDestroy()
    {
        if (this.SelfPlayer != null)
        {
            this.SelfPlayer.EventCenter.RemoveListener<SetPlayerHPEvent>(OnSetPlayerHPEvent);
        }
        this.com_Progress = null;
    }

    private void OnSetPlayerHPEvent(SetPlayerHPEvent e)
    {
        if (this.com_Progress != null)
        {
            this.com_Progress.SetProgress(this.SelfPlayer.HP, SelfPlayer.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }
    }

    private List<ShowMsgEvent> msgTaskList = new List<ShowMsgEvent>();


    public void OnPointerClick(PointerEventData eventData)
    {
        Hero hero = GameProcessor.Inst.PlayerManager.GetHero();

        if (this.SelfPlayer.GroupId != hero.GroupId)
        {
            if (hero.Enemy != null)
            {
                hero.Enemy.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            }
            hero.UpdateEnemy(this.SelfPlayer);
            this.SelfPlayer.EventCenter.Raise(new ShowAttackIcon { NeedShow = true });
        }
    }
}
