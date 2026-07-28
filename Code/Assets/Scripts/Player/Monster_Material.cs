using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Material : APlayer
    {
        public int Progress;
        public int Type;

        MaterialCopyConfig Config { get; set; }

        public Monster_Material(int type, long progress) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Materail;
            this.Quality = 3;

            this.Progress = (int)progress;
            this.Type = type;

            this.Config = MaterialCopyConfigCategory.Instance.GetByProgress(type, progress);

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = this.Config.MonsterName;
            this.Level = Progress;
            this.FashionId = (Progress - 1) / 100 + 1;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = Progress - Config.StartLevel;


            double hpRise = StringHelper.StringToNumber(Config.HpRise) * riseLevel;
            double defRise = StringHelper.StringToNumber(Config.DefRise) * riseLevel;
            double atkRise = StringHelper.StringToNumber(Config.AtrRise) * riseLevel;

            double hp = StringHelper.StringToNumber(Config.Hp) + hpRise;
            double atk = StringHelper.StringToNumber(Config.Atk) + defRise;
            double def = StringHelper.StringToNumber(Config.Def) + atkRise;

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, hp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, atk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, atk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, atk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, def);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.ConfigBase, (int)(Config.DamageIncrea * Progress));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.ConfigBase, (int)(Config.DamageResist * Progress));

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.ConfigBase, (int)(Config.Accuracy * Progress));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.ConfigBase, (int)(Config.Miss * Progress));


            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));

            //Debug.Log("MISS:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Miss));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            int[] SkillIdList;

            SkillIdList = new int[] { 2002 };

            int skl = this.Progress / 1000 + 1;
            for (int i = 0; i < SkillIdList.Length; i++)
            {
                int skillId = SkillIdList[i];

                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = Math.Min(skl, skillData.SkillConfig.MaxLevel);
                list.Add(skillData);
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //add default skill

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, 10);
                List<SkillSuit> suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillData.SkillId, 10);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);

            }

            base.SetSkillAfter();
        }


        //public override void OnHit(DamageResult dr)
        //{
        //    //if (this.Quality == 3)
        //    //{
        //    //    Debug.Log("monster babel " + this.Progeress + " hit damage:" + StringHelper.FormatNumber(dr.Damage));
        //    //}

        //    base.OnHit(dr);
        //}
    }
}
