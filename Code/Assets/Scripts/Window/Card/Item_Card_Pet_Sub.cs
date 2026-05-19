using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Card_Pet_Sub : MonoBehaviour
    {
        public Image Img_Logo;
        public Text Txt_Name;
        public Image Img_Active;

        public Transform Tf_Akt;
        private List<Text> Txt_Atk_List;



        private PetConfig Config;

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

        public void SetContent(PetConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.CardQuality), Config.Name);
            this.Img_Logo.sprite = PrefabHelper.Instance().GetMonster(config.Id);

            for (int i = 0; i < config.CardAtrList.Length; i++)
            {
                if (i < Txt_Atk_List.Count)
                {
                    this.Txt_Atk_List[i].gameObject.SetActive(true);
                    this.Txt_Atk_List[i].text = StringHelper.FormatAttrText(config.CardAtrList[i], config.CardVueList[i], "£º+");
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
