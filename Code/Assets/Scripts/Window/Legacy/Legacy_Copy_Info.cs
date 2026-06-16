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

    public Text Txt_Layer_2;
    public Button Btn_Start_2;

    public int Order => (int)ComponentOrder.Dialog;

    private int[] Layers;

    void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);

        this.Btn_Start_1.onClick.AddListener(() => { StartCopy(0); });
        this.Btn_Start_2.onClick.AddListener(() => { StartCopy(1); });
    }

    private void Start()
    {
        User user = GameProcessor.Inst.User;
        user.LegacyData.Check(user.MagicLevel.Data);
    }

    void OnEnable()
    {
        Show();
    }

    public void Show()
    {
        User user = GameProcessor.Inst.User;

        long time = (int)user.LegacyData.Time.Data;
        Txt_Time.text = "副本剩余时间：" + time + "秒";

        int min = (int)Math.Max(1, user.LegacyLayer.Select(m => m.Value.Data).Min()) + 1;
        int max = (int)Math.Max(1, user.LegacyLayer.Select(m => m.Value.Data).Max());
        max = Math.Min(max, min + 4);

        Txt_Layer_1.text = "（" + min + "阶，人物最低的传世装备等阶）";
        Txt_Layer_2.text = "（" + max + "阶，人物最高的传世装备等阶）";

        Layers = new int[] { min, max };
    }

    private void StartCopy(int type)
    {
        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

        int mapId = Layers[type];
        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Legacy, MapId = mapId });
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
