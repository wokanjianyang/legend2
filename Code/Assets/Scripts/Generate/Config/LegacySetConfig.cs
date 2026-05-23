using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacySetConfigCategory : ProtoObject, IMerge
    {
        public static LegacySetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacySetConfig> dict = new Dictionary<int, LegacySetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacySetConfig> list = new List<LegacySetConfig>();
		
        public LegacySetConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacySetConfigCategory s = o as LegacySetConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacySetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacySetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacySetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacySetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacySetConfig> GetAll()
        {
            return this.dict;
        }

        public LegacySetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacySetConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(5)]
		public string Name { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(6)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(7)]
		public int[] AtrVueList { get; set; }

	}
}
