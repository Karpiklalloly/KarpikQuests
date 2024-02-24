using KarpikQuests.Interfaces;
using KarpikQuests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    [Serializable]
    public class AND : ICompletionType
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            return !bundles.Any() || bundles.All(bundle => bundle.IsCompleted);
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            return !bundle.Any() || bundle.All(task => task.IsCompleted());
        }
    }
}
