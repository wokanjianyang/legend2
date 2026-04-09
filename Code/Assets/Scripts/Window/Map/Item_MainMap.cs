using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_MainMap : MonoBehaviour
{
    public Text txt_Name;

    public Button btn_Name;
    public Button btn_Change;
    public Button btn_Start;

    public Image img_Bg;
    public Text txt_Desc;

    private int MapId;


    // Start is called before the first frame update
    void Start()
    {
        btn_Start.gameObject.SetActive(true);

        btn_Name.onClick.AddListener(OnClick_Name);
        btn_Change.onClick.AddListener(OnClick_Change);
        btn_Start.onClick.AddListener(OnClick_Start);
    }

    private void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        if (this.MapId == user.MapId)
        {
            btn_Start.gameObject.SetActive(true);
        }
        else
        {
            btn_Start.gameObject.SetActive(false);
        }
    }

    public void SetContent(MapConfig mapConfig)
    {
        this.MapId = mapConfig.Id;

        txt_Name.text = mapConfig.Name;
        txt_Desc.text = mapConfig.Memo;
    }

    private void OnClick_Name()
    {
        img_Bg.gameObject.SetActive(!img_Bg.gameObject.activeSelf);
    }

    private void OnClick_Change()
    {
        var dialog = this.GetComponentInParent<Map_Dialog_Main>();
        dialog.gameObject.SetActive(false);

        AppHelper.CurrentMapId = this.MapId;

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { MapId = this.MapId });
    }

    private void OnClick_Start()
    {
        var dialog = this.GetComponentInParent<Map_Dialog_Main>();
        dialog.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new StartCopyEvent() { MapId = this.MapId });
    }
}
