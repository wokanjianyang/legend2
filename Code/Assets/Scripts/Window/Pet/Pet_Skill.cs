using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Pet_Skill : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;


        public void SetContent(int skillId, long level)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);

            this.Txt_Name.text = skillConfig.Name;
            this.Txt_Level.text = "Lv£º" + level;
        }
    }
}
