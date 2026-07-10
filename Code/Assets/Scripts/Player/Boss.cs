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

        private int[] excludeSkillList = { };
        //private int[] excludeSuitList = { 6 };

        public Boss(int bossId, RuleType ruleType) : base()
        {
            this.BossId = bossId;
            this.GroupId = 2;
            this.Quality = 6;

            this.RuleType = ruleType;

            this.Config = BossConfigCategory.Instance.Get(BossId);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;

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

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, (hp * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, (def * defModelRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.ConfigBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.ConfigBase, Config.DamageResist);

            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.ConfigBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.ConfigBase, Config.CritDamage);
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.ConfigBase, Config.CritRateResist);

            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.ConfigBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.ConfigBase, Config.Miss);
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.ConfigBase, Config.Lucky);
            AttributeBonus.SetAttr(AttributeEnum.Curse, AttributeFrom.ConfigBase, Config.Curse);

            this.SetSpeed(Config.Speed, Config.MoveSpeed);

            //回满当前血量
            SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {

            if (Config.SkillIdList != null)
            {
                for (int i = 0; i < Config.SkillIdList.Length; i++)
                {
                    int skillId = Config.SkillIdList[i];

                    SkillData skillData = new SkillData(skillId, i);
                    skillData.MagicLevel.Data = 1;

                    List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillId, Config.RuneCount);
                    List<SkillSuit> suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillId, Config.RuneCount);
                    List<SkillTalent> talentList = SkillTalentConfigCategory.Instance.GetAllTalent(skillId, Config.RuneCount);

                    SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talentList, false);

                    SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                    SelectSkillList.Add(skill);
                }
            }

            //增加普攻技能
            AddSkillNormal();
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            AppHelper.Boss = false;
            //Log.Info("Boss :" + this.ToString() + " dead");
            if (RuleType != RuleType.MainStage)
            {
                for (int i = 0; i < ConfigHelper.TestRate; i++)
                {
                    BuildReword();
                }
            }
        }

        private void BuildReword()
        {
            User user = User_Data_Manager.Data;

            //增加宠物经验，神器经验
            double kc = this.Config.Id;
            user.KillMonsterEnvent(kc, this.Quality, 1);

            //区域boss独特掉落


            double expRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpIncrea) + 100) / 100.0;
            double goldRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea) + 100) / 100.0;
            double burstRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstIncrea) + 100) / 100.0;
            double qualityRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.QualityIncrea) + 100) / 100.0;

            long exp = (long)(Config.Exp * expRise);
            long gold = (long)(Config.Gold * goldRise);

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

            //首先提交图鉴
            List<Item> cardList = user.AutoToCard(items);
            if (cardList.Count > 0 && showMessage)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = RuleType,
                    Important = 1,
                    Message = BattleMsgHelper.BuildAutoCardMessage(cardList)
                });
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


            //概率获取彩蛋
            if (RandomHelper.RandomNumber(0, 200) <= 0)
            {
                int achId = AchievementConfigCategory.Instance.RandomKillType(10002);
                if (achId > 0)
                {
                    user.AddAchievementLevel(achId);

                    GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                    {
                        Type = RuleType,
                        Important = 1,
                        Message = BattleMsgHelper.BuildAchKillType(achId)
                    });
                }
            }
        }
    }
}
