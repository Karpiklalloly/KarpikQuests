using System;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class Aggregator : IAggregator
    {
        public event Action<IQuest> QuestStarted;
        public event Action<IQuest> QuestUpdated; 
        public event Action<IQuest> QuestFailed;
        public event Action<IQuest> QuestCompleted;
        
        [JsonIgnore] public IReadOnlyQuestCollection Quests => new QuestCollection(_graphs.SelectMany(graph => graph.Quests).ToList());
        
        [JsonProperty("Graphs")]
        [SerializeThis("Graphs")]
        private IGraphCollection _graphs = new GraphCollection();

        public bool TryAddGraph(IGraph graph)
        {
            if (!graph.IsValid()) return false;
            if (Has(graph)) return false;
            
            _graphs.Add(graph);
            foreach (var quest in graph.Quests)
            {
                Subscribe(quest);
            }
            
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
            
            return true;
        }

        public bool TryReplace(IGraph graph, IQuest from, IQuest to)
        {
            var result = graph.TryReplace(from, to);
            if (!result) return false;
            
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
            return Quests.First(x => x.Id.Equals(id));
        }

        public void Start()
        {
            foreach (var graph in _graphs)
            {
                foreach (var quest in graph.Quests)
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
            foreach (var quest in Quests)
            {
                quest.Reset();
            }
        }

        public void Clear()
        {
            foreach (var graph in _graphs)
            {
                graph.Clear();
            }
        }
        
        public bool Equals(IAggregator other)
        {
            if (other is null) return false;

            return GetHashCode() == other.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            return obj is Aggregator agg && Equals(agg);
        }

        public override int GetHashCode()
        {
            return _graphs.GetHashCode();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var quest in Quests)
            {
                Subscribe(quest);
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