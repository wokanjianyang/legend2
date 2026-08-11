using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MapModeConfigCategory : ProtoObject, IMerge
    {
        public static MapModeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapModeConfig> dict = new Dictionary<int, MapModeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapModeConfig> list = new List<MapModeConfig>();
		
        public MapModeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MapModeConfigCategory s = o as MapModeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MapModeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapModeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapModeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapModeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapModeConfig> GetAll()
        {
            return this.dict;
        }

        public MapModeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapModeConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartMapId</summary>
		[ProtoMember(2)]
		public int StartMapId { get; set; }
		/// <summary>EndMapId</summary>
		[ProtoMember(3)]
		public int EndMapId { get; set; }
		/// <summary>HpRate</summary>
		[ProtoMember(4)]
		public double HpRate { get; set; }
		/// <summary>DefRate</summary>
		[ProtoMember(5)]
		public double DefRate { get; set; }
		/// <summary>AtkRate</summary>
		[ProtoMember(6)]
		public double AtkRate { get; set; }
		/// <summary>DropRate</summary>
		[ProtoMember(7)]
		public int DropRate { get; set; }
		/// <summary>QualityRate</summary>
		[ProtoMember(8)]
		public int QualityRate { get; set; }
		/// <summary>BossDropRate</summary>
		[ProtoMember(9)]
		public int BossDropRate { get; set; }
		/// <summary>BossQualityRate</summary>
		[ProtoMember(10)]
		public int BossQualityRate { get; set; }

	}
}
