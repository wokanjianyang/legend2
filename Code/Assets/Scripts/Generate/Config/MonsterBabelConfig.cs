using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterBabelConfigCategory : ProtoObject, IMerge
    {
        public static MonsterBabelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterBabelConfig> dict = new Dictionary<int, MonsterBabelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterBabelConfig> list = new List<MonsterBabelConfig>();
		
        public MonsterBabelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterBabelConfigCategory s = o as MonsterBabelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterBabelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterBabelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterBabelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterBabelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterBabelConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterBabelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterBabelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(2)]
		public int Layer { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(5)]
		public string Atk { get; set; }
		/// <summary>RiseAtk</summary>
		[ProtoMember(6)]
		public double RiseAtk { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(7)]
		public string Def { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(8)]
		public double RiseDef { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(9)]
		public string Hp { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(10)]
		public double RiseHp { get; set; }
		/// <summary>DamageMul</summary>
		[ProtoMember(11)]
		public string DamageMul { get; set; }
		/// <summary>MulRise</summary>
		[ProtoMember(12)]
		public double MulRise { get; set; }
		/// <summary>MulResist</summary>
		[ProtoMember(13)]
		public string MulResist { get; set; }
		/// <summary>MulResistRise</summary>
		[ProtoMember(14)]
		public double MulResistRise { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(15)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(16)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(17)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(18)]
		public int CritDamage { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(19)]
		public int Accuracy { get; set; }
		/// <summary>RiseAccuracy</summary>
		[ProtoMember(20)]
		public int RiseAccuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(21)]
		public int Miss { get; set; }
		/// <summary>RiseMiss</summary>
		[ProtoMember(22)]
		public int RiseMiss { get; set; }
		/// <summary>Lucky</summary>
		[ProtoMember(23)]
		public int Lucky { get; set; }
		/// <summary>Curse</summary>
		[ProtoMember(24)]
		public int Curse { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(25)]
		public int Speed { get; set; }

	}
}
