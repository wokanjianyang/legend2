using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Com_BossInfoItem : MonoBehaviour
{
    public Text txt_BossName;
    public Text txt_MapName;
    public Text txt_Time;
    //public Text txt_MapName;

    public Button btn_Start;
    public Text txt_Start;

    private int MapId;


    // Start is called before the first frame update
    void Start()
    {
        btn_Start.gameObject.SetActive(true);
        txt_Time.gameObject.SetActive(false);

        btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }

    private void OnClick_NavigateMap()
    {
        var vm = this.GetComponentInParent<ViewMap>();
        vm.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new StartCopyEvent() { MapId = this.MapId });
    }

    public void SetContent(MapConfig mapConfig)
    {
        this.MapId = mapConfig.Id;

        txt_MapName.text = mapConfig.Name;
        if (mapConfig.Memo != "")
        {
            txt_MapName.text += "(" + mapConfig.Memo + ")";
        }

        txt_BossName.text = mapConfig.LevelRequired + "";
    }
}
