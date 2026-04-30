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
    [Serializable]
    public class ExclusiveItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Exclusive : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Name;
        public Text Txt_Attr;
        public Text Txt_Desc;
        public Image Img_Active;
        public ExclusiveConfig Config { get; set; }

        [SerializeField]
        private ExclusiveItemSelectEvent _onSelected = new ExclusiveItemSelectEvent();

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
            User user = GameProcessor.Inst.User;
            if (user.ExclusiveDict.ContainsKey(Config.Id) && user.ExclusiveDict[Config.Id] > 0)
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
            this.Txt_Attr.text = StringHelper.FormatAttrText(config.AttrId, config.AttrValue);
            this.Txt_Desc.text = Config.Des;

            this.Show();
        }
    }
}
