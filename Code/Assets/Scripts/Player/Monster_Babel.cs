using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Babel : APlayer
    {
        public int Progress;
        public int Type;
        private int RiseRecord = 0;

        MonsterBabelConfig Config { get; set; }

        private string[] names = { "通天怪物", "通天恶魔", "通天邪魔" };

        public Monster_Babel(long progress, int type) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Babel;
            this.Quality = type + 3;

            this.Progress = (int)progress;
            this.Type = type;
            this.RiseRecord = (AppHelper.BabelMaxRecord - this.Progress - 1);

            this.Config = MonsterBabelConfigCategory.Instance.GetByProgress(progress);

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = names[Type - 1];
            this.Level = Progress;
            this.FashionId = (Progress - 1) / 100 + 1;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private double[] HpTypeList = new double[] { 1, 3, 6 };
        private double[] AtkTypeList = new double[] { 1, 1.5, 2 };
        private double[] DefTypeList = new double[] { 1, 2, 4 };

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = Progress - Config.StartLevel;

            double hpRise = Math.Pow(Config.RiseHp, riseLevel);
            double defRise = Math.Pow(Config.RiseDef, riseLevel);
            double atkRise = Math.Pow(Config.RiseAtk, riseLevel);

            double hp = StringHelper.StringToNumber(Config.Hp) * HpTypeList[Type - 1];
            double atk = StringHelper.StringToNumber(Config.Atk) * AtkTypeList[Type - 1];
            double def = StringHelper.StringToNumber(Config.Def) * DefTypeList[Type - 1];

            long missRise = (long)(Config.RiseMiss * riseLevel);
            long accuracyRise = (long)(Config.RiseAccuracy * riseLevel);
            long diRise = (long)(Config.DrRise * riseLevel);
            long drRise = (long)(Config.DrRise * riseLevel);
            long crRise = (long)(Config.CrRise * riseLevel);
            long crdRise = (long)(Config.CrdRise * riseLevel);

            //if (Progress >= 100)
            //{
            //    Debug.Log("hp:" + hp);
            //    Debug.Log("attr:" + attr);
            //    Debug.Log("def:" + def);
            //}
            //Debug.Log("Defend " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, hp * hpRise);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, atk * atkRise);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, atk * atkRise);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, atk * atkRise);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, def * defRise);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.ConfigBase, Config.DamageIncrea + diRise);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.ConfigBase, Config.DamageResist + drRise);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.ConfigBase, Config.CritRate + crRise);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.ConfigBase, Config.CritDamage + crdRise);

            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.ConfigBase, Config.Lucky);
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.ConfigBase, Config.Curse);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.ConfigBase, Config.Accuracy + accuracyRise);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.ConfigBase, Config.Miss + missRise);

            if (RiseRecord > 0)
            {
                AttributeBonus.SetAttr(AttributeEnum.DecreExtraDamage, AttributeFrom.ConfigBase, RiseRecord * 2);
            }

            this.SetSpeed(Config.Speed, Config.Speed);

            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));

            //Debug.Log("MISS:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.Miss));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            int[] SkillIdList;

            if (Type == 3)
            {
                SkillIdList = new int[] { 2002, 1004, 2004, 2007 };
            }
            else if (Type == 2)
            {
                SkillIdList = new int[] { 2002, 2003 };
            }
            else
            {
                SkillIdList = new int[] { 1002 };
            }

            int skl = this.Progress / 100 + 1;
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
