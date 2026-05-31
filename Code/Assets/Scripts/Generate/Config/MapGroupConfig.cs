using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MapGroupConfigCategory : ProtoObject, IMerge
    {
        public static MapGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapGroupConfig> dict = new Dictionary<int, MapGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapGroupConfig> list = new List<MapGroupConfig>();
		
        public MapGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MapGroupConfigCategory s = o as MapGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MapGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapGroupConfig> GetAll()
        {
            return this.dict;
        }

        public MapGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapGroupConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(4)]
		public string Memo { get; set; }
		/// <summary>地图Id掉落</summary>
		[ProtoMember(5)]
		public int[] DropIdList { get; set; }
		/// <summary>DropRateList</summary>
		[ProtoMember(6)]
		public int[] DropRateList { get; set; }
		/// <summary>BaseIdList</summary>
		[ProtoMember(7)]
		public int[] BaseIdList { get; set; }
		/// <summary>BaseRateList</summary>
		[ProtoMember(8)]
		public int[] BaseRateList { get; set; }

	}
}
