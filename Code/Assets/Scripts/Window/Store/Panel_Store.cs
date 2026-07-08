using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Store : MonoBehaviour
{
    private int SelectType = 0;
    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    public ScrollRect Sr_Bag;
    private List<Store_Item> bagList = new List<Store_Item>();

    public Text Txt_Title;

    public Dialog_Store_Info Dlg_Store_Info;

    public int Order => (int)ComponentOrder.Dialog;

    private GameObject PrefabItem = null;

    // Start is called before the first frame update
    void Awake()
    {
        PrefabItem = Resources.Load<GameObject>("Prefab/Window/Store/Store_Item");

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
            Store_Item item = CreateItem();
            item.Init(list[i]);
            this.bagList.Add(item);
        }
    }

    private Store_Item CreateItem()
    {
        var go = GameObject.Instantiate(PrefabItem);
        Store_Item comItem = go.GetComponent<Store_Item>();
        //comItem.SetItem(item, type, cycle);

        comItem.transform.SetParent(Sr_Bag.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void ChangePanel(int index)
    {
        this.SelectType = index;


        foreach (Store_Item sp in bagList)
        {
            sp.ChangeType(SelectType + 3);
        }
    }

    public void ShowInfo(StoreConfig config)
    {
        Dlg_Store_Info.Show(config);
    }

    public void ConvertSuccess(int sid)
    {
        Store_Item item = bagList.Where(m => m.Config.Id == sid).FirstOrDefault();
        item.Refresh();
    }
}
