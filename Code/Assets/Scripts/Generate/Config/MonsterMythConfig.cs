using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterMythConfigCategory : ProtoObject, IMerge
    {
        public static MonsterMythConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterMythConfig> dict = new Dictionary<int, MonsterMythConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterMythConfig> list = new List<MonsterMythConfig>();
		
        public MonsterMythConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterMythConfigCategory s = o as MonsterMythConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterMythConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterMythConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterMythConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterMythConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterMythConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterMythConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterMythConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(3)]
		public int Cycle { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(4)]
		public int Quality { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(5)]
		public string MonsterName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(6)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(7)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(8)]
		public string HP { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(9)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(10)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(11)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(12)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(13)]
		public int CritDamageResist { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(14)]
		public int Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(15)]
		public int Miss { get; set; }
		/// <summary>Lucky</summary>
		[ProtoMember(16)]
		public int Lucky { get; set; }
		/// <summary>Curse</summary>
		[ProtoMember(17)]
		public int Curse { get; set; }
		/// <summary>MulDamageResit</summary>
		[ProtoMember(18)]
		public int MulDamageResit { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(19)]
		public int[] SkillIdList { get; set; }

	}
}
