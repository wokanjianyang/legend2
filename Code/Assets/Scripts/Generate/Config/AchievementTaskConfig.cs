using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AchievementTaskConfigCategory : ProtoObject, IMerge
    {
        public static AchievementTaskConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AchievementTaskConfig> dict = new Dictionary<int, AchievementTaskConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AchievementTaskConfig> list = new List<AchievementTaskConfig>();
		
        public AchievementTaskConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AchievementTaskConfigCategory s = o as AchievementTaskConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AchievementTaskConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AchievementTaskConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AchievementTaskConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AchievementTaskConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AchievementTaskConfig> GetAll()
        {
            return this.dict;
        }

        public AchievementTaskConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AchievementTaskConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>GroupId</summary>
		[ProtoMember(2)]
		public int GroupId { get; set; }
		/// <summary>ConType</summary>
		[ProtoMember(3)]
		public int ConType { get; set; }
		/// <summary>ConRequire</summary>
		[ProtoMember(4)]
		public long ConRequire { get; set; }
		/// <summary>RewardGold</summary>
		[ProtoMember(5)]
		public long RewardGold { get; set; }
		/// <summary>RewardExp</summary>
		[ProtoMember(6)]
		public long RewardExp { get; set; }
		/// <summary>RewardTypeList</summary>
		[ProtoMember(7)]
		public int[] RewardTypeList { get; set; }
		/// <summary>RewardIdList</summary>
		[ProtoMember(8)]
		public int[] RewardIdList { get; set; }
		/// <summary>NumberList</summary>
		[ProtoMember(9)]
		public int[] NumberList { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(10)]
		public string Desc { get; set; }
		/// <summary>RewardText</summary>
		[ProtoMember(11)]
		public string RewardText { get; set; }
		/// <summary>Sort</summary>
		[ProtoMember(12)]
		public int Sort { get; set; }

	}
}
