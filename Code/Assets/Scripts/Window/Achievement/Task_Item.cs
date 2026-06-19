using Game;
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

    private AchievementTaskConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnClick_Active);
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
        Txt_No.gameObject.SetActive(false);
        Txt_Ok.gameObject.SetActive(false);

        long require = Config.ConRequire;
        Txt_Des.text = string.Format(Config.Desc, require);
        Txt_Reward.text = string.Format(Config.RewardText, StringHelper.FormatNumber(Config.RewardGold), Config.NumberList[0]);

        User user = GameProcessor.Inst.User;
        user.TaskLog.TryGetValue(Config.Id, out bool complete);
        if (complete)
        {
            Txt_Ok.gameObject.SetActive(true);
            return;
        }

        long progress = user.GetAchievementProgeress((AchievementProType)Config.ConType);
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

    private void OnClick_Active()
    {
        Btn_Active.gameObject.SetActive(false);
        Txt_Ok.gameObject.SetActive(true);

        User user = GameProcessor.Inst.User;

        user.TaskLog.TryGetValue(Config.Id, out bool complete);
        if (complete)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经领过奖励了" });
            return;
        }

        user.TaskLog[Config.Id] = true;

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

        Dialog_Task dialog = this.GetComponentInParent<Dialog_Task>();

        dialog.SelectItem(this.Config.GroupId);
    }
}
