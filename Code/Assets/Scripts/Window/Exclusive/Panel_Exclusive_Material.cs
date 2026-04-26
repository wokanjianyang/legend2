using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Exclusive_Material : MonoBehaviour
{
    public ScrollRect sr_Boss;

    private int Stage = 0;

    private List<Item_Exclusive_Material> items = new List<Item_Exclusive_Material>();

    private GameObject ItemPrefab;

    void Awake()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Exclusive/Item_Exclusive_Material");
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

        List<ExclusiveMaterialConfig> configs = ExclusiveMaterialConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Exclusive_Material>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }
}
