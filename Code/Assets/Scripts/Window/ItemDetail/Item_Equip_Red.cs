using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Equip_Red : MonoBehaviour
    {
        public Text Txt_Des;

        public void SetContent(EquipSetItem redItem, int quality)
        {
            string color = redItem.Count >= redItem.Config.Count ? QualityConfigHelper.GetQualityColor(quality) : "CCCCCC";

            int showLevel = Math.Max(1, redItem.Level);

            string qn = "橙装";

            if (quality == 6)
            {
                qn = "红装";
            }
            else if (quality == 7)
            {
                qn = "金装";
            }
            else if (quality == 8)
            {
                qn = "暗金";
            }
            else if (quality == 9)
            {
                qn = "混沌";
            }

            int attr = (int)(redItem.Config.AttrValue + (showLevel - 1) * redItem.Config.AttrRise);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}({2}/{3})</color>", color, StringHelper.FormatAttrText(redItem.Config.AttrId, attr, "+"), redItem.Count, redItem.Config.Count);
        }

        public void SetEquipSpecial(EquipSetItem redItem, EquipSpeicalConfig config)
        {
            string color = redItem.Count >= redItem.Config.Count ? QualityConfigHelper.GetQualityColor(config.Quality) : "CCCCCC";

            int showLevel = Math.Max(1, redItem.Level);

            int attr = (int)(redItem.Config.AttrValue + (showLevel - 1) * redItem.Config.AttrRise);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}({2}/{3})</color>", color, StringHelper.FormatAttrText(redItem.Config.AttrId, attr, "+"), redItem.Count, redItem.Config.Count);
        }

        public void SetShengxiaoGroup(ShengxiaoGroupItem item)
        {
            string color = item.Count >= item.Config.Count ? QualityConfigHelper.GetQualityColor(item.Config.Quality) : "CCCCCC";

            string qn = "红色";
            if (item.Config.Quality == 7)
            {
                qn = "金色";
            }
            else if (item.Config.Quality == 8)
            {
                qn = "暗金";
            }
            else if (item.Config.Quality == 9)
            {
                qn = "粉色";
            }

            string name = qn + "生肖" + string.Format("({0}/{1})", item.Count, item.Config.Count);

            int attr = (int)(item.Config.AttrValue);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}</color>", color, StringHelper.FormatAttrText(item.Config.AttrId, attr, "+"));
        }
    }
}
