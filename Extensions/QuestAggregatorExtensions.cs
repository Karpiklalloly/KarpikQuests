using System.Linq;
using KarpikQuests.Interfaces;

namespace KarpikQuests.Extensions
{
    public static class QuestAggregatorExtensions
    {
        public static IQuest? GetQuest(this IQuestAggregator aggregator, string questKey)
        {
            return aggregator.Quests.FirstOrDefault(x => x.Key == questKey);
        }
    }
}