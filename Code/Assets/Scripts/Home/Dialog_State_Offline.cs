using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_State_Offline : MonoBehaviour
{
    public Text Txt_Content;

    public Button Btn_Close;
    public Button Btn_Ok;


    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnOk);
    }

    private void OnEnable()
    {
        this.Show();
    }


    public void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user.OfflineLog != null && user.OfflineLog.Count == 2)
        {
            int mapId = user.OfflineLog[1];
            int total = user.OfflineLog[2];

            MapConfig config = MapConfigCategory.Instance.Get(mapId);

            Txt_Content.text = string.Format("记录离线副本为：{0}，\n300秒杀怪效率为：{1}个", config.Name, total);
        }
        else
        {
            Txt_Content.text = "还没有记录通关副本和时间，请先通关副本";
        }

        Btn_Ok.gameObject.SetActive(true);
    }


    public void OnOk()
    {
        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "开始记录离线效率", ToastType = ToastTypeEnum.Success });

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Offline, MapId = AppHelper.CurrentMapId });
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
