using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Hero_Pet : APlayer
    {
        public APlayer Master { get; set; }
        private Pet Self { get; set; }


        private int Life = 0;

        public Hero_Pet(APlayer master, Pet pet) : base()
        {
            this.GroupId = master.GroupId;

            this.Master = master;
            this.Self = pet;
            //this.FashionId = pet.Mid;
            this.FashionId = pet.ConfigId;

            this.Init();
        }

        private void Init()
        {
            this.Camp = PlayerType.Hero_Pet;

            this.Level = Self.PetLevel.Data;

            this.Name = Self.GetName();

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double levelRise = (1 + this.Level * 0.01);

            double asp = this.Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Speed);
            double msp = this.Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MoveSpeed);

            int role = Self.Role;

            double mAtk = Master.AttributeBonus.CalBaseRoleAtk(role) * levelRise; //职业攻击

            double mHp = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.HP) * levelRise;
            double mDef = Master.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Def) * levelRise;

            this.AttributeBonus = new AttributeBonus();
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.UserBase, mHp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.UserBase, mAtk);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.UserBase, mDef); //降低50%继承

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
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            User user = User_Data_Manager.Data;
            double cd = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Cd);

            //加载技能
            for (int i = 0; i < Self.Skills.Count; i++)
            {
                SkillData skillData = new SkillData(Self.Skills[i].Key, i);
                skillData.MagicLevel.Data = Self.Skills[i].Value.Data;

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId);

                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                List<SkillTalent> talentList = user.GetTalentList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talentList, 0, cd, false);

                SkillState skill = new SkillState(this, skillPanel, null, i, 0);
                SelectSkillList.Add(skill);
            }

            //加载默认普通攻击
            SkillData sdf = new SkillData(9001, (int)SkillPosition.Default);
            SkillPanel spf = new SkillPanel(sdf, null, null, null, false);
            SkillState sf = new SkillState(this, spf, null, 999, 0);
            SelectSkillList.Add(sf);

        }

        //public override float DoEvent()
        //{
        //    long now = TimeHelper.ClientNowSeconds();
        //    long lf = now - BirthDay;
        //    //Debug.Log("life:" + lf);
        //    if (lf >= Life) //auto dead
        //    {
        //        this.HP = 0;

        //        GameProcessor.Inst.PlayerManager.RemoveDeadPlayers(this);

        //        return 999f;
        //    }
        //    else
        //    {
        //        return base.DoEvent();
        //    }
        //}

        //public override void OnHit(DamageResult dr)
        //{
        //    //if (this.GroupId == 1 && dr.Damage > 200000000)
        //    //{
        //    //    Debug.Log("duplication player hit damage:" + StringHelper.FormatNumber(dr.Damage));
        //    //}

        //    base.OnHit(dr);
        //}
    }
}
