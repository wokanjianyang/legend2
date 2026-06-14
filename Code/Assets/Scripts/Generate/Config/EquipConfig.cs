using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipConfigCategory : ProtoObject, IMerge
    {
        public static EquipConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipConfig> dict = new Dictionary<int, EquipConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipConfig> list = new List<EquipConfig>();
		
        public EquipConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipConfigCategory s = o as EquipConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipConfig> GetAll()
        {
            return this.dict;
        }

        public EquipConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(2)]
		public int Cycle { get; set; }
		/// <summary>LevelRequired</summary>
		[ProtoMember(3)]
		public int LevelRequired { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(5)]
		public int Part { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(6)]
		public int[] Position { get; set; }
		/// <summary>基础属性列表</summary>
		[ProtoMember(7)]
		public int[] AttrIdList { get; set; }
		/// <summary>基础属性值</summary>
		[ProtoMember(8)]
		public long[] AttrValueList { get; set; }
		/// <summary>Price</summary>
		[ProtoMember(9)]
		public long Price { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(10)]
		public int Role { get; set; }
		/// <summary>RecoveryItemId</summary>
		[ProtoMember(11)]
		public int RecoveryItemId { get; set; }
		/// <summary>CardGroupId</summary>
		[ProtoMember(12)]
		public int CardGroupId { get; set; }
		/// <summary>CardQuality</summary>
		[ProtoMember(13)]
		public int CardQuality { get; set; }
		/// <summary>CardAtrList</summary>
		[ProtoMember(14)]
		public int[] CardAtrList { get; set; }
		/// <summary>CardVueList</summary>
		[ProtoMember(15)]
		public int[] CardVueList { get; set; }
		/// <summary></summary>
		[ProtoMember(16)]
		public int[] SpeAtrIdList { get; set; }
		/// <summary></summary>
		[ProtoMember(17)]
		public long[] SpeAtrVueList { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(18)]
		public int Quality { get; set; }

	}
}
