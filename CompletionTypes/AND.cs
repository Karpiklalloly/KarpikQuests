using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    [Serializable]
    public class AND : ICompletionType
    {
        public bool SuccessResult { get; }

        public AND(bool successResult = true)
        {
            SuccessResult = successResult;
        }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            if (!bundles.Any())
            {
                return true;
            }

            foreach (var bundle in bundles)
            {
                if (!bundle.CheckCompletion())
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
