using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Box_Drop : MonoBehaviour
    {
        public Transform Tf_Bg;
        public Transform Tf_Box;

        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Image Img_Bg;
        public Image Img_Logo;

        private Item CurrentItem;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(Item item)
        {
            this.CurrentItem = item;

            if (this.CurrentItem != null)
            {
                Tf_Bg.gameObject.SetActive(false);
                Tf_Box.gameObject.SetActive(true);
                Img_Logo.gameObject.SetActive(true);

                this.Txt_Layer.gameObject.SetActive(false);
                this.Txt_Level.gameObject.SetActive(false);

                int quality = CurrentItem.GetQuality();

                this.Txt_Name.text = CurrentItem.GetName();
                this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

                this.Img_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

                PrefabHelper.Instance().SetItemLogo(this.Img_Logo, CurrentItem);

                if (CurrentItem.Temp_Number > 1)
                {
                    this.Txt_Level.gameObject.SetActive(true);
                    this.Txt_Level.text = CurrentItem.Temp_Number + "";
                }
            }
            else
            {
                this.Img_Logo.gameObject.SetActive(false);
                Tf_Bg.gameObject.SetActive(true);
                Tf_Box.gameObject.SetActive(false);
            }
        }
    }
}