using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterQualityConfigCategory : ProtoObject, IMerge
    {
        public static MonsterQualityConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterQualityConfig> dict = new Dictionary<int, MonsterQualityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterQualityConfig> list = new List<MonsterQualityConfig>();
		
        public MonsterQualityConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterQualityConfigCategory s = o as MonsterQualityConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterQualityConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterQualityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterQualityConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterQualityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterQualityConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterQualityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterQualityConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>怪物称号</summary>
		[ProtoMember(2)]
		public string MonsterTitle { get; set; }
		/// <summary>攻击系数</summary>
		[ProtoMember(3)]
		public double AttrRate { get; set; }
		/// <summary>防御系数</summary>
		[ProtoMember(4)]
		public double DefRate { get; set; }
		/// <summary>血量系数</summary>
		[ProtoMember(5)]
		public double HpRate { get; set; }
		/// <summary>金币系数</summary>
		[ProtoMember(6)]
		public double GoldRate { get; set; }
		/// <summary>经验系数</summary>
		[ProtoMember(7)]
		public double ExpRate { get; set; }
		/// <summary>掉率系数</summary>
		[ProtoMember(8)]
		public double DropRate { get; set; }
		/// <summary>品质系数</summary>
		[ProtoMember(9)]
		public double QualityRate { get; set; }

	}
}
