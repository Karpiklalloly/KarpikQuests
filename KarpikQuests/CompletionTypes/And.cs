using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;
using Karpik.Quests.LinqPredicates;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class And : ICompletionType
    {
        public IStatus Check(IEnumerable<ITaskBundle> bundles)
        {
            var unStarted = new UnStarted();
            var started = new Started();
            var completed = new Completed();
            var failed = new Failed();

            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            if (!arr.Any()) return completed;
            if (arr.Any(Predicates.BundleIsFailed)) return failed;
            if (arr.All(Predicates.BundleIsCompleted)) return completed;
            if (arr.Any(Predicates.BundleIsCompleted) ||
                arr.Any(Predicates.BundleIsFailed)) return started;
            
            return unStarted;
        }

        public IStatus Check(ITaskBundle bundle)
        {
            var unStarted = new UnStarted();
            var started = new Started();
            var completed = new Completed();
            var failed = new Failed();

            var arr = bundle.ToArray();
            if (!arr.Any()) return completed;
            
            if (arr.Any(Predicates.TaskIsFailed)) return failed;
            if (arr.All(Predicates.TaskIsCompleted)) return completed;
            if (arr.Any(Predicates.TaskIsCompleted) ||
                arr.Any(Predicates.TaskIsFailed)) return started;
            //task is started
            return unStarted;
        }
    }
}
