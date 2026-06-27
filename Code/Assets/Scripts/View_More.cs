using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class View_More : AViewPage
    {
        [LabelText("副本容器")]
        public RectTransform scrollRect;


        public Button Btn_Boss;
        public Dialog_BossFamily ItemBoss;

        public Button Btn_Legacy;
        public Legacy_Copy_Info ItemLegacy;

        public Button Btn_Defend;
        public Dialog_Defend ItemDefend;

        public Button Btn_Babel;
        public Dialog_Babel ItemBabel;


        public Text Txt_Limit;

        void Start()
        {
            Btn_Boss.onClick.AddListener(OnClick_Boss);
            Btn_Legacy.onClick.AddListener(OnClick_Legacy);
            Btn_Babel.onClick.AddListener(OnClick_Babel);
            Btn_Defend.onClick.AddListener(OnClick_Defend);
        }

        void OnEnable()
        {
            User user = User_Data_Manager.Data;

            if (user == null)
            {
                return;
            }

            long level = user.MagicLevel.Data;

        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<CloseViewMoreEvent>(this.OnClose);
            GameProcessor.Inst.EventCenter.AddListener<BattlerEndEvent>(this.OnBattlerEnd);
        }

        private void OnClick_Boss()
        {
            this.ItemBoss.gameObject.SetActive(true);
        }

        private void OnClick_Babel()
        {
            User user = User_Data_Manager.Data;
            if (user.MagicLevel.Data < 25)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "25级才解锁", ToastType = ToastTypeEnum.Failure });
                return;
            }
            this.ItemBabel.gameObject.SetActive(true);
        }

        private void OnClick_Defend()
        {
            User user = User_Data_Manager.Data;
            if (user.MagicLevel.Data < 30)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "30级才解锁", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.ItemDefend.Show();
        }
        private void OnClick_Legacy()
        {
            User user = User_Data_Manager.Data;
            if (user.MagicLevel.Data < 35)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "35级才解锁", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.ItemLegacy.gameObject.SetActive(true);
        }

        public void HideItem()
        {
            this.scrollRect.gameObject.SetActive(false);
        }

        public void OnClose(CloseViewMoreEvent e)
        {
            scrollRect.gameObject.SetActive(false);
        }


        public void OnBattlerEnd(BattlerEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }



        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }

        public override void OnOpen()
        {
            base.OnOpen();

            Debug.Log("open view more");

            scrollRect.gameObject.SetActive(true);
        }
    }
}
