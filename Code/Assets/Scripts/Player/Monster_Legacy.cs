using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Legacy : APlayer
{
    MonsterLegacyConfig MonsterConfig;

    int Role = 0;
    int Layer = 0;
    int Type = 0;

    public Monster_Legacy(int layer, int type)
    {
        this.GroupId = 2;
        this.Role = RandomHelper.RandomNumber(1, 4);
        this.FashionId = (layer / 3 + this.Role) % 72 + 1;
        this.Layer = layer;
        this.Quality = 3;
        this.Type = type;

        MonsterConfig = MonsterLegacyConfigCategory.Instance.GetByRole(Role);

        this.Init();
        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = MonsterConfig.Name + "(" + Layer + "阶)";
        this.Level = Layer;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();

        if (MonsterConfig.SkillIdList != null)
        {
            for (int i = 0; i < MonsterConfig.SkillIdList.Length; i++)
            {
                list.Add(new SkillData(MonsterConfig.SkillIdList[i], i)); //增加默认技能
            }
        }

        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillConfig.Id, Layer);
            List<SkillSuit> suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillData.SkillConfig.Id, Layer);

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, null, false);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        double riseHp = Math.Pow(MonsterConfig.HpRise, Layer - 1);
        double riseDef = Math.Pow(MonsterConfig.DefRise, Layer - 1);
        double riseAkt = Math.Pow(MonsterConfig.AtkRise, Layer - 1);

        double riseRate = Math.Pow(3, Type);

        //Debug.Log("attrRate:" + attrRate);
        //Debug.Log("advanceRate:" + advanceRate);

        double atk = double.Parse(MonsterConfig.Atk) * riseAkt;
        double hp = double.Parse(MonsterConfig.HP) * riseHp;
        double def = double.Parse(MonsterConfig.Def) * riseDef;

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, hp * riseRate);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, atk * riseRate);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, atk * riseRate);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, atk * riseRate);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, def * riseRate);

        double MaxHP = AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    private void MakeReward(DeadRewarddEvent dead)
    {
        for (int i = 0; i < ConfigHelper.TestRate; i++)
        {
            BuildReward();
        }
    }

    private void BuildReward()
    {
        User user = User_Data_Manager.Data;

        double kc = (this.Layer * 10) / ConfigHelper.PetKillPercent;
        user.KillMonsterEnvent(kc, 1, 1);

        double expRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpIncrea) + 100) / 100.0;
        double goldRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea) + 100) / 100.0;
        double burstRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.BurstIncrea) + 100) / 100.0;
        double qualityRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.QualityIncrea) + 100) / 100.0;

        double expKI = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpKillIncrea);
        double goldKI = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldKillIncrea);

        double baseExp = MonsterConfig.Exp + (this.Layer - 1) * MonsterConfig.RiseExp;

        long exp = (long)((baseExp + expKI) * expRise);
        long gold = (long)((baseExp + goldKI) * goldRise);

        string message = "[" + MonsterConfig.Name + "]死亡，经验+" + exp + "，金币+" + gold + "，杀敌+" + kc;

        int max = (int)(user.MagicLevel.Data / 2);
        int dropLayer = this.RandomDropLayer(max);
        int importantMsg = 0;

        if (dropLayer > 0)
        {
            int legacyId = RandomPart();

            LegacyConfig config = LegacyConfigCategory.Instance.Get(legacyId);

            int currentLayer = user.GetLegacyLayer(legacyId);

            message += string.Format(",掉落<color=#{0}>{1}</color> ", QualityConfigHelper.GetQualityColor(5), config.Name + "(" + dropLayer + "阶) ");

            int recoveryStone = 0;
            if (dropLayer > currentLayer)
            {
                user.SaveLegacyLayer(legacyId, dropLayer);
                importantMsg = 1;
                message += ",自动装备";
                //auto Replace
                if (currentLayer > 0)
                {
                    recoveryStone = currentLayer / 2 + 1;

                    message += ",并且回收之前的获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(5) + ">传世精华</color>";

                    GameProcessor.Inst.UpdateInfo();
                }
            }
            else
            {
                recoveryStone = dropLayer / 2 + 1;

                message += ",自动回收获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(5) + ">传世精华</color>";
            }

            if (recoveryStone > 0)
            {
                Item item = ItemHelper.BuildMaterial(ItemHelper.Legacy_Stone, recoveryStone);

                List<Item> items = new List<Item>();
                items.Add(item);

                if (items.Count > 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
                }
            }
        }

        user.AddExpAndGold(exp, gold);

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Normal,
            Message = message,
            Important = importantMsg
        });
    }

    private int[] rates = { 1, 5, 20, 40, 100 };
    private int RandomDropLayer(int maxLayer)
    {
        //掉率 1/100
        if (RandomHelper.RandomNumber(0, 100) > 0)
        {
            return 0;
        }

        int dropLayer = MathHelper.RandomArrayIndex(rates, 1);
        dropLayer = this.Layer + dropLayer - 3;

        dropLayer = Math.Max(dropLayer, 1);

        dropLayer = Math.Min(dropLayer, maxLayer);

        return dropLayer;
    }

    private int[] rates1 = { 1, 2, 7, 12, 17, 22, 27, 32 };
    private int RandomPart()
    {
        int part = MathHelper.RandomArrayIndex(rates1, 1);

        return part + (this.Role - 1) * 8;
    }
}
