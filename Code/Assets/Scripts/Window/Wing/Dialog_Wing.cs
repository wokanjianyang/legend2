using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Wing : MonoBehaviour, IBattleLife
{
    public Text txt_Level;
    public Text txt_Fee;

    public Button Btn_Full;
    public Button Btn_Strong;

    public Transform tf_attr;
    private List<Forge_Atr_Item> AtrList;

    public Transform tf_spe;
    private List<Item_Atr_Spe> AtrSpeList;

    public Transform tf_list;
    private List<Wing_Item> item_list;

    public int Order => (int)ComponentOrder.Dialog;

    private int Part = 1;

    // Start is called before the first frame update
    void Start()
    {
        AtrList = tf_attr.GetComponentsInChildren<Forge_Atr_Item>(true).ToList();
        AtrSpeList = tf_spe.GetComponentsInChildren<Item_Atr_Spe>(true).ToList();
        item_list = tf_list.GetComponentsInChildren<Wing_Item>(true).ToList();

        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Strong.onClick.AddListener(OnStrong);

        this.Init();
        Show();
    }

    private void Init()
    {
        ToggleGroup toggleGroup = tf_list.GetComponent<ToggleGroup>();
        for (int i = 1; i <= item_list.Count; i++)
        {
            item_list[i - 1].Init(i, toggleGroup);
        }
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDialogEvent>(this.Open);
    }

    private void Open(OpenDialogEvent e)
    {
        if (e.Type == DialogType.Wing)
        {
            this.gameObject.SetActive(true);
        }
    }

    private long GetFee(int p, long level)
    {
        if (p > 1)
        {
            return (level - 1) / 30 + 1;
        }
        else
        {
            long rate = (level - 1) / 10 + 1;
            return 5 * level * rate;
        }
    }

    public void SelectItem(int p)
    {
        this.Part = p;
        this.Show();
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;
        long currentLevel = user.GetWingLevel(this.Part);
        int exLevel = (int)user.AttributeBonus.CalPanelAtr(AttributeEnum.WingLevel);
        int exPercent = (int)user.AttributeBonus.CalPanelAtr(AttributeEnum.WingIncrea);

        long nextLevel = currentLevel + 1;

        long MaxLevel = user.MagicLevel.Data - 30;

        if (exLevel > 0)
        {
            this.txt_Level.text = string.Format("等级（Lv{0}+{1}）", currentLevel, exLevel);
        }
        else
        {
            this.txt_Level.text = string.Format("等级（Lv{0}）", currentLevel);
        }

        if (currentLevel >= MaxLevel)
        {
            this.Btn_Strong.gameObject.SetActive(false);
            this.txt_Fee.text = "已满级";
        }
        else
        {
            //Fee
            int mcId = GetMcId();
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(mcId);

            long materialCount = user.GetMaterialCount(mcId);
            long fee = this.GetFee(this.Part, nextLevel);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            if (materialCount >= fee)
            {
                this.Btn_Strong.gameObject.SetActive(true);
            }

            txt_Fee.gameObject.SetActive(true);
            txt_Fee.text = string.Format("{3}：<color={0}>{1}/{2}</color>", color, fee, materialCount, itemConfig.Name);

        }

        List<WingConfig> list = WingConfigCategory.Instance.GetAllByPartAndType(this.Part, 1);

        for (int i = 0; i < AtrList.Count; i++)
        {
            Forge_Atr_Item attrItem = AtrList[i];

            if (i >= list.Count)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                WingConfig config = list[i];

                if (nextLevel >= config.RequireLevel)
                {
                    attrItem.gameObject.SetActive(true);
                    double attrBase = config.GetAttr(currentLevel + exLevel);
                    double rise = MathHelper.GetRiseByType(config.RiseType, nextLevel, 1) * config.AtrVue;
                    attrItem.SetContent(config.AtrId, attrBase, exPercent, rise);
                }
                else
                {
                    attrItem.gameObject.SetActive(false);
                }
            }
        }

        List<WingConfig> slist = WingConfigCategory.Instance.GetAllByPartAndType(this.Part, 2);
        for (int i = 0; i < AtrSpeList.Count; i++)
        {
            Item_Atr_Spe item = AtrSpeList[i];
            if (i >= list.Count)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                WingConfig config = slist[i];
                item.SetContent(config.AtrId, config.AtrVue, config.RequireLevel, currentLevel + exLevel);
                item.gameObject.SetActive(true);
            }
        }
    }

    public void OnStrong()
    {
        User user = User_Data_Manager.Data;

        long currentLevel = user.GetWingLevel(this.Part);
        long nextLevel = currentLevel + 1;

        int mcId = GetMcId();
        long materialCount = user.GetMaterialCount(mcId);

        long fee = this.GetFee(this.Part, nextLevel);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SaveWingLevel(this.Part);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = mcId,
            Quantity = fee
        });

        item_list[this.Part - 1].Refresh();

        Show();

        GameProcessor.Inst.UpdateInfo();
        //Debug.Log("OnStrong :" + user.WingData.Data);
    }

    private int GetMcId()
    {
        return ItemHelper.SpecialId_Wing_Stone + this.Part - 1;
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
