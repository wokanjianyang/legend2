using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Rebuild : MonoBehaviour
    {
        public Text Txt_Atr;
        public Text Txt_Percent;
        public Toggle toggle;

        private int Type = 1;
        private int Position = -1;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Select(isOn);
            });
        }

        public void Init(int type, int position, ToggleGroup group)
        {
            this.Type = type;
            this.Position = position;
            this.toggle.group = group;
        }


        public void SetContent(KeyValuePair<int, long> atr, int maxVue)
        {
            long percent = atr.Value * 100 / maxVue;

            this.Txt_Atr.text = StringHelper.FormatAttrText(atr.Key, atr.Value, "£º");

            string color = percent < 100 ? "#FFFFFF" : "#00FF00";

            this.Txt_Percent.text = string.Format("<color={0}>£¨{1}%£©</color>", color, percent);
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                Panel_Rebuild panel = this.gameObject.GetComponentInParent<Panel_Rebuild>();
                panel.SelectAtrItem(this.Type, this.Position);

            }
        }
    }
}
