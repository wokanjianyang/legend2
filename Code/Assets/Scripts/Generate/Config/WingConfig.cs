using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class WingConfigCategory : ProtoObject, IMerge
    {
        public static WingConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, WingConfig> dict = new Dictionary<int, WingConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<WingConfig> list = new List<WingConfig>();
		
        public WingConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            WingConfigCategory s = o as WingConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (WingConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public WingConfig Get(int id)
        {
            this.dict.TryGetValue(id, out WingConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (WingConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, WingConfig> GetAll()
        {
            return this.dict;
        }

        public WingConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class WingConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>RequireLevel</summary>
		[ProtoMember(3)]
		public int RequireLevel { get; set; }
		/// <summary>AtrId</summary>
		[ProtoMember(4)]
		public int AtrId { get; set; }
		/// <summary>AtrVue</summary>
		[ProtoMember(5)]
		public int AtrVue { get; set; }
		/// <summary>RiseType</summary>
		[ProtoMember(6)]
		public int RiseType { get; set; }

	}
}
