using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Hero : APlayer
    {
        private Dictionary<int, long> skillUseCache = new Dictionary<int, long>();

        public List<SkillState> DoubleHitSkillList { get; set; } = new List<SkillState>();

        public Hero(RuleType ruleType) : base()
        {
            this.GroupId = 1;
            this.RuleType = ruleType;

            this.Init();

            this.EventCenter.AddListener<HeroLevelUp>(LevelUp);
            this.EventCenter.AddListener<HeroAttrChangeEvent>(HeroAttrChange);
            this.EventCenter.AddListener<HeroBuffChangeEvent>(OnHeroBuffChange);

            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateAllSkillEvent);

        }

        private void LevelUp(HeroLevelUp e)
        {
            User user = GameProcessor.Inst.User;
            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //��������ֵ
            //this.ShowUI(); //����UI
        }

        public void HeroAttrChange(HeroAttrChangeEvent e)
        {
            User user = GameProcessor.Inst.User;
            this.SetAttr(user);  //��������ֵ
        }

        private void OnHeroUpdateAllSkillEvent(HeroUpdateSkillEvent e)
        {
            this.UpdateSkills();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //��������ֵ
            this.SetSkill(user); //���ü���

            base.Load();
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //����Buff
            if (RuleType == RuleType.Defend)
            {
                List<DefendBuffConfig> buffList = user.DefendData.GetBuffList(DefendBuffType.Attr);

                this.AttributeBonus.SetBuffList(buffList);
            }

            //���û�������ԣ�����ս���Ļ�������

            if (RuleType == RuleType.HeroPhantom)
            {
                AttributeBonus.SetAttr(AttributeEnum.MulHp, AttributeFrom.HeroPanel, ConfigHelper.PvpRate * 100);
            }

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def));
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MoveSpeed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageResist));
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.InheritIncrea));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHp, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RestoreHp));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RestoreHpPercent));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.SkillValetCount, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SkillValetCount));
            AttributeBonus.SetAttr(AttributeEnum.SkillValetSpeed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SkillValetSpeed));

            //this.AurasList = new List<AAuras>();
            //foreach (var ac in user.GetAurasList())
            //{
            //    AAuras auras = AurasFactory.BuildAuras(this, ac.Key, ac.Value);
            //    this.AurasList.Add(auras);
            //}

            if (user.SoulRingData.Count > 0)
            {
                this.RingType = 1;
            }

            //������ǰѪ��
            this.SetAttackSpeed((int)AttributeBonus.GetTotalAttrDouble(AttributeEnum.Speed));
            this.SetMoveSpeed((int)AttributeBonus.GetTotalAttrDouble(AttributeEnum.MoveSpeed));

            double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            SetHP(maxHP);
            //Debug.Log("Hero Hp:" + StringHelper.FormatNumber(maxHP));
        }

        private void OnHeroBuffChange(HeroBuffChangeEvent e)
        {
            //����Buff
            if (RuleType == RuleType.Defend)
            {
                List<DefendBuffConfig> buffList = GameProcessor.Inst.User.DefendData.GetBuffList(DefendBuffType.Attr);
                this.AttributeBonus.SetBuffList(buffList);

                double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
                SetHP(maxHP);
                //Debug.Log("Hero Hp:" + StringHelper.FormatNumber(maxHP));
            }
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            List<SkillData> list = user.GetCurrentSkill();
            list.Add(new SkillData(9001, (int)SkillPosition.Default));

            if (RuleType == RuleType.Defend)
            {
                List<int> ids = user.GetCurrentSkillList();

                List<DefendBuffConfig> buffList = user.DefendData.GetBuffList(DefendBuffType.Skill);
                foreach (DefendBuffConfig config in buffList)
                {
                    if (!ids.Contains(config.SkillId))
                    {
                        var sd = new SkillData(config.SkillId, (int)SkillPosition.Default);
                        sd.MagicLevel.Data = 500;
                        list.Add(sd);
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                SkillData skillData = list[i];

                List<SkillRuneConfig> buffRuneList = null;
                if (RuleType == RuleType.Defend)
                {
                    buffRuneList = user.DefendData.GetBuffRuneList(skillData.SkillId);
                }

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, buffRuneList);

                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

                SkillState skill = new SkillState(this, skillPanel, i, 0);
                SelectSkillList.Add(skill);
            }

            InitDoubleHitSkill(user);
        }

        private void InitDoubleHitSkill(User user)
        {
            DoubleHitSkillList.Clear();

            foreach (var kv in user.ExclusivePanelList[user.ExclusiveIndex])
            {
                ExclusiveItem exclusive = kv.Value;

                if (exclusive.DoubleHitId > 0)
                {
                    int skillId = exclusive.DoubleHitConfig.SkillId;

                    SkillData skillData = user.SkillList.Where(m => m.SkillId == skillId).FirstOrDefault();

                    if (skillData == null)
                    {
                        break;
                    }

                    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
                    List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                    SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

                    SkillState skill = DoubleHitSkillList.Where(m => m.SkillPanel.SkillId == skillId).FirstOrDefault();

                    if (skill == null)
                    {
                        skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        DoubleHitSkillList.Add(skill);
                    }
                    skill.AddRate(exclusive.DoubleHitConfig.Rate);
                }
            }
        }

        private void UpdateSkills()
        {
            foreach (var skillState in SelectSkillList)
            {
                skillUseCache[skillState.SkillPanel.SkillId] = skillState.LastUseTime;
            }

            var user = GameProcessor.Inst.User;

            this.SetSkill(user);

            foreach (var skillState in SelectSkillList)
            {
                skillUseCache.TryGetValue(skillState.SkillPanel.SkillId, out long LastUseTime);
                if (LastUseTime > 0)
                {
                    skillState.SetLastUseTime(LastUseTime);
                }
            }
        }

        public override float AttackLogic()
        {
            //Debug.Log("����:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
            //Debug.Log("����:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" ����:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

            //Debug.Log("˲��ħ���˺�:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

            //1. ����ǰ������ż�����
            SkillState skill;

            //3.Ѱ��Ŀ��
            _enemy = this.CalcEnemy();

            //4 ���Թ�������Ŀ��
            skill = this.GetSkill(0);
            if (skill != null)
            {  //ʹ�ü���
                //Debug.Log($"{(this.Name)}ʹ�ü���:{(skill.SkillPanel.SkillData.SkillConfig.Name)},����:" + targets.Count + "��");
                skill.Do();
                //this.EventCenter.Raise(new ShowAttackIcon ());

                if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
                {
                    this.DoubleHit();
                }

                return AttckSpeed;
            }

            //5.��Ŀ���ƶ�
            if (_enemy != null)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return MoveSpeed;
                }
            }

            //6 ���Ը��Ĺ���Ŀ��
            if (_enemy != null)
            {
                _enemy.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            }
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                //�������Ŀ��
                skill = this.GetSkill(0);

                //6.1 �ȹ�����Ŀ��
                if (skill != null)
                {
                    skill.Do();
                    if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
                    {
                        this.DoubleHit();
                    }
                    return AttckSpeed;
                }
                else
                {
                    //�������������ƶ���ȥ
                    var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                    if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                    {
                        this.Move(endPos);
                        return MoveSpeed;
                    }
                }
            }

            return AttckSpeed;
        }

        private void DoubleHit()
        {
            foreach (SkillState skill in this.DoubleHitSkillList)
            {
                if (RandomHelper.RandomRate(skill.Rate))
                {
                    skill.Do();
                    //Debug.Log(" Double Hit " + skill.SkillPanel.SkillData.SkillConfig.Name);
                    return;
                }
            }
        }

        public override APlayer CalcEnemy()
        {
            var ret = base.CalcEnemy();

            ret?.EventCenter.Raise(new ShowAttackIcon { NeedShow = true });

            return ret;
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Resurrection()
        {
            //this.ResetData();
            this._enemy = null;
        }

        public void UpdateEnemy(APlayer player)
        {
            this._enemy = player;
        }
    }
}
