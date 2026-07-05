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
    public class Lottery_Result_Box : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Points;

        public Image Img_Color_Bg;
        public Image Img_Logo;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void SetContent(Lottery_Result_Item data)
        {
            StoreConfig config = StoreConfigCategory.Instance.Get(data.Id);

            int quality = config.Quality;

            this.Txt_Name.text = config.Name;
            this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

            this.Img_Color_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

            if (data.Type == 1) //
            {
                this.Txt_Points.text = config.Name + "*1";
            }
            else
            {
                this.Txt_Points.text = "¶Ò»»" + data.Points + "»ý·Ö";
            }

            //PrefabHelper.Instance().SetItemLogo(this.Img_Logo, CurrentItem);
        }
    }
}