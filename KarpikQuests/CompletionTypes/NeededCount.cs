using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.CompletionTypes
{
    public class NeededCount : ICompletionType
    {
        public int Count { get; }

        public NeededCount(int count)
        {
            Count = count;
        }

        public IStatus Check(IEnumerable<ITaskBundle> bundles)
        {
            var unStarted = StatusPool.Instance.Pull<UnStarted>();
            var started = StatusPool.Instance.Pull<Started>();
            var completed = StatusPool.Instance.Pull<Completed>();
            var failed = StatusPool.Instance.Pull<Failed>();
            
            if (Count == 0) return completed;

            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            
            var completedCount = arr.Sum(Predicates.BundleCountCompleted);
            var failedCount = arr.Sum(Predicates.BundleCountFailed);
            
            if (completedCount >= Count) return completed;
            if (arr.Length - failedCount < Count) return failed;
            if (completedCount > 0 || failedCount > 0) return started;
            
            return unStarted;
        }

        public IStatus Check(ITaskBundle bundle)
        {
            var unStarted = StatusPool.Instance.Pull<UnStarted>();
            var started = StatusPool.Instance.Pull<Started>();
            var completed = StatusPool.Instance.Pull<Completed>();
            var failed = StatusPool.Instance.Pull<Failed>();
            
            if (Count == 0) return completed;

            var arr = bundle.ToArray();
            
            var completedCount = arr.Count(Predicates.TaskIsCompleted);
            var failedCount = arr.Count(Predicates.TaskIsFailed);
            
            if (completedCount >= Count) return completed;
            if (arr.Length - failedCount < Count) return failed;
            if (completedCount > 0 || failedCount > 0) return started;
            
            return unStarted;
        }
    }
}
