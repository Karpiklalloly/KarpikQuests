using System;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Statuses;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class QuestAggregator : IQuestAggregator
    {
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

            var node = new GraphNode(quest);
            var result = graph.TryAdd(node);

            if (!result) return false;
            
            _quests.Add(quest);
            Subscribe(node);

            return true;
        }
        
        public bool TryRemoveQuest(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            
            var node = graph.GetNode(quest);
            UnSubscribe(node);
            graph.TryRemove(node);
            _quests.Remove(quest);
            
            return true;
        }

        public bool TryAddDependence(IGraph graph, IQuest quest, IQuest dependence, IDependencyType dependencyType)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!dependence.IsValid()) return false;
            if (!dependencyType.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            if (!graph.Has(dependence)) return false;

            var node = graph.GetNode(quest);
            var dependenceNode = graph.GetNode(dependence);
            return node.TryAddDependency(new IGraphNode.Connection(dependenceNode.NodeId, dependencyType));
        }

        public bool TryRemoveDependence(IGraph graph, IQuest quest, IQuest dependence)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!dependence.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            if (!graph.Has(dependence)) return false;
            
            var node = graph.GetNode(quest);
            var dependenceNode = graph.GetNode(dependence);
            return node.TryRemoveDependency(dependenceNode.NodeId);
        }

        public bool TryRemoveDependencies(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            
            var node = graph.GetNode(quest);
            return graph.TryRemoveDependencies(node.NodeId);
        }

        public bool TryRemoveDependents(IGraph graph, IQuest quest)
        {
            if (!graph.IsValid()) return false;
            if (!Has(graph)) return false;
            if (!quest.IsValid()) return false;
            if (!graph.Has(quest)) return false;
            
            var node = graph.GetNode(quest);
            return graph.TryRemoveDependents(node.NodeId);
        }

        public IQuestCollection GetDependencies(IGraph graph, IQuest quest)
        {
            IQuestCollection collection = new QuestCollection();
            
            if (!graph.IsValid()) return collection;
            if (!Has(graph)) return collection;
            if (!quest.IsValid()) return collection;
            if (!graph.Has(quest)) return collection;
            
            var dependencies = graph.GetDependenciesNodes(quest);

            foreach (var dependency in dependencies)
            {
                var node = graph.GetNode(dependency.NodeId);
                var dep = _quests.FirstOrDefault(x => x.Equals(node.Quest));
                if (!dep.IsValid()) continue;
                
                collection.Add(dep);
            }

            return collection;
        }

        public IQuestCollection GetDependents(IGraph graph, IQuest quest)
        {
            IQuestCollection collection = new QuestCollection();
            
            if (!graph.IsValid()) return collection;
            if (!Has(graph)) return collection;
            if (!quest.IsValid()) return collection;
            if (!graph.Has(quest)) return collection;
            
            var dependents = graph.GetDependentsNodes(quest);
            
            foreach (var dependent in dependents)
            {
                var node = graph.GetNode(dependent.NodeId);
                var dep = _quests.FirstOrDefault(x => x.Equals(node.Quest));
                if (!dep.IsValid()) continue;
                
                collection.Add(dep);
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
            foreach (var graph in _graphs)
            {
                var quest = _quests.FirstOrDefault(x => x.Id.Equals(id));
                if (!quest.IsValid()) continue;
                return quest;
            }

            return null;
        }

        public void Start()
        {
            foreach (var quest in _quests)
            {
                foreach (var graph in _graphs)
                {
                    var node = graph.GetNode(quest);
                    if (node is null) continue;

                    if (node.Dependencies.Count == 0 && quest.Status is UnStarted)
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

        public bool HasCollisions()
        {
            var ids = _quests.GroupBy(x => x.Id)
                .Where(group => group.Any());

            return ids.Count() > 1;
        }
        
        public bool Equals(IQuestAggregator other)
        {
            if (other is null) return false;

            return GetHashCode() == other.GetHashCode();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var graph in _graphs)
            {
                foreach (var node in graph.Nodes)
                {
                    _quests.Add(node.Quest);
                    Subscribe(node);
                }
            }
        }
        
        public override bool Equals(object obj)
        {
            return obj is QuestAggregator agg && Equals(agg);
        }

        public override int GetHashCode()
        {
            return _quests.GetHashCode();
        }

        private void Subscribe(IGraphNode node)
        {
            if (node.Quest.IsFinished()) return;
            
            node.Started    += OnGraphNodeStarted;
            node.Updated    += OnGraphNodeUpdated;
            node.Completed  += OnGraphNodeCompleted;
            node.Failed     += OnGraphNodeFailed;
            node.KeyChanged += OnKeyChanged;
        }

        private void UnSubscribe(IGraphNode node)
        {
            node.Started    -= OnGraphNodeStarted;
            node.Updated    -= OnGraphNodeUpdated;
            node.Completed  -= OnGraphNodeCompleted;
            node.Failed     -= OnGraphNodeFailed;
            node.KeyChanged -= OnKeyChanged;
        }

        private void OnGraphNodeStarted(IGraphNode node)
        {
            
        }
        
        //TODO:
        private void OnGraphNodeCompleted(IGraphNode node)
        {
            UnSubscribe(node);
        }
        
        private void OnGraphNodeFailed(IGraphNode node)
        {
            UnSubscribe(node);
        }
        
        private void OnGraphNodeUpdated(IGraphNode node, ITaskBundle bundle)
        {
            
        }
        
        private void OnKeyChanged(string newKey, string oldKey)
        {
            
        }
    }
}