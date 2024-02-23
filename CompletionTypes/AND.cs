using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    [Serializable]
#pragma warning disable S101 // Types should be named in PascalCase
    public class AND : ICompletionType
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            return !arr.Any() || Array.TrueForAll(arr, bundle => bundle.IsCompleted);
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            return !bundle.Any() || bundle.All(task => task.Status == ITask.TaskStatus.Completed);
        }
    }
}
