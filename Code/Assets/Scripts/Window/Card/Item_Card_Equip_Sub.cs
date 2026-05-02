using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Card_Equip_Sub : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Attr;
        public Image Img_Active;

        private EquipConfig Config;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void OnEnable()
        {
            if (Config != null)
            {
                this.Show();
            }
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;
            if (user.GetCardEquipLevel(Config.Id) > 0)
            {
                this.Img_Active.gameObject.SetActive(false);
            }
            else
            {
                this.Img_Active.gameObject.SetActive(true);
            }

        }

        public void SetContent(EquipConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.CardQuality), Config.Name);
            this.Txt_Attr.text = StringHelper.FormatAttrText(config.CardAttr, config.CardValue);

            this.Show();
        }
    }
}
