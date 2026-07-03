using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

        StoreConfig Config;
        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Info.onClick.AddListener(OnClick);
        }

        public void Init(StoreConfig config)
        {
            this.Config = config;

            int quality = config.Quality;

            this.Txt_Name.text = config.Name;
            this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

            this.Img_Color_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

            this.Txt_Number.gameObject.SetActive(false);
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