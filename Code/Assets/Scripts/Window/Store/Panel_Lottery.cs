using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Lottery : MonoBehaviour
{
    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    public ScrollRect Sr_Bag;
    private List<Lottery_Item> bagList = new List<Lottery_Item>();

    private int SelectType = 0;
    //private string[] Titles = { "宠物图鉴", "1-50级装备", "60-120级装备", "130-180级装备" };

    private GameObject PrefabItem = null;


    public Dialog_Lottery_Info Dlg_Lottery_Info;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        PrefabItem = Resources.Load<GameObject>("Prefab/Window/Store/Lottery_Item");

        toggleStageList = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        this.Init();
        //this.ChangePanel(0);
    }


    public void Init()
    {
        List<StoreConfig> list = StoreConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            Lottery_Item item = CreateItem();
            item.Init(list[i]);
            this.bagList.Add(item);
        }
    }

    private Lottery_Item CreateItem()
    {
        var go = GameObject.Instantiate(PrefabItem);
        Lottery_Item comItem = go.GetComponent<Lottery_Item>();
        //comItem.SetItem(item, type, cycle);

        comItem.transform.SetParent(Sr_Bag.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void ChangePanel(int index)
    {
        this.SelectType = index;

        foreach (Lottery_Item sp in bagList)
        {
            sp.ChangeType(SelectType + 3);
        }
    }

    public void ShowInfo(StoreConfig config)
    {
        Dlg_Lottery_Info.Show(config);
    }
}
