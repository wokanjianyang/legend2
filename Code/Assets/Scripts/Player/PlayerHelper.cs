using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PlayerHelper
    {
        public static Dictionary<string, string> PlayerAttributeMap = new Dictionary<string, string>()
        {
            {nameof(AttributeEnum.Color),"��ɫ" },
            {nameof(AttributeEnum.Name),"����" },
            {nameof(AttributeEnum.Level),"�ȼ�" },
            {nameof(AttributeEnum.Exp), "����ֵ" },
            {nameof(AttributeEnum.Power), "ս��" },
            {nameof(AttributeEnum.HP), "����ֵ" },
            {nameof(AttributeEnum.PhyAtt), "������" },
            {nameof(AttributeEnum.MagicAtt),"ħ������" },
            {nameof(AttributeEnum.SpiritAtt), "��������" },
            {nameof(AttributeEnum.Def), "����" },
            {nameof(AttributeEnum.DefIgnore), "���ӷ���" },
            {nameof(AttributeEnum.Speed), "����" },
            {nameof(AttributeEnum.MoveSpeed), "�ƶ��ٶ�" },
            {nameof(AttributeEnum.Lucky), "����" },
            {nameof(AttributeEnum.CritRate), "������" },
            {nameof(AttributeEnum.CritDamage), "���˼ӳ�" },
            {nameof(AttributeEnum.CritRateResist), "������" },
            {nameof(AttributeEnum.CritDamageResist), "���˼���" },
            {nameof(AttributeEnum.DamageIncrea), "�˺�����" },
            {nameof(AttributeEnum.DamageResist), "�˺�����" },
            {nameof(AttributeEnum.AttIncrea), "�����ӳ�" },
            {nameof(AttributeEnum.HpIncrea), "�����ӳ�" },
            {nameof(AttributeEnum.DefIncrea), "�����ӳ�" },
            {nameof(AttributeEnum.InheritIncrea), "�̳мӳ�" },
            {nameof(AttributeEnum.ExpIncrea), "����ӳ�" },
            {nameof(AttributeEnum.BurstIncrea), "���ʼӳ�" },
            {nameof(AttributeEnum.GoldIncrea), "��Ҽӳ�" },
            {nameof(AttributeEnum.SecondExp), "��������" },
            {nameof(AttributeEnum.SecondGold), "�������" },
            {nameof(AttributeEnum.RestoreHp), "�̶���Ѫ" },
            {nameof(AttributeEnum.RestoreHpPercent), "������Ѫ" },
            {nameof(AttributeEnum.QualityIncrea), "Ʒ�ʼӳ�" },
            {nameof(AttributeEnum.Miss), "����" },
            {nameof(AttributeEnum.Accuracy), "��׼" },

            {nameof(AttributeEnum.PhyDamage), "���˼ӳ�" },
            {nameof(AttributeEnum.MagicDamage),"ħ�˼ӳ�" },
            {nameof(AttributeEnum.SpiritDamage), "���˼ӳ�" },

            {nameof(AttributeEnum.PhyAttIncrea), "�﹥�ӳ�" },
            {nameof(AttributeEnum.MagicAttIncrea),"ħ���ӳ�" },
            {nameof(AttributeEnum.SpiritAttIncrea), "�����ӳ�" },

            {nameof(AttributeEnum.EquipBaseIncrea), "װ����������" },
            {nameof(AttributeEnum.EquipRandomIncrea), "װ���������" },

            {nameof(AttributeEnum.AurasDamageIncrea), "��������" },
            {nameof(AttributeEnum.AurasDamageResist), "���ռ���" },
            {nameof(AttributeEnum.AurasAttrIncrea), "���չ���" },

            //{nameof(AttributeEnum.PanelAtt), "���չ���" },
            //{nameof(AttributeEnum.PanelDef),"���շ���" },
            //{nameof(AttributeEnum.PanelHp), "��������" },
            //{nameof(AttributeEnum.PanelPhyAtt), "�����﹥" },
            //{nameof(AttributeEnum.PanelMagicAtt),"����ħ��" },
            {nameof(AttributeEnum.SkillValetCount), "�ٻ�����" },
            {nameof(AttributeEnum.SkillValetSpeed), "���﹥��" },

            {nameof(AttributeEnum.MulAttr), "��������" },
            {nameof(AttributeEnum.MulDef),"��������" },
            {nameof(AttributeEnum.MulHp), "��������" },
            {nameof(AttributeEnum.MulAttrPhy), "�﹥����" },
            {nameof(AttributeEnum.MulAttrMagic),"ħ������" },
            {nameof(AttributeEnum.MulAttrSpirit), "��������" },
            {nameof(AttributeEnum.MulPhyDamageRise), "���˱���" },
            {nameof(AttributeEnum.MulMagicDamageRise),"ħ�˱���" },
            {nameof(AttributeEnum.MulSpiritDamageRise), "���˱���" },
            {nameof(AttributeEnum.MulDamageIncrea), "���˱���" },
            {nameof(AttributeEnum.MulDamageResist),"���˱���" },
        };
    }
}
