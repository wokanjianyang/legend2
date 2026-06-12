using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Defend : APlayer
    {
        public int Progress;
        MonsterDefendConfig Config { get; set; }
        MonsterQualityConfig QualityConfig { get; set; }

        private int Layer = 0;

        public Monster_Defend(int layer, long progess, int quality) : base()
        {
            this.GroupId = 2;
            this.Layer = layer;
            this.Progress = (int)progess;
            this.Quality = quality;


            this.Config = MonsterDefendConfigCategory.Instance.GetByLayerAndLevel(this.Layer, this.Progress);

            this.QualityConfig = MonsterQualityConfigCategory.Instance.Get(this.Quality);

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = Config.Name + QualityConfig.MonsterTitle;
            this.Level = (Layer - 1) * 100 + this.Progress;
            this.FashionId = (this.Progress + 1) / 2;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            int riseLevel = Progress - Config.StartLevel;

            double riseHp = Math.Pow(Config.RiseHp, riseLevel);
            double riseAttr = Math.Pow(Config.RiseAtk, riseLevel);
            double riseDef = Math.Pow(Config.RiseDef, riseLevel);

            double riseResist = Config.MulResistRise * riseLevel;
            double riseMul = Config.MulRise * riseLevel;
            double riseMiss = Config.RiseMiss * riseLevel;
            double RiseAccuracy = Config.RiseAccuracy * riseLevel;

            double hp = StringHelper.StringToNumber(Config.Hp) * (1 + riseHp);
            double attr = StringHelper.StringToNumber(Config.Atk) * (1 + riseAttr);
            double def = StringHelper.StringToNumber(Config.Def) * (1 + riseDef);

            double resistMul = StringHelper.StringToNumber(Config.MulResist) * (1 + riseResist);
            double damageMul = StringHelper.StringToNumber(Config.DamageMul) * (1 + riseMul);

            //if (Progress >= 100)
            //{
            //    Debug.Log("hp:" + hp);
            //    Debug.Log("attr:" + attr);
            //    Debug.Log("def:" + def);
            //}
            //Debug.Log("Defend " + this.Progress + " HP:" + StringHelper.FormatNumber(hp));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp * QualityConfig.HpRate);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroBase, attr * QualityConfig.AttrRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def * QualityConfig.DefRate);

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
        }



        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            //random model
            List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.StartMapId == 0).ToList();
            int index = RandomHelper.RandomNumber(0, models.Count);
            PlayerModel model = models[index];

            if (model.SkillList != null)
            {
                for (int i = 0; i < model.SkillList.Length; i++)
                {
                    list.Add(new SkillData(model.SkillList[i], i)); //增加默认技能
                }
            }

            this.Name = model.Name + "·" + Config.Name;


            foreach (SkillData skillData in list)
            {
                SkillPanel skillPanel = new SkillPanel(skillData, null, null, null, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        public override float AttackLogic()
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            if (hero != null)
            {
                this._enemy = hero;
            }

            return base.AttackLogic();
        }
    }
}
