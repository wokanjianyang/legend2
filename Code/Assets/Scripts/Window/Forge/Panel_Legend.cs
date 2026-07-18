using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Legend : MonoBehaviour
{
    public Transform Tran_Item_List;
    private Box_Forge[] items;

    public ScrollRect Sr_Bag;

    public List<Text> Txt_Fee_List;

    public Button Btn_Ok;

    public Transform Tf_Fee;
    public Text Txt_Info;

    private int SelectMainIndex = 1;
    private int SelectBagIndex = -1;

    private Item CurrentItem;

    private int ForgeType = 4;

    private List<Box_Forge_Bag> bagList = new List<Box_Forge_Bag>();

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
        for (var i = 0; i < 20; i++)
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


        if (this.CurrentItem == null)
        {
            Txt_Info.text = "此部位没有装备";
            return;
        }
        else
        {
            Equip equip = this.CurrentItem as Equip;

            if (equip.Config.Cycle >= 10)
            {
                Txt_Info.text = "此部位没有装备";
                return;
            }
            else if (equip.LegendData.Key > 0)
            {
                Txt_Info.text = "此装备已经继承过了";
                return;
            }
            else
            {
                Tf_Fee.gameObject.SetActive(false);
                Txt_Info.text = "请选择材料";
            }
        }


        this.Select_Main();
    }

    private void Select_Main()
    {
        User user = User_Data_Manager.Data;

        Equip equip = this.CurrentItem as Equip;

        int part = equip.Config.Part;

        var equips = user.Bags.Where(m => m.Item.GetItemType() == ItemType.Equip && m.Item.ConfigId >= 201001 && m.Item.ConfigId <= 209999).ToList();

        List<Equip> bags = new List<Equip>();
        foreach (var item in equips)
        {
            Equip bi = item.Item as Equip;
            if (bi.Config.Cycle == 10 && bi.Config.Part == part)
            {
                bags.Add(bi);
            }
        }

        ToggleGroup tgBag = Sr_Bag.GetComponent<ToggleGroup>();

        int BoxId = 0;
        for (int i = 0; i < 20; i++)
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
                box.Init(1, BoxId, tgBag);
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

        Tf_Fee.gameObject.SetActive(false);
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

            if (equip.Config.Cycle == 10)
            {
                Tf_Fee.gameObject.SetActive(false);
                Txt_Info.text = "传奇装备不可以接受传承";
            }
            else if (equip.LegendData.Key > 0)
            {
                Tf_Fee.gameObject.SetActive(false);
                Txt_Info.text = "此装备已经继承过了";
            }
            else
            {
                Tf_Fee.gameObject.SetActive(true);

                Equip me = bagList[SelectBagIndex].CurrentItem as Equip;

                EquipLegendConfig legendConfig = EquipLegendConfigCategory.Instance.Get(me.LegendData.Key);

                Btn_Ok.gameObject.SetActive(true);

                Txt_Info.text = string.Format("{0}：资质{1}", legendConfig.Name, me.LegendData.Value);

                string color = user.MagicGold.Data >= legendConfig.Fee ? "#11FF11" : "#FF1111";
                Txt_Fee_List[0].text = string.Format("所需金币：<color={0}>{1}</color>", color, legendConfig.Fee);

                long mc = user.GetMaterialCount(ItemHelper.Equip_Legend);
                color = mc >= legendConfig.Mc ? "#11FF11" : "#FF1111";
                Txt_Fee_List[1].text = string.Format("传奇精华：<color={0}>{1}</color>/{2}", color, mc, legendConfig.Mc);

                if (user.MagicGold.Data < legendConfig.Fee || mc < legendConfig.Mc)
                {
                    Btn_Ok.gameObject.SetActive(false);
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

        EquipLegendConfig legendConfig = EquipLegendConfigCategory.Instance.Get(me.LegendData.Key);

        user.SubGold(legendConfig.Fee);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.Equip_Legend,
            Quantity = legendConfig.Mc
        });

        int lgId = me.LegendData.Key;
        int lgFliar = me.LegendData.Value;

        //销毁
        me.IsDelete = true;
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent() { });

        equip.ToLegend(lgId, lgFliar);

        GameProcessor.Inst.UpdateInfo();

        this.Refresh();
    }


}

