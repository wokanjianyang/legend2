using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetAtrConfigCategory : ProtoObject, IMerge
    {
        public static PetAtrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetAtrConfig> dict = new Dictionary<int, PetAtrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetAtrConfig> list = new List<PetAtrConfig>();
		
        public PetAtrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetAtrConfigCategory s = o as PetAtrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetAtrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetAtrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetAtrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetAtrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetAtrConfig> GetAll()
        {
            return this.dict;
        }

        public PetAtrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetAtrConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(3)]
		public int Role { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>MinValue</summary>
		[ProtoMember(5)]
		public int MinValue { get; set; }
		/// <summary>MaxValue</summary>
		[ProtoMember(6)]
		public int MaxValue { get; set; }
		/// <summary>QualitRise</summary>
		[ProtoMember(7)]
		public int QualitRise { get; set; }
		/// <summary>Percent</summary>
		[ProtoMember(8)]
		public int Percent { get; set; }
		/// <summary>StartQuality</summary>
		[ProtoMember(9)]
		public int StartQuality { get; set; }
		/// <summary>EndQuality</summary>
		[ProtoMember(10)]
		public int EndQuality { get; set; }
		/// <summary>MaxCount</summary>
		[ProtoMember(11)]
		public int MaxCount { get; set; }

	}
}
