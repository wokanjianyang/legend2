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

    private int MapId = 0;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_Start);
    }

    private void OnEnable()
    {
    }

    public void SetContent(MapConfig mapConfig)
    {
        this.MapId = mapConfig.Id;
        Txt_Name.text = mapConfig.Name;
    }

    private void OnClick_Start()
    {
        var dialog = this.GetComponentInParent<Map_Dialog_Main>();
        dialog.gameObject.SetActive(false);

        AppHelper.CurrentMapId = this.MapId;

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { MapId = this.MapId });
    }
}
