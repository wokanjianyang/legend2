using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Monster_Myth : APlayer
{
    MonsterMythConfig config;

    public Monster_Myth(int mapId, int quality)
    {
        this.GroupId = 2;
        this.RuleType = RuleType.Myth;
        this.Quality = quality;

        config = MonsterMythConfigCategory.Instance.GetByMapIdAndQuality(mapId, quality);

        this.Init();
    }

    private void Init()
    {
        this.Camp = PlayerType.Enemy;
        this.Name = config.MonsterName;
        this.Level = config.Id * 10 + config.Quality;

        int fid = this.config.Id;
        fid = (Quality == 5 ? fid % 24 : fid % 72) + 1;

        this.FashionId = fid;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();

        for (int i = 0; i < config.SkillIdList.Length; i++)
        {
            int skillId = config.SkillIdList[i];
            SkillData skillData = new SkillData(skillId, i);
            skillData.MagicLevel.Data = 10;
            list.Add(skillData);
        }

        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, 10);
            List<SkillSuit> suitList = SkillSuitConfigCategory.Instance.GetAllSuit(skillData.SkillId, 10);
            List<SkillTalent> talentList = SkillTalentConfigCategory.Instance.GetAllTalent(skillData.SkillId, 10);

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, talentList, false);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {
        double hp = StringHelper.StringToNumber(config.HP);
        double atk = StringHelper.StringToNumber(config.Attr);
        double def = StringHelper.StringToNumber(config.Def);

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.ConfigBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtk, AttributeFrom.ConfigBase, atk);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtk, AttributeFrom.ConfigBase, atk);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtk, AttributeFrom.ConfigBase, atk);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.ConfigBase, def);

        AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.ConfigBase, config.DamageIncrea);
        AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.ConfigBase, config.DamageResist);
        AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.ConfigBase, config.CritRateResist);
        AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.ConfigBase, config.CritDamageResist);

        AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.ConfigBase, config.Accuracy);
        AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.ConfigBase, config.Miss);
        AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.ConfigBase, config.MulDamageResit);

        this.SetSpeed(config.Speed, config.Speed);

        //回满当前血量
        SetHP(AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP));
    }

    public override float DoEvent()
    {
        return base.DoEvent();
    }

}
