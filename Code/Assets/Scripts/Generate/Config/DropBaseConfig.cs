using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class DropBaseConfigCategory : ProtoObject, IMerge
    {
        public static DropBaseConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DropBaseConfig> dict = new Dictionary<int, DropBaseConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DropBaseConfig> list = new List<DropBaseConfig>();
		
        public DropBaseConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DropBaseConfigCategory s = o as DropBaseConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DropBaseConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DropBaseConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DropBaseConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DropBaseConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DropBaseConfig> GetAll()
        {
            return this.dict;
        }

        public DropBaseConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DropBaseConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>掉落名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>道具类型</summary>
		[ProtoMember(3)]
		public int ItemType { get; set; }
		/// <summary>掉落Id列表</summary>
		[ProtoMember(4)]
		public int[] ItemIdList { get; set; }

	}
}
