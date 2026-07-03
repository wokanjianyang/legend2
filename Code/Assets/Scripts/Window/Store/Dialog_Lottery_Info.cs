using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Lottery_Info : MonoBehaviour
{

    public Button Btn_Close;

    public Text Txt_Name;
    public Transform Tf_Atr_List;
    private List<Text> Txt_Atr_List;
    public Text Txt_Desc;

    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Txt_Atr_List = Tf_Atr_List.GetComponentsInChildren<Text>().ToList();
    }

    public void Show(StoreConfig config)
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < Txt_Atr_List.Count; i++)
        {
            if (i < config.AtrIdList.Length)
            {
                Txt_Atr_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i], "+");
                Txt_Atr_List[i].gameObject.SetActive(true);
            }
            else
            {
                Txt_Atr_List[i].gameObject.SetActive(false);
            }
        }

        if (config.SpeId > 0)
        {
            Txt_Desc.text = StringHelper.FormatAttrText(config.SpeId, config.SpeVue, "+");
        }
        else
        {
            Txt_Desc.text = config.Des;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
