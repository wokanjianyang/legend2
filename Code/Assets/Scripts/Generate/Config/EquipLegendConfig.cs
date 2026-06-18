using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipLegendConfigCategory : ProtoObject, IMerge
    {
        public static EquipLegendConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipLegendConfig> dict = new Dictionary<int, EquipLegendConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipLegendConfig> list = new List<EquipLegendConfig>();
		
        public EquipLegendConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipLegendConfigCategory s = o as EquipLegendConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipLegendConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipLegendConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipLegendConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipLegendConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipLegendConfig> GetAll()
        {
            return this.dict;
        }

        public EquipLegendConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipLegendConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(3)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(4)]
		public long[] AtrVueList { get; set; }
		/// <summary>SetId</summary>
		[ProtoMember(5)]
		public int SetId { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(6)]
		public double Fee { get; set; }
		/// <summary>Mc</summary>
		[ProtoMember(7)]
		public int Mc { get; set; }

	}
}
