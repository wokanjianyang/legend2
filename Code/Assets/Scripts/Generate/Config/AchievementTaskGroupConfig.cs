using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AchievementTaskGroupConfigCategory : ProtoObject, IMerge
    {
        public static AchievementTaskGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AchievementTaskGroupConfig> dict = new Dictionary<int, AchievementTaskGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AchievementTaskGroupConfig> list = new List<AchievementTaskGroupConfig>();
		
        public AchievementTaskGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AchievementTaskGroupConfigCategory s = o as AchievementTaskGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AchievementTaskGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AchievementTaskGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AchievementTaskGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AchievementTaskGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AchievementTaskGroupConfig> GetAll()
        {
            return this.dict;
        }

        public AchievementTaskGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AchievementTaskGroupConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>RequireLevel</summary>
		[ProtoMember(3)]
		public int RequireLevel { get; set; }

	}
}
