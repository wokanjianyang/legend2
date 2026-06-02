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
        private int Record = 0;

        MonsterBabelConfig Config { get; set; }

        private string[] names = { "通天怪物", "通天恶魔", "通天邪魔" };

        public Monster_Babel(long progress, int type) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.Babel;
            this.Quality = type + 3;

            this.Progress = (int)progress;
            this.Type = type;
            this.Record = AppHelper.BabelRecord;

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

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = Progress - Config.StartLevel;

            double hp = StringHelper.StringToNumber(Config.Hp);
            double atk = StringHelper.StringToNumber(Config.Atk);
            double def = StringHelper.StringToNumber(Config.Def);

            double riseHp = Config.RiseHp * riseLevel * hp;
            double riseAtk = Config.RiseAtk * riseLevel * atk;
            double riseDef = Config.RiseDef * riseLevel * def;

            double riseResist = Config.MulResistRise * riseLevel;
            double riseMul = Config.MulRise * riseLevel;
            double riseMiss = Config.RiseMiss * riseLevel;
            double RiseAccuracy = Config.RiseAccuracy * riseLevel;

            double resistMul = StringHelper.StringToNumber(Config.MulResist) * (1 + riseResist);
            double damageMul = StringHelper.StringToNumber(Config.DamageMul) * (1 + riseMul);

            //if (Progress >= 100)
            //{
            //    Debug.Log("hp:" + hp);
            //    Debug.Log("attr:" + attr);
            //    Debug.Log("def:" + def);
            //}
            //Debug.Log("Defend " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp + riseHp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroBase, atk + riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroBase, atk + riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroBase, atk + riseAtk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def + riseDef);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroBase, Config.Lucky);
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.HeroBase, Config.Curse);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy + RiseAccuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss + riseMiss);

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroBase, damageMul);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroBase, resistMul);

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
                SkillIdList = new int[] { 2002, 2007 };
            }
            else if (Type == 2)
            {
                SkillIdList = new int[] { 2002 };
            }
            else
            {
                SkillIdList = new int[] { 1002 };
            }

            for (int i = 0; i < SkillIdList.Length; i++)
            {
                int skillId = SkillIdList[i];

                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = skillData.SkillConfig.MaxLevel;
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
