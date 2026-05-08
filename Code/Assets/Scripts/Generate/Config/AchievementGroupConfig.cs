using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AchievementGroupConfigCategory : ProtoObject, IMerge
    {
        public static AchievementGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AchievementGroupConfig> dict = new Dictionary<int, AchievementGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AchievementGroupConfig> list = new List<AchievementGroupConfig>();
		
        public AchievementGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AchievementGroupConfigCategory s = o as AchievementGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AchievementGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AchievementGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AchievementGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AchievementGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AchievementGroupConfig> GetAll()
        {
            return this.dict;
        }

        public AchievementGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AchievementGroupConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Pid</summary>
		[ProtoMember(3)]
		public int Pid { get; set; }

	}
}
