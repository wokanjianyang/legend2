using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Skill : MonoBehaviour, IPointerClickHandler
    {
        [Title("基础")]
        public Text Txt_Name;

        public Text Txt_Level;
        public Text Txt_CD;
        public Text Txt_Des;
        public Text Txt_Dis;
        public Toggle Tg_Recovery;
        public Image Img_Icon;

        public Button Btn_Level;
        public Button Btn_Detail;

        //List<Text> runeList = new List<Text>();
        //List<Text> suitList = new List<Text>();

        public SkillPanel SkillPanel { get; private set; }

        void Awake()
        {
            this.Btn_Level.onClick.AddListener(OnClick_Level);
            this.Btn_Detail.onClick.AddListener(OnClick_Detail);
        }

        // Start is called before the first frame update
        void Start()
        {

            Tg_Recovery.onValueChanged.AddListener((isOn) =>
            {
                SkillData sd = User_Data_Manager.Data.SkillList.Where(m => m.SkillId == this.SkillPanel.SkillId).FirstOrDefault();
                sd.Recovery = isOn;
            });


            //this.Btn_UpLevel.onClick.AddListener(this.Click_UpLevel);
            //this.Btn_Divine.onClick.AddListener(this.OnClickDivine);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(SkillPanel skillPanel)
        {
            this.SkillPanel = skillPanel;

            string name = SkillPanel.Config.Name;

            //string color = skillPanel.DivineLevel > 0 ? "FFFFFF" : "000000";
            string color = "FFFFFF";
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            this.Img_Icon.sprite = PrefabHelper.Instance().GetSkillLog(skillPanel.SkillId);

            this.Show();
        }

        public void Show()
        {
            User user = User_Data_Manager.Data;
            if (user == null || SkillPanel == null)
            {
                return;
            }


            int limitLevel = user.GetSkillLimit(this.SkillPanel.Config);
            //if (this.SkillPanel.SkillData.MagicLevel.Data >= limitLevel)
            //{
            //    this.Btn_UpLevel.gameObject.SetActive(false);
            //}
            SkillData sd = user.SkillList.Where(m => m.SkillId == SkillPanel.SkillId).FirstOrDefault();

            Tg_Recovery.isOn = sd.Recovery;

            //this.Txt_Level.text = string.Format("LV:{0} (上限:{1})", SkillPanel.Level, limitLevel);
            this.Txt_Level.text = string.Format("LV:{0} (预览)", SkillPanel.Level);
            this.Txt_CD.text = string.Format("CD：{0}秒", SkillPanel.CD);
            this.Txt_Dis.text = string.Format("距离：{0}", SkillPanel.Dis);
            this.Txt_Des.text = SkillPanel.Desc;

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(sd.MagicExp.Data, sd.GetLevelUpExp());
        }

        private string formatText(KeyValuePair<string, int> kp)
        {
            string name = kp.Key;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            //Debug.Log("name:" + name);

            string ct = kp.Value > 0 ? "+" + kp.Value : "无";
            return name + "：" + string.Format("<color=#FF0000>{0}</color>", ct);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            User user = User_Data_Manager.Data;

            List<int> list = user.GetCurrentSkillList();
            List<SkillData> skillList = user.SkillList.FindAll(m => list.Contains(m.SkillId));

            if (this.SkillPanel == null || skillList.Count >= user.SkillNumber)
            {
                return;
            }

            if (this.SkillPanel.Config.Type == (int)SkillType.Passive || this.SkillPanel.Config.Type == (int)SkillType.Expert)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "被动技能无需上阵", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (list.Contains(this.SkillPanel.SkillId))
            {
                return;
            }

            int repet = this.SkillPanel.Config.Repet;
            if (repet > 0)
            {
                //查找是否已经上阵了同类技能
                if (skillList.Where(m => m.SkillConfig.Repet == repet).Count() > 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经上阵了同类技能", ToastType = ToastTypeEnum.Failure });
                    return;
                }
            }

            list.Add(this.SkillPanel.SkillId);
            GameProcessor.Inst.EventCenter.Raise(new SkillUpEvent());
        }

        public void OnClick_Level()
        {
            View_Skill view = this.GetComponentInParent<View_Skill>();

            view.ShowLevelInfo(this.SkillPanel);
        }

        public void OnClick_Detail()
        {
            View_Skill view = this.GetComponentInParent<View_Skill>();

            view.ShowDetail(this.SkillPanel);
        }

        //public void Click_UpLevel()
        //{
        //    int upCount = 20;

        //    int metailId = this.SkillPanel.SkillData.SkillConfig.UpItemId;
        //    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(metailId);

        //    User user = User_Data_Manager.Data;
        //    long total = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == metailId).Select(m => m.MagicNubmer.Data).Sum();

        //    //Debug.Log("max skill level:" + user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig));

        //    if (total < upCount)
        //    {
        //        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = itemConfig.Name + "数量不足" + upCount + "个", ToastType = ToastTypeEnum.Failure });
        //        return;
        //    }

        //    SkillData skill = this.SkillPanel.SkillData;

        //    if (skill.MagicLevel.Data >= user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig))
        //    {
        //        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "技能已经满级了" });
        //        return;
        //    }

        //    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        //    {
        //        Type = ItemType.Material,
        //        ItemId = metailId,
        //        Quantity = upCount
        //    });

        //    skill.MagicLevel.Data++;
        //    SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId, null), user.GetSuitList(skill.SkillId), true);
        //    this.SetItem(skillPanel);


        //    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upCount + "个" + itemConfig.Name + "升级成功", ToastType = ToastTypeEnum.Success });

        //    GameProcessor.Inst.SaveData();
        //}

        //public void OnClickDivine()
        //{

        //    GameProcessor.Inst.EventCenter.Raise(new OpenDivineEvent() { SkillId = SkillPanel.SkillId });
        //}
    }
}
