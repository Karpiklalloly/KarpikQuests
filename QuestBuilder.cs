﻿using KarpikQuests.Interfaces;
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
            if (aggregator is null) throw new ArgumentNullException(nameof(aggregator));

            _questAggregator = aggregator;
        }

        public QuestBuilder()
        {
            _addToAggregator = false;
        }

        public QuestBuilder Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>("Empty key:" + KeyGenerator.GenerateKey(), name, description);
        }

        public QuestBuilder Start<T>(string key, string name, string description) where T : IQuest, new()
        {
            _quest = new T();
            _quest.Init(key, name, description);
            _addToAggregator = true;
            return this;
        }

        /// <summary>
        /// Clone quest
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public QuestBuilder Start(IQuest quest)
        {
            _quest = (IQuest)quest.Clone();
            _addToAggregator = false;
            return this;
        }

        public QuestBuilder AddTask(IQuestTask task)
        {
            if (_quest.TaskBundles.Has(task)) throw new InvalidOperationException("Quest can't contain equel tasks");

            _quest.AddTask(task);
            return this;
        }

        public QuestBuilder AddBundle(ITaskBundle bundle)
        {
            if (_quest.TaskBundles.Has(bundle)) throw new InvalidOperationException("Quest can't contain equel bundle");

            _quest.AddBundle(bundle);
            return this;
        }

        public QuestBuilder SetComplitionType(ICompletionType completionType)
        {
            _quest.SetCompletionType(completionType);
            return this;
        }

        public QuestBuilder SetTaskProcessorType(IProcessorType processorType)
        {
            _quest.SetTaskProcessorType(processorType);
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
            if (_quest is null) throw new InvalidOperationException("Quest is not setted");

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