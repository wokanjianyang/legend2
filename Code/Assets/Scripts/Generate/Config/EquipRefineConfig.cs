using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipRefineConfigCategory : ProtoObject, IMerge
    {
        public static EquipRefineConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipRefineConfig> dict = new Dictionary<int, EquipRefineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipRefineConfig> list = new List<EquipRefineConfig>();
		
        public EquipRefineConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipRefineConfigCategory s = o as EquipRefineConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipRefineConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipRefineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipRefineConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipRefineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipRefineConfig> GetAll()
        {
            return this.dict;
        }

        public EquipRefineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipRefineConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(2)]
		public int Position { get; set; }
		/// <summary>AtrList</summary>
		[ProtoMember(3)]
		public int[] AtrList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(4)]
		public long[] AtrVueList { get; set; }
		/// <summary>RequireLevel</summary>
		[ProtoMember(5)]
		public int[] RequireLevel { get; set; }
		/// <summary>SpeAtrList</summary>
		[ProtoMember(6)]
		public int[] SpeAtrList { get; set; }
		/// <summary>SpeVueList</summary>
		[ProtoMember(7)]
		public long[] SpeVueList { get; set; }
		/// <summary>SpeLevel</summary>
		[ProtoMember(8)]
		public int[] SpeLevel { get; set; }

	}
}
