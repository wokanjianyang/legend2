using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Equip_Item_Legend : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;



        public void ShowBase(EquipLegendConfig config, int flair)
        {
            this.Txt_Name.text = string.Format("{0}£¨×ÊÖÊ{1}£©£º", config.Name, flair);

            string desc = "";

            for (int i = 0; i < config.AtrIdList.Length; i++)
            {
                desc += i > 0 ? "£¬" : "";
                desc += StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i], "+");
            }

            this.Txt_Des.text = desc;
        }

        public void ShowSet(EquipLegendSet legendSet)
        {
            this.Txt_Name.text = string.Format("{0}£¨{3}-{1}/{2}£©£º", legendSet.Config.Name, legendSet.Count, legendSet.Config.Count, legendSet.Total_Fliar);
            this.Txt_Des.text = legendSet.FormatDesc();

        }
    }
}
