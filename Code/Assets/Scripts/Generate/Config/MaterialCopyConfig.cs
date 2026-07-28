using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MaterialCopyConfigCategory : ProtoObject, IMerge
    {
        public static MaterialCopyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MaterialCopyConfig> dict = new Dictionary<int, MaterialCopyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MaterialCopyConfig> list = new List<MaterialCopyConfig>();
		
        public MaterialCopyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MaterialCopyConfigCategory s = o as MaterialCopyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MaterialCopyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MaterialCopyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MaterialCopyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MaterialCopyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MaterialCopyConfig> GetAll()
        {
            return this.dict;
        }

        public MaterialCopyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MaterialCopyConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(3)]
		public string MonsterName { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(5)]
		public int Layer { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(6)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(7)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(8)]
		public string Atk { get; set; }
		/// <summary>AtrRise</summary>
		[ProtoMember(9)]
		public string AtrRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(10)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(11)]
		public string DefRise { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(12)]
		public string Hp { get; set; }
		/// <summary>HpRise</summary>
		[ProtoMember(13)]
		public string HpRise { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(14)]
		public double DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(15)]
		public double DamageResist { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(16)]
		public double Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(17)]
		public double Miss { get; set; }
		/// <summary>RewardId</summary>
		[ProtoMember(18)]
		public int RewardId { get; set; }
		/// <summary>RewardCount</summary>
		[ProtoMember(19)]
		public int RewardCount { get; set; }

	}
}
