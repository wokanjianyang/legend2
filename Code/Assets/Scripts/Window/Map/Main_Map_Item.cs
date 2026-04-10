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

    private void OnEnable()
    {
        this.Show();
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

    private void Show()
    {
        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

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
        var dialog = this.GetComponentInParent<Main_Map_Dialog>();
        dialog.gameObject.SetActive(false);

        AppHelper.CurrentMapId = this.Config.Id;

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { MapId = this.Config.Id });
    }
}
