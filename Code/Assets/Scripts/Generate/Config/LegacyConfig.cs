using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyConfigCategory : ProtoObject, IMerge
    {
        public static LegacyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyConfig> dict = new Dictionary<int, LegacyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyConfig> list = new List<LegacyConfig>();
		
        public LegacyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyConfigCategory s = o as LegacyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(3)]
		public int Part { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(4)]
		public int ItemId { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(5)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(6)]
		public int EndLevel { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(7)]
		public string Name { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(8)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(9)]
		public int[] AtrVueList { get; set; }
		/// <summary>SpeIdList</summary>
		[ProtoMember(10)]
		public int[] SpeIdList { get; set; }
		/// <summary>SpeVueList</summary>
		[ProtoMember(11)]
		public int[] SpeVueList { get; set; }
		/// <summary>SpeLayerList</summary>
		[ProtoMember(12)]
		public int[] SpeLayerList { get; set; }
		/// <summary>LevelIdList</summary>
		[ProtoMember(13)]
		public int[] LevelIdList { get; set; }
		/// <summary>LevelValueList</summary>
		[ProtoMember(14)]
		public int[] LevelValueList { get; set; }
		/// <summary>SpeAtrList</summary>
		[ProtoMember(15)]
		public int[] SpeAtrList { get; set; }
		/// <summary>SpeLevel</summary>
		[ProtoMember(16)]
		public int[] SpeLevel { get; set; }
		/// <summary>Fee1</summary>
		[ProtoMember(17)]
		public long Fee1 { get; set; }
		/// <summary>Fee2</summary>
		[ProtoMember(18)]
		public long Fee2 { get; set; }

	}
}
