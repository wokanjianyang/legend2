using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pet : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;

    private GameObject prefab;
    private List<Item_Pet> PetItems = new List<Item_Pet>();

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Item_Pet");
        this.Init();
    }

    private void OnEnable()
    {
        this.Show();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<PetBattleDownEvent>(this.PetBattleDown);
    }

    private void PetBattleDown(PetBattleDownEvent e)
    {
        User user = User_Data_Manager.Data;

        //判断空格
        int ic = User_Data_Manager.Data.GetBagIdleCount(3);
        if (ic < 10)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Item_Pet item = e.Item;

        Pet pet = item.pet;

        PetItems.Remove(item);
        GameObject.Destroy(item.gameObject);

        user.PetList.Remove(pet);

        List<Item> items = new List<Item>();
        items.Add(pet);
        if (items.Count > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        //更新属性面板
        GameProcessor.Inst.UpdateInfo();

        //更新技能描述
        GameProcessor.Inst.EventCenter.Raise(new SkillShowEvent());
    }

    // Start is called before the first frame update
    public void Show()
    {
        //Debug.Log("pet onable");

        User user = User_Data_Manager.Data;
        if (user == null)
        {
            return;
        }

        foreach (var cb in PetItems)
        {
            GameObject.Destroy(cb.gameObject);
        }
        PetItems.Clear();

        List<Pet> pets = user.PetList;

        for (int i = 0; i < pets.Count; i++)
        {
            Item_Pet item = this.CreateItem(pets[i], i);
            this.PetItems.Add(item);
        }
    }

    private Item_Pet CreateItem(Pet pet, int position)
    {
        var go = GameObject.Instantiate(prefab);
        Item_Pet comItem = go.GetComponent<Item_Pet>();
        comItem.Init(pet);

        comItem.transform.SetParent(this.sr_Boss.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void Init()
    {

    }
}
