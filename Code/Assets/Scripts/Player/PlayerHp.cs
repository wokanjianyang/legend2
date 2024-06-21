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

public class PlayerHp : MonoBehaviour
{
    public Image avatar;

    public Text Txt_Name;
    public Text Txt_Level;

    public Progress_Hp ProgressHp;

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
        this.Init();

        this.SelfPlayer.EventCenter.AddListener<SetPlayerHPEvent>(SetPlayerHPEvent);

    }

    public void Init()
    {
        this.Txt_Name.text = this.SelfPlayer.Name;
        this.Txt_Level.text = this.SelfPlayer.Level + "";

        SetHpProgress(this.SelfPlayer.GetHpProgress());
    }

    public void SetPlayerHPEvent(SetPlayerHPEvent e)
    {
        SetHpProgress(this.SelfPlayer.GetHpProgress());
    }

    private void SetHpProgress(float progress)
    {
        this.ProgressHp.SetProgress(progress);
    }


    public void OnDestroy()
    {

    }
}
