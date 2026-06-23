using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillMapState
    {
        //public APlayer SelfPlayer { get; set; }

        private Skill_Attack_Map skill;

        private float CD = 1;

        private float TotalTime = 0;
        private float RunTime = 0;

        private bool complete = false;

        public SkillMapState(Skill_Attack_Map skill, float cd)
        {
            //this.SelfPlayer = player;
            this.skill = skill;
            this.CD = cd;
            this.TotalTime = 0;
        }

        public void Run(APlayer enemy, float time)
        {
            this.TotalTime += time;
            this.RunTime += time;

            if (enemy != null && !complete && RunTime >= CD)
            {
                RunTime = 0;
                skill.Run(enemy);
            }

            if (TotalTime >= skill.SkillPanel.Duration)
            {
                complete = true;
            }
        }

        public bool IsOver()
        {
            return complete;
        }
    }
}
