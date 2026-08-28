using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_SoulRing : MonoBehaviour
{
    public Transform Tf_Item_List;
    private List<Item_SoulRing> items;

    public Transform Tf_Atr_List;
    private List<Forge_Atr_Item> AtrList;

    public Transform Tf_Atr_Spe;
    private List<Forge_Atr_Item> SpeAtrList;

    public Text Txt_Desc;
    public Text Txt_Fee;

    public Button Btn_Ok;

    private SoulRingConfig CurrentConfig = null;

    int maxLevel = 10;


    private void Awake()
    {
        items = Tf_Item_List.GetComponentsInChildren<Item_SoulRing>().ToList();
        AtrList = Tf_Atr_List.GetComponentsInChildren<Forge_Atr_Item>().ToList();
        SpeAtrList = Tf_Atr_Spe.GetComponentsInChildren<Forge_Atr_Item>().ToList();

        Btn_Ok.onClick.AddListener(OnClick_Ok);

        this.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            items[i].toggle.onValueChanged.AddListener((isOn) =>
            {
                ShowItem(item);
            });
        }

        this.ShowItem(items[0]);
    }

    private void Init()
    {
        ToggleGroup toggleGroup = Tf_Item_List.GetComponent<ToggleGroup>();

        List<SoulRingConfig> configs = SoulRingConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            SoulRingConfig config = configs[i];
            Item_SoulRing box = items[i];

            box.Init(toggleGroup, config);
        }
    }

    private void ShowItem(Item_SoulRing currentItem)
    {
        User user = User_Data_Manager.Data;

        SoulRingConfig config = currentItem.Config;
        this.CurrentConfig = config;

        long currentLevel = user.GetSoulRingLevel(config.Id);

        long maxRingLevel = user.GetSoulRingLimit();

        currentItem.SetContent(currentLevel);

        //attr
        for (int i = 0; i < AtrList.Count; i++)
        {
            if (i < config.AtrIdList.Length)
            {
                AtrList[i].gameObject.SetActive(true);
                AtrList[i].SetContent(config.AtrIdList[i], config.AtrVueList[i] * currentLevel, config.AtrVueList[i]);
            }
            else
            {
                AtrList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < SpeAtrList.Count; i++)
        {
            if (i < config.SpeIdList.Length)
            {
                int attrId = config.SpeIdList[i];
                long atrVue = config.SpeVueList[i];
                int rv = config.SpeRequireList[i];

                SpeAtrList[i].SetSpContent(attrId, atrVue, rv, currentLevel);
                SpeAtrList[i].gameObject.SetActive(true);
            }
            else
            {
                SpeAtrList[i].gameObject.SetActive(false);
            }
        }

        long total = user.GetBagItemCount(config.ItemId);
        long needNumber = GetNeedNumber(currentLevel);

        string color = total >= needNumber ? "#FFFF00" : "#FF0000";

        if (currentLevel < maxRingLevel)
        {
            Txt_Fee.text = string.Format("消耗{4}<color={0}>{1}</color> /{2} (满级：{3})", color, total, needNumber, maxRingLevel, config.Name);
        }
        else
        {
            Txt_Fee.text = "已满级";
        }

        if (total >= needNumber && currentLevel < maxRingLevel)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private long GetNeedNumber(long level)
    {
        return level / 30 + 1;
    }

    public void OnClick_Ok()
    {
        Item_SoulRing currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        SoulRingConfig config = currentItem.Config;

        User user = User_Data_Manager.Data;
        long currentLevel = user.GetSoulRingLevel(config.Id);

        long total = user.GetBagItemCount(config.ItemId);
        long needCount = GetNeedNumber(currentLevel);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "魂环数量不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = config.ItemId,
            Quantity = needCount
        });
        user.AddSoulRingLevel(config.Sid);

        this.ShowItem(currentItem);

        GameProcessor.Inst.UpdateInfo();
    }
}
