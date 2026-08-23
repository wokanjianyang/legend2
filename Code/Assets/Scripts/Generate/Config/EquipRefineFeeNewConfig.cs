using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipRefineFeeNewConfigCategory : ProtoObject, IMerge
    {
        public static EquipRefineFeeNewConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipRefineFeeNewConfig> dict = new Dictionary<int, EquipRefineFeeNewConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipRefineFeeNewConfig> list = new List<EquipRefineFeeNewConfig>();
		
        public EquipRefineFeeNewConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipRefineFeeNewConfigCategory s = o as EquipRefineFeeNewConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipRefineFeeNewConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipRefineFeeNewConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipRefineFeeNewConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipRefineFeeNewConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipRefineFeeNewConfig> GetAll()
        {
            return this.dict;
        }

        public EquipRefineFeeNewConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipRefineFeeNewConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Fee1</summary>
		[ProtoMember(2)]
		public long Fee1 { get; set; }
		/// <summary>Fee2</summary>
		[ProtoMember(3)]
		public long Fee2 { get; set; }

	}
}
