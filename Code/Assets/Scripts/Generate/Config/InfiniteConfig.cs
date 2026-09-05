using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class InfiniteConfigCategory : ProtoObject, IMerge
    {
        public static InfiniteConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InfiniteConfig> dict = new Dictionary<int, InfiniteConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InfiniteConfig> list = new List<InfiniteConfig>();
		
        public InfiniteConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            InfiniteConfigCategory s = o as InfiniteConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (InfiniteConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public InfiniteConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InfiniteConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InfiniteConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InfiniteConfig> GetAll()
        {
            return this.dict;
        }

        public InfiniteConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InfiniteConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(4)]
		public string Atk { get; set; }
		/// <summary>RiseAtk</summary>
		[ProtoMember(5)]
		public double RiseAtk { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(6)]
		public string Def { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(7)]
		public double RiseDef { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(8)]
		public string Hp { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(9)]
		public double RiseHp { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(10)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(11)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(12)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(13)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(14)]
		public int CritDamage { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(15)]
		public int Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(16)]
		public int Miss { get; set; }
		/// <summary>MulDamageIncrea</summary>
		[ProtoMember(17)]
		public int MulDamageIncrea { get; set; }
		/// <summary>MulDamageResist</summary>
		[ProtoMember(18)]
		public int MulDamageResist { get; set; }
		/// <summary>Exp</summary>
		[ProtoMember(19)]
		public double Exp { get; set; }
		/// <summary>Gold</summary>
		[ProtoMember(20)]
		public double Gold { get; set; }

	}
}
