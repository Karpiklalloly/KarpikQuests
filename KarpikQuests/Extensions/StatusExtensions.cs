using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;

namespace Karpik.Quests.Extensions
{
    public static class StatusExtensions
    {
        public static bool IsCompleted(this IQuest quest)
        {
            return quest.Status is Completed;
        }

        public static bool IsStarted(this IQuest quest)
        {
            return quest.Status is Started;
        }

        public static bool IsUnStarted(this IQuest quest)
        {
            return quest.Status is UnStarted;
        }

        public static bool IsFailed(this IQuest quest)
        {
            return quest.Status is Failed;
        }
    }
}