using Karpik.Quests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class Or : ICompletionType
    {
        public IStatus Check(IEnumerable<ITaskBundle> bundles)
        {
            var unStarted = StatusPool.Instance.Pull<UnStarted>();
            var started = StatusPool.Instance.Pull<Started>();
            var completed = StatusPool.Instance.Pull<Completed>();
            var failed = StatusPool.Instance.Pull<Failed>();

            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();

            if (!arr.Any()) return completed;
            if (arr.Any(Predicates.BundleIsCompleted)) return completed;
            if (arr.All(Predicates.BundleIsFailed)) return failed;
            if (arr.Any(Predicates.BundleIsFailed)) return started;
            
            return unStarted;
        }

        public IStatus Check(ITaskBundle bundle)
        {
            var unStarted = StatusPool.Instance.Pull<UnStarted>();
            var started = StatusPool.Instance.Pull<Started>();
            var completed = StatusPool.Instance.Pull<Completed>();
            var failed = StatusPool.Instance.Pull<Failed>();

            var arr = bundle.ToArray();

            if (!arr.Any()) return completed;
            if (arr.Any(Predicates.TaskIsCompleted)) return completed;
            if (arr.All(Predicates.TaskIsFailed)) return failed;
            if (arr.Any(Predicates.TaskIsFailed)) return started;
            
            return unStarted;
        }
    }
}
