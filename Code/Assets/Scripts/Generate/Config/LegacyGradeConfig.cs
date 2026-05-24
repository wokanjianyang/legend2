using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyGradeConfigCategory : ProtoObject, IMerge
    {
        public static LegacyGradeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyGradeConfig> dict = new Dictionary<int, LegacyGradeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyGradeConfig> list = new List<LegacyGradeConfig>();
		
        public LegacyGradeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyGradeConfigCategory s = o as LegacyGradeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyGradeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyGradeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyGradeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyGradeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyGradeConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyGradeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyGradeConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>KeyId</summary>
		[ProtoMember(2)]
		public int KeyId { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(5)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(6)]
		public int[] AtrVueList { get; set; }
		/// <summary>RequireList</summary>
		[ProtoMember(7)]
		public int[] RequireList { get; set; }
		/// <summary>SpeIdList</summary>
		[ProtoMember(8)]
		public int[] SpeIdList { get; set; }
		/// <summary>SpeVueList</summary>
		[ProtoMember(9)]
		public int[] SpeVueList { get; set; }
		/// <summary>SpeRequireList</summary>
		[ProtoMember(10)]
		public int[] SpeRequireList { get; set; }
		/// <summary>Fee1</summary>
		[ProtoMember(11)]
		public long Fee1 { get; set; }
		/// <summary>Fee2</summary>
		[ProtoMember(12)]
		public long Fee2 { get; set; }

	}
}
