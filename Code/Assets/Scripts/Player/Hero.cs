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
        private Dictionary<int, float> SkillCDCache = new Dictionary<int, float>();

        public List<SkillState> DoubleHitSkillList { get; set; } = new List<SkillState>();

        public Hero()
        {

        }

        public Hero(RuleType ruleType) : base()
        {
            this.GroupId = 1;
            this.RuleType = ruleType;

            this.Init();

            this.EventCenter.AddListener<HeroLevelUp>(LevelUp);
            this.EventCenter.AddListener<HeroAttrChangeEvent>(HeroAttrChange);
            this.EventCenter.AddListener<HeroBuffChangeEvent>(OnHeroBuffChange);
            this.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateAllSkillEvent);
        }

        public override void Reset()
        {
            base.Reset();

            User user = GameProcessor.Inst.User;
            this.SetSkill(user); //设置技能,先设置技能，因为技能会影响属性
            this.SetAttr(user);  //设置属性值
        }

        private void LevelUp(HeroLevelUp e)
        {
            User user = GameProcessor.Inst.User;
            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //设置属性值
            this.Logic.SetData(null); //设置UI
        }

        public void HeroAttrChange(HeroAttrChangeEvent e)
        {


            User user = GameProcessor.Inst.User;

            Debug.Log("HeroAttrChangeEvent atk:" + user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PhyAtk));

            this.SetAttr(user);  //设置属性值
        }


        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.MagicLevel.Data;
            this.FashionId = user.FashionUpId;

            this.SetSkill(user); //设置技能,先设置技能，因为技能会影响属性
            this.SetAttr(user);  //设置属性值

            double maxHP = AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
            SetHP(maxHP);

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            ////计算Buff
            //if (RuleType == RuleType.Defend)
            //{
            //    List<DefendBuffConfig> buffList = user.DefendData.GetBuffList();

            //    this.AttributeBonus.SetBuffList(buffList);
            //}

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.HP));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Def));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PhyAtk));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MagicAtk));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SpiritAtk));

            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MoveSpeed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Curse));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Accuracy));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Miss));

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.DeadlyRate, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyRate));
            AttributeBonus.SetAttr(AttributeEnum.DeadlyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DeadlyDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.DamageResist));

            AttributeBonus.SetAttr(AttributeEnum.Strong, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Strong));
            AttributeBonus.SetAttr(AttributeEnum.Shatter, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Shatter));
            AttributeBonus.SetAttr(AttributeEnum.Parry, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.Parry));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.AchievementDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.AchievementDamage));
            AttributeBonus.SetAttr(AttributeEnum.CardDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.CardDamage));
            AttributeBonus.SetAttr(AttributeEnum.FashionDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.FashionDamage));
            AttributeBonus.SetAttr(AttributeEnum.LegacyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.LegacyDamage));

            //此处不能回血，因为会修改人物属性之类的
        }

        private void OnHeroBuffChange(HeroBuffChangeEvent e)
        {
            //计算Buff
            //if (RuleType == RuleType.Defend)
            //{
            //    List<DefendBuffConfig> buffList = GameProcessor.Inst.User.DefendData.GetBuffList();
            //    this.AttributeBonus.SetBuffList(buffList);

            //    double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            //    SetHP(maxHP);
            //    //Debug.Log("Hero Hp:" + StringHelper.FormatNumber(maxHP));
            //}
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            List<SkillData> list = new List<SkillData>();
            foreach (KeyValuePair<int, Data.MagicData> sp in user.RingData)
            {
                long ringLevel = sp.Value.Data;
                RingConfig ringConfig = RingConfigCategory.Instance.Get(sp.Key);

                if (ringLevel >= ringConfig.RequireLevel && !user.RingSelect.ContainsKey(sp.Key))
                {
                    if (ringConfig.SkillId > 0)
                    {
                        SkillData sd = list.Where(m => m.SkillId == ringConfig.SkillId).FirstOrDefault();
                        if (sd == null)
                        {
                            sd = user.SkillList.Where(m => m.SkillId == ringConfig.SkillId).FirstOrDefault();
                            if (sd == null)  //没有学习，则默认为1级
                            {
                                sd = new SkillData(ringConfig.SkillId, 0);
                                sd.MagicLevel.Data = 1;
                            }
                            list.Add(sd); //没有上阵，则自动上阵
                        }
                    }
                }
            }

            List<int> rids = list.Select(m => m.SkillId).ToList();
            list.AddRange(user.GetCurrentSkill(rids));


            list.Add(new SkillData(9001, (int)SkillPosition.Default));

            //Debug.Log("skill list:" + list.Select(m => m.SkillId).ToList().ListToString());

            for (int i = 0; i < list.Count; i++)
            {
                SkillData skillData = list[i];

                List<SkillRuneConfig> buffRuneList = null;

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, buffRuneList);

                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                List<SkillTalent> talentList = user.GetTalentList(skillData.SkillId);

                int petRate = user.GetPetSkillRate(skillData.SkillConfig.Role);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talentList, true, RuleType, petRate);

                SkillPanel from = null;
                if (skillPanel.SkillData.SkillConfig.FromId > 0)
                {
                    SkillData fromData = user.SkillList.Where(m => m.SkillId == skillPanel.SkillData.SkillConfig.FromId).FirstOrDefault();

                    if (fromData == null)
                    {
                        continue;
                    }

                    from = new SkillPanel(fromData, user.GetRuneList(fromData.SkillId, null), user.GetSuitList(fromData.SkillId), user.GetTalentList(fromData.SkillId), true, RuleType, petRate);
                }

                SkillState skill = new SkillState(this, skillPanel, from, i, 0);
                SelectSkillList.Add(skill);
            }

            base.SetSkillAfter();
        }


        private void OnHeroUpdateAllSkillEvent(HeroUpdateSkillEvent e)
        {
            this.UpdateSkills();
        }

        private void UpdateSkills()
        {
            foreach (var skillState in SelectSkillList)
            {
                SkillCDCache[skillState.SkillPanel.SkillId] = skillState.CD;
            }

            var user = GameProcessor.Inst.User;

            this.SetSkill(user);

            foreach (var skillState in SelectSkillList)
            {
                SkillCDCache.TryGetValue(skillState.SkillPanel.SkillId, out float cd);
                if (cd > 0)
                {
                    skillState.CD = cd;
                }
            }
        }

        public override float AttackLogic()
        {
            //Debug.Log("生命:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
            //Debug.Log("减伤:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" 增伤:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

            //Debug.Log("瞬移魔法伤害:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

            //1. 控制前计算高优级技能
            SkillState skill;

            //3.寻找目标
            _enemy = this.CalcEnemy();

            //4 尝试攻击优先目标
            skill = this.GetSkill(0);
            if (skill != null)
            {  //使用技能
                //Debug.Log($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {(this.Name)}使用技能：{(skill.SkillPanel.SkillData.SkillConfig.Name)}-{skill.UserCount}-技能攻速：{skill.SkillPanel.Speed}");
                skill.Do();
                //this.EventCenter.Raise(new ShowAttackIcon ());

                return CalAtkInterval(skill.SkillPanel.Speed);
            }

            //5.朝目标移动
            if (_enemy != null)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return CalMoveInterval();
                }
            }

            //6 尝试更改攻击目标
            if (_enemy == null)
            {
                _enemy = this.FindNearestEnemy();
                if (_enemy != null)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowAttackIcon { NeedShow = true, Player = _enemy });
                }
            }


            if (_enemy != null)
            {
                //如果有新目标
                skill = this.GetSkill(0);

                //6.1 先攻击新目标
                if (skill != null)
                {
                    skill.Do();
                    return CalAtkInterval(skill.SkillPanel.Speed);
                }
                else
                {
                    //攻击不到，则移动过去
                    var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                    if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                    {
                        this.Move(endPos);

                        return CalMoveInterval();
                    }
                }
            }

            return CalAtkInterval(0);
        }

        public override APlayer CalcEnemy()
        {
            var ret = base.CalcEnemy();

            if (ret != null)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowAttackIcon { NeedShow = true, Player = ret });
            }

            return ret;
        }

        public void UpdateEnemy(APlayer player)
        {
            this._enemy = player;
        }
        public override void OnHit(DamageResult dr)
        {
            Debug.Log($"{DateTime.Now.ToString("mm:ss.fff")} heor hit damage");

            //Debug.Log("heor hit damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));

            base.OnHit(dr);
        }
    }
}
