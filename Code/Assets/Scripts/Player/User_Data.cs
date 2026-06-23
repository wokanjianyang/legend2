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
            User user = User_Data_Manager.Data;
            List<SkillData> skills = user.SkillList;

            List<SkillPanel> list = new List<SkillPanel>();

            double cd = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Cd);

            //先加载3个精通
            int[] experts = new int[] { 1006, 2006, 3006 };
            Dictionary<int, int> dictExpert = new Dictionary<int, int>();

            int riseLevel = (int)(user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SkillLevelRise));

            for (int i = 0; i < 3; i++)
            {
                int sid = experts[i];

                int atrId = sid + 20000;
                int atrLevel = (int)(user.AttributeBonus.CalPanelTotalAttr((AttributeEnum)atrId));


                dictExpert[i] = 0;

                SkillData skill = skills.Where(m => m.SkillId == sid).FirstOrDefault();
                if (skill != null)
                {
                    SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), user.GetTalentList(skill.SkillId), 0, riseLevel + atrLevel, cd, true);
                    list.Add(skillPanel);

                    dictExpert[i] = (int)skillPanel.Percent;
                }
            }

            //再根据精通加载其他技能
            foreach (var skill in skills)
            {
                int atrId = skill.SkillId + 20000;
                int atrLevel = (int)(user.AttributeBonus.CalPanelTotalAttr((AttributeEnum)atrId));

                if (skill.SkillConfig.Type != (int)SkillType.Expert)
                {
                    int risePercent = dictExpert[skill.SkillConfig.Role - 1];
                    SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), user.GetTalentList(skill.SkillId), risePercent, riseLevel + atrLevel, cd, true);

                    list.Add(skillPanel);
                }
            }

            return list;
        }

    }
}
