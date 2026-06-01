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

    private List<Babel_Rank_Item> ItemList = new List<Babel_Rank_Item>();

    private GameObject ItemPrefab = null;


    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    // Update is called once per frame
    void Start()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/More/Babel/Babel_Rank_Item");

        this.Init();
    }

    private void Init()
    {
        foreach (var sp in ItemList)
        {
            sp.gameObject.SetActive(false);
        }

        this.Show();
    }


    private void Show()
    {
        //Log.Debug("ShowStrengthInfo");
        for (int i = 0; i < 10; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            Babel_Rank_Item com = item.GetComponentInChildren<Babel_Rank_Item>();

            com.SetContent(i + 1, "ÕÅÈý", 999);

            item.transform.SetParent(Tf_Atr_List);
            item.transform.localScale = Vector3.one;

            ItemList.Add(com);
        }
    }

    private void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}

