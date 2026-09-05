using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Monster : APlayer
    {
        public int MapId;
        public int MonsterId;
        public int Model;

        MonsterConfig Config { get; set; }
        MonsterQualityConfig QualityConfig { get; set; }

        MapModeConfig MModelConfig { get; set; }


        public Monster(int mapId, int quality, RuleType ruleType, int model) : base()
        {
            this.MapId = mapId;
            this.MonsterId = mapId;
            this.GroupId = 2;
            this.Quality = quality;

            this.Model = model;
            this.RuleType = ruleType;

            this.MModelConfig = MapModeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.StartMapId <= mapId && mapId <= m.EndMapId).FirstOrDefault();

            this.Config = MonsterConfigCategory.Instance.Get(MonsterId);
            this.QualityConfig = MonsterQualityConfigCategory.Instance.Get(Quality);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);

            User_Data_Manager.Data.LoadMonsterEvent(1);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;

            this.Name = Config.Name + "（N" + this.Model + "）";

            this.Level = Config.MapId;
            this.FashionId = Config.ModelId;


            this.SetAttr();  //设置属性值

            if (this.Config.Layer <= 2)
            {
                this.SetSkill(); //设置技能
            }
            else
            {
                this.SetSkillNew();
            }

            base.Load();
            this.Logic.SetData(null); //设置UI
        }


        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpRate = 1;
            double atkRate = 1;
            double defRate = 1;

            if (this.RuleType == RuleType.MainStage)
            {
                if (this.MapId <= 12)
                {
                    atkRate = 1.2;
                    defRate = 1.1;
                    hpRate = 1.5;
                }
                else
                {
                    atkRate = 1.5;
                    defRate = 1.2;
                    hpRate = 2;
                }
            }

            double mhpRate = Math.Pow(MModelConfig.HpRate, this.Model - 1);
            double mdefRate = Math.Pow(MModelConfig.DefRate, this.Model - 1);
            double matkRate = Math.Pow(MModelConfig.AtkRate, this.Model - 1);

            double hp = StringHelper.StringToNumber(Config.HP);
            double atk = StringHelper.StringToNumber(Config.Atk);
            double def = StringHelper.StringToNumber(Config.Def);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, (hp * hpRate * QualityConfig.HpRate * mhpRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, (atk * atkRate * QualityConfig.AttrRate * matkRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, (atk * atkRate * QualityConfig.AttrRate * matkRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, (atk * atkRate * QualityConfig.AttrRate * matkRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, (def * defRate * QualityConfig.DefRate * mdefRate));

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
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        private void SetSkillNew()
        {
            List<SkillData> list = new List<SkillData>();

            PlayerModel model = null;

            List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Quality == Quality
            && m.StartMapId <= MapId && MapId <= m.EndMapId).ToList();

            if (models.Count > 0)
            {
                int index = RandomHelper.RandomNumber(0, models.Count);
                model = models[index];
                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        list.Add(new SkillData(model.SkillList[i], i)); //增加默认技能
                    }
                }
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

        public override void OnHit(DamageResult dr)
        {
            //Debug.Log("monster hit damage:" + StringHelper.FormatNumber(dr.Damage));

            base.OnHit(dr);
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            //Log.Info("Monster :" + this.ToString() + " dead");

            for (int i = 0; i < ConfigHelper.TestRate; i++)
            {

            }
            BuildReword();

            //存档
            //UserData.Save();
        }

        private void BuildReword()
        {
            User user = User_Data_Manager.Data;

            //增加宠物经验，神器经验
            MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);
            double kc = (mapConfig.GroupId + 1 + this.Quality) / ConfigHelper.PetKillPercent;
            user.KillMonsterEnvent(kc, this.Quality, 1);
            user.SaveTaskProgress(1);

            int riseModel = this.Model - 1;

            double expRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpIncrea) + 100) / 100.0;
            double goldRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea) + 100) / 100.0;
            double burstRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstIncrea) + 100 + mapConfig.DropRise + MModelConfig.DropRate * riseModel) / 100.0;
            double qualityRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.QualityIncrea) + 100 + mapConfig.QualtityRise + MModelConfig.QualityRate * riseModel) / 100.0;

            double expKI = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpKillIncrea);
            double goldKI = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldKillIncrea);

            burstRise = burstRise * QualityConfig.DropRate;
            qualityRise = qualityRise * QualityConfig.QualityRate;

            long exp = (long)((Config.Exp + expKI) * QualityConfig.ExpRate * expRise);
            long gold = (long)((Config.Gold + goldKI) * QualityConfig.GoldRate * goldRise);

            List<Item> items = new List<Item>();

            if (RuleType != RuleType.MainStage)
            {
                //生成道具奖励
                items.AddRange(DropConfigCategory.Instance.BuildDropItem(MapId, burstRise, qualityRise));

                //限时奖励
                //items.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));
                //items.AddRange(DropLimitHelper.Build((int)DropLimitType.Map, this.MapId, dropRate, modelRate, limit, countRate, dropFinal));
                //items.AddRange(DropLimitHelper.BuildJieRi(modelRate * dropFinal));

                //double rs = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstMul);
                //int itemCount = MathHelper.RandomBurstMul(rs);
            }

            int itemCount = 0;

            bool showMessage = QualityConfigHelper.GetMaxColor(items) >= AppHelper.SetData.InfoColor;
            if (showMessage)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
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

            //再回收
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
            if (RandomHelper.RandomNumber(0, 300000) <= 0)
            {
                int achId = AchievementConfigCategory.Instance.RandomKillType(10001);
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
