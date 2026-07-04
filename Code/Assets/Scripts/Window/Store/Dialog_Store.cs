using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Store : MonoBehaviour, IBattleLife
{
    public Button Btn_Close;

    public Toggle toggle_Lottery;
    public Panel_Lottery Pnl_Lottery;

    public Toggle toggle_Store;
    public Panel_Store Pnl_Store;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);

        this.toggle_Lottery.onValueChanged.AddListener((isOn) =>
        {
            Pnl_Lottery.gameObject.SetActive(isOn);
        });

        this.toggle_Store.onValueChanged.AddListener((isOn) =>
        {
            Pnl_Store.gameObject.SetActive(isOn);
        });
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDialogEvent>(this.Open);
    }

    private void Open(OpenDialogEvent e)
    {
        if (e.Type == DialogType.Store)
        {
            this.gameObject.SetActive(true);

            //再加载net数据
            try
            {
                if (User_Data_Manager.Data.Account != "")
                {
                    StartCoroutine(NetworkHelper.GetStore(
                        (WebResultWrapper result) =>
                        {
                            if (result.Code == StatusMessage.OK)
                            {
                                User_Data_Manager.StoreData = result.List.ToObject<Store_Data>();
                                Pnl_Lottery.Refresh();
                            }
                            else
                            {
                                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获取数据失败", ToastType = ToastTypeEnum.Failure });
                            }

                        },
                         () =>
                         {
                             GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获取数据失败", ToastType = ToastTypeEnum.Failure });
                         }));
                }
            }
            catch (Exception ex)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获取数据失败", ToastType = ToastTypeEnum.Failure });
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
