using KarpikQuests.Interfaces;
using System.Collections.Generic;

namespace KarpikQuests
{
    public static class QuestInfo
    {
        private static readonly List<IQuestAggregator> _aggregators = new List<IQuestAggregator>();

        public static void RegisterAggregator(IQuestAggregator aggregator)
        {
            if (_aggregators.Contains(aggregator))
            {
                return;
            }

            _aggregators.Add(aggregator);
        }

        public static IQuest GetQuest(string key)
        {
            foreach (var item in _aggregators)
            {
                foreach (var quest in item.Quests)
                {
                    if (quest.Key.Equals(key))
                    {
                        return quest;
                    }
                }
            }

            return null;
        }
    }
}
