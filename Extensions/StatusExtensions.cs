using KarpikQuests.Interfaces;
using KarpikQuests.Statuses;

namespace KarpikQuests.Extensions
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

        public static bool IsNotStarted(this IQuest quest)
        {
            return quest.Status is UnStarted;
        }
    }
}