using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Item_Skill_Rune : MonoBehaviour
    {
        public Text Txt_Name;
        public Button Btn_Show;

        private string desc;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Show.onClick.AddListener(this.OnClick_Show);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClick_Show()
        {
            View_Skill view = this.GetComponentInParent<View_Skill>();

            view.ShowRuneDesc(this.desc);
        }

        public void SetRune(int runeId, int count)
        {
            SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);

            string name = config.Name;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }

            string ct = count > 0 ? "+" + count + "" : "无";
            Txt_Name.text = string.Format("{0}：<color=#FF6600>{1}</color>", name, ct);

            this.desc = string.Format(config.Des, config.Damage, config.Percent, config.DeadlyRate) + "，最大叠加数量" + config.Max;
        }

        public void SetSuit(int suitId, int count)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);

            string name = config.Name;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            string ct = count > 0 ? "+" + count + "" : "无";
            Txt_Name.text = string.Format("{0}：<color=#FF6600>{1}</color>", name, ct);

            this.desc = string.Format(config.Des, config.Damage, config.Percent, config.DeadlyRate, config.DeadlyDamage, config.RateDamage, config.AttrIncrea, config.FinalIncrea);
        }

        public void SetTalent(int tid, int count)
        {
            SkillTalentConfig config = SkillTalentConfigCategory.Instance.Get(tid);

            string name = config.Name;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            string ct = count > 0 ? "+" + count + "" : "无";
            Txt_Name.text = string.Format("{0}：<color=#FF6600>{1}</color>", name, ct);

            this.desc = string.Format(config.Des, config.Percent);
        }
    }
}
