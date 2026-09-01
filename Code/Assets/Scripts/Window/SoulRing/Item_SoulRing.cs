using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_SoulRing : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public Image image_Background;

        public SoulRingConfig Config;
        private long Level;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void Init(ToggleGroup toggleGroup, SoulRingConfig config)
        {
            toggle.group = toggleGroup;

            this.Config = config;

            Txt_Name.text = config.Name;
            Txt_Level.text = "";
            this.image_Background.color = ColorHelper.GetColorByQuality(config.Id);

            //Txt_Name.color = ColorHelper.GetColorByQuality(6);
        }

        public void SetContent(long level)
        {
            this.Level = level;

            if (level > 0)
            {
                Txt_Level.text = level + "¼¶";
            }
            else
            {
                Txt_Level.text = "";
            }
        }
    }
}
