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
    [Serializable]
    public class ExclusiveItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Exclusive : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Name;
        public Image Img_Logo;
        public Transform Tf_Atr_List;
        private List<Text> Txt_Atr_List;
        public Text Txt_Desc;
        public Image Img_Active;
        public ExclusiveConfig Config { get; set; }

        [SerializeField]
        private ExclusiveItemSelectEvent _onSelected = new ExclusiveItemSelectEvent();

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Atr_List = Tf_Atr_List.GetComponentsInChildren<Text>().ToList();
        }

        // Update is called once per frame
        void OnEnable()
        {
            if (Config != null)
            {
                this.Show();
            }
        }

        public void AddListener(UnityAction<int> callback)
        {
            _onSelected.AddListener(callback);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onSelected.Invoke(Config.Id);
        }

        public void Show()
        {
            User user = User_Data_Manager.Data;
            if (user.GetExclusiveLevel(Config.Id) > 0)
            {
                this.Img_Active.gameObject.SetActive(false);
            }
            else
            {
                this.Img_Active.gameObject.SetActive(true);
            }

        }

        public void SetContent(ExclusiveConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), Config.Name);
            this.Txt_Desc.text = Config.Des;
            this.Img_Logo.sprite = PrefabHelper.Instance().GetItemLogo(config.LogoId);

            for (int i = 0; i < Txt_Atr_List.Count; i++)
            {
                if (i < config.AtrIdList.Length)
                {
                    this.Txt_Atr_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i], "+");
                    this.Txt_Atr_List[i].gameObject.SetActive(true);
                }
                else
                {
                    this.Txt_Atr_List[i].gameObject.SetActive(false);
                }
            }

            this.Show();
        }
    }
}
