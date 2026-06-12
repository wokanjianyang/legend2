using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetConfigCategory : ProtoObject, IMerge
    {
        public static PetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetConfig> dict = new Dictionary<int, PetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetConfig> list = new List<PetConfig>();
		
        public PetConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetConfigCategory s = o as PetConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetConfig> GetAll()
        {
            return this.dict;
        }

        public PetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>CardGroupId</summary>
		[ProtoMember(3)]
		public int CardGroupId { get; set; }
		/// <summary>CardQuality</summary>
		[ProtoMember(4)]
		public int CardQuality { get; set; }
		/// <summary>CardAtrList</summary>
		[ProtoMember(5)]
		public int[] CardAtrList { get; set; }
		/// <summary>CardVueList</summary>
		[ProtoMember(6)]
		public int[] CardVueList { get; set; }
		/// <summary>TraitId</summary>
		[ProtoMember(7)]
		public int TraitId { get; set; }
		/// <summary>TraitLevel</summary>
		[ProtoMember(8)]
		public int TraitLevel { get; set; }

	}
}
