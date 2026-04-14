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
        public int MapId;
        BossConfig Config { get; set; }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        private int[] excludeSkillList = { 1004, 2007, 2010, 3004, 3007, 3008, 3009 };
        //private int[] excludeSuitList = { 6 };

        public Boss(int mapId, RuleType ruleType) : base()
        {
            this.BossId = mapId;
            this.MapId = mapId;
            this.GroupId = 2;
            this.Quality = 5;

            this.RuleType = ruleType;

            this.Config = BossConfigCategory.Instance.Get(BossId);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.ModelType = MondelType.Boss;

            this.Name = Config.Name;
            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp;
            this.Gold = Config.Gold;


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
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (atk * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (atk * attrModelRate));
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

            this.SetAttackSpeed(Config.Speed);
            this.SetMoveSpeed(Config.MoveSpeed);

            //回满当前血量
            SetHP(AttributeBonus.GetBattleAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            List<SkillData> list = new List<SkillData>();

            PlayerModel model = null;

            int position = this.MapId % 5 + 1;

            List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Quality == 5
            && m.StartMapId <= MapId && MapId <= m.EndMapId).ToList();

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
                        suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, model.Suit);
                    }
                }

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

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


            double exp = (this.Exp * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
            double gold = (this.Gold * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(Quality);

            //user.AddStartRate(this.MapId, qualityConfig.CountRate * countModelRate);

            double dropRate = user.GetRealDropRate();
            double modelRate = qualityConfig.DropRate;
            double countRate = qualityConfig.CountRate / 2;
            int soulPercent = (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.SoulPercent);

            List<Item> items = new List<Item>();
            //生成道具奖励 ,爆率 = 人物爆率*怪物类型爆率*怪物品质爆率
            List<KeyValuePair<double, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, dropRate * modelRate);

            // Debug.Log("count Rate:" + countRate);

            double dropFinal = 1;

            //Debug.Log("dropFinal:" + dropFinal);

            //限时奖励
            int limit = user.GetLimitId();
            items.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));
            items.AddRange(DropLimitHelper.Build((int)DropLimitType.Map, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));

            items.AddRange(DropLimitHelper.BuildJieRi(modelRate * dropFinal));

            int qualityRate = qualityConfig.QualityRate * user.GetRealQualityRate();
            items.AddRange(DropHelper.BuildDropItem(dropList, qualityRate, RuleType.BossFamily, 0));

            int mapIndex = Config.MapId - ConfigHelper.MapStartId;
            int quantity = mapIndex / 10 + 1 + user.SoulRingNumber + user.GetArtifactValue(ArtifactType.SoulStone);

            items.Add(ItemHelper.BuildSoulRingShard(quantity * 2));

            double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            int itemCount = MathHelper.RandomBurstMul(rs);


            bool showMessage = QualityConfigHelper.GetMaxColor(items) >= user.InfoColor;

            if (showMessage)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = RuleType,
                    Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items, itemCount, 0, 0)
                });
            }

            if (itemCount > 0)
            {
                exp += exp * itemCount;
                gold += gold * itemCount;
                items.AddRange(ItemHelper.BurstMul(items, itemCount, qualityRate, RuleType.BossFamily));
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
            user.AddExpAndGold(exp, gold);
            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }
    }
}
