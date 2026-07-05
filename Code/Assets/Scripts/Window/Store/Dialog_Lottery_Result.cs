using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Lottery_Result : MonoBehaviour
{
    public Button Btn_Close;

    public Text Txt_Name;

    public Transform Tf_List;
    private List<Lottery_Result_Box> items;

    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        items = Tf_List.GetComponentsInChildren<Lottery_Result_Box>().ToList();
    }

    public void ShowResult(Lottery_Result data)
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < items.Count; i++)
        {
            if (i < data.List.Count)
            {
                items[i].gameObject.SetActive(true);
                items[i].SetContent(data.List[i]);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
