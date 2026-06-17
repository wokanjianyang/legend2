using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipLegendSetConfigCategory : ProtoObject, IMerge
    {
        public static EquipLegendSetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipLegendSetConfig> dict = new Dictionary<int, EquipLegendSetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipLegendSetConfig> list = new List<EquipLegendSetConfig>();
		
        public EquipLegendSetConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipLegendSetConfigCategory s = o as EquipLegendSetConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipLegendSetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipLegendSetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipLegendSetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipLegendSetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipLegendSetConfig> GetAll()
        {
            return this.dict;
        }

        public EquipLegendSetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipLegendSetConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Count</summary>
		[ProtoMember(3)]
		public int Count { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(4)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(5)]
		public int[] AtrVueList { get; set; }
		/// <summary>RiseList</summary>
		[ProtoMember(6)]
		public double[] RiseList { get; set; }
		/// <summary>BuffId</summary>
		[ProtoMember(7)]
		public int BuffId { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(8)]
		public string Desc { get; set; }

	}
}
