using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestCompletionTypes
{
    [Serializable]
    public class CompletionAND : ICompletionType
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            if (!bundles.Any())
            {
                return true;
            }

            foreach (var bundle in bundles)
            {
                if (!bundle.CompletionType.CheckCompletion(bundle))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            if (!bundle.Any())
            {
                return true;
            }

            foreach (var task in bundle)
            {
                if (task.Status == IQuestTask.TaskStatus.UnCompleted)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
