using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipRefineFeeConfigCategory : ProtoObject, IMerge
    {
        public static EquipRefineFeeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipRefineFeeConfig> dict = new Dictionary<int, EquipRefineFeeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipRefineFeeConfig> list = new List<EquipRefineFeeConfig>();
		
        public EquipRefineFeeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipRefineFeeConfigCategory s = o as EquipRefineFeeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipRefineFeeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipRefineFeeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipRefineFeeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipRefineFeeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipRefineFeeConfig> GetAll()
        {
            return this.dict;
        }

        public EquipRefineFeeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipRefineFeeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public long StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public long EndLevel { get; set; }
		/// <summary>Fee1</summary>
		[ProtoMember(4)]
		public long Fee1 { get; set; }
		/// <summary>RiseFee1</summary>
		[ProtoMember(5)]
		public long RiseFee1 { get; set; }
		/// <summary>Fee2</summary>
		[ProtoMember(6)]
		public long Fee2 { get; set; }
		/// <summary>RiseFee2</summary>
		[ProtoMember(7)]
		public long RiseFee2 { get; set; }

	}
}
