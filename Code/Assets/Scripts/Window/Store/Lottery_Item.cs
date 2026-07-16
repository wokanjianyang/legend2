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
    public class Lottery_Item : MonoBehaviour
    {
        public Button Btn_Info;
        public Text Txt_Name;

        public Image Img_Color_Bg;
        public Image Img_Logo;

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

            string logoId = "Store/Store" + config.Id;
            this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(logoId);
            //PrefabHelper.Instance().SetItemLogo(this.Img_Logo, CurrentItem);
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


        private void OnClick()
        {
            Panel_Lottery panel = this.gameObject.GetComponentInParent<Panel_Lottery>();
            panel.ShowInfo(this.Config);
        }
    }
}