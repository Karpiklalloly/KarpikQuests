using System.Linq;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static class QuestAggregatorExtensions
    {
        public static IQuest? GetQuest(this IQuestAggregator aggregator, string questKey)
        {
            foreach (var x in aggregator.Quests)
            {
                if (x.Key == questKey) return x;
            }

            return null;
        }
    }
}