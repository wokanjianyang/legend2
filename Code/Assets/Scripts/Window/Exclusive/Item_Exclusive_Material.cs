using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Exclusive_Material : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Name;
        public Text Txt_Count;

        public ExclusiveMaterialConfig Config { get; set; }

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
            User user = GameProcessor.Inst.User;

           
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            long count = user.GetMaterialCount(Config.ItemId);

            this.Txt_Count.text = "数量：" + count;
        }

        public void SetContent(ExclusiveMaterialConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), Config.Name);

            this.Show();
        }
    }
}
