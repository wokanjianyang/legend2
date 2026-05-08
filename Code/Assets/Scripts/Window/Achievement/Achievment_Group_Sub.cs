using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment_Group_Sub : MonoBehaviour
{
    public Button Btn_Start;
    public Text Txt_Name;

    private AchievementGroupConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void SetContent(AchievementGroupConfig config)
    {
        this.Config = config;
        Txt_Name.text = config.Name;
    }

    private void Show()
    {

    }

    private void OnClick_Start()
    {
        Dialog_Achievement dialog = this.GetComponentInParent<Dialog_Achievement>();

        dialog.SelectItem(this.Config.Id);
    }
}
