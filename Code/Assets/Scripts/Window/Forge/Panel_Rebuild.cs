using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Rebuild : MonoBehaviour
{
    public Transform Tran_Item_List;
    private Box_Forge[] items;

    public ScrollRect Sr_Bag;

    public Text Txt_Fee;
    public Button Btn_Ok;

    private int SelectMainIndex = 1;
    private int SelectBagIndex = -1;

    private Item CurrentItem;

    private int ForgeType = 6;
    private int BagType = 3;

    private List<Box_Forge_Bag> bagList = new List<Box_Forge_Bag>();
    private int MaxMC = 40;

    public Transform Tf_Atr_Main;
    public Text Txt_Info_1;
    private List<Item_Rebuild> RebuildItemList1;
    private int SelectAtrIndex1 = 0;

    public Transform Tf_Atr_Material;
    public Text Txt_Info_2;
    private List<Item_Rebuild> RebuildItemList2;
    private int SelectAtrIndex2 = 0;

    private int FeeId = ItemHelper.Specail_Equip_Layer[0];

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<Box_Forge>();

        RebuildItemList1 = Tf_Atr_Main.GetComponentsInChildren<Item_Rebuild>().ToList();
        RebuildItemList2 = Tf_Atr_Material.GetComponentsInChildren<Item_Rebuild>().ToList();

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

        ToggleGroup tg = Tran_Item_List.GetComponent<ToggleGroup>();
        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;

            items[i].Init(ForgeType, position, tg);
        }

        ToggleGroup tg1 = Tf_Atr_Main.GetComponent<ToggleGroup>();
        for (int i = 0; i < RebuildItemList1.Count(); i++)
        {
            RebuildItemList1[i].Init(1, i, tg1);
        }

        ToggleGroup tg2 = Tf_Atr_Material.GetComponent<ToggleGroup>();
        for (int i = 0; i < RebuildItemList2.Count(); i++)
        {
            RebuildItemList2[i].Init(2, i, tg2);
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
        this.SelectBagIndex = -1;

        foreach (var sp in items)
        {
            sp.SetItem(null);
        }

        foreach (var sp in bagList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        bagList.Clear();

        for (int i = 0; i < items.Count(); i++)
        {
            items[i].SetItem(user.GetEquip(i + 1));
        }

        this.Txt_Info_1.gameObject.SetActive(true);
        this.Txt_Info_1.text = "请先选择重铸装备";
        this.Tf_Atr_Main.gameObject.SetActive(false);

        this.Txt_Info_2.gameObject.SetActive(true);
        this.Txt_Info_2.text = "请先选择材料装备";
        this.Tf_Atr_Material.gameObject.SetActive(false);

        this.Txt_Fee.text = string.Format("材料需求");
    }


    public void SelectItem(int p, Item item, Box_Forge box)
    {
        this.SelectMainIndex = p;
        this.CurrentItem = item;
        this.SelectBagIndex = -1;

        this.Txt_Info_1.gameObject.SetActive(true);
        this.Tf_Atr_Main.gameObject.SetActive(false);
        this.Txt_Fee.text = string.Format("材料需求");

        this.Txt_Info_2.gameObject.SetActive(true);
        this.Tf_Atr_Material.gameObject.SetActive(false);

        //清理
        foreach (var sp in bagList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        bagList.Clear();

        if (this.CurrentItem == null)
        {
            Txt_Info_1.text = "此部位没有装备";
            return;
        }
        else
        {
            Equip equip = this.CurrentItem as Equip;

            if (equip.Config.Cycle >= 10)
            {
                Txt_Info_1.text = "只有普通准备可以改造";
                return;
            }
            else if (equip.GetQuality() < 6)
            {
                Txt_Info_1.text = "只能改造红色以上装备";
                return;
            }
            else
            {
                //显示左边属性
                this.Txt_Info_1.gameObject.SetActive(false);
                this.Tf_Atr_Main.gameObject.SetActive(true);

                for (int i = 0; i < this.RebuildItemList1.Count; i++)
                {
                    if (i < equip.AttrEntryList.Count())
                    {
                        AttrEntryConfig config = AttrEntryConfigCategory.Instance.GetConfig(equip.Config.Cycle, equip.AttrEntryList[i].Key, equip.Config.LevelRequired);

                        RebuildItemList1[i].gameObject.SetActive(true);
                        RebuildItemList1[i].SetContent(equip.AttrEntryList[i], config.MaxValue);
                    }
                    else
                    {
                        RebuildItemList1[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        this.Show_Bag();
    }

    public void SelectAtrItem(int type, int position)
    {
        if (type == 1)
        {
            this.SelectAtrIndex1 = position;
        }
        else if (type == 2)
        {
            this.SelectAtrIndex2 = position;
        }
    }

    private void Show_Bag()
    {
        User user = User_Data_Manager.Data;

        Equip equip = this.CurrentItem as Equip;

        int part = equip.Config.Part;
        int quality = equip.GetQuality();
        int configId = equip.ConfigId;
        int layer = equip.Layer;

        var equips = user.Bags.Where(m => m.Item.GetItemType() == ItemType.Equip && m.Item.GetQuality() == quality
        && !m.Item.IsLock).ToList();

        List<Equip> bags = new List<Equip>();
        foreach (var item in equips)
        {
            Equip bi = item.Item as Equip;

            if (bi.Config.Part == part)
            {
                bags.Add(bi);
            }
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
                box.Init(BagType, BoxId, tgBag);
                this.bagList.Add(box);

                BoxId++;
            }
        }
    }

    public void SelectBag(int p, Item item, Box_Forge_Bag bag)
    {
        this.SelectBagIndex = p;

        //显示右边属性
        Equip equip = item as Equip;

        this.Txt_Info_2.gameObject.SetActive(false);
        this.Tf_Atr_Material.gameObject.SetActive(true);

        for (int i = 0; i < this.RebuildItemList2.Count; i++)
        {
            if (i < equip.AttrEntryList.Count())
            {
                AttrEntryConfig config = AttrEntryConfigCategory.Instance.GetConfig(equip.Config.Cycle, equip.AttrEntryList[i].Key, equip.Config.LevelRequired);

                RebuildItemList2[i].gameObject.SetActive(true);
                RebuildItemList2[i].SetContent(equip.AttrEntryList[i], config.MaxValue);
            }
            else
            {
                RebuildItemList2[i].gameObject.SetActive(false);
            }
        }

        this.Show();
    }

    private void Show()
    {
        Btn_Ok.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;

        if (CurrentItem == null)
        {
            Txt_Info_1.text = "此部位没有装备";
        }
        else if (SelectBagIndex < 0)
        {
            Txt_Info_1.text = "请选择材料装备";
        }
        else
        {
            Equip equip = this.CurrentItem as Equip;

            if (equip.Config.Cycle >= 10)
            {
                Txt_Info_1.text = "此装备不可以重铸";
            }
            else
            {
                Equip me = bagList[SelectBagIndex].CurrentItem as Equip;

                int fee = equip.GetRebuildFee();

                long mc = user.GetMaterialCount(FeeId);

                string color = mc >= fee ? "#11FF11" : "#FF1111";
                Txt_Fee.text = string.Format("红装精华：<color={0}>{1}/{2}</color>", color, mc, fee);

                if (mc < fee)
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

        if (this.SelectBagIndex < 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请选择材料装备", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Equip equip = CurrentItem as Equip;
        Equip me = bagList[SelectBagIndex].CurrentItem as Equip;

        var ky = me.AttrEntryList[SelectAtrIndex2];

        //判断是否跨职业的装备
        AttrEntryConfig config = AttrEntryConfigCategory.Instance.GetConfig(me.Config.Cycle, ky.Key, me.Config.LevelRequired);
        if (config.Role > 0 && config.Role != equip.Config.Role)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "属性职业不匹配", ToastType = ToastTypeEnum.Failure });
            return;
        }

        //判断属性是否超数量了
        int count = equip.AttrEntryList.Where(m => m.Key == ky.Key).Count();
        if (count >= config.MaxCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "属性数量不能超出最大设定", ToastType = ToastTypeEnum.Failure });
            return;
        }


        User user = User_Data_Manager.Data;

        int fee = equip.GetRebuildFee();
        long mc = user.GetMaterialCount(FeeId);

        if (mc < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = FeeId,
            Quantity = fee
        });

        equip.AttrEntryList[SelectAtrIndex1] = new KeyValuePair<int, long>(ky.Key, ky.Value);
        equip.RebuildExp++;

        //销毁
        me.IsDelete = true;
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent() { });

        GameProcessor.Inst.UpdateInfo();

        this.Refresh();
    }


}

