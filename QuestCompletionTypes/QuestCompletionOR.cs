using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestCompletionTypes
{
    [Serializable]
    public class QuestCompletionOR : IQuestCompletionType
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            if (!bundles.Any())
            {
                return true;
            }

            foreach (var bundle in bundles)
            {
                foreach (var task in bundle)
                {
                    if (task.Status == IQuestTask.TaskStatus.Completed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            if (!bundle.Any())
            {
                return true;
            }

            foreach (var task in bundle)
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
