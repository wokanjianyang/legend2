using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class StoreConfigCategory : ProtoObject, IMerge
    {
        public static StoreConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StoreConfig> dict = new Dictionary<int, StoreConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StoreConfig> list = new List<StoreConfig>();
		
        public StoreConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            StoreConfigCategory s = o as StoreConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (StoreConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StoreConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StoreConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (StoreConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StoreConfig> GetAll()
        {
            return this.dict;
        }

        public StoreConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StoreConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(3)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(4)]
		public int[] AtrVueList { get; set; }
		/// <summary>SpeId</summary>
		[ProtoMember(5)]
		public int SpeId { get; set; }
		/// <summary>SpeVue</summary>
		[ProtoMember(6)]
		public int SpeVue { get; set; }
		/// <summary>SpeLevel</summary>
		[ProtoMember(7)]
		public int SpeLevel { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(8)]
		public int Quality { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(9)]
		public int Max { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(10)]
		public string Des { get; set; }

	}
}
