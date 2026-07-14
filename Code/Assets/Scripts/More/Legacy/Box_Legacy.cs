using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Box_Legacy : MonoBehaviour
    {
        public Transform Tf_Bg;
        public Transform Tf_Box;

        public Toggle toggle;
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;

        public Image Img_Bg;
        public Image Img_Logo;

        private int Role = 0;
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
            if (this.Position > 0)
            {
                User user = User_Data_Manager.Data;
                int part = (Role - 1) * 8 + Position;

                long layer = user.GetLegacyLayer(part);

                if (layer > 0)
                {
                    long level = user.GetLegacyLevel(part);

                    Tf_Bg.gameObject.SetActive(false);
                    Tf_Box.gameObject.SetActive(true);
                    Img_Logo.gameObject.SetActive(true);

                    this.Txt_Layer.gameObject.SetActive(false);
                    this.Txt_Level.gameObject.SetActive(false);

                    LegacyConfig config = LegacyConfigCategory.Instance.GetByPart(Role, Position);

                    this.Txt_Name.text = config.Name;
                    this.Txt_Name.color = QualityConfigHelper.GetColor(6);

                    this.Img_Logo.sprite = PrefabHelper.Instance().GetLegacyLogo(Role, Position);

                    if (layer > 0)
                    {
                        this.Txt_Layer.text = layer + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }

                    if (level > 0)
                    {
                        this.Txt_Level.text = level + "级";
                        this.Txt_Level.gameObject.SetActive(true);
                    }

                    return;
                }
            }

            this.Img_Logo.gameObject.SetActive(false);
            Tf_Bg.gameObject.SetActive(true);
            Tf_Box.gameObject.SetActive(false);

        }

        public void Init(int role, int position, ToggleGroup group)
        {
            this.Role = role;
            this.Position = position;

            int[] pl = { 1, 2, 3, 4, 5, 7, 9, 10 };
            int p = pl[position - 1];
            this.Img_Bg.sprite = PrefabHelper.Instance().GetEquipBg(p);

            this.toggle.group = group;

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
                Panel_Legacy panel = this.gameObject.GetComponentInParent<Panel_Legacy>();
                panel.SelectItem(this.Position);
            }
        }
    }
}