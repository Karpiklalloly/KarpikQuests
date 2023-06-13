using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using System;

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
            return this;
        }

        public QuestBuilder AddTask(IQuestTask task)
        {
            _quest.AddTask(task);
            task.Completed += _quest.OnTaskComplete;
            return this;
        }

        public QuestBuilder SetCustomKey(string key)
        {
            _quest.SetKey(key);
            return this;
        }

        public QuestBuilder DoNotAddToAggregatorOnCreate()
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