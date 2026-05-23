using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Spirit_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;

    private SpiritCopyConfig Config;

    private long MaxId = 1;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }

    private void OnEnable()
    {
        if (this.Config != null)
        {
            this.Show();
        }
    }


    private void Show()
    {
        if (Config.Require <= MaxId)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnClick_NavigateMap()
    {
        var dialog = this.GetComponentInParent<Map_Dialog_Spirit>();
        dialog.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<View_More>();
        vm.HideItem();

        GameProcessor.Inst.EventCenter.Raise(new SpiritStartEvent() { Id = Config.Id });
    }



    public void SetContent(SpiritCopyConfig config)
    {
        this.Config = config;

        Txt_Name.text = config.MapName;

        this.Show();
    }

    public void SetMax(long max)
    {
        this.MaxId = max;
        this.Show();
    }
}
