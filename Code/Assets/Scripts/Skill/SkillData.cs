using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillData
    {
        public int SkillId { get; set; }
        public MagicData MagicExp { get; set; } = new MagicData();
        public MagicData MagicLevel { get; set; } = new MagicData();

        //技能状态
        public SkillStatus Status { get; set; }

        //装配位置
        public int Position { get; set; }

        public bool Recovery { get; set; } = false;

        public Dictionary<int, MagicData> DivineData = new Dictionary<int, MagicData>();

        [JsonIgnore]
        public SkillConfig SkillConfig { get; set; }

        public long GetLevelUpExp()
        {
            long rate = 50;

            long tempLevel = MagicLevel.Data;

            if (tempLevel < 1000)
            {
                rate = 10;
            }
            else if (tempLevel >= 1000 && tempLevel < 1500)
            {
                rate = 20;
            }
            else if (tempLevel >= 1500 && tempLevel < 2000)
            {
                rate = 30;
            }
            else if (tempLevel >= 2000 && tempLevel < 2500)
            {
                rate = 40;
            }
            else
            {
                rate = 50;
            }

            //if (tempLevel >= SkillConfig.MaxLevel)
            //{
            //    rate = 9999999;
            //}

            return rate * SkillConfig.Exp;
        }

        public SkillData(int skillId, int position)
        {
            this.Position = position;
            SkillConfig = SkillConfigCategory.Instance.Get(skillId);
            this.SkillId = SkillConfig.SkillId;
        }

        public void AddExp(long exp)
        {
            User user = GameProcessor.Inst.User;

            this.MagicExp.Data += exp;
            while (this.MagicExp.Data >= GetLevelUpExp() && this.MagicLevel.Data < user.GetSkillLimit(SkillConfig))
            {
                var upExp = GetLevelUpExp();
                this.MagicLevel.Data++;
                this.MagicExp.Data -= upExp;
            }
        }

        public long GetDivineItemLevel(int divinePart)
        {
            if (!DivineData.ContainsKey(divinePart))
            {
                DivineData[divinePart] = new MagicData();
            }

            return DivineData[divinePart].Data;
        }

        public void AddDivineItemLevel(int divinePart)
        {
            if (!DivineData.ContainsKey(divinePart))
            {
                DivineData[divinePart] = new MagicData();
            }

            DivineData[divinePart].Data++;
        }

        public long GetDivineLevel()
        {
            if (DivineData.Count == 10)
            {
                return DivineData.Select(m => m.Value.Data).Min();
            }

            return 0;
        }
        //----------------
    }

    public enum SkillType
    {
        Passive = 1,//被动技能
        Single_Attack = 2,  //单体攻击技能
        Mutil_Attack = 3, //范围攻击技能
        Valet = 4, //召唤技能
        Map = 5,  //场景技能（比如火墙）
        Shield = 6,//护盾技能
        Expert = 7,//专精技能
        Restore = 8, //恢复技能
        Row_Attack = 9, //直线攻击
    }

    public enum SkillStatus
    {
        /// <summary>
        /// 未学习
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 已学习
        /// </summary>
        Learn = 1,
        /// <summary>
        /// 已装配
        /// </summary>
        Equip = 2,
    }

    public enum SkillPosition
    {
        Default = 999999
    }
}
