using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {
        SkillDamage = -6,
        Color = -5,
        Name = -4,
        Level = -3,
        Exp = -2, //����ֵ
        Power = -1, //ս��
        CurrentHp = 0, //��ǰ����
        HP = 1, //����ֵ
        PhyAtt = 2, //������
        MagicAtt = 3,//ħ������
        SpiritAtt = 4, //��������
        Def = 5, //����
        Speed = 6, //����
        Lucky = 7, //����
        CritRate = 8, //������
        CritDamage = 9, //��������
        CritRateResist = 10, //����
        CritDamageResist = 11, //���˼���
        DamageIncrea = 12, //�˺�����
        DamageResist = 13, //�˺�����
        AttIncrea = 14, //�����ӳ�
        HpIncrea = 15, //�����ӳ�
        DefIncrea = 16,//�����ӳ�
        InheritIncrea = 17, //�̳мӳ�
        ExpIncrea = 18, //����ӳ�
        BurstIncrea = 19, //���ʼӳ�
        GoldIncrea = 20, //��Ҽӳ�
        SecondExp = 21, //ÿ�뾭������
        RestoreHp = 22, //�̶���Ѫ��ֵ
        RestoreHpPercent = 23,//�ٷֱȻ�Ѫ��ֵ
        QualityIncrea = 24,//Ʒ�ʼӳ�
        SecondGold = 25, //ÿ��������
        PhyAttIncrea = 26, //�﹥�ӳ�
        MagicAttIncrea = 27, //ħ���ӳ�
        SpiritAttIncrea = 28, //�����ӳ�
        MoveSpeed = 29,//�ƶ��ٶ�
        DefIgnore = 30,//���ӷ���
        Miss = 31, //����
        Accuracy = 32, //��׼
        PhyDamage = 33, //���˼ӳ�
        MagicDamage = 34,//ħ�˼ӳ�
        SpiritDamage = 35, //���˼ӳ�
        InheritAdvance = 36, //�߼��̳�

        WarriorSkillPercent = 41, //սʿ���ܰٷֱ�ϵ��
        WarriorSkillDamage = 42, //սʿ���̶ܹ�ϵ��
        MageSkillPercent = 43, //��ʦ���ܰٷֱ�ϵ��
        MageSkillDamage = 44, //��ʦ���̶ܹ�ϵ��
        WarlockSkillPercent = 45, //��ʿ���ܰٷֱ�ϵ��
        WarlockSkillDamage = 46, //��ʿ���̶ܹ�ϵ��

        AurasDamageResist = 201, //�⻷����
        AurasDamageIncrea = 202, //�⻷����
        AurasAttrIncrea = 203,//

        EquipBaseIncrea = 101, //װ���������԰ٷֱ�
        EquipRandomIncrea = 102, //װ��������԰ٷֱ�

        SkillPhyDamage = 302, //�����˺�
        SkillMagicDamage = 303,//ħ���˺�
        SkillSpiritDamage = 304, //�����˺�
        SkillAllDamage = 305, //�����˺��ӳ�
        SkillValetCount = 306, //�ٻ�����+1
        SkillValetSpeed = 307, //�����ٶ�
        SkillValetHp = 308, //�����ӳ�

        ExtraDamage = 401,//�����˺�

        PanelHp = 1001, //�������
        PanelPhyAtt = 1002, //����﹥
        PanelMagicAtt = 1003, //���ħ��
        PanelSpiritAtt = 1004, //������
        PanelDef = 1005, //������
        PanelAtt = 1006,//��幥��

        MulAttr = 2001,  //��������
        MulDef = 2002,  //��������
        MulHp = 2003,  //��������
        MulAttrPhy = 2004, //�﹥����
        MulAttrMagic = 2005,  //��������
        MulAttrSpirit = 2006,  //��������

        MulPhyDamageRise = 2007,
        MulMagicDamageRise = 2008,
        MulSpiritDamageRise = 2009,

        MulDamageIncrea = 2010,  //���˱���
        MulDamageResist = 2011, //���˱���
    }

    /// <summary>
    /// ������Դ
    /// </summary>
    public enum AttributeFrom
    {
        HeroPanel = 0, //�������������
        HeroBase = 1, //������������
        EquipBase = 2, //װ����������
        EquiStrong = 3, //װ��ǿ������
        Skill = 4,//��������
        Tower = 5,//�޾���
        Phantom = 6,//����
        EquipSuit = 7, //װ����װ
        SoulRing = 8, //�껷
        Auras = 9, //�⻷
        Achivement = 10, //�ɾ�
        Exclusive = 11, //ר��
        Card = 12,//ͼ��
        Wing = 13, //���
        Fashion = 14, //ʱװ
        EquipRed = 15, //װ����װ
        Halidom = 16, //����
        Metal = 17,//��ʯ

        Dingzhi = 98,
        /// <summary>
        /// ��������
        /// </summary>
        Test = 99,
    }

    public enum PlayerType
    {
        Hero = 0,
        Enemy,
        Valet,
        Defend,
        HeroPhatom,
        Duplication,
    }

    public enum MondelType
    {
        Nomal = 1,
        Boss = 2,
        YueLing = 5,
    }

    public enum SlotType
    {
        [LabelText("����")]
        ���� = 1,
        [LabelText("�·�")]
        �·� = 2,
        [LabelText("����")]
        ���� = 3,
        [LabelText("ͷ��")]
        ͷ�� = 4,
        [LabelText("������")]
        ������ = 5,
        [LabelText("������")]
        ������ = 6,
        [LabelText("���ָ")]
        ���ָ = 7,
        [LabelText("�ҽ�ָ")]
        �ҽ�ָ = 8,
        [LabelText("����")]
        ���� = 9,
        [LabelText("Ь��")]
        Ь�� = 10,
        [LabelText("����")]
        ���� = 11,
        [LabelText("����")]
        ���� = 12,
        [LabelText("���")]
        ��� = 13,
        [LabelText("ħʯ")]
        ħʯ = 14,

        [LabelText("ר��1")]
        ��ʥŭն = 15,
        [LabelText("ר��2")]
        ��ʥ�ɻ� = 16,
        [LabelText("ר��3")]
        ��ʥѪ�� = 17,
        [LabelText("ר��4")]
        ��ʥ���� = 18,
        [LabelText("ר��5")]
        ��ʥ���� = 19,
        [LabelText("ר��6")]
        ��ʥ���� = 20,

        [LabelText("��ר��")]
        ��ר�� = 101,
        [LabelText("��ר��")]
        ��ר�� = 102,
    }
    public enum ProgressType
    {
        [LabelText("��ɫ����")]
        PlayerExp = 0,

        [LabelText("���ܾ���")]
        SkillExp = 1,

        [LabelText("��ɫ����")]
        PlayerHP = 2,
    }

    public enum CopyType
    {
        [LabelText("װ������")]
        װ������ = 1,
        [LabelText("��Ӱ��ս")]
        ��Ӱ��ս = 2,
        [LabelText("BOSS֮��")]
        BossFamily = 3,
        [LabelText("δ֪����")]
        AnDian = 4,
        [LabelText("����ɳ��")]
        Defend = 5,
        HeorPhantom = 6,
        Mine = 7,
        Infinite = 8,
    }

    public enum RoleType
    {
        Metal = 1, //��ϵ
        Wood = 2, //ľ
        Water = 3, //ˮ
        Fire = 4, //��
        Earth = 5, //��
        Dark = 6, //��
        Light = 7, //��
    }
}
