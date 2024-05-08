using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class EffectData
    {
        public EffectConfig Config { get; set; }
        public int FromId { get; }
        /// <summary>
        /// �˺������ӳ�
        /// </summary>
        public int Percent { get; }
        /// <summary>
        /// �˺��̶��ӳ�
        /// </summary>
        public long Damage { get; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// ���Ӳ���
        /// </summary>
        public int Max { get; }

        public EffectData(int configId, int fromId, int percent, long damage, int duration, int max)
        {
            this.Config = EffectConfigCategory.Instance.Get(configId);
            this.FromId = fromId;

            this.Duration = duration;
            this.Max = max;
            this.Percent = percent;
            this.Damage = damage;
        }

    }

    public enum EffectType
    {
        Attr = 1,  //��������
        HP = 2, //�˺��ظ�
        Pause = 3, //����
        IgnorePause = 4, //���߿���

    }

    public enum EffectTarget
    {
        Self = 1,
        Enemy = 2,
        Skill = 3,
        Valet = 4,
    }

    public enum EffectSpecialId
    {
        DefinitelyCrit = 500, //�ض�����
    }
}
