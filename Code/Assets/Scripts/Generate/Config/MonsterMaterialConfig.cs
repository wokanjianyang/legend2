using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterMaterialConfigCategory : ProtoObject, IMerge
    {
        public static MonsterMaterialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterMaterialConfig> dict = new Dictionary<int, MonsterMaterialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterMaterialConfig> list = new List<MonsterMaterialConfig>();
		
        public MonsterMaterialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterMaterialConfigCategory s = o as MonsterMaterialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterMaterialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterMaterialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterMaterialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterMaterialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterMaterialConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterMaterialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterMaterialConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(5)]
		public string Atk { get; set; }
		/// <summary>AtrRise</summary>
		[ProtoMember(6)]
		public string AtrRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(7)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(8)]
		public string DefRise { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(9)]
		public string Hp { get; set; }
		/// <summary>HpRise</summary>
		[ProtoMember(10)]
		public string HpRise { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(11)]
		public double DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(12)]
		public double DamageResist { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(13)]
		public double Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(14)]
		public double Miss { get; set; }
		/// <summary>Reward</summary>
		[ProtoMember(15)]
		public int Reward { get; set; }

	}
}
