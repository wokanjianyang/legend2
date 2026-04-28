using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipSetConfigCategory : ProtoObject, IMerge
    {
        public static EquipSetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipSetConfig> dict = new Dictionary<int, EquipSetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipSetConfig> list = new List<EquipSetConfig>();
		
        public EquipSetConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipSetConfigCategory s = o as EquipSetConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipSetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipSetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipSetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipSetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipSetConfig> GetAll()
        {
            return this.dict;
        }

        public EquipSetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipSetConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(3)]
		public int Cycle { get; set; }
		/// <summary>Count</summary>
		[ProtoMember(4)]
		public int Count { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(5)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(6)]
		public int AttrValue { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(7)]
		public double AttrRise { get; set; }

	}
}
