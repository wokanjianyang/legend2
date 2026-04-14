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

public class PlayerInfo : MonoBehaviour, IBattleLife
{
    public Text txt_Name;

    public HP_Progress hp_Progress;

    public HP_Progress sp_Progress;

    public Button btn_Query;

    public APlayer SelfPlayer { get; set; }

    public int Order => (int)ComponentOrder.Dialog;

    private bool isShow = true;

    // Start is called before the first frame update
    void Start()
    {

        //this.size = this.transform.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowAttackIcon>(this.OnShowAttackIcon);
    }

    private void OnShowAttackIcon(ShowAttackIcon e)
    {
        if (e.NeedShow)
        {
            this.gameObject.SetActive(isShow);
            this.SelfPlayer = e.Player;
            this.SelfPlayer.Info = this;
            this.Init();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetShow(bool show)
    {
        Debug.Log("change player info show:" + show);

        this.isShow = show;


        this.gameObject.SetActive(isShow);
    }



    public void OnDestroy()
    {
        if (GameProcessor.Inst != null)
        {
            GameProcessor.Inst.EventCenter.RemoveListener<ShowAttackIcon>(this.OnShowAttackIcon);
        }
    }

    private void Init()
    {
        this.txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(SelfPlayer.Quality), SelfPlayer.Name);
        if (SelfPlayer.MaxSP > 0)
        {
            this.sp_Progress.gameObject.SetActive(true);
            this.sp_Progress.SetProgress(this.SelfPlayer.SP, SelfPlayer.MaxSP);
        }
        else
        {
            this.sp_Progress.gameObject.SetActive(false);
        }
        this.hp_Progress.SetProgress(this.SelfPlayer.HP, SelfPlayer.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
    }

    public void SetPlayerHP()
    {
        if (!this.isShow)
        {
            return;
        }

        if (SelfPlayer.SP <= 0 && SelfPlayer.HP <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (SelfPlayer.MaxSP > 0)
        {
            this.sp_Progress.gameObject.SetActive(true);
            this.sp_Progress.SetProgress(this.SelfPlayer.SP, SelfPlayer.MaxSP);
        }
        else
        {
            this.sp_Progress.gameObject.SetActive(false);
        }
        this.hp_Progress.SetProgress(this.SelfPlayer.HP, SelfPlayer.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));


        //this.hp_Progress.HideTitle();
        //this.hp_Progress.ShowTitle();
    }
}
