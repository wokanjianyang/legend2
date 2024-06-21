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

public class PlayerUI : MonoBehaviour, IPlayer, IPointerClickHandler
{
    public Text Txt_Name;
    public Text Txt_Level;

    public Image progress;

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
    }

    public void Init()
    {

        Debug.Log("Name:" + this.SelfPlayer.Name);

        this.Txt_Name.text = this.SelfPlayer.Name;
        this.Txt_Level.text = "lv." + this.SelfPlayer.Level;

    }

    public void SetHpProgress()
    {
        double maxHp = this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
        double percent = this.SelfPlayer.HP / maxHp;

        percent = Math.Max(percent, 0);
        percent = Math.Min(percent, 1);

        progress.fillAmount = (float)percent;
    }

    public void ClearPlayer()
    {
        StartCoroutine(this.SendClearPlayerEvent());
    }

    private IEnumerator SendClearPlayerEvent()
    {
        yield return new WaitForSeconds(ConfigHelper.DelayShowTime);
        GameProcessor.Inst.PlayerManager.RemoveDeadPlayers(this.SelfPlayer);
        yield return null;
    }


    public void OnDestroy()
    {

    }

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