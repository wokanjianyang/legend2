using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Progress;
    public Text Txt_Des;

    public Text Txt_Atr;

    public Text Txt_No;
    public Text Txt_Ok;

    public Button Btn_Active;

    private AchievementConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnClick_Active);
    }

    private void OnEnable()
    {
        if (Config != null)
        {
            this.Show();
        }
    }

    public void SetContent(AchievementConfig config)
    {
        this.Config = config;
        this.Show();
    }

    private void Show()
    {
        Btn_Active.gameObject.SetActive(false);
        Txt_No.gameObject.SetActive(false);
        Txt_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int level = user.GetAchievementLevel(Config.Id);
        long progress = user.GetAchievementProgeress((AchievementProType)Config.ConType);
        long require = AchievementConfigCategory.Instance.CalRequire(Config, level + 1);

        if (level > 0)
        {
            Txt_Name.text = Config.Name + "（Lv" + level + "）";
        }
        else
        {
            Txt_Name.text = Config.Name + "（未激活）";
        }

        string color = progress >= require ? "00FF00" : "FF0000";
        Txt_Progress.text = string.Format("进度：<color=#{0}>{1}</color> /{2}", color, progress, require);

        Txt_Des.text = string.Format(Config.Memo, require);
        Txt_Atr.text = StringHelper.FormatAttrText(Config.AtrId, Config.AtrVue * level, "+");

        if (level >= Config.Max)
        {
            Txt_Ok.gameObject.SetActive(true);
            return;
        }

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
        User user = GameProcessor.Inst.User;

        int level = user.GetAchievementLevel(Config.Id);
        long progress = user.GetAchievementProgeress((AchievementProType)Config.ConType);
        long require = AchievementConfigCategory.Instance.CalRequire(Config, level + 1);

        if (progress >= require)
        {
            user.AddAchievementLevel(Config.Id);

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "激活成就成功", ToastType = ToastTypeEnum.Success });

            GameProcessor.Inst.UpdateInfo();

            this.Show();
        }

    }
}
