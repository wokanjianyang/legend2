using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Babel : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public Text Txt_Count;
    public Text Txt_Progress;
    public Text Txt_Reward;

    public Text Txt_Rise;

    public Button Btn_Rank;

    public Button Btn_Atr;
    public Dialog_Babel_Atr Dlg_Babel_Atr;

    public Toggle toggle_Auto;
    public Dialog_Babel_Rank Dlg_Babel_Rank;

    public Button Btn_Start;
    public Button Btn_Close;

    private bool IsNet = false;

    // Start is called before the first frame update
    void Start()
    {
        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.Babel_Auto = isOn;
        });

        Btn_Atr.onClick.AddListener(OnClick_Atr);
        Btn_Rank.onClick.AddListener(OnClick_Rank);

        Btn_Start.onClick.AddListener(OnClick_Start);
        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    void OnEnable()
    {
        this.Show();
    }

    private void Show()
    {
        User user = User_Data_Manager.Data;
        user.BabelData.Check();

        long progress = user.BabelData.Progress.Data;

        if (user.Account != "" && ConfigHelper.Channel != ConfigHelper.Channel_Tap)
        {
            IsNet = true;
            Txt_Rise.gameObject.SetActive(true);
            Btn_Rank.gameObject.SetActive(true);
        }
        else
        {
            Txt_Rise.gameObject.SetActive(false);
            Btn_Rank.gameObject.SetActive(false);
        }

        long nextProgress = progress + 1;

        Txt_Progress.text = "当前层数:" + nextProgress + "";
        Txt_Count.text = "今日挑战次数:" + user.BabelData.Count;

        if (nextProgress > ConfigHelper.BabelMax)
        {
            Txt_Reward.text = "已通关，等待开发上限";
        }
        else
        {
            BabelConfig rewardConfig = BabelConfigCategory.Instance.GetByProgress(nextProgress);
            Item item = rewardConfig.BuildItem(nextProgress);
            Txt_Reward.text = "通过奖励:" + item.GetName() + "*" + item.Temp_Number;
        }

        if (IsNet)
        {
            try
            {
                //再存储新档
                StartCoroutine(NetworkHelper.GetRank("Babel",
                        (WebResultWrapper result) =>
                        {
                            if (result.Code == StatusMessage.OK)
                            {
                                List<BabelRank> list = result.List.ToObject<List<BabelRank>>();
                                Dlg_Babel_Rank.Init(list);

                                if (list.Count > 0)
                                {
                                    AppHelper.BabelMaxRecord = list[0].Rank;
                                    AppHelper.BabelMinRecord = list[^1].Rank;

                                    long total = AppHelper.BabelMaxRecord - User_Data_Manager.Data.BabelData.Progress.Data - 1;
                                    total = total > 0 ? total * 2 : 0;

                                    this.Txt_Rise.text = "最高纪录 " + list[0].Name + " " + list[0].Rank + "层" + " (怪物额外承伤" + total + "%)";
                                }
                                else
                                {

                                }
                                //AppHelper.BabelRecord = int.Parse(rank);
                            }
                            else
                            {
                                this.Txt_Rise.text = "读取失败.";
                            }
                        },
                        () =>
                        {
                            this.Txt_Rise.text = "读取失败.";
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.Txt_Rise.text = "读取失败，请稍等一会重试...";
            }
        }
    }


    public void OnClick_Start()
    {
        this.gameObject.SetActive(false);

        User user = User_Data_Manager.Data;

        if (user.BabelData.Count <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战次数不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (user.BabelData.Progress.Data >= ConfigHelper.BabelMax)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "你已经通关了，请等待开放上限", ToastType = ToastTypeEnum.Failure });
            return;
        }

        //this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

        GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Babel, MapId = 0 });
    }

    public void OnClick_Atr()
    {
        this.Dlg_Babel_Atr.gameObject.SetActive(true);
    }

    public void OnClick_Rank()
    {
        this.Dlg_Babel_Rank.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
