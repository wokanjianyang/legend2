using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Box_Forge_Bag : MonoBehaviour
    {
        public Transform Tf_Box;

        public Toggle toggle;
        public Text Txt_Name;

        public Image Img_Bg;
        public Image Img_Logo;

        public Item CurrentItem;

        private int Type = 0;
        private int BoxId = 0;

        // Start is called before the first frame update
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Select(isOn);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Show()
        {
            if (this.CurrentItem != null)
            {
                Tf_Box.gameObject.SetActive(true);
                Img_Logo.gameObject.SetActive(true);

                int quality = CurrentItem.GetQuality();

                this.Txt_Name.text = CurrentItem.GetName();
                this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

                this.Img_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

                PrefabHelper.Instance().SetItemLogo(this.Img_Logo, CurrentItem);
            }
            else
            {
                this.Img_Logo.gameObject.SetActive(false);
                Tf_Box.gameObject.SetActive(false);
            }
        }

        public void SetItem(Item item)
        {
            this.CurrentItem = item;
            this.Show();
        }

        public void Init(int type, int boxId, ToggleGroup group)
        {
            this.Type = type;
            this.BoxId = boxId;
            this.toggle.group = group;
        }

        public void Refresh()
        {
            this.Show();
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                if (Type == 1)
                {
                    Panel_Legend panel = this.gameObject.GetComponentInParent<Panel_Legend>();
                    panel.SelectBag(this.BoxId, CurrentItem, this);
                }
                else if (Type == 2) {
                    Panel_Reform panel = this.gameObject.GetComponentInParent<Panel_Reform>();
                    panel.SelectBag(this.BoxId, CurrentItem, this);
                }
            }
        }
    }
}