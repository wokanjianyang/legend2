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
        //public Button Btn_UpLevel;
        //public Button Btn_Divine;
        //public Text Txt_Divine;

        [Title("词条")]
        public Transform Tf_Talent;
        public Transform Tf_Rune;
        public Transform Tf_Suit;

        List<Item_Skill_Rune> tList = new List<Item_Skill_Rune>();
        List<Item_Skill_Rune> rList = new List<Item_Skill_Rune>();
        List<Item_Skill_Rune> sList = new List<Item_Skill_Rune>();

        //List<Text> runeList = new List<Text>();
        //List<Text> suitList = new List<Text>();

        public SkillPanel SkillPanel { get; private set; }

        void Awake()
        {
            rList = Tf_Rune.GetComponentsInChildren<Item_Skill_Rune>().ToList();
            sList = Tf_Suit.GetComponentsInChildren<Item_Skill_Rune>().ToList();
        }

        // Start is called before the first frame update
        void Start()
        {

            Tg_Recovery.onValueChanged.AddListener((isOn) =>
            {
                this.SkillPanel.SkillData.Recovery = isOn;
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

            string name = SkillPanel.SkillData.SkillConfig.Name;
            //if (SkillPanel.SkillData.SkillConfig.Name.Length > 2)
            //{
            //    name = SkillPanel.SkillData.SkillConfig.Name.Insert(2, "\n");
            //}
            //else
            //{
            //    name = SkillPanel.SkillData.SkillConfig.Name;
            //}

            //string color = skillPanel.DivineLevel > 0 ? "FFFFFF" : "000000";
            string color = "FFFFFF";
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            //if (skillPanel.DivineAttrConfig != null)
            //{
            //    if (skillPanel.DivineAttrConfig.LevelRequire <= skillPanel.SkillData.MagicLevel.Data)
            //    {
            //        this.Btn_Divine.gameObject.SetActive(true);
            //        this.Txt_Divine.gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        this.Btn_Divine.gameObject.SetActive(false);
            //        this.Txt_Divine.gameObject.SetActive(true);

            //        this.Txt_Divine.text = "技能" + skillPanel.DivineAttrConfig.LevelRequire + "级解锁神技";
            //    }
            //}
            //else
            //{
            //    this.Btn_Divine.gameObject.SetActive(false);
            //}

            //if (skillPanel.SkillData.SkillConfig.UpItemId > 0)
            //{
            //    this.Btn_UpLevel.gameObject.SetActive(true);
            //}
            //else
            //{
            //    this.Btn_UpLevel.gameObject.SetActive(false);
            //}


            for (int i = 0; i < rList.Count; i++)
            {
                if (i < skillPanel.RuneTextList.Count)
                {
                    rList[i].gameObject.SetActive(true);
                    rList[i].SetRune(skillPanel.RuneTextList[i].Key, skillPanel.RuneTextList[i].Value);
                }
                else
                {
                    rList[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < sList.Count; i++)
            {
                if (i < skillPanel.SuitTextList.Count)
                {
                    sList[i].gameObject.SetActive(true);
                    sList[i].SetSuit(skillPanel.SuitTextList[i].Key, skillPanel.SuitTextList[i].Value);
                }
                else
                {
                    sList[i].gameObject.SetActive(false);
                }
            }

            User user = GameProcessor.Inst.User;

            int limitLevel = user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig);
            //if (this.SkillPanel.SkillData.MagicLevel.Data >= limitLevel)
            //{
            //    this.Btn_UpLevel.gameObject.SetActive(false);
            //}

            Tg_Recovery.isOn = skillPanel.SkillData.Recovery;

            this.Txt_Level.text = string.Format("LV:{0} (上限:{1})", SkillPanel.Level, limitLevel);
            this.Txt_CD.text = string.Format("CD：{0}秒", SkillPanel.CD);
            this.Txt_Dis.text = string.Format("距离：{0}", SkillPanel.Dis);
            this.Txt_Des.text = SkillPanel.Desc;

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(SkillPanel.SkillData.MagicExp.Data, SkillPanel.SkillData.GetLevelUpExp());
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
            User user = GameProcessor.Inst.User;

            List<int> list = user.GetCurrentSkillList();
            List<SkillData> skillList = user.SkillList.FindAll(m => list.Contains(m.SkillId));

            if (this.SkillPanel == null || this.SkillPanel.SkillData == null || skillList.Count >= user.SkillNumber)
            {
                return;
            }

            if (list.Contains(this.SkillPanel.SkillId))
            {
                return;
            }

            int repet = this.SkillPanel.SkillData.SkillConfig.Repet;
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
            GameProcessor.Inst.User.EventCenter.Raise(new SkillUpEvent());
        }

        //public void Click_UpLevel()
        //{
        //    int upCount = 20;

        //    int metailId = this.SkillPanel.SkillData.SkillConfig.UpItemId;
        //    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(metailId);

        //    User user = GameProcessor.Inst.User;
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
