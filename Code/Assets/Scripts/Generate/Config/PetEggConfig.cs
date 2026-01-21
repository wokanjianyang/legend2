using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetEggConfigCategory : ProtoObject, IMerge
    {
        public static PetEggConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetEggConfig> dict = new Dictionary<int, PetEggConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetEggConfig> list = new List<PetEggConfig>();
		
        public PetEggConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetEggConfigCategory s = o as PetEggConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetEggConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetEggConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetEggConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetEggConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetEggConfig> GetAll()
        {
            return this.dict;
        }

        public PetEggConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetEggConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>PhyAttr</summary>
		[ProtoMember(5)]
		public string PhyAttr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(6)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(7)]
		public string HP { get; set; }

	}
}
