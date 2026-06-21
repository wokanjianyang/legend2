using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Card_Equip : MonoBehaviour, IPointerClickHandler
    {
        public Image Img_Logo;
        public Text Txt_Name;
        public Text Txt_Require;

        public Transform Tf_Akt;
        private List<Text> Txt_Atk_List;

        public CardConfig Config { get; set; }

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Atk_List = Tf_Akt.GetComponentsInChildren<Text>().ToList();
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
            User user = User_Data_Manager.Data;

            int cardCount = user.GetCardEquipCount(Config.Stage, Config.Id);

            string color = cardCount >= Config.Count ? "#00FF00" : "#FF0000";
            string rt = cardCount + "/" + Config.Count;
            this.Txt_Require.text = string.Format("<color={0}>{1}</color>", color, rt);
        }

        public void SetContent(CardConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = config.Name;
            this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(config.LogoId);

            for (int i = 0; i < config.AtrIdList.Length; i++)
            {
                if (i < Txt_Atk_List.Count)
                {
                    this.Txt_Atk_List[i].gameObject.SetActive(true);
                    this.Txt_Atk_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i], "£º+");
                }
                else
                {
                    this.Txt_Atk_List[i].gameObject.SetActive(false);
                }
            }

            this.Show();
        }
    }
}
