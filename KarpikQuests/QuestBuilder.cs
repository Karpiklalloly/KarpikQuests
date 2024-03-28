using System;
using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests
{
    public static class QuestBuilder
    {
        public static QuestBuilderPart Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>(
                name, 
                description, 
                ProcessorTypesPool.Instance.Pull<Disorderly>(), 
                CompletionTypesPool.Instance.Pull<And>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            IProcessorType? processor, ICompletionType? completionType) where T : IQuest, new()
        {
            processor ??= ProcessorTypesPool.Instance.Pull<Disorderly>();
            completionType ??= CompletionTypesPool.Instance.Pull<And>();
            return Start<T>(name, description, new TaskBundleCollection(), processor, completionType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            ITaskBundleCollection? bundles,
            IProcessorType? processor,
            ICompletionType? completionType) where T : IQuest, new()
        {
            var quest = new T();

            bundles ??= new TaskBundleCollection();
            processor ??= ProcessorTypesPool.Instance.Pull<Orderly>();
            completionType ??= CompletionTypesPool.Instance.Pull<And>();
            
            quest.Init(name, description, bundles, completionType, processor);

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
            private IGraph _graph;

            public QuestBuilderPart(IQuest quest)
            {
#if DEBUG
                if (quest is null) throw new ArgumentNullException(nameof(quest));
#endif

                _quest = quest;
                _questAggregator = null;
                _graph = null;
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

            public QuestBuilderPart SetAggregator(IQuestAggregator aggregator)
            {
#if DEBUG
                if (aggregator is null) throw new ArgumentNullException(nameof(aggregator));
#endif

                _questAggregator = aggregator;
                return this;
            }
            
            public QuestBuilderPart SetGraph(IGraph graph)
            {
#if DEBUG
                if (graph is null) throw new ArgumentNullException(nameof(graph));
#endif

                _graph = graph;
                return this;
            }

            public readonly QuestBuilderPart OnComplete(Action<IQuest> onComplete)
            {
                _quest.Completed += onComplete;
                return this;
            }

            public readonly QuestBuilderPart OnFail(Action<IQuest> onFail)
            {
                _quest.Failed += onFail;
                return this;
            }

            public IQuest Create()
            {
                if (_quest is null) throw new InvalidOperationException("Quest is not setted");

                if (!_questAggregator.TryAddGraph(_graph))
                {
                    _graph = new QuestGraph();
                    _questAggregator.TryAddGraph(_graph);
                }
                _questAggregator.TryAddQuest(_graph, _quest);

                var quest = _quest;
                _quest = null;
                return quest;
            }
        }
    }
}