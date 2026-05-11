using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task_Group : MonoBehaviour
{
    public Button Btn_Start;
    public Text Txt_Name;

    private AchievementTaskGroupConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void SetContent(AchievementTaskGroupConfig config)
    {
        this.Config = config;
        Txt_Name.text = config.Name;
    }

    private void Show()
    {

    }

    private void OnClick_Start()
    {
        Dialog_Task dialog = this.GetComponentInParent<Dialog_Task>();

        dialog.SelectItem(this.Config.Id);
    }
}
