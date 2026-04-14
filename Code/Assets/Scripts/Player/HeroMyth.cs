using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class HeroMyth : Hero
    {
        private int Scale = 0;

        public HeroMyth(bool isfestive) : base()
        {
            this.GroupId = 1;
            this.RuleType = RuleType.Myth;
            this.IsFestive = isfestive;

            //this.Init();
        }

    //    private void Init()
    //    {
    //        User user = GameProcessor.Inst.User;
    //        this.Camp = PlayerType.Hero;
    //        this.Name = user.Name;
    //        this.Level = user.MagicLevel.Data;
    //        this.FashionId = user.FashionUpId;

    //        //double power = user.AttributeBonus.GetPower();
    //        //double scale = Math.Log10(power) - 9;
    //        double scale = 1;
    //        this.Scale = (int)scale;

    //        this.SetAttr(user);  //设置属性值
    //        this.SetSkill(user); //设置技能

    //        double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
    //        SetHP(maxHP);

    //        base.Load();
    //        this.Logic.SetData(null); //设置UI
    //    }

    //    private void SetAttr(User user)
    //    {
    //        this.AttributeBonus = new AttributeBonus();


    //        //回满当前血量
    //        SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
    //    }

    //    private void SetSkill(User user)
    //    {
            

    //        base.SetSkillAfter();
    //    }

    //    private void InitDoubleHitSkill(User user)
    //    {
    //        DoubleHitSkillList.Clear();

    //        foreach (var kv in user.ExclusivePanelList[user.ExclusiveIndex])
    //        {
    //            ExclusiveItem exclusive = kv.Value;

    //            if (exclusive.DoubleHitId > 0)
    //            {
    //                int skillId = exclusive.DoubleHitConfig.SkillId;

    //                SkillData skillData = user.SkillList.Where(m => m.SkillId == skillId).FirstOrDefault();

    //                if (skillData == null)
    //                {
    //                    break;
    //                }

    //                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
    //                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

    //                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

    //                SkillState skill = DoubleHitSkillList.Where(m => m.SkillPanel.SkillId == skillId).FirstOrDefault();

    //                if (skill == null)
    //                {
    //                    skill = new SkillState(this, skillPanel, skillData.Position, 0);
    //                    DoubleHitSkillList.Add(skill);
    //                }
    //                skill.AddRate(exclusive.DoubleHitConfig.Rate);
    //            }
    //        }
    //    }

    //    public override float AttackLogic()
    //    {
    //        //Debug.Log("生命:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
    //        //Debug.Log("减伤:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" 增伤:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

    //        //Debug.Log("瞬移魔法伤害:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

    //        //1. 控制前计算高优级技能
    //        SkillState skill;

    //        //3.寻找目标
    //        _enemy = this.CalcEnemy();

    //        //4 尝试攻击优先目标
    //        skill = this.GetSkill(0);
    //        if (skill != null)
    //        {  //使用技能
    //            //Debug.Log($"{(this.Name)}使用技能:{(skill.SkillPanel.SkillData.SkillConfig.Name)}");
    //            skill.Do();
    //            //this.EventCenter.Raise(new ShowAttackIcon ());

    //            if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
    //            {
    //                this.DoubleHit();
    //            }

    //            return AttckSpeed;
    //        }

    //        //5.朝目标移动
    //        if (_enemy != null)
    //        {
    //            var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
    //            if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
    //            {
    //                this.Move(endPos);
    //                return MoveSpeed;
    //            }
    //        }

    //        //6 尝试更改攻击目标
    //        if (_enemy != null)
    //        {
    //            GameProcessor.Inst.EventCenter.Raise(new ShowAttackIcon { NeedShow = true, Player = _enemy });
    //        }
    //        _enemy = this.FindNearestEnemy();
    //        if (_enemy != null)
    //        {
    //            //如果有新目标
    //            skill = this.GetSkill(0);

    //            //6.1 先攻击新目标
    //            if (skill != null)
    //            {
    //                skill.Do();
    //                if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
    //                {
    //                    this.DoubleHit();
    //                }
    //                return AttckSpeed;
    //            }
    //            else
    //            {
    //                //攻击不到，则移动过去
    //                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
    //                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
    //                {
    //                    this.Move(endPos);
    //                    return MoveSpeed;
    //                }
    //            }
    //        }

    //        return AttckSpeed;
    //    }

    //    private void DoubleHit()
    //    {
    //        foreach (SkillState skill in this.DoubleHitSkillList)
    //        {
    //            if (RandomHelper.RandomRate(skill.Rate))
    //            {
    //                skill.Do(SkillRunType.Double);
    //                //Debug.Log(" Double Hit " + skill.SkillPanel.SkillData.SkillConfig.Name);
    //                return;
    //            }
    //        }
    //    }

    //    public override void OnHit(DamageResult dr)
    //    {
    //        //if (dr.Damage > 10000)
    //        //{
    //        //    Debug.Log("heor hit by skill " + dr.SkillId + " damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));
    //        //}

    //        base.OnHit(dr);
    //    }
    }
}
