using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AchievementTaskConfigCategory
    {
        public AchievementTaskConfig GetById(int taskId)
        {
            try
            {
                AchievementTaskConfig config = Get(taskId);

                return config;
            }
            catch
            {
            }

            return null;
        }

        public AchievementTaskConfig GetCurrent(int gid, Dictionary<int, bool> dict)
        {
            AchievementTaskConfig config = this.list.Where(m => m.GroupId == gid && !dict.ContainsKey(m.Id)).OrderBy(m => m.Sort).FirstOrDefault();
            return config;
        }

        public static void CheckTask(TaskType type, long condition)
        {
            User user = GameProcessor.Inst.User;

            AchievementTaskConfig config = AchievementTaskConfigCategory.Instance.GetById(1);

            if (config == null)
            {
                return;
            }

            if (config.ConType != (int)type)
            {
                return;
            }

            if (config.ConRequire <= condition)
            {
                user.TaskLog[1] = true;
            }

            GameProcessor.Inst.EventCenter.Raise(new TaskChangeEvent() { });

            return;
        }
    }

    public class TaskHelper
    {


    }
}
