using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Valet : APlayer
    {
        public APlayer Master { get; set; }
        private SkillPanel SkillPanel { get; set; }

        private ValetModelConfig ModelConfig { get; set; }

        public Valet(APlayer player, SkillPanel skill) : base()
        {
            this.GroupId = player.GroupId;
            this.Master = player;
            this.SkillPanel = skill;
            this.RuleType = player.RuleType;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Valet;
            this.Level = SkillPanel.Level;

            this.ModelConfig = ValetModelConfigCategory.Instance.GetAll().Values.Where(m => m.FromSkillId == SkillPanel.SkillId).FirstOrDefault();

            this.FashionId = ModelConfig.ModelType;
            this.Name = ModelConfig.Name + "(" + Master.Name + ")";


            //白虎新继承
            this.SetAttr();
            this.SetSkill();

            base.Load();
            this.Logic.SetData(null); //设置UI
        }


        private void SetAttr()
        {
            double asp = this.Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Speed) + this.SkillPanel.Speed;
            double msp = this.Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MoveSpeed);

            int role = SkillPanel.Config.Role;

            double levelRise = this.SkillPanel.Percent * 0.01;
            double mAtk = Master.AttributeBonus.CalBaseRoleAtk(role) * levelRise; //职业攻击

            double mHp = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.HP);
            double mDef = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Def);

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.UserBase, mHp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.UserBase, mDef); 

            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MoveSpeed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Curse));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Accuracy));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Miss));
            AttributeBonus.SetAttr(AttributeEnum.RestoreIncrea, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.RestoreIncrea));

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.DeadlyRate, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyDamage));
            AttributeBonus.SetAttr(AttributeEnum.DeadlyDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageResist));

            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Strong));
            AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Shatter));
            AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Parry));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.AchievementDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.AchievementDamage));
            AttributeBonus.SetAttr(AttributeEnum.CardDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CardDamage));
            AttributeBonus.SetAttr(AttributeEnum.FashionDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.FashionDamage));
            AttributeBonus.SetAttr(AttributeEnum.LegacyDamage, AttributeFrom.UserBase, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.LegacyDamage));

            this.SetSpeed((int)asp, (int)msp);
            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));

            //Debug.Log("白虎 速度:" + StringHelper.FormatNumber(ModelConfig.SpeedRate + sp));
        }

        private void SetSkill()
        {
            //加载技能
            if (this.ModelConfig.SkillList != null)
            {
                foreach (int skillId in this.ModelConfig.SkillList)
                {
                    SkillData skillData = new SkillData(skillId, (int)SkillPosition.Default);
                    skillData.MagicLevel.Data = SkillPanel.Level;

                    SkillPanel skillPanel = new SkillPanel(skillData, null, null, null, false);

                    SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                    SelectSkillList.Add(skill);
                }
            }


            if (Master.Camp == PlayerType.Hero)
            {
                int c1001 = this.SkillPanel.EffectList.Where(m => m.Key == 1001).Count();
                if (c1001 > 0)
                {
                    List<SkillPanel> skills = User_Data.GetSkills();
                    SkillPanel masterSkill = skills.Where(m => m.SkillId == 3002).FirstOrDefault();

                    if (masterSkill != null)
                    {
                        SkillState skill = new SkillState(this, masterSkill, null, 1, 0);
                        SelectSkillList.Add(skill);
                    }
                }
            }

            AddSkillNormal();
            //if (Master.Camp == PlayerType.Hero) //继承护体戒指
            //{
            //    User user = User_Data_Manager.Data;
            //    SkillData skillData = user.SkillList.Where(m => m.SkillConfig.Id == 3010).FirstOrDefault();

            //    int[] ringId = { 2, 4, }; //6
            //    int[] skillId = { 4002, 1008 }; //3008

            //    if (skillData != null && skillData.GetDivineLevel() > 0)
            //    {
            //        for (int i = 0; i < ringId.Length; i++)
            //        {
            //            long ringLevel = user.GetRingLevel(ringId[i]);
            //            SkillData sd = new SkillData(skillId[i], 0);
            //            long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
            //            sd.MagicLevel.Data = rp;

            //            SkillPanel skillPanel = new SkillPanel(sd, null, null, null, false);
            //            SkillState skill = new SkillState(this, skillPanel, 0, 0);
            //            SelectSkillList.Add(skill);
            //        }

            //        for (int i = 0; i < ringId.Length; i++)
            //        {
            //            long ringLevel = user.GetRingLevel(ringId[i]);
            //            SkillData sd = new SkillData(skillId[i], 0);
            //            long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
            //            sd.MagicLevel.Data = rp;

            //            List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
            //            List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

            //            SkillPanel skillPanel = new SkillPanel(sd, runeList, suitList, false, RuleType, 0);

            //            SkillState skill = new SkillState(this, skillPanel, 0, 0);
            //            SelectSkillList.Add(skill);
            //        }
            //    }
            //}
        }



        public override APlayer CalcEnemy()
        {
            //攻击主人的目标
            var mm = this.Master.CalcEnemy();

            return mm != null ? mm : base.CalcEnemy();
        }

        public override void OnHit(DamageResult dr)
        {
            //Debug.Log("valet hit damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));

            base.OnHit(dr);
        }
    }
}
