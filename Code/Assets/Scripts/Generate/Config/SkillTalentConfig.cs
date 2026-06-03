using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillTalentConfigCategory : ProtoObject, IMerge
    {
        public static SkillTalentConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillTalentConfig> dict = new Dictionary<int, SkillTalentConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillTalentConfig> list = new List<SkillTalentConfig>();
		
        public SkillTalentConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillTalentConfigCategory s = o as SkillTalentConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillTalentConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillTalentConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillTalentConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillTalentConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillTalentConfig> GetAll()
        {
            return this.dict;
        }

        public SkillTalentConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillTalentConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>技能ID</summary>
		[ProtoMember(3)]
		public int SkillId { get; set; }
		/// <summary>词条名字</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(5)]
		public string Des { get; set; }
		/// <summary>减少冷却时间</summary>
		[ProtoMember(6)]
		public int CD { get; set; }
		/// <summary>持续时间</summary>
		[ProtoMember(7)]
		public int Duration { get; set; }
		/// <summary>增加攻击距离</summary>
		[ProtoMember(8)]
		public int Dis { get; set; }
		/// <summary>增加最大敌人数量</summary>
		[ProtoMember(9)]
		public int EnemyMax { get; set; }
		/// <summary>行</summary>
		[ProtoMember(10)]
		public int Row { get; set; }
		/// <summary>列</summary>
		[ProtoMember(11)]
		public int Column { get; set; }
		/// <summary>增加伤害比例</summary>
		[ProtoMember(12)]
		public int Percent { get; set; }
		/// <summary>固定伤害</summary>
		[ProtoMember(13)]
		public int Damage { get; set; }
		/// <summary>无视防御</summary>
		[ProtoMember(14)]
		public int IgnoreDef { get; set; }
		/// <summary>暴击率</summary>
		[ProtoMember(15)]
		public int CritRate { get; set; }
		/// <summary>暴击倍率</summary>
		[ProtoMember(16)]
		public int CritDamage { get; set; }
		/// <summary>致命率</summary>
		[ProtoMember(17)]
		public int DeadlyRate { get; set; }
		/// <summary>致命伤害</summary>
		[ProtoMember(18)]
		public int DeadlyDamage { get; set; }
		/// <summary>伤害加成</summary>
		[ProtoMember(19)]
		public int RateDamage { get; set; }
		/// <summary>攻击加成</summary>
		[ProtoMember(20)]
		public int AttrIncrea { get; set; }
		/// <summary>最终加成</summary>
		[ProtoMember(21)]
		public int FinalIncrea { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(22)]
		public int Accuracy { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(23)]
		public int Speed { get; set; }
		/// <summary>附加属性</summary>
		[ProtoMember(24)]
		public int AttrId { get; set; }
		/// <summary>属性值</summary>
		[ProtoMember(25)]
		public double AttrValue { get; set; }
		/// <summary>附带效果</summary>
		[ProtoMember(26)]
		public int EffectId { get; set; }
		/// <summary>EffectVue</summary>
		[ProtoMember(27)]
		public int EffectVue { get; set; }
		/// <summary>EffectDuration</summary>
		[ProtoMember(28)]
		public int EffectDuration { get; set; }
		/// <summary>EffectMax</summary>
		[ProtoMember(29)]
		public int EffectMax { get; set; }
		/// <summary>StartQuality</summary>
		[ProtoMember(30)]
		public int StartQuality { get; set; }
		/// <summary>EndQuality</summary>
		[ProtoMember(31)]
		public int EndQuality { get; set; }

	}
}
