using System;
using System.Runtime.CompilerServices;
using Karpik.Quests.Factories;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests
{
    public static class QuestBuilder
    {
        public static QuestBuilderPart Start<T>(string name, string description) where T : IQuest, new()
        {
            return Start<T>(
                name, 
                description, 
                ProcessorFactory.Instance.Create(), 
                CompletionTypesFactory.Instance.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            IProcessorType processor, ICompletionType completionType) where T : IQuest, new()
        {
            return Start<T>(
                name,
                description,
                TaskBundleCollectionFactory.Instance.Create(),
                processor,
                completionType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start<T>(string name, string description,
            ITaskBundleCollection? bundles,
            IProcessorType? processor,
            ICompletionType? completionType) where T : IQuest, new()
        {
            var quest = new T();

            bundles ??= new TaskBundleCollection();
            processor ??= ProcessorFactory.Instance.Create();
            completionType ??= CompletionTypesFactory.Instance.Create();
            
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
            return new QuestBuilderPart((IQuest)quest.Clone());
        }
        
        public struct QuestBuilderPart
        {
            private IQuest _quest;
            private IQuestAggregator _questAggregator;
            private IGraph _graph;

            public QuestBuilderPart(IQuest quest)
            {
                _quest = quest;
                _questAggregator = null;
                _graph = null;
            }
        
            public readonly QuestBuilderPart AddBundle(ITaskBundle bundle)
            {
                if (_quest.TaskBundles.Has(bundle)) throw new InvalidOperationException("Quest can't contain equel bundle");

                _quest.AddBundle(bundle);
                return this;
            }

            public QuestBuilderPart SetAggregator(IQuestAggregator aggregator)
            {
                _questAggregator = aggregator;
                return this;
            }
            
            public QuestBuilderPart SetGraph(IGraph graph)
            {
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

                if (!(_questAggregator is null))
                {
                    _graph ??= new QuestGraph();
                    _questAggregator.TryAddGraph(_graph);
                    _questAggregator.TryAddQuest(_graph, _quest);
                }

                var quest = _quest;
                _quest = null;
                _questAggregator = null;
                _graph = null;
                return quest;
            }
        }
    }
}