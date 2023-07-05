using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using System;
using System.Linq;

namespace KarpikQuests
{
    public class QuestBuilder
    {
        private IQuest _quest;
        private readonly IQuestAggregator _questAggregator;
        private bool _addToAggregator = true;

        public QuestBuilder(IQuestAggregator aggregator)
        {
            _questAggregator = aggregator;
        }

        public QuestBuilder Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>("Empty key" + QuestKeyGenerator.GenerateNextAutoKey(), name, description);
        }

        public QuestBuilder Start<T>(string key, string name, string description) where T : IQuest, new()
        {
            _quest = new T();
            _quest.Init(key, name, description);
            _addToAggregator = true;
            return this;
        }

        /// <summary>
        /// Uses to modify quest (not to copy)
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public QuestBuilder Start(IQuest quest)
        {
            _quest = quest;
            return this;
        }

        public QuestBuilder AddTask(IQuestTask task)
        {
            if (_quest.Tasks.Select(x => x.Key).Contains(task.Key))
            {
                throw new InvalidOperationException("Quest can't contain equel tasks' keys");
            }
            _quest.AddTask(task);
            task.Completed += _quest.OnTaskComplete;
            return this;
        }

        public QuestBuilder RemoveTask(IQuestTask task)
        {
            if (!_quest.Tasks.Select(x => x.Key).Contains(task.Key))
            {
                throw new InvalidOperationException("Quest does not contain equel tasks' keys");
            }
            _quest.RemoveTask(task);
            return this;
        }

        public QuestBuilder SetCustomKey(string key)
        {
            _quest.SetKey(key);
            return this;
        }

        public QuestBuilder DoNotAddAggregatorOnCreate()
        {
            _addToAggregator = false;
            return this;
        }

        public QuestBuilder OnComplete(Action<IQuest> onComplete)
        {
            _quest.Completed += onComplete;
            return this;
        }

        public IQuest Create()
        {
            if (_addToAggregator)
            {
                _questAggregator.TryAddQuest(_quest);
            }
            IQuest quest = _quest;
            _quest = null;
            return quest;
        }
    }
}