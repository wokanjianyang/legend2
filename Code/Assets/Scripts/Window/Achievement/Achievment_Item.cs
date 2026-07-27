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

        string lvName = level > 0 ? "（Lv" + level + "）" : "（未激活）";
        Txt_Name.text = string.Format("【{0}<color=#FDFD00>{1}</color>】", Config.Name, lvName);

        string color = progress >= require ? "00FF00" : "FF0000";
        Txt_Progress.text = string.Format("进度：<color=#{0}>{1}</color> /{2}", color, progress, require);

        Txt_Des.text = string.Format(Config.Memo, require);

        for (int i = 0; i < Config.AtrIdList.Length; i++)
        {
            string atrIdName = StringHelper.FormatAttrValueName(Config.AtrIdList[i]);
            string atrVueName = StringHelper.FormatAttrValueText(Config.AtrIdList[i], Config.GetAtrVue(i, level));
            Txt_Atr_List[i].text = string.Format("{0}<color=#FDFD00>+{1}</color>", atrIdName, atrVueName);
        }

        if (level >= Config.Max)
        {
            Txt_Ok.gameObject.SetActive(true);
            Btn_Active.gameObject.SetActive(false);
            Txt_Progress.text = "进度：已满级";
            Txt_Des.text = "已满级";
            return;
        }


        //彩蛋未激活，独立判断
        if (this.Config.GroupId == 502 && level <= 0)
        {
            this.CheckSpeical();
        }
        else
        {
            if (progress >= require && require > 0)
            {
                Btn_Active.gameObject.SetActive(true);
            }
            else
            {
                Txt_No.gameObject.SetActive(true);
            }
        }
    }

    private void CheckSpeical()
    {
        bool complete = false;

        User user = User_Data_Manager.Data;
        switch (this.Config.Id)
        {
            case 52001:  //传奇人生，全身传奇装备
                long lc = user.EquipPanelList[user.EquipPanelIndex].Select(m => m.Value.Config.Cycle == 10).Count();
                //Debug.Log("52001:" + lc);
                if (lc >= 10)
                {
                    complete = true;
                }
                break;
            case 52002:  //百折不挠，死亡166次 
                long dc = user.GetAchievementProgeress(AchievementProType.DeadCount);
                //Debug.Log("52002:" + dc);
                if (dc >= 66)
                {
                    complete = true;
                }
                break;
            case 52003: //全职大师,上三系装备和3系技能
                long rc = user.EquipPanelList[user.EquipPanelIndex].Select(m => m.Value.Config.Role).Distinct().Count();
                long sc = user.GetCurrentSkillList().Select(m => m / 1000).Distinct().Count();
                //Debug.Log("52003:" + rc + "-" + sc);
                if (rc >= 3 && sc >= 3)
                {
                    complete = true;
                }
                break;
        }

        if (complete)
        {
            Btn_Active.gameObject.SetActive(true);
            Txt_No.gameObject.SetActive(false);
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
