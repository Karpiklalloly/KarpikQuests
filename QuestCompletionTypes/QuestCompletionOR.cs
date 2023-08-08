using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestCompletionTypes
{
    [Serializable]
    public class QuestCompletionOR : IQuestCompletionType
    {
        public bool CheckCompletion(IEnumerable<IQuestTask> tasks)
        {
            if (tasks.Count() == 0)
            {
                return true;
            }

            foreach (var task in tasks)
            {
                if (task.Status == IQuestTask.TaskStatus.Completed)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
