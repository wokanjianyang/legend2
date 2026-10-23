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

        private int Type = 0;
        private MythConfig Config;

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

        private void Show()
        {
            User user = User_Data_Manager.Data;

            int maxId = user.MythData.GetMax() + 1;

            if (this.Config.Id <= maxId)
            {
                this.gameObject.SetActive(true);

                if (this.Config.Id == maxId)
                {
                    this.Txt_Over.gameObject.SetActive(false);
                    Btn_Start.gameObject.SetActive(true);
                }
                else
                {
                    this.Txt_Over.gameObject.SetActive(true);
                    Btn_Start.gameObject.SetActive(false);
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }

        }

        public void SetContent(MythConfig config)
        {
            this.Config = config;
            Txt_Name.text = Config.MapName;

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
            Dialog_Myth dlg = this.GetComponentInParent<Dialog_Myth>();
            dlg.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new ChangePageEvent() { Page = ViewPageType.View_Battle });

            GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.Myth, MapId = this.Config.Id });
        }
    }
}
