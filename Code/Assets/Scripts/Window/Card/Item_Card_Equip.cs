using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Card_Equip : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Name;
        public Text Txt_Require;
        public Text Txt_Attr;

        public CardConfig Config { get; set; }

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

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click card item");

            Dialog_Card panel = this.GetComponentInParent<Dialog_Card>();

            panel.SelectItem(Config.Id);
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            int cardCount = user.GetCardEquipCount(Config.Id);
            this.Txt_Require.text = cardCount + "/" + Config.Count;
        }

        public void SetContent(CardConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = config.Name;
            this.Txt_Attr.text = StringHelper.FormatAttrText(config.AttrIdList[0], config.AttrValueList[0]);

            this.Show();
        }
    }
}
