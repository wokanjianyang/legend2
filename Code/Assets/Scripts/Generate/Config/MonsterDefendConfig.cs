using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterDefendConfigCategory : ProtoObject, IMerge
    {
        public static MonsterDefendConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterDefendConfig> dict = new Dictionary<int, MonsterDefendConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterDefendConfig> list = new List<MonsterDefendConfig>();
		
        public MonsterDefendConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterDefendConfigCategory s = o as MonsterDefendConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterDefendConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterDefendConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterDefendConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterDefendConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterDefendConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterDefendConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterDefendConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(4)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(5)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(6)]
		public string Atk { get; set; }
		/// <summary>RiseAtk</summary>
		[ProtoMember(7)]
		public double RiseAtk { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(8)]
		public string Def { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(9)]
		public double RiseDef { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(10)]
		public string Hp { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(11)]
		public double RiseHp { get; set; }
		/// <summary>DamageMul</summary>
		[ProtoMember(12)]
		public string DamageMul { get; set; }
		/// <summary>MulRise</summary>
		[ProtoMember(13)]
		public double MulRise { get; set; }
		/// <summary>MulResist</summary>
		[ProtoMember(14)]
		public string MulResist { get; set; }
		/// <summary>MulResistRise</summary>
		[ProtoMember(15)]
		public double MulResistRise { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(16)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(17)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(18)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(19)]
		public int CritDamage { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(20)]
		public int Accuracy { get; set; }
		/// <summary>RiseAccuracy</summary>
		[ProtoMember(21)]
		public int RiseAccuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(22)]
		public int Miss { get; set; }
		/// <summary>RiseMiss</summary>
		[ProtoMember(23)]
		public int RiseMiss { get; set; }
		/// <summary>Lucky</summary>
		[ProtoMember(24)]
		public int Lucky { get; set; }
		/// <summary>Curse</summary>
		[ProtoMember(25)]
		public int Curse { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(26)]
		public int Speed { get; set; }
		/// <summary>Exp</summary>
		[ProtoMember(27)]
		public double Exp { get; set; }
		/// <summary>RiseExp</summary>
		[ProtoMember(28)]
		public double RiseExp { get; set; }

	}
}
