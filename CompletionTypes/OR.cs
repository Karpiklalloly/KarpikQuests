using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    [Serializable]
    public class OR : ICompletionType
    {
        public bool SuccessResult { get; }

        public OR(bool successResult = true)
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
                foreach (var task in bundle)
                {
                    if (task.Status == ITask.TaskStatus.Completed)
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
                if (task.Status == ITask.TaskStatus.Completed)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
