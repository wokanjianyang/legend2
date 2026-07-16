using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Store_Item : MonoBehaviour
    {
        public Button Btn_Info;
        public Text Txt_Name;

        public Text Txt_Number;


        public Image Img_Color_Bg;
        public Image Img_Logo;

        public Image Img_Active;

        public StoreConfig Config;
        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Info.onClick.AddListener(OnClick);
        }


        void OnEnable()
        {
            if (this.Config != null)
            {
                this.Refresh();
            }
        }

        public void Refresh()
        {
            int storeId = Config.Id;

            if (User_Data_Manager.StoreData == null)
            {
                return;
            }

            Store_Data_Item data = User_Data_Manager.StoreData.StoreList.Where(m => m.StoreId == storeId).FirstOrDefault();

            if (data != null)
            {
                this.Img_Active.gameObject.SetActive(false);

                if (data.Number > 1)
                {
                    this.Txt_Number.gameObject.SetActive(true);
                    this.Txt_Number.text = data.Number + "";
                }
            }
            else
            {
                this.Txt_Number.gameObject.SetActive(false);
                this.Img_Active.gameObject.SetActive(true);
            }
        }

        public void Init(StoreConfig config)
        {
            this.Config = config;

            int quality = config.Quality;

            this.Txt_Name.text = config.Name;
            this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

            this.Img_Color_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

            this.Txt_Number.gameObject.SetActive(false);
            this.Img_Active.gameObject.SetActive(true);

            string logoId = "Store/Store" + config.Id;
            this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(logoId);

            this.Refresh();
            //PrefabHelper.Instance().SetItemLogo(this.Img_Logo, CurrentItem);
        }


        private void OnClick()
        {
            Panel_Store panel = this.gameObject.GetComponentInParent<Panel_Store>();
            panel.ShowInfo(this.Config);
        }

        public void ChangeType(int q)
        {
            if (this.Config.Quality == q)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}