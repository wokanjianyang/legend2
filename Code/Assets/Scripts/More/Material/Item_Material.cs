using Game.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Material : MonoBehaviour
    {
        //public Image Img_Active;
        public Text Txt_Name;

        public Button Btn_Start;
        public Text Txt_Start;
        public Text Txt_Over;
        public Text Txt_Progress;

        private int Type = 0;

        // Update is called once per frame
        void Start()
        {
            Btn_Start.onClick.AddListener(() => { this.OnClick_Start(); });
        }

        void OnEnable()
        {
            if (this.Type > 0)
            {
                this.Show();
            }
        }

        private AchievementProType[] TypeList = { AchievementProType.Material1, AchievementProType.Material2, AchievementProType.Material3 };
        private string[] names = { "金币副本", "强化副本", "精炼副本" };

        private void Show()
        {
            User user = User_Data_Manager.Data;

            Materail_Record record = user.MaterailData.GetRecordType(this.Type);

            AchievementProType at = TypeList[this.Type - 1];
            long maxProgess = user.GetAchievementProgeress(at);

            int cp = record.Progress;
            int max = MaterialCopyConfigCategory.Instance.GetMaxProgress(this.Type);

            Txt_Name.text = names[this.Type - 1];
            if (cp > max)
            {
                this.Txt_Progress.text = "完美通关";

                this.Txt_Over.gameObject.SetActive(true);
                Btn_Start.gameObject.SetActive(false);
            }
            else
            {
                this.Txt_Progress.text = "当前进度：" + cp + "层";
            }

            if (record.Count <= 0)
            {
                this.Txt_Over.gameObject.SetActive(true);
                Btn_Start.gameObject.SetActive(false);
            }
        }

        public void SetContent(int type)
        {
            this.Type = type;
            Txt_Name.text = "难度";

            this.Show();
        }

        public void ShowType(int type)
        {
            if (this.Type == type)
            {
                this.gameObject.SetActive(true);
                this.Show();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        private void OnClick_Start()
        {
            Dialog_Material dlg = this.GetComponentInParent<Dialog_Material>();
            dlg.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;
            Materail_Record record = user.MaterailData.GetRecordType(this.Type);

            if (record == null || record.Count <= 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
                return;
            }

            record.Count--;

            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

            GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Materail, MapId = this.Type });

            //if (Type < 3)
            //{

            //}
            //else
            //{
            //    this.Btn_Start.gameObject.SetActive(false);
            //    this.Txt_Over.gameObject.SetActive(true);

            //    double exp = 0;
            //    double gold = 0;

            //    for (int i = 1; i <= 100; i++)
            //    {
            //        DefendConfig rewardConfig = DefendConfigCategory.Instance.GetByLayerAndLevel(this.Level, i);

            //        exp += rewardConfig.Exp;
            //        gold += rewardConfig.Gold;
            //    }

            //    //增加经验,金币
            //    user.AddExpAndGold(exp, gold);

            //    List<int> dropIdList = user.DefendData.GetDropIdList(this.Level);

            //    List<Item> items = DropConfigCategory.Instance.BuildByDropBaseIdList(dropIdList, 1, 0);

            //    if (items.Count > 0)
            //    {
            //        GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            //    }

            //    user.DefendData.Complete();

            //    //显示掉落列表
            //    string message = "获得金币：" + StringHelper.FormatNumber(gold) + " 经验：" + StringHelper.FormatNumber(exp) + "";
            //    GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });
            //}
        }
    }
}
