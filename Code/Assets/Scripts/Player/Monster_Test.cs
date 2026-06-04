using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Monster_Test : APlayer
    {


        public Monster_Test() : base()
        {

            this.GroupId = 2;
            this.Quality = 5;

            this.RuleType = RuleType.Test;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;

            this.Name = "测试怪物";

            this.Level = 999;
            this.FashionId = 2;

            this.SetAttr();  //设置属性值
            this.SetSkillNew();


            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();


            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, 1E10);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroBase, 100);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroBase, 100);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroBase, 100);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, 0); //1000000

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, 0);

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, 0);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroBase, 0);
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.HeroBase, 0);

            this.SetSpeed(0, 0);

            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
        }

        private void SetSkillNew()
        {
            List<SkillData> list = new List<SkillData>();


            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能
            list.Add(new SkillData(3004, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                skillData.MagicLevel.Data = 1;

                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();


                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        public override void OnHit(DamageResult dr)
        {
            if (dr.SkillId >= 2003)
            {
                Debug.Log($"{DateTime.Now.ToString("mm:ss.fff")} test hit " + dr.SkillId + " damage：" + this.AttributeBonus.CalBattleTotalAttr(AttributeEnum.DecreExtraDamage));
            }
            //Debug.Log("monster hit damage:" + StringHelper.FormatNumber(dr.Damage));

            base.OnHit(dr);
        }
    }
}
