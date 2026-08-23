using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Refine_New : MonoBehaviour
{
    public Transform Tran_Item_List;
    private ItemForge[] items;

    public Transform Tf_Atr_List;
    private Forge_Atr_Item[] AtrList;

    public Transform Tf_Atr_Spe_List;
    private Forge_Atr_Item[] AtrSpeList;

    public Text Txt_Fee1;
    public Text Txt_Fee2;

    public Button Btn_Strengthen;

    private int SelectPosition = 1;
    private int ForgeType = 2;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();

        AtrList = Tf_Atr_List.GetComponentsInChildren<Forge_Atr_Item>();
        AtrSpeList = Tf_Atr_Spe_List.GetComponentsInChildren<Forge_Atr_Item>();

        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
    }

    // Update is called once per frame
    void Start()
    {
        //GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);

        this.Init();
        this.ShowInfo();
    }

    private void Init()
    {
        User user = User_Data_Manager.Data;

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            long level = user.GetRefineLevel(position);

            items[i].Init(ForgeType, position, level, toggleGroup);
        }

        foreach (var sp in AtrList)
        {
            sp.gameObject.SetActive(false);
        }

        foreach (var sp in AtrSpeList)
        {
            sp.gameObject.SetActive(false);
        }
    }

    public void SelectItem(int p)
    {
        this.SelectPosition = p;
        this.ShowInfo();
    }


    private void ShowInfo()
    {
        //Log.Debug("ShowStrengthInfo");

        User user = User_Data_Manager.Data;

        long MaxLevel = Math.Min(EquipRefineFeeNewConfigCategory.Instance.GetMaxLevel(), user.GetRefineLimit());

        long currentLevel = user.GetRefineLevel(SelectPosition);

        items[SelectPosition - 1].SetLevel(currentLevel);

        int nextLevel = (int)currentLevel + 1;

        if (currentLevel >= MaxLevel)
        {
            Txt_Fee1.text = "已满级";
            Txt_Fee2.text = "已满级";
            Btn_Strengthen.gameObject.SetActive(false);
        }
        else
        {
            EquipRefineFeeNewConfig feeConfig = EquipRefineFeeNewConfigCategory.Instance.Get(nextLevel);
            int feeBase = SelectPosition <= 2 ? 2 : 1;

            long fee1 = feeConfig.Fee1 * feeBase;
            string color = user.MagicGold.Data >= fee1 ? "#11FF11" : "#FF1111";

            string feeText = fee1 > 1000000 ? StringHelper.FormatNumber(fee1) : fee1 + "";
            Txt_Fee1.text = string.Format("金币：<color={0}>{1}</color>", color, feeText);


            long fee2 = feeConfig.Fee2 * feeBase;
            long mc = user.GetMaterialCount(ItemHelper.Equip_Refine);
            color = mc >= fee2 ? "#11FF11" : "#FF1111";
            Txt_Fee2.text = string.Format("黑铁矿：<color={0}>{1}</color>/{2}", color, mc, fee2);

            if (user.MagicGold.Data >= fee1 && mc >= fee2)
            {
                Btn_Strengthen.gameObject.SetActive(true);
            }
            else
            {
                Btn_Strengthen.gameObject.SetActive(false);
            }
        }

        EquipRefineConfig config = EquipRefineConfigCategory.Instance.GetByPart(SelectPosition);

        for (int i = 0; i < AtrList.Length; i++)
        {
            if (i < config.AtrList.Length && currentLevel >= config.RequireLevel[i])
            {
                int attrId = config.AtrList[i];

                long atrRise = config.AtrVueList[i];
                long attrCurrent = config.GetAtrVue(i, currentLevel);

                AtrList[i].SetContent(attrId, attrCurrent, atrRise);
                AtrList[i].gameObject.SetActive(true);
            }
            else
            {
                AtrList[i].gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < AtrSpeList.Length; i++)
        {
            if (i < config.SpeAtrList.Length)
            {
                int attrId = config.SpeAtrList[i];
                long atrVue = config.SpeVueList[i];
                int rv = config.SpeLevel[i];

                AtrSpeList[i].SetSpContent(attrId, atrVue, rv, currentLevel);
                AtrSpeList[i].gameObject.SetActive(true);
            }
            else
            {
                AtrSpeList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Strengthen()
    {
        User user = User_Data_Manager.Data;

        int nextLevel = (int)user.GetRefineLevel(SelectPosition) + 1;

        EquipRefineFeeNewConfig feeConfig = EquipRefineFeeNewConfigCategory.Instance.Get(nextLevel);

        int feeBase = SelectPosition <= 2 ? 2 : 1;
        long fee1 = feeConfig.Fee1 * feeBase;

        if (user.MagicGold.Data < fee1)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long fee2 = feeConfig.Fee2 * feeBase;
        long mc = user.GetMaterialCount(ItemHelper.Equip_Refine);

        if (mc < fee2)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的黑铁矿", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(fee1);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.Equip_Refine,
            Quantity = fee2
        });


        user.SaveRefineLevel(SelectPosition, 1);

        GameProcessor.Inst.UpdateInfo();

        ShowInfo();
    }


}

