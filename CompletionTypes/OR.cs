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
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            return !arr.Any() || arr.SelectMany(bundle => bundle).Any(task => task.Status == ITask.TaskStatus.Completed);
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            return !bundle.Any() || bundle.Any(task => task.Status == ITask.TaskStatus.Completed);
        }
    }
}
