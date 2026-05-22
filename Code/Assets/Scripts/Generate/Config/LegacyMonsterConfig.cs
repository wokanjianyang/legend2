using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyMonsterConfigCategory : ProtoObject, IMerge
    {
        public static LegacyMonsterConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyMonsterConfig> dict = new Dictionary<int, LegacyMonsterConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyMonsterConfig> list = new List<LegacyMonsterConfig>();
		
        public LegacyMonsterConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyMonsterConfigCategory s = o as LegacyMonsterConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyMonsterConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyMonsterConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyMonsterConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyMonsterConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyMonsterConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyMonsterConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyMonsterConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(3)]
		public int Role { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(4)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(5)]
		public int EndLevel { get; set; }
		/// <summary>Atk</summary>
		[ProtoMember(6)]
		public string Attr { get; set; }
		/// <summary>AtkRise</summary>
		[ProtoMember(7)]
		public string AtkRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(8)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(9)]
		public string DefRise { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(10)]
		public string HP { get; set; }
		/// <summary>HpRise</summary>
		[ProtoMember(11)]
		public string HpRise { get; set; }
		/// <summary>LegacyResist</summary>
		[ProtoMember(12)]
		public double LegacyResist { get; set; }
		/// <summary>LegacyRise</summary>
		[ProtoMember(13)]
		public double LegacyRise { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(14)]
		public int[] SkillIdList { get; set; }

	}
}
