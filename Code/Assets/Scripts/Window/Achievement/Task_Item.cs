using Game;
using Game.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task_Item : MonoBehaviour
{

    public Text Txt_Des;
    public Text Txt_Progress;
    public Text Txt_Reward;

    public Text Txt_No;
    public Text Txt_Ok;

    public Button Btn_Active;
    public Button Btn_Accept;

    private AchievementTaskConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnClick_Active);
        Btn_Accept.onClick.AddListener(OnClick_Accept);
    }

    float time = 0;

    void Start()
    {
        this.Show();
    }

    private void Update()
    {
        if (Config != null)
        {
            time += Time.unscaledDeltaTime;
            if (time > 1)
            {
                time = 0;
                this.Show();
            }
        }
    }
    public void SetContent(AchievementTaskConfig config)
    {
        this.Config = config;
        this.Show();
    }

    private void Show()
    {
        Btn_Active.gameObject.SetActive(false);
        Btn_Accept.gameObject.SetActive(false);
        Txt_No.gameObject.SetActive(false);
        Txt_Ok.gameObject.SetActive(false);

        long require = Config.ConRequire;
        Txt_Des.text = string.Format(Config.Desc, require);
        Txt_Reward.text = string.Format(Config.RewardText, StringHelper.FormatNumber(Config.RewardGold), Config.NumberList[0]);


        if (Config.CalId <= 0)
        {
            this.Show0();  //普通任务
        }
        else if (Config.CalId > 0 && Config.CalId <= 10)
        {

            this.Show1();
        }
        else if (Config.CalId == 11)
        {
            //每日任务
            this.Show11();
        }
        else if (Config.CalId == 12)
        {
            //福利任务
            this.Show1();
        }
    }

    private void Show0()
    {
        //指引任务
        User user = User_Data_Manager.Data;
        user.TaskLog.TryGetValue(Config.Id, out bool complete);
        if (complete)
        {
            Txt_Ok.gameObject.SetActive(true);
            return;
        }

        long progress = user.GetAchievementProgeress((AchievementProType)Config.ConType);
        long require = Config.ConRequire;
        string color = progress >= require ? "00FF00" : "FF0000";
        Txt_Progress.text = string.Format("进度：<color=#{0}>{1}</color> /{2}", color, progress, require);


        if (progress >= require)
        {
            Btn_Active.gameObject.SetActive(true);
        }
        else
        {
            Txt_No.gameObject.SetActive(true);
        }
    }

    private void Show1()
    {
        //杀怪任务
        User user = User_Data_Manager.Data;
        user.TaskLog.TryGetValue(Config.Id, out bool complete);
        if (complete)
        {
            Txt_Ok.gameObject.SetActive(true);
            return;
        }

        long progress = user.GetTaskProgress(Config.CalId);
        long require = Config.ConRequire;
        string color = progress >= require ? "00FF00" : "FF0000";
        Txt_Progress.text = string.Format("进度：<color=#{0}>{1}</color> /{2}", color, progress, require);

        if (!user.TaskRecord.ContainsKey(Config.CalId))
        {
            this.Btn_Accept.gameObject.SetActive(true);
        }
        else
        {
            this.Btn_Accept.gameObject.SetActive(false);

            if (progress >= require)
            {
                Btn_Active.gameObject.SetActive(true);
            }
            else
            {
                Txt_No.gameObject.SetActive(true);
            }
        }
    }

    private void Show11()
    {
        //循环任务
        long require = Config.ConRequire;
        Txt_Des.text = string.Format(Config.Desc, require);
        Txt_Reward.text = string.Format(Config.RewardText, StringHelper.FormatNumber(Config.RewardGold), Config.NumberList[0]);

        User user = User_Data_Manager.Data;

        Task_Item_Data data = user.TaskData.GetItem(Config.Id);

        if (data == null)
        {
            return;
        }

        long progress = data.Progress;

        string color = progress >= require ? "00FF00" : "FF0000";
        Txt_Progress.text = string.Format("进度：<color=#{0}>{1}</color> /{2}", color, progress, require);


        //日常任务
        if (data.TaskStatus == 0)  //未接取
        {
            this.Btn_Accept.gameObject.SetActive(true);
        }
        else if (data.TaskStatus == 1)  //进行中
        {
            if (progress >= require)
            {
                Btn_Active.gameObject.SetActive(true);
            }
            else
            {
                Txt_No.gameObject.SetActive(true);
                this.Txt_No.text = "进行中";
            }
        }
        else if (data.TaskStatus == 2)  //已完成
        {
            this.Txt_No.gameObject.SetActive(true);
            this.Txt_No.text = "已完成";
        }
    }

    private void OnClick_Active()
    {
        Btn_Active.gameObject.SetActive(false);
        Txt_Ok.gameObject.SetActive(true);

        if (Config.CalId <= 0)
        {
            this.Ok0();  //普通任务
        }
        else if (Config.CalId == 11)
        {
            //每日任务
            this.Ok11();
        }
        else if (Config.CalId == 12)
        {
            //福利任务
            this.Ok12();
        }



        Dialog_Task dialog = this.GetComponentInParent<Dialog_Task>();

        dialog.SelectItem(this.Config.GroupId);
    }

    private void Ok0()
    {
        //普通任务
        User user = User_Data_Manager.Data;

        if (Config.CalId != 11)  //日常任务，自动刷新
        {
            user.TaskLog.TryGetValue(Config.Id, out bool complete);
            if (complete)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经领过奖励了" });
                return;
            }

            user.TaskLog[Config.Id] = true;
        }


        if (Config.CalId > 0)  //日常任务
        {
            user.TaskRecord.Remove(Config.CalId);
        }

        //奖励
        user.AddExpAndGold(Config.RewardExp, Config.RewardGold);

        if (Config.RewardIdList != null)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < Config.RewardIdList.Length; i++)
            {
                int itemId = Config.RewardIdList[i];
                ItemType type = (ItemType)Config.RewardTypeList[i];

                Item item = ItemHelper.BuildItem(type, itemId, 1, Config.NumberList[i]);
                if (item != null)
                {
                    items.Add(item);
                }
            }
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获得任务奖励", ToastType = ToastTypeEnum.Success });
    }

    private void Ok11()
    {
        Btn_Active.gameObject.SetActive(false);

        //循环任务
        User user = User_Data_Manager.Data;

        Task_Item_Data data = user.TaskData.GetItem(Config.Id);

        if (data == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "还没有领取任务" });
            return;
        }

        if (data.TaskStatus == 1)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经领取过任务奖励了" });
            return;
        }

        this.Txt_Progress.text = "领取中...";

        //再加载net数据
        try
        {
            if (User_Data_Manager.Data.Account != "")
            {
                StartCoroutine(NetworkHelper.SubmitTask(Config.Id,
                    (WebResultWrapper result) =>
                    {
                        if (result.Code == StatusMessage.OK)
                        {
                            JToken lotteryData = result.Extend.SelectToken("LotteryData");
                            Lottery_Result lr = lotteryData.ToObject<Lottery_Result>();


                            this.Txt_Progress.text = "领取成功";
                        }
                        else
                        {
                            ErrorResutlt11();
                        }

                    },
                     () =>
                     {
                         ErrorResutlt11();
                     }));
            }
        }
        catch (Exception ex)
        {
            ErrorResutlt11();
        }
    }

    private void ErrorResutlt11()
    {
        this.Txt_Progress.text = "领取失败，请稍后重试";
    }

    private void Ok12()
    {
        //福利任务

    }

    public void OnClick_Accept()
    {
        User user = User_Data_Manager.Data;

        if (Config.CalId == 11) //循环任务
        {
            Task_Item_Data data = user.TaskData.GetItem(Config.Id);
            data.TaskStatus = 1;
            data.TaskDay = DateTime.Today.Ticks;
            data.Progress = 0;
        }
        else
        {
            if (user.TaskRecord.ContainsKey(Config.CalId))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "任务还没完成" });
                return;
            }

            user.TaskRecord[Config.CalId] = 0;
        }

        this.Show();
    }
}
