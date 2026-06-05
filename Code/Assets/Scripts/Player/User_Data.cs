using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;
using Game.Data;
using SDD.Events;

namespace Game
{
    public class User_Data
    {

        public AttributeBonus AttributeBonus { get; set; }

        public long TempUpExp { get; set; } = 0;


        public User_Data()
        {


        }


        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();


        }



        public static List<SkillPanel> GetSkills()
        {
            User user = GameProcessor.Inst.User;
            List<SkillData> skills = user.SkillList;

            List<SkillPanel> list = new List<SkillPanel>();

            //先加载3个精通
            int[] experts = new int[] { 1006, 2006, 3006 };
            Dictionary<int, int> dictExpert = new Dictionary<int, int>();

            for (int i = 0; i < 3; i++)
            {
                int sid = experts[i];
                dictExpert[i] = 0;

                SkillData skill = skills.Where(m => m.SkillId == sid).FirstOrDefault();
                if (skill != null)
                {
                    SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), user.GetTalentList(skill.SkillId), true);
                    list.Add(skillPanel);

                    dictExpert[i] = (int)skillPanel.Percent;
                }
            }

            //再根据精通加载其他技能
            foreach (var skill in skills)
            {
                if (skill.SkillConfig.Type != (int)SkillType.Expert)
                {
                    int risePercent = dictExpert[skill.SkillConfig.Role - 1];
                    SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), user.GetTalentList(skill.SkillId), risePercent, true);

                    list.Add(skillPanel);
                }
            }

            return list;
        }

    }
}
