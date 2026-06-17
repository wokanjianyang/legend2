using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class View_Forge : AViewPage
{
    public List<Toggle> Toggle_Nav_List;
    public List<Transform> Tf_Nav_List;

    public Toggle toggle_Refine;
    public Panel_Refine PanelRefine;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;

    public Toggle Toggle_Legend;
    public Panel_Legend PanelLegend;

    public Toggle Toggle_Grade;
    public Panel_Grade PanelGrade;

    public Transform Tf_Toggle_Legacy;
    private List<Toggle> Toggle_Legacy_List;
    public Panel_Legacy PanelLegacy;

    private void Awake()
    {
        Toggle_Legacy_List = Tf_Toggle_Legacy.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < Toggle_Nav_List.Count; i++)
        {
            int index = i;
            this.Toggle_Nav_List[index].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeType(index, isOn);
            });
        }

        for (int i = 0; i < Toggle_Legacy_List.Count; i++)
        {
            int index = i;
            this.Toggle_Legacy_List[index].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLegacy(index);
            });
        }

        this.Toggle_Legend.onValueChanged.AddListener((isOn) =>
        {
            this.PanelLegend.gameObject.SetActive(isOn);
        });

        this.toggle_Strengthen.onValueChanged.AddListener((isOn) =>
        {
            PanelStrengthen.gameObject.SetActive(isOn);
        });

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            PanelRefine.gameObject.SetActive(isOn);
        });

        this.Toggle_Grade.onValueChanged.AddListener((isOn) =>
        {
            PanelGrade.gameObject.SetActive(isOn);
        });
    }

    private void ChangeType(int index, bool isOn)
    {
        Tf_Nav_List[index].gameObject.SetActive(isOn);
    }

    private void ChangeLegacy(int index)
    {
        PanelLegacy.gameObject.SetActive(true);
        PanelLegacy.ChangeRole(index + 1);
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
