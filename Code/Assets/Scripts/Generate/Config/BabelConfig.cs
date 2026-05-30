using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class BabelConfigCategory : ProtoObject, IMerge
    {
        public static BabelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BabelConfig> dict = new Dictionary<int, BabelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BabelConfig> list = new List<BabelConfig>();
		
        public BabelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            BabelConfigCategory s = o as BabelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (BabelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BabelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BabelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BabelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BabelConfig> GetAll()
        {
            return this.dict;
        }

        public BabelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BabelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Start</summary>
		[ProtoMember(2)]
		public int Start { get; set; }
		/// <summary>End</summary>
		[ProtoMember(3)]
		public int End { get; set; }
		/// <summary>ItemTypeList</summary>
		[ProtoMember(4)]
		public int[] ItemTypeList { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(5)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemCountList</summary>
		[ProtoMember(6)]
		public int[] ItemCountList { get; set; }
		/// <summary>RewardLevelList</summary>
		[ProtoMember(7)]
		public int[] RewardLevelList { get; set; }

	}
}
