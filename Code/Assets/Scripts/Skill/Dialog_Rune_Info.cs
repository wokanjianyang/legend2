using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Rune_Info : MonoBehaviour
{
    public Text Txt_Desc;

    public Button Btn_Close;
    
    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void Show(string desc)
    {
        this.gameObject.SetActive(true);

        Txt_Desc.text = desc;
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
