using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetTraitConfigCategory : ProtoObject, IMerge
    {
        public static PetTraitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetTraitConfig> dict = new Dictionary<int, PetTraitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetTraitConfig> list = new List<PetTraitConfig>();
		
        public PetTraitConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetTraitConfigCategory s = o as PetTraitConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetTraitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetTraitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetTraitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetTraitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetTraitConfig> GetAll()
        {
            return this.dict;
        }

        public PetTraitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetTraitConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(3)]
		public int Role { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>LevelRate</summary>
		[ProtoMember(5)]
		public int LevelRate { get; set; }
		/// <summary>StartPetId</summary>
		[ProtoMember(6)]
		public int StartPetId { get; set; }
		/// <summary>EndPetId</summary>
		[ProtoMember(7)]
		public int EndPetId { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(8)]
		public int Rate { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(9)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(10)]
		public int[] AtrVueList { get; set; }
		/// <summary>AtrVueList1</summary>
		[ProtoMember(11)]
		public int[] AtrVueList1 { get; set; }
		/// <summary>RiseType</summary>
		[ProtoMember(12)]
		public int[] RiseType { get; set; }
		/// <summary>StartQuality</summary>
		[ProtoMember(13)]
		public int StartQuality { get; set; }
		/// <summary>EndQuality</summary>
		[ProtoMember(14)]
		public int EndQuality { get; set; }

	}
}
