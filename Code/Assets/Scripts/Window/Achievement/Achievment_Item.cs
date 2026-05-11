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
    public Text Txt_Active;

    public Text Txt_Atr;
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
        User user = GameProcessor.Inst.User;

        long progress = user.GetAchievementProgeress((AchievementProType)Config.ConType);
        int level = user.GetAchievementLevel(Config.Id);

        long require = AchievementConfigCategory.Instance.CalRequire(Config, level + 1);

        Txt_Name.text = Config.Name + "£¨Lv" + level + "£©";
        Txt_Progress.text = progress + "/" + require;

        Txt_Des.text = string.Format(Config.Memo, require);
        Txt_Atr.text = StringHelper.FormatAttrText(Config.AtrId, Config.AtrVue * level);
    }

    private void OnClick_Active()
    {

    }
}
