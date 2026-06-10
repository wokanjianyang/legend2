using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Skill_Level_Item : MonoBehaviour
    {
        public Text Txt_Require;

        public Text Txt_Atr;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private string[] names = { "技能系数", "技能伤害", "持续时间", "目标数量", "行", "列", "攻速", "冷却", "距离", "技能攻击", "技能终伤", "暴击概率", "致命概率" };
        private int[] ps = { 1, 7, 8, 10, 11, 12, 13 };

        public void SetContent(SkillConfig config, int index, int currentLevel)
        {
            this.Txt_Require.text = config.RiseRequireLevel[index] + "级解锁";

            int riseId = config.RiseId[index];

            string unit = ps.Contains(riseId) ? "%" : "";
            this.Txt_Atr.text = names[riseId-1] + "+" + config.RiseVue[index] + unit;
        }


    }
}
