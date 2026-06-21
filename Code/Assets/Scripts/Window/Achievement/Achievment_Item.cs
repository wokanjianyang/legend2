using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Achievment_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Progress;
    public Text Txt_Des;

    public Transform Tf_Atr;
    private List<Text> Txt_Atr_List;

    public Text Txt_No;
    public Text Txt_Ok;

    public Button Btn_Active;

    private AchievementConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnClick_Active);

        Txt_Atr_List = Tf_Atr.GetComponentsInChildren<Text>().ToList();
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

        User user = User_Data_Manager.Data;

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

        for (int i = 0; i < Config.AtrIdList.Length; i++)
        {
            Txt_Atr_List[i].text = StringHelper.FormatAttrText(Config.AtrIdList[i], Config.GetAtrVue(i, level), "+");
        }

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
        User user = User_Data_Manager.Data;

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
