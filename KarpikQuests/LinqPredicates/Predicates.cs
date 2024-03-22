using System.Linq;
using Karpik.Quests.Extensions;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.LinqPredicates
{
    public static class Predicates
    {
        public static bool BundleIsCompleted(ITaskBundle bundle)
        {
            return bundle.IsCompleted;
        }

        public static bool TaskIsCompleted(ITask task)
        {
            return task.IsCompleted();
        }

        public static int BundleCount(ITaskBundle bundle)
        {
            return bundle.Count(TaskIsCompleted);
        }
    }
}