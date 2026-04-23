using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttackData
    {
        public int Tid { get; set; }

        public Vector3Int Cell { get; set; }
        public float Ratio { get; set; }

    }
    abstract public class ASkill : IPlayer
    {
        public APlayer SelfPlayer { get; set; }
        public SkillPanel SkillPanel { get; set; }

        protected List<AttackData> attackDataCache { get; set; }

        protected SkillGraphic skillGraphic { get; set; }

        public ASkill(APlayer player, SkillPanel skill)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skill;
        }
        abstract public void Do(SkillRunType runType);

        public virtual void Do(DamageResult baseDr)
        {

        }


        public int CalDistance(Vector3Int from, Vector3Int to)
        {
            return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
        }

        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }

        abstract public bool IsCanUse();
    }
}
