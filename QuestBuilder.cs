using JetBrains.Annotations;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            if (!IsValid(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!IsValid(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!IsValid(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            _quest = new T();
            _quest.Init(key, name, description);
            _addToAggregator = true;
            return this;
        }

        /// <summary>
        /// Copy quest
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
            if (_quest.TaskBundles.ContainsTask(task))
            {
                throw new InvalidOperationException("Quest can't contain equel tasks");
            }
            _quest.AddTask(task);
            return this;
        }

        public QuestBuilder RemoveTask(IQuestTask task)
        {
            if (!_quest.TaskBundles.ContainsTask(task))
            {
                throw new InvalidOperationException("Quest does not contain task");
            }
            _quest.RemoveTask(task);
            return this;
        }

        public QuestBuilder AddBundle(ITaskBundle bundle)
        {
            if (_quest.TaskBundles.Contains(bundle))
            {
                throw new InvalidOperationException("Quest can't contain equel bundles");
            }
            _quest.AddBundle(bundle);
            return this;
        }

        public QuestBuilder RemoveBundle(ITaskBundle bundle)
        {
            if (!_quest.TaskBundles.Contains(bundle))
            {
                throw new InvalidOperationException("Quest can't contain equel bundles");
            }
            _quest.RemoveBundle(bundle);
            return this;
        }

        public QuestBuilder SetCustomKey(string key)
        {
            if (!IsValid(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            IQuest quest = (IQuest)_quest.Clone();
            quest.SetKey(key);

            _questAggregator.TryToReplace(_quest, quest, true);
            _quest.Dispose();
            _quest = quest;
            return this;
        }

        public QuestBuilder SetComplitionType([NotNull] IQuestCompletionType completionType)
        {
            _quest.SetCompletionType(completionType);
            return this;
        }

        public QuestBuilder SetTaskProcessorType([NotNull] IQuestTaskProcessorType processorType)
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
            if (_quest == null)
            {
                throw new InvalidOperationException("Quest is not setted");
            }

            if (_addToAggregator)
            {
                _questAggregator.TryAddQuest(_quest);
            }
            IQuest quest = _quest;
            _quest = null;
            return quest;
        }

        private bool IsValid(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return true;
        }
    }
}