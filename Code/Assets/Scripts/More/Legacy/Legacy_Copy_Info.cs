using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Legacy_Copy_Info : MonoBehaviour
{
    public Text Txt_Time;
    public Button Btn_Close;

    public Text Txt_Layer_1;
    public Button Btn_Start_1;
    public Text Txt_Info_1;

    public Text Txt_Layer_2;
    public Button Btn_Start_2;
    public Text Txt_Info_2;

    public Text Txt_Layer_3;
    public Button Btn_Start_3;
    public Text Txt_Info_3;

    public int Order => (int)ComponentOrder.Dialog;

    void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);

        this.Btn_Start_1.onClick.AddListener(() => { StartCopy(0); });
        this.Btn_Start_2.onClick.AddListener(() => { StartCopy(1); });
        this.Btn_Start_3.onClick.AddListener(() => { StartCopy(2); });
    }

    private void Start()
    {
        User user = User_Data_Manager.Data;
        user.LegacyData.Check(user.MagicLevel.Data);
    }

    void OnEnable()
    {
        Show();
    }

    public int GetMiddleNumber(int a, int b, int c)
    {
        int max = Math.Max(a, Math.Max(b, c));
        int min = Math.Min(a, Math.Min(b, c));
        return a + b + c - max - min;
    }

    public void Show()
    {
        User user = User_Data_Manager.Data;

        long time = (int)user.LegacyData.Time.Data;
        Txt_Time.text = "副本剩余时间：" + time + "秒";


        int currentLayer = user.GetLegacyCurrentSet();

        Txt_Layer_1.text = currentLayer + "阶";

        Txt_Layer_2.text = (currentLayer + 1) + "阶";
        int k1 = user.GetExclusiveLevel(103);
        if (k1 > 0)
        {
            Btn_Start_2.gameObject.SetActive(true);
            Txt_Info_2.gameObject.SetActive(false);
        }
        else {
            Btn_Start_2.gameObject.SetActive(false);
            Txt_Info_2.gameObject.SetActive(true);
        }

        Txt_Layer_3.text = (currentLayer + 2) + "阶";
        int k2 = user.GetExclusiveLevel(104);
        if (k2 > 0)
        {
            Btn_Start_3.gameObject.SetActive(true);
            Txt_Info_3.gameObject.SetActive(false);
        }
        else {
            Btn_Start_3.gameObject.SetActive(false);
            Txt_Info_3.gameObject.SetActive(true);
        }
    }

    private void StartCopy(int type)
    {
        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Legacy, MapId = type });
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
