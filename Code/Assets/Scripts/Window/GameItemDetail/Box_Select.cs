using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Box_Select : MonoBehaviour
    {
        public Transform Tf_Bg;
        public Transform Tf_Box;

        public Toggle toggle;
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Image Img_Bg;
        public Image Img_Logo;

        private Item CurrentItem;

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
                Tf_Bg.gameObject.SetActive(false);
                Tf_Box.gameObject.SetActive(true);

                this.Txt_Layer.gameObject.SetActive(false);
                this.Txt_Level.gameObject.SetActive(false);

                int quality = CurrentItem.GetQuality();

                this.Txt_Name.text = CurrentItem.GetName();
                this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

                this.Img_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

                if (CurrentItem.GetItemType() == ItemType.Equip)
                {
                    Equip equip = CurrentItem as Equip;

                    this.Img_Logo.sprite = PrefabHelper.Instance().GetEquipLog(equip.Config.Role, equip.Config.Part);

                    if (equip.Layer > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
                else if (CurrentItem.GetItemType() == ItemType.EquipSpeical)
                {
                    Equip_Special equip = CurrentItem as Equip_Special;

                    this.Img_Logo.sprite = PrefabHelper.Instance().GetEquipLog(0, equip.Config.Part);

                    if (equip.Layer > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                Tf_Bg.gameObject.SetActive(true);
                Tf_Box.gameObject.SetActive(false);
            }
        }

        public void Init(int type, int position, ToggleGroup group)
        {
            this.Type = type;
            this.Position = position;

            this.Img_Bg.sprite = PrefabHelper.Instance().GetEquipBg(Position);

            this.toggle.group = group;
        }

        public void SetItem(Item item)
        {
            this.CurrentItem = item;
            this.Show();
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                if (Type == 3)
                {
                    //Panel_Grade panel = this.gameObject.GetComponentInParent<Panel_Grade>();
                    //panel.SelectItem(this.Position, CurrentItem);
                }
            }
        }

    }

}