using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Exclusive : MonoBehaviour
{
    public ScrollRect sr_Boss;
    public Dialog_Exclusive_Fuse Dialog_Fuse;

    private int Role = 0;

    private List<Item_Exclusive> items = new List<Item_Exclusive>();

    private GameObject ItemPrefab;

    void Awake()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Exclusive/Item_Exclusive");
    }

    public void Show(int role)
    {
        this.gameObject.SetActive(true);
        this.Role = role;

        foreach (var sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }
        items.Clear();

        List<ExclusiveConfig> configs = ExclusiveConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Role == this.Role).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Exclusive>();

            com.SetContent(configs[i]);
            com.AddListener(SelectItem);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

    private void SelectItem(int id)
    {
        Dialog_Fuse.Open(id);
    }

    public void Refresh()
    {
        foreach (Item_Exclusive item in items)
        {
            item.Show();
        }
    }
}