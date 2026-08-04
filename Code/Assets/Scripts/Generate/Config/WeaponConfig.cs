using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class WeaponConfigCategory : ProtoObject, IMerge
    {
        public static WeaponConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, WeaponConfig> dict = new Dictionary<int, WeaponConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<WeaponConfig> list = new List<WeaponConfig>();
		
        public WeaponConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            WeaponConfigCategory s = o as WeaponConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (WeaponConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public WeaponConfig Get(int id)
        {
            this.dict.TryGetValue(id, out WeaponConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (WeaponConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, WeaponConfig> GetAll()
        {
            return this.dict;
        }

        public WeaponConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class WeaponConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Sid</summary>
		[ProtoMember(2)]
		public int Sid { get; set; }
		/// <summary>Condtion</summary>
		[ProtoMember(3)]
		public int Condtion { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>Logo</summary>
		[ProtoMember(5)]
		public string Logo { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(6)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(7)]
		public int[] AtrVueList { get; set; }
		/// <summary>GradeAtrIdList</summary>
		[ProtoMember(8)]
		public int[] GradeAtrIdList { get; set; }
		/// <summary>GradeAtrVueList</summary>
		[ProtoMember(9)]
		public int[] GradeAtrVueList { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(10)]
		public string Des { get; set; }

	}
}
