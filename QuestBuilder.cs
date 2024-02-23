using KarpikQuests.CompletionTypes;
using KarpikQuests.Factories;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;
using System;

namespace KarpikQuests
{
    public class QuestBuilder
    {
        public QuestBuilderPart Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>(name, description, new TaskBundleCollection());
        }

        public QuestBuilderPart Start<T>(string name, string description,
            IProcessorType processor, ICompletionType completionType) where T : IQuest, new()
        {
            processor ??= new Disorderly();
            completionType ??= new AND();
            return Start<T>(name, description, new TaskBundleCollection(completionType, processor));
        }

        public QuestBuilderPart Start<T>(string name, string description,
            ITaskBundleCollection? bundles) where T : IQuest, new()
        {
            var quest = new T();

            bundles ??= new TaskBundleCollection();

            quest.Init(KeyGenerator.GenerateKey(""), name, description, bundles);

            return new QuestBuilderPart(quest);
        }

        public QuestBuilderPart Start(IQuest quest)
        {
            return new QuestBuilderPart((IQuest)quest.Clone());
        }
        
        public class QuestBuilderPart
        {
            private IQuest _quest;
            private IQuestAggregator _questAggregator;
            private bool _addToAggregator;

            public QuestBuilderPart(IQuest quest)
            {
                if (quest is null) throw new ArgumentNullException(nameof(quest));

                _quest = quest;
            }
        
            /// <summary>
            /// Clone quest
            /// </summary>
            /// <param name="quest"></param>
            /// <returns></returns>

            public QuestBuilderPart AddBundle(ITaskBundle bundle)
            {
                if (_quest.TaskBundles.Has(bundle)) throw new InvalidOperationException("Quest can't contain equel bundle");

                _quest.AddBundle(bundle);
                return this;
            }

            public QuestBuilderPart AddToAggregatorOnCreate(IQuestAggregator aggregator)
            {
#if DEBUG
                if (aggregator is null) throw new ArgumentNullException(nameof(aggregator));
#endif

                _questAggregator = aggregator;
                _addToAggregator = true;
                return this;
            }

            public QuestBuilderPart OnComplete(Action<IQuest> onComplete)
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
}