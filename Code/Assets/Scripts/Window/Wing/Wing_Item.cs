using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Wing_Item : MonoBehaviour
    {
        public Transform Tf_Box;

        public Toggle toggle;
        public Text Txt_Name;
        public Text Txt_Level;

        public Image Img_Logo;

        private int Part = 0;

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
            if (this.Part > 0)
            {
                User user = User_Data_Manager.Data;

                int level = user.GetWingFullLevel(this.Part);

                this.Txt_Level.text = string.Format("{0}¼¶", level);
                this.Txt_Level.gameObject.SetActive(true);
            }
            else
            {

            }
        }

        string[] names = { "³á°ò", "·ÉÓð", "ÒíÓð", "ÏËÓð", "ÇáÓð", "ôáÓð", "Î²Óð" };
        public void Init(int part, ToggleGroup group)
        {
            this.Part = part;

            this.toggle.group = group;
            this.Txt_Name.text = names[part - 1];

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
                Dialog_Wing dlg = this.gameObject.GetComponentInParent<Dialog_Wing>();
                dlg.SelectItem(this.Part);
            }
        }
    }
}