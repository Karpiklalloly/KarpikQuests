using System;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class QuestAggregator : IQuestAggregator
    {
        public event Action<IQuest> QuestStarted; 
        public event Action<IQuest> QuestUpdated; 
        public event Action<IQuest> QuestFailed; 
        public event Action<IQuest> QuestCompleted;
        
        [JsonIgnore] public IReadOnlyQuestCollection Quests => _quests;
        
        [JsonProperty("Graphs")]
        private IGraphCollection _graphs = new GraphCollection();
        private IQuestCollection _quests = new QuestCollection();

        public bool TryAddGraph(IGraph graph)
        {
            if (!graph.IsValid()) return false;
            if (Has(graph)) return false;
            
            _graphs.Add(graph);
            return true;
        }
        
        public bool TryAddQuest(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (graph.Has(quest)) return false;
            
            var result = graph.TryAdd(quest);

            if (!result) return false;
            
            _quests.Add(quest);
            Subscribe(quest);

            return true;
        }
        
        public bool TryRemoveQuest(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            
            UnSubscribe(quest);
            graph.TryRemove(quest);
            _quests.Remove(quest);
            
            return true;
        }

        public bool TryReplace(IGraph graph, IQuest from, IQuest to)
        {
            var result = graph.TryReplace(from, to);
            if (!result) return false;

            _quests.Remove(from);
            _quests.Add(to);
            
            return true;
        }

        public bool TryAddDependence(IGraph graph, IQuest quest, IQuest dependence, IDependencyType dependencyType)
        {
            if (!graph.IsValid()) return false;

            return graph.TryAddDependency(quest, dependence, dependencyType);
        }

        public bool TryRemoveDependence(IGraph graph, IQuest quest, IQuest dependence)
        {
            if (!graph.IsValid()) return false;
            
            return graph.TryRemoveDependency(quest.Id, dependence.Id);
        }

        public bool TryRemoveDependencies(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;

            return graph.TryRemoveDependencies(quest);
        }

        public bool TryRemoveDependents(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            
            return graph.TryRemoveDependents(quest);
        }

        public IQuestCollection GetDependencies(IGraph graph, IQuest quest)
        {
            IQuestCollection collection = new QuestCollection();
            
            if (!graph.IsValid()) return collection;

            foreach (var connection in graph.GetDependenciesQuests(quest))
            {
                collection.Add(connection.DependencyQuest);
            }
            return collection;
        }

        public IQuestCollection GetDependents(IGraph graph, IQuest quest)
        {
            IQuestCollection collection = new QuestCollection();
            
            if (!graph.IsValid()) return collection;

            foreach (var connection in graph.GetDependentsQuests(quest))
            {
                collection.Add(connection.DependentQuest);
            }
            return collection;
        }

        public bool Has(IGraph graph)
        {
            return _graphs.Has(graph);
        }

        public bool Has(IQuest quest)
        {
            return _graphs.Any(graph => graph.Has(quest));
        }

        public IQuest Get(Id id)
        {
            return _quests.First(x => x.Id.Equals(id));
        }

        public void Start()
        {
            foreach (var quest in _quests)
            {
                foreach (var graph in _graphs)
                {
                    if (!graph.GetDependenciesQuests(quest).Any() && quest.IsUnStarted())
                    {
                        quest.Start();
                    }
                }
            }
        }

        public void ResetQuests()
        {
            foreach (var quest in _quests)
            {
                quest.Reset();
            }
        }

        public void Clear()
        {
            foreach (var quest in _quests)
            {
                quest.Clear();
            }
            
            _quests.Clear();
        }
        
        public bool Equals(IQuestAggregator other)
        {
            if (other is null) return false;

            return GetHashCode() == other.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return obj is QuestAggregator agg && Equals(agg);
        }

        public override int GetHashCode()
        {
            return _quests.GetHashCode();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var graph in _graphs)
            {
                foreach (var quest in graph.Quests)
                {
                    _quests.Add(quest);
                    Subscribe(quest);
                }
            }
        }

        private void Subscribe(IQuest quest)
        {
            quest.Started    += OnQuestStarted;
            quest.Updated    += OnQuestUpdated;
            quest.Completed  += OnQuestCompleted;
            quest.Failed     += OnQuestFailed;
        }

        private void UnSubscribe(IQuest quest)
        {
            quest.Started    -= OnQuestStarted;
            quest.Updated    -= OnQuestUpdated;
            quest.Completed  -= OnQuestCompleted;
            quest.Failed     -= OnQuestFailed;
        }

        private void OnQuestStarted(IQuest quest)
        {
            QuestStarted?.Invoke(quest);
        }

        private void OnQuestCompleted(IQuest quest)
        {
            QuestCompleted?.Invoke(quest);
        }
        
        private void OnQuestFailed(IQuest quest)
        {
            QuestFailed?.Invoke(quest);
        }
        
        private void OnQuestUpdated(IQuest quest, ITaskBundle bundle)
        {
            foreach (var graph in _graphs)
            {
                foreach (var connection in graph.GetDependentsQuests(quest))
                {
                    if (connection.Dependency.IsOk(connection.DependencyQuest))
                    {
                        connection.DependentQuest.Start();
                    }
                }
            }
            
            QuestUpdated?.Invoke(quest);
        }
    }
}