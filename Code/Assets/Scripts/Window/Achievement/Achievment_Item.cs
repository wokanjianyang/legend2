using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Des;
    public Text Txt_Atr;
    public Text Txt_Active;
    public Button Btn_Active;

    private AchievementConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnClick_Active);
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void SetContent(AchievementConfig config)
    {
        this.Config = config;
        Txt_Name.text = config.Name;
        Txt_Atr.text = StringHelper.FormatAttrText(config.AtrId, config.AtrVue);
        Txt_Des.text = string.Format(config.Memo, config.Condition);
    }

    private void Show()
    {

    }

    private void OnClick_Active()
    {
        
    }
}
