using Game.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Myth : MonoBehaviour
    {
        //public Image Img_Active;
        public Text Txt_Name;

        public Button Btn_Start;
        public Text Txt_Over;

        private int cycle = 0;
        private MythConfig Config;

        // Update is called once per frame
        void Start()
        {
            Btn_Start.onClick.AddListener(() => { this.OnClick_Start(); });
        }

        void OnEnable()
        {
            if (this.Config != null)
            {
                this.Show();
            }
        }

        private void Show()
        {
            User user = User_Data_Manager.Data;

            if (user.MythData.GetOver(Config.Id))
            {
                Txt_Over.gameObject.SetActive(true);
                Btn_Start.gameObject.SetActive(false);
            }
            else
            {
                this.Txt_Over.gameObject.SetActive(false);
                Btn_Start.gameObject.SetActive(true);
            }
        }

        public void SetContent(MythConfig config)
        {
            this.Config = config;
            Txt_Name.text = Config.MapName;

            this.Show();
        }

        public void SetMax(int cycle, int maxId)
        {
            if (Config.Id - 1 <= maxId && Config.Id >= cycle)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        private void OnClick_Start()
        {
            Dialog_Myth dlg = this.GetComponentInParent<Dialog_Myth>();
            dlg.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;

            if (user.MythData.GetOver(this.Config.Id))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已通过，请等下周", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

            GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Myth, MapId = this.Config.Id });
        }
    }
}
