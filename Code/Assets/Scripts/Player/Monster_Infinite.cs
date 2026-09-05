using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Infinite : APlayer
    {
        public int Progress;
        InfiniteConfig Config { get; set; }
        InfiniteModelConfig ModelConfig { get; set; }

        private double[] HpRate = { 1, 2.5, 5, 7.5, 10 };
        private double[] DefRate = { 1, 2, 3, 4, 5 };
        private double[] AttrRate = { 1, 1.25, 1.5, 1.75, 2 };

        private int[] excludeSuitList = { };

        public Monster_Infinite(long progress, int quality) : base()
        {
            this.Progress = (int)progress;
            this.GroupId = 2;
            this.Quality = quality;
            this.RuleType = RuleType.Infinite;

            this.Config = InfiniteConfigCategory.Instance.GetByLevel(progress);

            this.ModelConfig = InfiniteModelConfigCategory.Instance.RandomConfig();

            this.Init();
        }

        private void Init()
        {
            MonsterQualityConfig qualityConfig = MonsterQualityConfigCategory.Instance.Get(this.Quality);

            this.Camp = PlayerType.Enemy;
            this.Name = ModelConfig.Name + qualityConfig.MonsterTitle;
            this.Level = Progress / 5 + 1;
            this.FashionId = (Progress - 1) / 10 + 1;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = this.Progress - Config.StartLevel;

            double hpRise = Math.Pow(Config.RiseHp, riseLevel);
            double defRise = Math.Pow(Config.RiseDef, riseLevel);
            double atkRise = Math.Pow(Config.RiseAtk, riseLevel);

            double hp = StringHelper.StringToNumber(Config.Hp) * hpRise;
            double atk = StringHelper.StringToNumber(Config.Atk) * defRise;
            double def = StringHelper.StringToNumber(Config.Def) * atkRise;



            //Debug.Log("Infinit " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));
            //Debug.Log("Infinit " + this.Progress + " Def:" + StringHelper.FormatNumber(def));
            //Debug.Log("Infinit " + this.Progress + " Attr:" + StringHelper.FormatNumber(attr));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, hp);
            AttributeBonus.SetAttr(AttributeEnum.Atk, AttributeFrom.ConfigBase, atk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, def);

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.ConfigBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.ConfigBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.ConfigBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.ConfigBase, Config.CritDamage);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.ConfigBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.ConfigBase, Config.Miss);

            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.ConfigBase, Config.MulDamageResist);
            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.ConfigBase, Config.MulDamageIncrea);

            this.SetSpeed(Config.Speed, Config.Speed);

            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            List<int> rdList = SkillConfigCategory.Instance.RandomList(Quality, this.Progress);

            for (int i = 0; i < rdList.Count; i++)
            {
                int skillId = rdList[i];
                SkillData skillData = new SkillData(skillId, i);
                skillData.MagicLevel.Data = this.Progress / 100;
                list.Add(skillData);
            }

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, this.Quality);
                List<SkillSuit> suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillData.SkillId, this.Quality, excludeSuitList);
                List<SkillTalent> talents = SkillTalentConfigCategory.Instance.GetAllTalent(skillData.SkillId, this.Quality);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talents, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }

            base.AddSkillNormal();

            base.SetSkillAfter();
        }
    }
}
