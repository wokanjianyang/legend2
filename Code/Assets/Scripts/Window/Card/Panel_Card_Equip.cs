using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Card_Equip : MonoBehaviour
{
    public ScrollRect sr_Boss;

    private int Stage = 0;

    private List<Item_Card_Equip> items = new List<Item_Card_Equip>();

    private GameObject ItemPrefab;

    void Awake()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Card/Item_Card_Equip");
    }

    public void Show(int stage)
    {
        this.gameObject.SetActive(true);
        this.Stage = stage;

        foreach (var sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }
        items.Clear();

        List<CardConfig> configs = CardConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Stage == stage).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Card_Equip>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

    public void SelectItem(int id) { 
       
    }
}
