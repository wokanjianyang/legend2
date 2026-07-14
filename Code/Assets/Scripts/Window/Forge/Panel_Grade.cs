using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Grade : MonoBehaviour
{
    public Transform Tran_Item_List;
    private Box_Forge[] items;

    public Transform Tran_Spe_Item_List;
    private Box_Forge[] speItems;

    public List<Text> Txt_Fee_List;

    public Button Btn_Ok;

    public Transform Tf_Fee;
    public Text Txt_Info;

    private int SelectPosition = 1;
    private Item CurrentItem;
    private Box_Forge CurrentBox;

    private int ForgeType = 3;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<Box_Forge>();
        speItems = Tran_Spe_Item_List.GetComponentsInChildren<Box_Forge>();

        Btn_Ok.onClick.AddListener(OnClick_OK);
    }

    // Update is called once per frame

    private void OnEnable()
    {
        this.Init();
        this.Show();
    }

    private void Init()
    {
        User user = User_Data_Manager.Data;
        if (user == null)
        {
            return;
        }

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;

            items[i].Init(ForgeType, position, toggleGroup);
            items[i].SetItem(user.GetEquip(position));
        }

        for (int i = 0; i < speItems.Count(); i++)
        {
            int position = i + 1001;

            speItems[i].Init(ForgeType, position, toggleGroup);
            speItems[i].SetItem(user.GetEquip(position));
        }
    }

    public void SelectItem(int p, Item item, Box_Forge box)
    {
        this.SelectPosition = p;
        this.CurrentItem = item;
        this.CurrentBox = box;

        this.Show();
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;

        if (CurrentItem == null)
        {
            Tf_Fee.gameObject.SetActive(false);
            Txt_Info.text = "此部位没有装备";
        }
        else if (SelectPosition >= 1001 && SelectPosition <= 1004)//四格
        {
            Tf_Fee.gameObject.SetActive(true);

            int nextLayer = CurrentItem.Layer + 1;

            EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetConfig(SelectPosition, nextLayer);

            int maxLevel = (int)(Math.Max(5, user.MagicLevel.Data / 5));

            if (config == null || nextLayer >= maxLevel)
            {
                foreach (Text Txt_Fee in Txt_Fee_List)
                {
                    Txt_Fee.text = "已满阶";
                }

                Btn_Ok.gameObject.SetActive(false);
            }
            else
            {
                Btn_Ok.gameObject.SetActive(true);

                Txt_Info.text = string.Format("{0}阶---->{1}阶", CurrentItem.Layer, nextLayer);

                for (int i = 0; i < config.MidList.Length; i++)
                {
                    long fee = config.GetFee(i, nextLayer);
                    long mc = user.GetMaterialCount(config.MidList[i]);
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(config.MidList[i]);

                    string color = mc >= fee ? "#11FF11" : "#FF1111";
                    Txt_Fee_List[i].text = string.Format("{3}：<color={0}>{1}</color>/{2}", color, mc, fee, itemConfig.Name);

                    if (mc < fee)
                    {
                        Btn_Ok.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            Tf_Fee.gameObject.SetActive(false);
            Btn_Ok.gameObject.SetActive(false);
            Txt_Info.text = "此装备不能升阶";
        }
    }

    private void OnClick_OK()
    {
        User user = User_Data_Manager.Data;

        int nextLayer = CurrentItem.Layer + 1;

        EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetConfig(SelectPosition, nextLayer);

        for (int i = 0; i < config.MidList.Length; i++)
        {
            long fee = config.GetFee(i, nextLayer);
            long mc = user.GetMaterialCount(config.MidList[i]);

            if (mc < fee)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        for (int i = 0; i < config.MidList.Length; i++)
        {
            long fee = config.GetFee(i, nextLayer);
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = config.MidList[i],
                Quantity = fee
            });
        }

        CurrentItem.Grade();

        CurrentBox.Refresh();

        GameProcessor.Inst.UpdateInfo();

        Show();
    }


}

