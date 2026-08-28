using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SoulRingConfigCategory : ProtoObject, IMerge
    {
        public static SoulRingConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SoulRingConfig> dict = new Dictionary<int, SoulRingConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SoulRingConfig> list = new List<SoulRingConfig>();
		
        public SoulRingConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SoulRingConfigCategory s = o as SoulRingConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SoulRingConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SoulRingConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SoulRingConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SoulRingConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SoulRingConfig> GetAll()
        {
            return this.dict;
        }

        public SoulRingConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SoulRingConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Sid</summary>
		[ProtoMember(3)]
		public int Sid { get; set; }
		/// <summary>Logo</summary>
		[ProtoMember(4)]
		public string Logo { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(5)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(6)]
		public double[] AtrVueList { get; set; }
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
		/// <summary>ItemId</summary>
		[ProtoMember(11)]
		public int ItemId { get; set; }

	}
}
