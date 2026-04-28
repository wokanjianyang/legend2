using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Item_Skill_Selected : MonoBehaviour
    {
        [Title("技能")]
        [LabelText("技能")]
        public Transform tran_Skill;

        [LabelText("移除")]
        public Button btn_Skill;

        public Image Img_Icon;

        public SkillData SkillData { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Skill.onClick.AddListener(this.OnClick_RemoveSkill);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClick_RemoveSkill()
        {
            Clear();

            User user = GameProcessor.Inst.User;

            List<int> list = user.GetCurrentSkillList();
            list.Remove(this.SkillData.SkillId);

            GameProcessor.Inst.EventCenter.Raise(new SkillDownEvent());
        }

        public void SetItem(SkillData skillData)
        {
            this.SkillData = skillData;
            this.Img_Icon.sprite = PrefabHelper.Instance().GetSkillLog(skillData.SkillId);
            this.tran_Skill.gameObject.SetActive(true);
        }

        public void Clear()
        {
            this.tran_Skill.gameObject.SetActive(false);
        }
    }
}
