using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetNameConfigCategory : ProtoObject, IMerge
    {
        public static PetNameConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetNameConfig> dict = new Dictionary<int, PetNameConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetNameConfig> list = new List<PetNameConfig>();
		
        public PetNameConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetNameConfigCategory s = o as PetNameConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetNameConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetNameConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetNameConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetNameConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetNameConfig> GetAll()
        {
            return this.dict;
        }

        public PetNameConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetNameConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Mid</summary>
		[ProtoMember(3)]
		public int Mid { get; set; }

	}
}
