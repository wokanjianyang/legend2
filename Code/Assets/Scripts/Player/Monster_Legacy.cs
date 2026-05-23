using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Legacy : APlayer
{
    LegacyMonsterConfig config;

    int Role = 0;
    int Layer = 0;

    public Monster_Legacy(int layer)
    {
        this.GroupId = 2;
        this.Role = RandomHelper.RandomNumber(1, 4);
        this.FashionId = layer;
        this.Layer = layer;
        this.Quality = 3;

        config = LegacyMonsterConfigCategory.Instance.GetByRole(Role);

        this.Init();
        this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.Name + "(" + Layer + "阶)";
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

        if (config.SkillIdList != null)
        {
            for (int i = 0; i < config.SkillIdList.Length; i++)
            {
                list.Add(new SkillData(config.SkillIdList[i], i)); //增加默认技能
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
        double attrRate = Layer;
        double advanceRate = Layer;

        //Debug.Log("attrRate:" + attrRate);
        //Debug.Log("advanceRate:" + advanceRate);

        double attr = Double.Parse(config.Attr) * attrRate;
        double hp = Double.Parse(config.HP) * attrRate;
        double def = Double.Parse(config.Def) * attrRate;


        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        double MaxHP = AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
        SetHP(MaxHP);
    }

    private void MakeReward(DeadRewarddEvent dead)
    {
        BuildReward();
    }

    private void BuildReward()
    {
        User user = GameProcessor.Inst.User;

        int legacyId = (Role - 1) * 8 + RandomHelper.RandomNumber(1, 9);

        LegacyConfig config = LegacyConfigCategory.Instance.Get(legacyId);

        int dropLayer = RandomHelper.RandomNumber(Layer - 2, Layer + 2);

        int currentLayer = user.GetLegacyLayer(legacyId);

        string message = "掉落 " + string.Format("<color=#{0}>{1}</color> ", QualityConfigHelper.GetQualityColor(1), config.Name + "(" + dropLayer + "阶) ");

        int recoveryStone = 0;
        if (dropLayer > currentLayer)
        {
            user.SaveLegacyLayer(legacyId, dropLayer);

            message += ",自动装备";
            //auto Replace
            if (currentLayer > 0)
            {
                recoveryStone += currentLayer;

                message += ",并且回收之前的获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(1) + ">传世精华</color>";

                GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());
            }
        }
        else
        {
            recoveryStone += dropLayer;

            message += ",自动回收获得" + recoveryStone + "个<color=#" + QualityConfigHelper.GetQualityColor(6) + ">传世精华</color>";
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

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Legacy,
            Message = message
        });
    }
}
