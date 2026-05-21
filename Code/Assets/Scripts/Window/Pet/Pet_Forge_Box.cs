using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Pet_Forge_Box : MonoBehaviour
    {
        public Toggle toggle;
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Image Img_Rect;
        public Image Img_Logo;

        public Pet CurrentItem;

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
                Img_Logo.gameObject.SetActive(true);

                this.Txt_Layer.gameObject.SetActive(false);
                this.Txt_Level.gameObject.SetActive(false);

                int quality = CurrentItem.GetQuality();

                this.Txt_Name.text = CurrentItem.GetName();
                this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

                this.Img_Rect.sprite = PrefabHelper.Instance().GetBoxImage(quality);

                this.Img_Logo.sprite = PrefabHelper.Instance().GetMonster(CurrentItem.ConfigId);

                if (CurrentItem.Level > 0)
                {
                    this.Txt_Level.text = CurrentItem.Level + "级";
                    this.Txt_Level.gameObject.SetActive(true);
                }
            }
            else
            {
                //this.gameObject.SetActive(false);
            }
        }

        public void Init(int type, int position, ToggleGroup group)
        {
            this.Type = type;
            this.Position = position;

            this.toggle.group = group;
        }

        public void SetItem(Pet pet)
        {
            this.CurrentItem = pet;
            this.Show();
        }

        public void Refresh()
        {
            this.Show();
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                Panel_Pet_Forge panel = this.gameObject.GetComponentInParent<Panel_Pet_Forge>();
                panel.SelectItem(this.Type, this.Position);
            }
        }
    }
}