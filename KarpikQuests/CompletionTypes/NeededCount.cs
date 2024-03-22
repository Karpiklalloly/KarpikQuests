using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;

namespace Karpik.Quests.CompletionTypes
{
    public class NeededCount : ICompletionType
    {
        public static NeededCount Instance => new NeededCount();
        
        public int Count { get; private set; }

        public NeededCount()
        {
            
        }

        public NeededCount SetCount(int count)
        {
#if DEBUG
            if (count < 0) throw new ArgumentException(null, nameof(count));
#endif
            
            Count = count;
            return this;
        }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            if (!arr.Any())
            {
                return false;
            }

            var curCount = arr.Sum(Predicates.BundleCount);

            return curCount >= Count;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            if (!bundle.Any())
            {
                return true;
            }

            var curCount = bundle.Count(Predicates.TaskIsCompleted);

            return curCount >= Count;
        }
    }
}
