using KarpikQuests.Interfaces;
using KarpikQuests.QuestStatuses;

namespace KarpikQuests.Extensions
{
    public static partial class QuestExtensions
    {
        public static bool IsCompleted(this IQuest quest)
        {
            return quest.Status is CompletedQuest;
        }

        public static bool IsStarted(this IQuest quest)
        {
            return quest.Status is StartedQuest;
        }

        public static bool IsNotStarted(this IQuest quest)
        {
            return quest.Status is UnStartedQuest;
        }
    }
}