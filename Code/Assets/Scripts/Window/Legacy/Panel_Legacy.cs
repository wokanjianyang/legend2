using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Legacy : MonoBehaviour
{
    public Transform Tran_Item_List;
    private Box_Legacy[] items;

    public Transform Tf_Set_List;
    private Forge_Atr_Item[] SetList;

    public Transform Tf_Atr_List_Base;
    private Forge_Atr_Item[] AtrListBase;

    public Transform Tf_Atr_Spe_List_Base;
    private Forge_Atr_Item[] AtrSpeListBase;

    public Transform Tf_Atr_List_Level;
    private Forge_Atr_Item[] AtrListLevel;

    public Transform Tf_Atr_Spe_List_Level;
    private Forge_Atr_Item[] AtrSpeListLevel;

    public Text Txt_Fee1;
    public Text Txt_Fee2;

    public Button Btn_Ok;

    private int Role = 1;
    private int SelectPosition = 1;
    private int ForgeType = 4;

    // Start is called before the first frame update
    void Awake()
    {
        SetList = Tf_Set_List.GetComponentsInChildren<Forge_Atr_Item>();

        AtrListBase = Tf_Atr_List_Base.GetComponentsInChildren<Forge_Atr_Item>();
        AtrSpeListBase = Tf_Atr_Spe_List_Base.GetComponentsInChildren<Forge_Atr_Item>();

        AtrListLevel = Tf_Atr_List_Level.GetComponentsInChildren<Forge_Atr_Item>();
        AtrSpeListLevel = Tf_Atr_Spe_List_Level.GetComponentsInChildren<Forge_Atr_Item>();

        items = Tran_Item_List.GetComponentsInChildren<Box_Legacy>();
        Btn_Ok.onClick.AddListener(OnClick_OK);
    }

    // Update is called once per frame
    void Start()
    {
        this.ChangeRole(1);
    }

    private void Init()
    {
        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            items[i].Init(Role, position, toggleGroup);
        }
    }

    public void ChangeRole(int role)
    {
        this.Role = role;

        this.Init();
        this.Show();
    }

    public void SelectItem(int p)
    {
        this.SelectPosition = p;
        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        int part = (Role - 1) * 8 + SelectPosition;

        long MaxLevel = user.GetLegacyLayer(part);
        long currentLevel = user.GetLegacyLevel(part);

        items[SelectPosition - 1].Refresh();

        long nextLevel = currentLevel + 1;

        LegacyConfig config = LegacyConfigCategory.Instance.GetByPosition(Role, SelectPosition);

        if (currentLevel >= MaxLevel)
        {
            Txt_Fee1.text = "已满级";
            Txt_Fee2.text = "已满级";
            Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            long fee1 = config.GetFee1(nextLevel);
            string color = user.MagicGold.Data >= fee1 ? "#11FF11" : "#FF1111";

            string feeText = fee1 > 1000000 ? StringHelper.FormatNumber(fee1) : fee1 + "";
            Txt_Fee1.text = string.Format("金币：<color={0}>{1}</color>", color, feeText);


            long fee2 = config.GetFee2(nextLevel);
            long mc = user.GetMaterialCount(ItemHelper.Legacy_Stone);
            color = mc >= fee2 ? "#11FF11" : "#FF1111";
            Txt_Fee2.text = string.Format("传世精华：<color={0}>{1}</color>/{2}", color, mc, fee2);

            if (user.MagicGold.Data >= fee1 && mc >= fee2)
            {
                Btn_Ok.gameObject.SetActive(true);
            }
            else
            {
                Btn_Ok.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < AtrListBase.Length; i++)
        {
            if (i < config.AtrIdList.Length)
            {
                int attrId = config.AtrIdList[i];

                long atrRise = config.AtrVueList[i];
                long attrCurrent = config.AtrVueList[i] * currentLevel;

                AtrListBase[i].SetContent(attrId, attrCurrent, atrRise);
                AtrListBase[i].gameObject.SetActive(true);
            }
            else
            {
                AtrListBase[i].gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < AtrSpeListBase.Length; i++)
        {
            if (i < config.SpeAtrList.Length)
            {
                int attrId = config.SpeAtrList[i];
                long atrVue = config.SpeVueList[i];
                int rv = config.SpeLevel[i];

                AtrSpeListBase[i].SetSpContent(attrId, atrVue, rv);
                AtrSpeListBase[i].gameObject.SetActive(true);
            }
            else
            {
                AtrSpeListBase[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < AtrListLevel.Length; i++)
        {
            if (i < config.LevelIdList.Length)
            {
                int attrId = config.LevelIdList[i];

                long atrRise = config.LevelValueList[i];
                long attrCurrent = config.LevelValueList[i] * currentLevel;

                AtrListLevel[i].SetContent(attrId, attrCurrent, atrRise);
                AtrListLevel[i].gameObject.SetActive(true);
            }
            else
            {
                AtrListLevel[i].gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < AtrSpeListLevel.Length; i++)
        {
            if (i < config.SpeLevel.Length)
            {
                int attrId = config.SpeAtrList[i];
                long atrVue = config.SpeVueList[i];
                int rv = config.SpeLevel[i];

                AtrSpeListLevel[i].SetSpContent(attrId, atrVue, rv);
                AtrSpeListLevel[i].gameObject.SetActive(true);
            }
            else
            {
                AtrSpeListLevel[i].gameObject.SetActive(false);
            }
        }

        LegacySetConfig setConfig = LegacySetConfigCategory.Instance.GetByRole(this.Role);
        for (int i = 0; i < SetList.Length; i++)
        {
            if (i < setConfig.AtrIdList.Length)
            {
                int attrId = setConfig.AtrIdList[i];

                long atrRise = setConfig.AtrVueList[i];
                long attrCurrent = setConfig.AtrVueList[i] * currentLevel;

                SetList[i].SetContent(attrId, attrCurrent, atrRise);
                SetList[i].gameObject.SetActive(true);
            }
            else
            {
                SetList[i].gameObject.SetActive(false);
            }

        }
    }

    private void OnClick_OK()
    {
        User user = GameProcessor.Inst.User;

        int part = (Role - 1) * 8 + SelectPosition;
        long currentLevel = user.GetLegacyLevel(part);
        long nextLevel = currentLevel + 1;

        LegacyConfig config = LegacyConfigCategory.Instance.GetByPosition(Role, SelectPosition);

        long fee1 = config.GetFee1(nextLevel);

        if (user.MagicGold.Data < fee1)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long fee2 = config.GetFee2(nextLevel);
        long mc = user.GetMaterialCount(ItemHelper.Equip_Refine);

        if (mc < fee2)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的传世精华", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(fee1);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.Legacy_Stone,
            Quantity = fee2
        });


        user.SaveLegacyLevel(part, 1);

        GameProcessor.Inst.UpdateInfo();

        Show();
    }


}

