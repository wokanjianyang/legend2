using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class BabelAtrConfigCategory : ProtoObject, IMerge
    {
        public static BabelAtrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BabelAtrConfig> dict = new Dictionary<int, BabelAtrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BabelAtrConfig> list = new List<BabelAtrConfig>();
		
        public BabelAtrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            BabelAtrConfigCategory s = o as BabelAtrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (BabelAtrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BabelAtrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BabelAtrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BabelAtrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BabelAtrConfig> GetAll()
        {
            return this.dict;
        }

        public BabelAtrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BabelAtrConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(3)]
		public int Rate { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>AtrId</summary>
		[ProtoMember(5)]
		public int AtrId { get; set; }
		/// <summary>AtrValue</summary>
		[ProtoMember(6)]
		public double AtrValue { get; set; }

	}
}
