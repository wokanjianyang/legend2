using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Reform : MonoBehaviour
{
    public Transform Tran_Item_List;
    private Box_Forge[] items;

    public ScrollRect Sr_Bag;

    public Text Txt_Fee;

    public Button Btn_Ok;

    public Text Txt_Info;
    public Text Txt_Exp;

    private int SelectMainIndex = 1;
    private int SelectBagIndex = -1;

    private Item CurrentItem;

    private int ForgeType = 5;

    private List<Box_Forge_Bag> bagList = new List<Box_Forge_Bag>();
    private int MaxMC = 40;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<Box_Forge>();

        Btn_Ok.onClick.AddListener(OnClick_OK);
    }

    // Update is called once per frame
    void Start()
    {
        this.Init();
    }

    private void Init()
    {
        var emptyPrefab = PrefabHelper.Instance().ComBoxEmpty;
        for (var i = 0; i < MaxMC; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.Sr_Bag.content);
            empty.name = "Box_" + i;
            //yield return null;
        }
    }

    void OnEnable()
    {
        this.Refresh();
    }

    private void Refresh()
    {
        User user = User_Data_Manager.Data;

        if (user == null)
        {
            return;
        }

        //清理
        this.CurrentItem = null;

        foreach (var sp in items)
        {
            sp.SetItem(null);
        }

        foreach (var sp in bagList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        bagList.Clear();

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;

            items[i].Init(ForgeType, position, toggleGroup);
            items[i].SetItem(user.GetEquip(position));
        }

        this.Txt_Exp.text = "请选择材料装备";
        this.Txt_Info.text = "请选择改造装备";
    }


    public void SelectItem(int p, Item item, Box_Forge box)
    {
        this.SelectMainIndex = p;
        this.CurrentItem = item;

        //清理
        foreach (var sp in bagList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        bagList.Clear();

        this.Txt_Exp.text = "请选择材料装备";

        if (this.CurrentItem == null)
        {
            Txt_Info.text = "此部位没有装备";
            return;
        }
        else
        {
            Equip equip = this.CurrentItem as Equip;

            if (equip.Config.Cycle > 1)
            {
                Txt_Info.text = "只有普通准备可以改造";
                return;
            }
            else if (equip.GetQuality() < 5)
            {
                Txt_Info.text = "只能改造橙色装备";
                return;
            }
            else if (equip.GetReformLevel() >= 5)
            {
                Txt_Info.text = "此装备改造已经满级了";
                return;
            }
            else
            {
                Txt_Info.text = string.Format("{0}：当前经验{1}/{2}", equip.GetName(), equip.ReformExp, equip.GetReformNeedExp());
            }
        }


        this.Select_Main();
    }

    private void Select_Main()
    {
        User user = User_Data_Manager.Data;

        Equip equip = this.CurrentItem as Equip;

        int part = equip.Config.Part;
        int quality = equip.GetQuality();
        int configId = equip.ConfigId;

        var equips = user.Bags.Where(m => m.Item.GetItemType() == ItemType.Equip && m.Item.GetQuality() == quality
        && m.Item.ConfigId == configId && !m.Item.IsLock).ToList();

        List<Equip> bags = new List<Equip>();
        foreach (var item in equips)
        {
            Equip bi = item.Item as Equip;
            bags.Add(bi);
        }

        ToggleGroup tgBag = Sr_Bag.GetComponent<ToggleGroup>();

        int BoxId = 0;
        for (int i = 0; i < MaxMC; i++)
        {
            if (i < bags.Count)
            {
                var bagBox = Sr_Bag.content.GetChild(i);
                if (bagBox == null)
                {
                    return;
                }

                Box_Forge_Bag box = PrefabHelper.Instance().CreateForgeBag(bagBox);
                box.SetItem(bags[i]);
                box.Init(2, BoxId, tgBag);
                this.bagList.Add(box);

                BoxId++;
            }
        }

    }

    public void SelectBag(int p, Item item, Box_Forge_Bag bag)
    {
        this.SelectBagIndex = p;
        //this.SelectPosition = p;
        //this.CurrentItem = item;
        //this.CurrentBox = box;

        //Debug.Log("legend select bag p" + p);

        this.Show();
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;

        if (CurrentItem == null)
        {
            Txt_Info.text = "此部位没有装备";
        }
        else if (SelectBagIndex < 0)
        {
            Txt_Info.text = "请选择材料";
        }
        else
        {
            Equip equip = this.CurrentItem as Equip;

            if (equip.Config.Cycle != 1)
            {
                Txt_Exp.text = "没有选择材料";
                Txt_Info.text = "此不可以改造";
            }
            else
            {
                Equip me = bagList[SelectBagIndex].CurrentItem as Equip;

                long fee = 10000;

                Txt_Exp.text = "材料提供经验值：" + (me.ReformExp + 1);
                Txt_Info.text = string.Format("{0}：当前经验{1}/{2}", equip.GetName(), equip.ReformExp, equip.GetReformNeedExp());

                string color = user.MagicGold.Data >= fee ? "#11FF11" : "#FF1111";
                Txt_Fee.text = string.Format("所需金币：<color={0}>{1}</color>", color, fee);

                if (user.MagicGold.Data < fee)
                {
                    Btn_Ok.gameObject.SetActive(false);
                }
                else
                {
                    Btn_Ok.gameObject.SetActive(true);
                }
            }
        }
    }


    private void OnClick_OK()
    {
        Btn_Ok.gameObject.SetActive(true);

        User user = User_Data_Manager.Data;

        Equip equip = CurrentItem as Equip;
        Equip me = bagList[SelectBagIndex].CurrentItem as Equip;

        int fee = 10000;

        if (user.MagicGold.Data <= fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(fee);

        if (me.LegendData.Key > 0)
        {
            int lgId = me.LegendData.Key;
            int lgFliar = me.LegendData.Value;
            equip.ToLegend(lgId, lgFliar);
        }

        equip.AddReformExp(me.ReformExp + 1);

        //销毁
        me.IsDelete = true;
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent() { });

        GameProcessor.Inst.UpdateInfo();

        this.Refresh();
    }


}

