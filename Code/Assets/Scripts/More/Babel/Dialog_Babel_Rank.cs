using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Babel_Rank : MonoBehaviour
{
    public Button Btn_Close;

    public Transform Tf_Atr_List;

    private List<Babel_Rank_Item> ItemList;

    private List<BabelRank> DataList;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        ItemList = Tf_Atr_List.GetComponentsInChildren<Babel_Rank_Item>().ToList();

    }

    // Update is called once per frame
    void Start()
    {
    }

    public void Init(List<BabelRank> list)
    {
        this.DataList = list;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        //Log.Debug("ShowStrengthInfo");
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (i < DataList.Count)
            {
                ItemList[i].SetContent(i + 1, DataList[i]);
            }
            else
            {
                ItemList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}

