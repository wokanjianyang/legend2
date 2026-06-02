using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Gift_Item : MonoBehaviour
    {
        public Transform Tf_Box;

        public Toggle toggle;
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Image Img_Bg;
        public Image Img_Logo;

        private BoxItem CurrentItem;

        private int Type = 0;
        private int Position = 0;

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

        void OnEnable()
        {
            this.Show();
        }

        private void Show()
        {
            if (this.CurrentItem != null)
            {
                this.Txt_Layer.gameObject.SetActive(false);
                this.Txt_Level.gameObject.SetActive(false);

                Item gameItem = CurrentItem.Item;
                int quality = gameItem.GetQuality();

                this.Txt_Name.text = gameItem.GetName();
                this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

                this.Img_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

                PrefabHelper.Instance().SetItemLogo(this.Img_Logo, gameItem);

                if (gameItem.GetItemType() == ItemType.Equip)
                {
                    Equip equip = gameItem as Equip;

                    if (equip.Layer > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "½×";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
                else if (gameItem.GetItemType() == ItemType.EquipSpeical)
                {
                    Equip_Special equip = gameItem as Equip_Special;

                    if (equip.Layer > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "½×";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void SetItem(BoxItem item, ToggleGroup group)
        {
            this.CurrentItem = item;
            this.toggle.group = group;
            this.Show();
        }




        private void Select(bool isOn)
        {
            if (isOn)
            {
                Dialog_Detail_Select panel = this.gameObject.GetComponentInParent<Dialog_Detail_Select>();
                panel.OnSelectItem(CurrentItem);

            }
        }
    }
}
