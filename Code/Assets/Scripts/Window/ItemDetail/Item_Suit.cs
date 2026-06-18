using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Suit : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;

        public void SetContent(int suitId, int count, int max)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(config.SkillId);

            this.Txt_Name.text = skillConfig.Name + config.Name + string.Format("£¨{0}/{1}£©£º", count, max); ;
            this.Txt_Des.text = string.Format(config.Des, config.Damage, config.Percent, config.DeadlyRate, config.DeadlyDamage, config.RateDamage, config.AttrIncrea, config.FinalIncrea);

        }

        public void SetContent(int suitId)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(config.SkillId);

            this.Txt_Name.text = skillConfig.Name + config.Name;
            this.Txt_Des.text = string.Format(config.Des, config.Damage, config.Percent, config.DeadlyRate, config.DeadlyDamage, config.RateDamage, config.AttrIncrea, config.FinalIncrea);
        }

        public void SetLegend(EquipLegendSet legendSet)
        {
            this.Txt_Name.text = string.Format("{0}£¨{1}/{2}£©£º", legendSet.Config.Name, legendSet.Count, legendSet.Config.Count);
            this.Txt_Des.text = legendSet.FormatDesc();

        }
    }
}
