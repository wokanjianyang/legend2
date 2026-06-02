using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Boss : APlayer
    {
        public int BossId;
        BossConfig Config { get; set; }

        QualityConfig QualityConfig { get; set; }

        private int[] excludeSkillList = { };
        //private int[] excludeSuitList = { 6 };

        public Boss(int bossId, RuleType ruleType) : base()
        {
            this.BossId = bossId;
            this.GroupId = 2;
            this.Quality = 6;

            this.RuleType = ruleType;

            this.Config = BossConfigCategory.Instance.Get(BossId);
            this.QualityConfig = QualityConfigCategory.Instance.Get(Quality);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.ModelType = MondelType.Boss;

            this.Name = Config.Name;
            this.Level = (Config.MapId - 999) * 100;
            this.FashionId = BossId;

            this.SetAttr();  //设置属性值
            this.SetSkill();

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpModelRate = 1;
            double attrModelRate = 1;
            double defModelRate = 1;

            double hp = StringHelper.StringToNumber(Config.HP);
            double atk = StringHelper.StringToNumber(Config.Atk);
            double def = StringHelper.StringToNumber(Config.Def);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (hp * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (def * defModelRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroBase, Config.CritRateResist);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroBase, Config.Lucky);
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.HeroBase, Config.Curse);

            this.SetSpeed(Config.Speed, Config.MoveSpeed);

            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            PlayerModel model = null;

            List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Quality == 0).ToList();

            if (models.Count > 0)
            {
                int index = RandomHelper.RandomNumber(0, models.Count);
                model = models[index];
                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        int skillId = model.SkillList[i];
                        if (this.RuleType == RuleType.BossFamily && this.excludeSkillList.Contains(skillId))
                        {
                            continue;
                        }

                        list.Add(new SkillData(skillId, i)); //增加默认技能
                    }
                }
                this.Title = model.Name;
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                if (model != null)
                {
                    if (model.Rune > 0)
                    {
                        runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, model.Rune);
                    }

                    if (model.Suit > 0)
                    {
                        suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillData.SkillId, model.Suit);
                    }
                }

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            //Log.Info("Boss :" + this.ToString() + " dead");
            if (RuleType != RuleType.MainStage)
            {
                BuildReword();
            }
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            //增加宠物经验，神器经验
            double kc = this.Config.Id;
            user.KillMonsterEnvent(kc, this.Quality, 1);

            //区域boss独特掉落


            double expRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpIncrea) + 100) / 100.0;
            double goldRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea) + 100) / 100.0;
            double burstRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstIncrea) + 100) / 100.0;
            double qualityRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.QualityIncrea) + 100) / 100.0;

            burstRise = burstRise * QualityConfig.DropRate;
            qualityRise = qualityRise * QualityConfig.QualityRate;

            long exp = (long)(Config.Exp * QualityConfig.ExpRate * expRise);
            long gold = (long)(Config.Gold * QualityConfig.GoldRate * goldRise);

            List<Item> items = new List<Item>();

            //生成道具奖励
            items.AddRange(DropConfigCategory.Instance.BuildBossDropItem(Config.Id, burstRise, qualityRise));


            //限时奖励
            //items.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));
            //items.AddRange(DropLimitHelper.Build((int)DropLimitType.Map, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));
            //items.AddRange(DropLimitHelper.BuildJieRi(modelRate * dropFinal));

            //double rs = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstMul);
            //int itemCount = MathHelper.RandomBurstMul(rs);
            int itemCount = 0;

            bool showMessage = QualityConfigHelper.GetMaxColor(items) >= user.InfoColor;
            if (showMessage)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Important = 1,
                    Type = RuleType,
                    Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items, itemCount, kc)
                });
            }

            if (itemCount > 0)
            {
                exp += exp * itemCount;
                gold += gold * itemCount;
                items.AddRange(ItemHelper.BurstMulNew(items, itemCount, qualityRise));
            }

            //先回收
            List<Item> recoveryList = user.CheckRecovery(items, out long recoveryGold, out int recoveryCount);
            if (recoveryCount > 0 && showMessage)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = RuleType,
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryCount, recoveryList, recoveryGold)
                });
            }

            //增加经验,金币
            user.AddExpAndGold(exp, gold + recoveryGold);
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }
    }
}
