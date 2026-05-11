using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class ViewForgeProcessor : AViewPage
{
    public Toggle toggle_Refine;
    public Panel_Refine PanelRefine;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;

    public Toggle toggle_Compound;
    public Panel_Compound PanelCompound;



    private void Awake()
    {
        this.toggle_Compound.onValueChanged.AddListener((isOn) =>
        {
            this.PanelCompound.Show(isOn);
        });

        this.toggle_Strengthen.onValueChanged.AddListener((isOn) =>
        {
            PanelStrengthen.gameObject.SetActive(isOn);
        });

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            PanelRefine.gameObject.SetActive(isOn);
        });
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";


        //if (user.MagicLevel.Data >= 10)
        //{
        //    toggle_Refine.gameObject.SetActive(true);
        //}
        //else
        //{
        //    toggle_Refine.gameObject.SetActive(false);
        //}
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();
    }

    protected override bool CheckPageType(ViewPageType page)
    {
        return page == ViewPageType.View_Forge;
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }
}
