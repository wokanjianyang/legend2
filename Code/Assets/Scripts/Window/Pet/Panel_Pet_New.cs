using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pet_New : MonoBehaviour
{
    public ScrollRect sr_Boss;

    private GameObject prefab;
    private List<Item_Pet_New> PetItems = new List<Item_Pet_New>();

    public Transform Tf_Plan;
    private List<Toggle> Toggle_Plan_List = new List<Toggle>();

    void Awake()
    {
        Toggle_Plan_List = Tf_Plan.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < Toggle_Plan_List.Count; i++)
        {
            int index = i;
            Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ChangePlan(index);
                }
            });
        }
    }

    private void Start()
    {
        prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Pet_New");
        this.Init();
    }

    private void OnEnable()
    {
        //if (this.PetItems.Count > 0)
        //{
        //    this.ChangePlan(User_Data_Manager.Data.PetPanelIndex);
        //}
    }

    private void ChangePlan(int index)
    {
        Debug.Log("ChangePlan" + index);

        User user = User_Data_Manager.Data;
        user.PetPanelIndex = index;
        //Toggle_Plan_List[index].isOn = true;

        this.Refresh();

        //更新属性面板
        GameProcessor.Inst.UpdateInfo();

        //更新技能描述
        GameProcessor.Inst.EventCenter.Raise(new SkillShowEvent());
    }

    private Item_Pet_New CreateItem(int position)
    {
        var go = GameObject.Instantiate(prefab);
        Item_Pet_New comItem = go.GetComponent<Item_Pet_New>();
        comItem.Init(position);

        comItem.transform.SetParent(this.sr_Boss.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void Init()
    {
        User user = User_Data_Manager.Data;
        int pg = (int)user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PetOnLimit) + ConfigHelper.PetMax;

        for (int i = 1; i <= pg; i++)
        {
            Item_Pet_New item = this.CreateItem(i);
            this.PetItems.Add(item);
        }
    }

    public void Refresh()
    {
        foreach (var sp in PetItems)
        {
            sp.Show();
        }
    }
}
