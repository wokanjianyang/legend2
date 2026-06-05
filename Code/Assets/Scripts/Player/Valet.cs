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
            double asp = this.Master.AttributeBonus.CalPanelSingleAttr(AttributeEnum.Speed) + this.SkillPanel.Speed;
            double msp = this.Master.AttributeBonus.CalPanelSingleAttr(AttributeEnum.MoveSpeed);

            int role = SkillPanel.Config.Role;

            double mAtk = Master.AttributeBonus.CalBaseRoleAtk(role); //职业攻击

            double mHp = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.HP);
            double mDef = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Def);

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, mHp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroPanel, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroPanel, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroPanel, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, mDef); //降低50%继承

            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MoveSpeed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Curse));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Accuracy));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Miss));

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRate) + SkillPanel.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamage) + SkillPanel.CritDamage);
            AttributeBonus.SetAttr(AttributeEnum.DeadlyRate, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyRate) + SkillPanel.DeadlyRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyDamage) + SkillPanel.DeadlyDamage);
            AttributeBonus.SetAttr(AttributeEnum.DeadlyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageIncrea) + SkillPanel.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageResist));

            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Strong));
            AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Shatter));
            AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Parry));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SpiritDamage));

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
                    SkillData skillData = GameProcessor.Inst.User.SkillList.Where(m => m.SkillConfig.Id == skillId).FirstOrDefault();


                    if (Master.Camp == PlayerType.Hero)
                    {
                        User user = GameProcessor.Inst.User;

                        if (skillData == null)
                        {
                            //白虎的飓风破
                            skillData = new SkillData(skillId, (int)SkillPosition.Default);
                            skillData.MagicLevel.Data = SkillPanel.Level;
                        }

                        int petRate = user.GetPetSkillRate(skillData.SkillConfig.Role);

                        SkillPanel from = null;
                        if (skillData.SkillConfig.FromId > 0)
                        {
                            SkillData fromData = user.SkillList.Where(m => m.SkillId == skillData.SkillConfig.FromId).FirstOrDefault();

                            if (fromData == null)
                            {
                                fromData = new SkillData(skillId, (int)SkillPosition.Default);
                                fromData.MagicLevel.Data = SkillPanel.Level;
                            }

                            from = new SkillPanel(fromData, user.GetRuneList(fromData.SkillId), user.GetSuitList(fromData.SkillId), user.GetTalentList(fromData.SkillId), true);
                        }

                        List<SkillRune> runeList = user.GetRuneList(skillData.SkillId);
                        List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);
                        List<SkillTalent> talentList = user.GetTalentList(skillData.SkillId);
                        //if (skillId == 4004)
                        //{
                        //    //飓风破，使用白虎的吸血效果
                        //    SkillRune rune = user.GetRuneList(3012, null).Where(m => m.EffectId == 101).FirstOrDefault();
                        //    if (rune != null)
                        //    {
                        //        runeList.Add(rune);
                        //    }
                        //}

                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talentList, false);

                        SkillState skill = new SkillState(this, skillPanel, from, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                    else
                    {
                        skillData = new SkillData(skillId, (int)SkillPosition.Default);
                        skillData.MagicLevel.Data = SkillPanel.Level;

                        List<SkillRune> runeList = new List<SkillRune>();
                        List<SkillSuit> suitList = new List<SkillSuit>();


                        SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

                        SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        SelectSkillList.Add(skill);
                    }
                }
            }


            if (Master.Camp == PlayerType.Hero) //继承护体戒指
            {
                User user = GameProcessor.Inst.User;
                SkillData skillData = user.SkillList.Where(m => m.SkillConfig.Id == 3010).FirstOrDefault();

                int[] ringId = { 2, 4, }; //6
                int[] skillId = { 4002, 1008 }; //3008

                if (skillData != null && skillData.GetDivineLevel() > 0)
                {
                    for (int i = 0; i < ringId.Length; i++)
                    {
                        long ringLevel = user.GetRingLevel(ringId[i]);
                        SkillData sd = new SkillData(skillId[i], 0);
                        long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
                        sd.MagicLevel.Data = rp;

                        SkillPanel skillPanel = new SkillPanel(sd, null, null, null, false);
                        SkillState skill = new SkillState(this, skillPanel, 0, 0);
                        SelectSkillList.Add(skill);
                    }

                    //for (int i = 0; i < ringId.Length; i++)
                    //{
                    //    long ringLevel = user.GetRingLevel(ringId[i]);
                    //    SkillData sd = new SkillData(skillId[i], 0);
                    //    long rp = Math.Max(1, ringLevel * skillData.GetDivineLevel() * 20 / 100);
                    //    sd.MagicLevel.Data = rp;

                    //    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
                    //    List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                    //    SkillPanel skillPanel = new SkillPanel(sd, runeList, suitList, false, RuleType, 0);

                    //    SkillState skill = new SkillState(this, skillPanel, 0, 0);
                    //    SelectSkillList.Add(skill);
                    //}
                }
            }
        }



        //public override APlayer CalcEnemy()
        //{
        //    //攻击主人的目标
        //    var mm = this.Master.CalcEnemy();

        //    return mm != null ? mm : base.CalcEnemy();
        //}

        public override void OnHit(DamageResult dr)
        {
            //Debug.Log("valet hit damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));

            base.OnHit(dr);
        }
    }
}
