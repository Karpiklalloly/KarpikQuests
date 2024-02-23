using KarpikQuests.Extensions;
using KarpikQuests.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    public class NeededCount : ICompletionType
    {
        public readonly int Count;

        public NeededCount(int count)
        {
            Count = count;
        }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            if (!arr.Any())
            {
                return false;
            }

            int curCount = arr.Sum(bundle => bundle.Count(task => task.IsCompleted()));

            if (curCount >= Count)
            {
                return true;
            }

            return false;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            if (!bundle.Any())
            {
                return true;
            }

            int curCount = bundle.Count(task => task.IsCompleted());

            if (curCount >= Count)
            {
                return true;
            }

            return false;
        }
    }
}
