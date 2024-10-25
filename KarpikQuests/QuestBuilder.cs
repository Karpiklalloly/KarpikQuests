using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Processors;

namespace Karpik.Quests
{
    public static class QuestBuilder
    {
        public static QuestBuilderPart Start(string name, string description)
        {
            return Start(
                name, 
                description, 
                new Disorderly(),
                new And());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuestBuilderPart Start(string name, string description,
            IProcessorType processor,
            ICompletionType completionType)
        {
            processor ??= new Disorderly();
            completionType ??= new And();
            
            var quest = new Quest(Id.NewId(), name, description,  completionType, processor);

            return new QuestBuilderPart(quest);
        }
        
        public struct QuestBuilderPart
        {
            private Quest _quest;
            private IGraph _graph;
            
            private Action<Quest> _onComplete;
            private Action<Quest> _onFail;

            public QuestBuilderPart(Quest quest)
            {
                _quest = quest;
                _graph = null;
            }

            public readonly QuestBuilderPart SetSubQuests(params Quest[] quests)
            {
                _quest.Clear();
                return this;
            }

            public readonly QuestBuilderPart AddSubQuest(Quest quest)
            {
                if (_quest.Has(quest)) throw new InvalidOperationException("Quest can't contain equal bundle");
                
                _quest.Add(quest);
                return this;
            }
            
            public QuestBuilderPart SetGraph(IGraph graph)
            {
                _graph = graph;
                return this;
            }

            public QuestBuilderPart OnComplete(Action<Quest> onComplete)
            {
                _onComplete += onComplete;
                return this;
            }

            public QuestBuilderPart OnFail(Action<Quest> onFail)
            {
                _onFail += onFail;
                return this;
            }

            public Quest Build()
            {
                if (_quest is null) throw new InvalidOperationException("Quest is not setted");

                _graph ??= new Graph();
                _graph.TryAdd(_quest);
                var quest = _quest;
                    
                _quest = null;
                _graph = null;
                return quest;
            }
        }
    }
}