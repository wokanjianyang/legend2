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
        [LabelText("¸±±¾ÈÝÆ÷")]
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
            User user = GameProcessor.Inst.User;

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
        private void OnClick_Legacy()
        {
            this.ItemLegacy.gameObject.SetActive(true);
        }

        private void OnClick_Babel()
        {
            this.ItemBabel.gameObject.SetActive(true);
        }

        private void OnClick_Defend()
        {
            this.ItemDefend.Show();
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
