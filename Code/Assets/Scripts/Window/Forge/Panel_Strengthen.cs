using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Strengthen : MonoBehaviour
{
    public Transform Tran_Item_List;
    private ItemForge[] items;

    public Transform Tf_Atr_List;
    private StrenthAttrItem[] AtrList;

    public Transform Tf_Atr_Spe_List;
    private StrenthAttrItem[] AtrSpeList;

    public Text Txt_Fee;
    public Button Btn_Strengthen;

    private int SelectPosition = 1;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();

        AtrList = Tf_Atr_List.GetComponentsInChildren<StrenthAttrItem>();
        AtrSpeList = Tf_Atr_Spe_List.GetComponentsInChildren<StrenthAttrItem>();

        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);

        this.Init();
        this.ShowStrengthInfo();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            long level = user.GetStrengthLevel(position);

            items[i].Init(1, position, level, toggleGroup);
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

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        this.SelectPosition = e.Position;
        this.ShowStrengthInfo();
    }


    private void ShowStrengthInfo()
    {
        //Log.Debug("ShowStrengthInfo");

        User user = GameProcessor.Inst.User;
        long MaxLevel = Math.Min(EquipStrengthFeeConfigCategory.Instance.GetMaxLevel(), user.MagicLevel.Data);
        long currentLevel = user.GetStrengthLevel(SelectPosition);

        items[SelectPosition - 1].SetLevel(currentLevel);

        long nextLevel = currentLevel + 1;

        EquipStrengthConfig config = EquipStrengthConfigCategory.Instance.GetByPositioin(SelectPosition);

        if (currentLevel >= MaxLevel)
        {
            Txt_Fee.text = "已满级";
            Btn_Strengthen.gameObject.SetActive(false);
        }
        else
        {
            long fee = EquipStrengthFeeConfigCategory.Instance.GetFee(nextLevel) * config.FeeBase;
            string color = user.MagicGold.Data >= fee ? "#FFFF00" : "#FF0000";

            string feeText = fee > 1000000 ? StringHelper.FormatNumber(fee) : fee + "";
            Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, feeText);

            Btn_Strengthen.gameObject.SetActive(true);
        }

        for (int i = 0; i < AtrList.Length; i++)
        {
            if (i < config.AtrList.Length && currentLevel >= config.RequireLevel[i])
            {
                int attrId = config.AtrList[i];

                long atrRise = config.AtrVueList[i];
                long attrCurrent = config.AtrVueList[i] * currentLevel;

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

                AtrSpeList[i].SetSpContent(attrId, atrVue, rv);
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
        User user = GameProcessor.Inst.User;

        long nextLevel = 1;

        if (user.MagicEquipStrength.TryGetValue(SelectPosition, out MagicData strengthData))
        {
            nextLevel = strengthData.Data + 1;
        }

        EquipStrengthFeeConfig config = EquipStrengthFeeConfigCategory.Instance.GetByLevel(nextLevel);

        long levelAttr = LevelConfigCategory.GetLevelAttr(nextLevel);
        double fee = levelAttr * config.Fee;

        if (user.MagicGold.Data < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.MagicEquipStrength[SelectPosition].Data++;

        user.SubGold(fee);

        GameProcessor.Inst.UpdateInfo();

        ShowStrengthInfo();

        //TaskHelper.CheckTask(TaskType.Strength, 1);

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "强化成功", ToastType = ToastTypeEnum.Success });
    }


}

