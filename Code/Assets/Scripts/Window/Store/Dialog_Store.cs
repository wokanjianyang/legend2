using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Store : MonoBehaviour
{

    public Toggle toggle_Refine;
    public Panel_Refine PanelRefine;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;


    private void Awake()
    {
        this.toggle_Strengthen.onValueChanged.AddListener((isOn) =>
        {
            PanelStrengthen.gameObject.SetActive(isOn);
        });

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            PanelRefine.gameObject.SetActive(isOn);
        });
    }


}
