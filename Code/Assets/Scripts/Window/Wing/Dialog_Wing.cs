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
    public Text txt_Fee;
    public Text txt_Level;

    public Button Btn_Full;
    public Button Btn_Strong;

    public Transform tf_attr;
    private List<Forge_Atr_Item> AtrList;

    public Transform tf_spe;
    private List<Item_Atr_Spe> AtrSpeList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        AtrList = tf_attr.GetComponentsInChildren<Forge_Atr_Item>(true).ToList();
        AtrSpeList = tf_spe.GetComponentsInChildren<Item_Atr_Spe>(true).ToList();

        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Strong.onClick.AddListener(OnStrong);

        Show();
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

    private long GetFee(long level)
    {
        return 5 * level;
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;
        long currentLevel = user.WingData.Data;
        long nextLevel = currentLevel + 1;
        //Debug.Log("currentLevel show:" + currentLevel);

        long MaxLevel = 60;

        this.txt_Level.text = "等级:" + currentLevel;

        if (currentLevel >= MaxLevel)
        {
            this.Btn_Strong.gameObject.SetActive(false);
            this.txt_Fee.text = "已满级";
        }
        else
        {
            //Fee
            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);
            long fee = this.GetFee(nextLevel);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            txt_Fee.gameObject.SetActive(true);
            txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "需要:" + fee + " 凤凰之羽");

        }

        List<WingConfig> list = WingConfigCategory.Instance.GetAllByType(0);

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
                    long attrBase = config.GetAttr(currentLevel);
                    long rise = MathHelper.GetRiseByType(config.RiseType, nextLevel, config.AtrVue);
                    attrItem.SetContent(config.AtrId, attrBase, rise);
                }
                else
                {
                    attrItem.gameObject.SetActive(false);
                }
            }
        }

        List<WingConfig> slist = WingConfigCategory.Instance.GetAllByType(1);
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
                item.SetContent(config.AtrId, config.AtrVue, config.RequireLevel, currentLevel);
                item.gameObject.SetActive(true);
            }
        }
    }

    public void OnStrong()
    {
        User user = User_Data_Manager.Data;

        long currentLevel = user.WingData.Data;
        long nextLevel = currentLevel + 1;

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);

        long fee = this.GetFee(nextLevel);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.WingData.Data = nextLevel;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Wing_Stone,
            Quantity = fee
        });

        Show();

        GameProcessor.Inst.UpdateInfo();

        GameProcessor.Inst.SaveData();
        //Debug.Log("OnStrong :" + user.WingData.Data);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
