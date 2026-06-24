using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Pet_Talent : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;


        public void SetContent(int tid)
        {
            SkillTalentConfig config = SkillTalentConfigCategory.Instance.Get(tid);
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(config.SkillId);

            this.Txt_Name.text = skillConfig.Name + "" + config.Name;
            this.Txt_Des.text = string.Format(config.Des, config.Percent);
        }
    }
}
