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
    public class Item_Card : MonoBehaviour, IPointerClickHandler
    {
        public Image Img_Logo;
        public Text Txt_Name;
        public Text Txt_Level;

        public Transform Tf_Akt;
        private List<Text> Txt_Atk_List;

        public Text Txt_Atr_Spe;

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
            //Debug.Log("click card item");

            //Dialog_Card panel = this.GetComponentInParent<Dialog_Card>();

            //panel.SelectItem(Config.Id);
        }

        public void Show()
        {
            User user = User_Data_Manager.Data;

            CardConfig config = CardConfigCategory.Instance.Get(Config.Id);

            int cardExp = user.GetCardExp(Config.Id);

            int cardLevel = config.CalLevel(cardExp);

            if (cardLevel >= config.MaxLevel)
            {
                this.Txt_Level.text = string.Format("Lv.{0}£¨Max{1}£©", cardLevel, config.MaxLevel);
            }
            else
            {
                int nextExp = config.CalNextExp(cardExp);
                this.Txt_Level.text = string.Format("Lv.{0}£¨Exp{1}/{2}£©", cardLevel, cardExp, nextExp);
            }

            if (cardLevel >= config.SpeRequire)
            {
                this.Txt_Atr_Spe.text = StringHelper.FormatAttrText(config.SpeAtrId, config.SpeAtrVue, "£º+");
            }
            else
            {
                this.Txt_Atr_Spe.text = config.SpeRequire + "¼¶½âËø";
            }


            for (int i = 0; i < config.AtrIdList.Length; i++)
            {
                if (i < Txt_Atk_List.Count)
                {
                    this.Txt_Atk_List[i].gameObject.SetActive(true);
                    this.Txt_Atk_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i] * cardLevel, "£º+");
                }
                else
                {
                    this.Txt_Atk_List[i].gameObject.SetActive(false);
                }
            }

        }

        public void SetContent(CardConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = config.Name;
            this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(config.LogoId);



            this.Show();
        }
    }
}
