using Game.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Defend : MonoBehaviour
    {
        //public Image Img_Active;
        public Text Txt_Name;

        public Button Btn_Start;
        public Button Btn_Sweep;

        public Text Txt_Over;
        public Text Txt_Progress;

        private string[] names = new string[] { "普通" };  //, "困难", "噩梦", "地狱", "深渊", "混沌", "虚无", "寂灭"

        private int Level = 0;

        // Update is called once per frame
        void Start()
        {
            Btn_Start.onClick.AddListener(() => { this.OnClick_Start(); });
            Btn_Sweep.onClick.AddListener(() => { this.OnClick_Sweep(); });
        }

        private void OnEnable()
        {
            if (this.Level > 0)
            {
                this.Show();
            }
        }

        private void Show()
        {
            User user = User_Data_Manager.Data;

            long p = user.GetAchievementProgeress(AchievementProType.Defend) - (this.Level - 1) * 100;

            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);

            if (record.Progress > 100)
            {
                this.Txt_Progress.text = "完美通关";
            }
            else
            {
                this.Txt_Progress.text = "当前进度：" + record.Progress + "层";
            }

            if (record.Count <= 0)
            {
                this.Txt_Over.gameObject.SetActive(true);

                Btn_Start.gameObject.SetActive(false);
                Btn_Sweep.gameObject.SetActive(false);
            }
            else
            {
                if (p >= 100)
                {
                    Btn_Start.gameObject.SetActive(false);
                    Btn_Sweep.gameObject.SetActive(true);
                }
                else
                {
                    Btn_Start.gameObject.SetActive(true);
                    Btn_Sweep.gameObject.SetActive(false);
                }
            }
        }

        public void SetContent(int index)
        {
            Txt_Name.text = names[index];
            this.Level = index + 1;

            this.Show();
        }

        private void OnClick_Start()
        {
            Dialog_Defend dlg = this.GetComponentInParent<Dialog_Defend>();
            dlg.gameObject.SetActive(false);

            AppHelper.DefendLevel = Level;

            User user = User_Data_Manager.Data;
            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);

            if (record == null || record.Count <= 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
                return;
            }

            record.Count--;

            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

            GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Defend, MapId = Level });
        }

        private void OnClick_Sweep()
        {
            this.Btn_Sweep.gameObject.SetActive(false);

            Dialog_Defend dlg = this.GetComponentInParent<Dialog_Defend>();
            dlg.gameObject.SetActive(false);

            AppHelper.DefendLevel = Level;

            User user = User_Data_Manager.Data;
            DefendRecord record = user.DefendData.GetCurrentRecord(this.Level);

            if (record == null || record.Count <= 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
                return;
            }

            record.Complete();

            double expTotal = 0;
            double goldTotal = 0;

            Dictionary<int, Item> dicts = new Dictionary<int, Item>();

            for (int i = 1; i <= 100; i++)
            {
                MonsterDefendConfig rewardConfig = MonsterDefendConfigCategory.Instance.GetByLayerAndLevel(this.Level, i);
                long exp = (long)(rewardConfig.Exp + (i - 1) * rewardConfig.RiseExp);
                expTotal += exp;
                goldTotal += exp;

                int dropId = user.DefendData.GetDropId(this.Level, i);
                Item item = DropConfigCategory.Instance.BuildByDropBaseId(dropId, 1, 0, 0);

                DefendDropConfig defendDropConfig = DefendDropConfigCategory.Instance.GetConfig(this.Level, dropId);
                if (defendDropConfig != null && defendDropConfig.Number > 1)
                {
                    int nr = 1;
                    if (defendDropConfig.RateNumber > 0)
                    {
                        nr = (i / defendDropConfig.RateNumber + 1);
                    }

                    item.Temp_Number = defendDropConfig.Number * nr;
                }

                int key = item.ConfigId;
                if (dicts.ContainsKey(key) && item.GetMaxNum() > 1)
                {
                    dicts[key].Temp_Number += item.Temp_Number;
                }
                else
                {
                    dicts[key] = item;
                }
            }



            List<Item> items = dicts.Select(m => m.Value).ToList();

            //List<int> dropIdList = user.DefendData.GetDropIdList(this.Level);

            //List<Item> items = DropConfigCategory.Instance.BuildByDropBaseIdList(dropIdList, 1, 0);

            //显示掉落列表
            string message = "获得金币：" + StringHelper.FormatNumber(goldTotal) + " 经验：" + StringHelper.FormatNumber(expTotal) + "";
            GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });


            //增加经验,金币
            user.AddExpAndGold(expTotal, goldTotal);

            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }
    }
}
