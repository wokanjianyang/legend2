using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExclusiveMaterialConfigCategory : ProtoObject, IMerge
    {
        public static ExclusiveMaterialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExclusiveMaterialConfig> dict = new Dictionary<int, ExclusiveMaterialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExclusiveMaterialConfig> list = new List<ExclusiveMaterialConfig>();
		
        public ExclusiveMaterialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExclusiveMaterialConfigCategory s = o as ExclusiveMaterialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExclusiveMaterialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExclusiveMaterialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExclusiveMaterialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExclusiveMaterialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExclusiveMaterialConfig> GetAll()
        {
            return this.dict;
        }

        public ExclusiveMaterialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExclusiveMaterialConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(4)]
		public int Quality { get; set; }

	}
}
