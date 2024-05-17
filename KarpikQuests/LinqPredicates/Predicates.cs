using System.Linq;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.LinqPredicates
{
    public static class Predicates
    {
        public static bool BundleIsCompleted(ITaskBundle bundle)
        {
            return bundle.Status.IsCompleted();
        }

        public static bool BundleIsFailed(ITaskBundle bundle)
        {
            return bundle.Status.IsFailed();
        }

        public static bool TaskIsCompleted(ITask task)
        {
            return task.IsCompleted();
        }
        
        public static bool TaskIsFailed(ITask task)
        {
            return task.IsFailed();
        }

        public static int BundleCountCompleted(ITaskBundle bundle)
        {
            return bundle.Tasks.Count(TaskIsCompleted);
        }
        
        public static int BundleCountFailed(ITaskBundle bundle)
        {
            return bundle.Tasks.Count(TaskIsFailed);
        }
    }
}