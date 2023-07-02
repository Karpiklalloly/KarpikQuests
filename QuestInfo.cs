using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests
{
    //TODO: optimize getters
    //TODO: Везде вместо комлит нормальные сравнения сделать
    public static class QuestInfo
    {
        private static readonly List<IQuestAggregator> _aggregators = new List<IQuestAggregator>();

        public static void RegisterAggregator(IQuestAggregator aggregator)
        {
            if (Contains(aggregator))
            {
                return;
            }

            _aggregators.Add(aggregator);
        }

        public static void UnRegisterAggregator(IQuestAggregator aggregator)
        {
            if (!Contains(aggregator))
            {
                return;
            }

            _aggregators.Remove(aggregator);
        }

        public static IQuest GetQuest(string questKey)
        {
            var aggregator = GetAggregator(questKey);
            foreach (var quest in aggregator.Quests)
            {
                if (quest.Key.Equals(questKey))
                {
                    return quest;
                }
            }

            return null;
        }

        public static IQuestAggregator GetAggregator(string questKey)
        {
            foreach (var item in _aggregators)
            {
                foreach (var quest in item.Quests)
                {
                    if (quest.Key.Equals(questKey))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public static IQuest GetQuestByTask(string taskKey)
        {
            var quests = GetQuests();

            foreach (var quest in quests)
            {
                if (quest.Tasks.Any( x=> x.Key.Equals(taskKey)))
                {
                    return quest;
                }
            }

            return null;
        }

        public static IQuestCollection GetQuests()
        {
            var listOfLists = _aggregators.Select(x => x.Quests);
            QuestCollection list = new QuestCollection();
            foreach (var collection in listOfLists)
            {
                foreach (var quest in collection)
                {
                    if (list.Contains(quest))
                    {
                        continue;
                    }

                    list.Add(quest);
                }
            }
            return list;
        }

        public static IQuestTaskCollection GetTasks()
        {
            var tasksList = GetQuests().Select(x => x.Tasks);
            QuestTaskCollection collection = new QuestTaskCollection();
            foreach (var taskList in tasksList)
            {
                foreach (var task in taskList)
                {
                    if (collection.Contains(task))
                    {
                        continue;
                    }
                    collection.Add(task);
                }
            }
            return collection;
        }

        public static IQuestTask GetTask(string taskKey)
        {
            var quest = GetQuestByTask(taskKey);
            return quest?.Tasks.First(x => x.Key.Equals(taskKey));
        }

        private static bool Contains(IQuestAggregator aggregator)
        {
            foreach (var item in _aggregators)
            {
                if (aggregator.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
