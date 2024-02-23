using KarpikQuests.CompletionTypes;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;
using System;
using System.Runtime.CompilerServices;

namespace KarpikQuests
{
    public static class QuestBuilder
    {
        public static QuestBuilderPart Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>(name, description, new TaskBundleCollection());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            IProcessorType? processor, ICompletionType? completionType) where T : IQuest, new()
        {
            processor ??= new Disorderly();
            completionType ??= new AND();
            return Start<T>(name, description, new TaskBundleCollection(completionType, processor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            ITaskBundleCollection? bundles) where T : IQuest, new()
        {
            var quest = new T();

            bundles ??= new TaskBundleCollection();

            quest.Init(KeyGenerator.GenerateKey(""), name, description, bundles);

            return new QuestBuilderPart(quest);
        }

        /// <summary>
        /// Clone quest
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start(IQuest quest)
        {
#if DEBUG
            if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

            return new QuestBuilderPart((IQuest)quest.Clone());
        }
        
        public struct QuestBuilderPart
        {
            private IQuest _quest;
            private IQuestAggregator _questAggregator;
            private bool _addToAggregator;

            public QuestBuilderPart(IQuest quest)
            {
#if DEBUG
                if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

                _quest = quest;
                _questAggregator = null;
                _addToAggregator = false;
            }
        
            public readonly QuestBuilderPart AddBundle(ITaskBundle bundle)
            {
#if DEBUG
                if (bundle is null) throw new ArgumentNullException(nameof(bundle));
#endif
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

            public readonly QuestBuilderPart OnComplete(Action<IQuest> onComplete)
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

                var quest = _quest;
                _quest = null;
                return quest;
            }
        }
    }
}