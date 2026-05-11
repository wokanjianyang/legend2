using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ItemForge : MonoBehaviour
    {
        public Image Img_Bg;

        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        private int Type = 1;
        private int Position = -1;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
        }

        // Update is called once per frame
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Select(isOn);
            });
        }

        public void Init(int type, int position, long level, ToggleGroup group)
        {
            this.Type = type;
            this.Position = position;

            this.Txt_Level.text = level + "¼¶";
            this.Img_Bg.sprite = PrefabHelper.Instance().GetEquipBg(Position);

            this.toggle.group = group;
        }

        public void SetLevel(long level)
        {
            Txt_Level.text = level + "¼¶";
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                if (Type == 1)
                {
                    Panel_Strengthen panel = this.gameObject.GetComponentInParent<Panel_Strengthen>();
                    panel.SelectItem(this.Position);
                }
                else if (Type == 2)
                {
                    GameProcessor.Inst.EventCenter.Raise(new EquipRefineSelectEvent() { Position = this.Position });
                }
                else if (Type == 3)
                {
                    GameProcessor.Inst.EventCenter.Raise(new EquipReformSelectEvent() { Position = this.Position });
                }
            }
        }
    }
}
