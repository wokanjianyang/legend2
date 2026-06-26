using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Map_Item : MonoBehaviour
{
    public Button Btn_Start;
    public Text Txt_Name;
    public Image Img_Icon;

    private MapConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
    }

    public void SetContent(MapConfig mapConfig)
    {
        this.Config = mapConfig;
        Txt_Name.text = mapConfig.Name;

        if (mapConfig.BossId > 0)
        {
            Img_Icon.gameObject.SetActive(true);
        }
        else
        {
            Img_Icon.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        User user = User_Data_Manager.Data;
        if (user == null || this.Config == null)
        {
            return;
        }

        //Debug.Log("MapId:" + user.MapId);

        if (this.Config.Id <= user.MapId)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnClick_Start()
    {
        long ns = TimeHelper.ClientNowSeconds();
        if (ns - AppHelper.ChangeMapTime < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请稍后点击，间隔少于5秒", ToastType = ToastTypeEnum.Failure });
            return;
        }
        else
        {
            AppHelper.ChangeMapTime = ns;
        }

        var dialog = this.GetComponentInParent<Main_Map_Dialog>();
        dialog.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { MapId = this.Config.Id });
    }
}
