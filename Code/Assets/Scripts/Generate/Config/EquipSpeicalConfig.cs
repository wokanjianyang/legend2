using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipSpeicalConfigCategory : ProtoObject, IMerge
    {
        public static EquipSpeicalConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipSpeicalConfig> dict = new Dictionary<int, EquipSpeicalConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipSpeicalConfig> list = new List<EquipSpeicalConfig>();
		
        public EquipSpeicalConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipSpeicalConfigCategory s = o as EquipSpeicalConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipSpeicalConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipSpeicalConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipSpeicalConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipSpeicalConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipSpeicalConfig> GetAll()
        {
            return this.dict;
        }

        public EquipSpeicalConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipSpeicalConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Sid</summary>
		[ProtoMember(2)]
		public int Sid { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>StartLayer</summary>
		[ProtoMember(4)]
		public int StartLayer { get; set; }
		/// <summary>EndLayer</summary>
		[ProtoMember(5)]
		public int EndLayer { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(6)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(7)]
		public double[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(8)]
		public double[] AttrRiseList { get; set; }

	}
}
