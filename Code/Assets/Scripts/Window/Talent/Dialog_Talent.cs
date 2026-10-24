using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Talent : MonoBehaviour, IBattleLife
{
    public Dialog_Talent_Detail DialogDetail;

    public Text Txt_Total;
    public Text Txt_Enable;

    public Transform Tf_Layer;
    private List<Toggle> tgLevelList;

    public List<Transform> tfs;
    private List<Item_Talent> ItemList = new List<Item_Talent>();

    public Button Btn_Close;
    public Button Btn_Reset;

    private int SelectLayer = 0;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        tgLevelList = Tf_Layer.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Reset.onClick.AddListener(OnClick_Reset);

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDialogEvent>(this.Open);
        GameProcessor.Inst.EventCenter.AddListener<TalentDetailShowEvent>(this.OnShowDetailEvent);
    }

    private void Open(OpenDialogEvent e)
    {
        if (e.Type == DialogType.Talent)
        {
            this.gameObject.SetActive(true);
            this.Refresh();
        }
    }

    private void Start()
    {
        this.ChangeLevel(0);

        this.Show();
    }


    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            Transform tf = tfs[i];
            if (i == SelectLayer)
            {
                tf.gameObject.SetActive(true);
                ItemList = tf.GetComponentsInChildren<Item_Talent>().ToList();
            }
            else
            {
                tf.gameObject.SetActive(false);
            }
        }

        this.Show();
    }

    private void OnShowDetailEvent(TalentDetailShowEvent e)
    {
        DialogDetail.Open(e.Tid);
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;

        long total = user.TalentTotal.Data;
        long used = user.TalentUsed;
        long enabled = total - used;

        Txt_Total.text = "总天赋点：" + total;
        Txt_Enable.text = "剩余天赋点：" + enabled;

        int startId = SelectLayer * 100;
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetContent(i + 1 + startId);
        }
    }

    public void Refresh()
    {
        User user = User_Data_Manager.Data;

        long total = user.TalentTotal.Data;
        long used = user.TalentUsed;
        long enabled = total - used;

        Txt_Total.text = "总天赋点：" + total;
        Txt_Enable.text = "剩余天赋点：" + enabled;

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].Show();
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Reset()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重置天赋消耗1亿金币。是否确认？", true,
         () =>
         {
             ResetTalent();
         }, () =>
         {

         });
    }

    private void ResetTalent()
    {
        User user = User_Data_Manager.Data;

        if (user.MagicGold.Data <= 1E8)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足1亿", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(1E8);

        user.TalentData.Clear();
        user.TalentUsed = 0;

        GameProcessor.Inst.UpdateInfo();
        this.Refresh();
    }
}
