using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Karpik.Quests.DependencyTypes;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class QuestGraph : IGraph
    {
        [JsonIgnore] public IQuestCollection Quests
        {
            get
            {
                var quests = new QuestCollection();
                foreach (var node in _nodes)
                {
                    quests.Add(node.Quest);
                }

                return quests;
            }
        }

        [JsonIgnore] public IReadOnlyList<IGraphNode> Nodes => _nodes;
        
        [JsonProperty("Nodes")]
        private readonly List<IGraphNode> _nodes = new List<IGraphNode>();
        private readonly List<IGraphNode> _startNodes = new List<IGraphNode>();

        public bool TryAdd(IGraphNode node)
        {
            if (!node.IsValid()) return false;
        
            if (_nodes.Contains(node)) return false;
        
            _nodes.Add(node);

            if (node.Dependencies.Count == 0)
            {
                _startNodes.Add(node);
            }

            return true;
        }

        public bool TryRemove(IGraphNode node)
        {
            if (!node.IsValid()) return false;
        
            if (!_nodes.Contains(node)) return false;

            var dependents = GetDependentsNodes(node.NodeId);

            foreach (var dependent in dependents)
            {
                dependent.TryRemoveDependency(node.NodeId);
                if (dependent.Dependencies.Count == 0)
                {
                    _startNodes.Add(dependent);
                }
            }

            return true;
        }

        public bool TryRemove(IQuest quest)
        {
            if (!quest.IsValid()) return false;

            foreach (var node in _nodes)
            {
                if (node.Quest.Equals(quest)) return TryRemove(node);
            }

            return false;
        }

        public bool TryRemove(Id nodeId)
        {
            return nodeId.IsValid() && TryRemove(GetNode(nodeId));
        }

        public bool TrySetDependency(Id nodeId, Id dependencyNodeId, IDependencyType dependencyType)
        {
            if (!dependencyType.IsValid()) return false;
            
            var node = GetNode(nodeId);

            if (node is null) return false;

            var result = node.TryAddDependency(new IGraphNode.Connection(dependencyNodeId, dependencyType));
            if (result && _startNodes.Contains(node)) _startNodes.Remove(node);
            
            return result;
        }

        public bool TrySetDependency(IQuest quest, IQuest dependencyQuest, IDependencyType dependencyType)
        {
            if (!quest.IsValid() || !dependencyQuest.IsValid()) return false;
            
            var questNode = GetNode(quest);
            var dependencyQuestNode = GetNode(dependencyQuest);
            if (!questNode.IsValid() || !dependencyQuestNode.IsValid()) return false;
            
            return TrySetDependency(questNode.NodeId, dependencyQuestNode.NodeId, dependencyType);
        }

        public bool TrySetDependency(Id nodeId, Id dependencyNodeId, IGraph.DependencyType dependencyTypeType)
        {
            return dependencyTypeType switch
            {
                IGraph.DependencyType.Completion => TrySetDependency(
                    nodeId, dependencyNodeId, new Completion()),
                IGraph.DependencyType.Fail => TrySetDependency(
                    nodeId, dependencyNodeId, new Fail()),
                IGraph.DependencyType.Start => TrySetDependency(
                    nodeId, dependencyNodeId, new Start()),
                IGraph.DependencyType.Unneccesary => TrySetDependency(
                    nodeId, dependencyNodeId, new Unneccesary()),
                _ => false
            };
        }

        public bool TrySetDependency(IQuest quest, IQuest dependencyQuest, IGraph.DependencyType dependencyTypeType)
        {
            if (!quest.IsValid() || !dependencyQuest.IsValid()) return false;
            
            var questNode = GetNode(quest);
            var dependencyQuestNode = GetNode(dependencyQuest);
            if (!questNode.IsValid() || !dependencyQuestNode.IsValid()) return false;
            
            return TrySetDependency(questNode.NodeId, dependencyQuestNode.NodeId, dependencyTypeType);
        }

        public bool TryRemoveDependencies(Id nodeId)
        {
            if (!nodeId.IsValid()) return false;
            var node = GetNode(nodeId);
            if (!node.IsValid()) return false;
            
            while (node.Dependencies.Count > 0)
            {
                node.TryRemoveDependency(0);
            }

            return true;
        }

        public bool TryRemoveDependencies(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            
            var questNode = GetNode(quest);
            if (!questNode.IsValid()) return false;

            return TryRemoveDependencies(questNode.NodeId);
        }

        public bool TryRemoveDependents(Id nodeId)
        {
            if (!nodeId.IsValid()) return false;
            var node = GetNode(nodeId);
            if (!node.IsValid()) return false;

            var dependents = GetDependentsNodes(node.NodeId);
            foreach (var dependent in dependents)
            {
                TryRemoveDependency(dependent.NodeId, nodeId);
            }

            return true;
        }

        public bool TryRemoveDependents(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            
            var questNode = GetNode(quest);
            if (!questNode.IsValid()) return false;

            return TryRemoveDependents(questNode.NodeId);
        }

        public bool TryRemoveDependency(Id nodeId, Id dependencyNodeId)
        {
            var node = GetNode(nodeId);

            if (!node.IsValid()) return false;

            return node.TryRemoveDependency(dependencyNodeId);
        }

        public bool IsCyclic()
        {
            if (_startNodes.Count == 0) return true;
            
            var visited = new Dictionary<IGraphNode, bool>();
            var recStack = new Dictionary<IGraphNode, bool>();
            foreach (var node in _nodes)
            {
                visited.Add(node, false);
                recStack.Add(node, false);
            }
            
            foreach (var node in _startNodes)
            {
                if (IsCyclicUtil(node, visited, recStack)) return true;
            }

            return visited.Any(x => x.Value != true);
        }

        public bool Has(Id nodeId)
        {
            return GetNode(nodeId).IsValid();
        }

        public bool Has(IQuest quest)
        {
            if (!quest.IsValid()) return false;
            
            var questNode = GetNode(quest);
            if (!questNode.IsValid()) return false;

            return Has(questNode.NodeId);
        }

        public IGraphNode GetNode(Id nodeId)
        {
            return _nodes.FirstOrDefault(x => x.NodeId == nodeId);
        }

        public IGraphNode GetNode(IQuest quest)
        {
            if (!quest.IsValid()) return null;

            IGraphNode questNode = null;
            foreach (var node in _nodes)
            {
                if (node.Quest.Equals(quest))
                {
                    questNode = node;
                    break;
                }
            }

            if (!questNode.IsValid()) return null;

            return GetNode(questNode.NodeId);
        }

        public IEnumerable<IGraphNode> GetDependenciesNodes(Id nodeId)
        {
            var list = new List<IGraphNode>();
            
            var questNode = GetNode(nodeId);
            if (!questNode.IsValid()) return list;
            
            foreach (var dependency in questNode.Dependencies)
            {
                var node = GetNode(dependency.NodeId);
                if (!node.IsValid()) continue;

                list.Add(node);
            }

            return list;
        }

        public IEnumerable<IGraphNode> GetDependenciesNodes(IQuest quest)
        {
            var list = new List<IGraphNode>();
            
            if (!quest.IsValid()) return list;
            
            var questNode = GetNode(quest);
            if (!questNode.IsValid()) return list;
            
            list.AddRange(GetDependenciesNodes(questNode.NodeId));
            return list;
        }

        public IEnumerable<IGraphNode> GetDependentsNodes(Id nodeId)
        {
            var list = new List<IGraphNode>();
            
            var questNode = GetNode(nodeId);
            if (!questNode.IsValid()) return list;
            
            foreach (var node1 in _nodes)
            {
                if (node1.Dependencies.Any(x => x.NodeId.Equals(questNode.NodeId)))
                {
                    list.Add(node1);
                }
            }

            return list;
        }

        public IEnumerable<IGraphNode> GetDependentsNodes(IQuest quest)
        {
            List<IGraphNode> nodes = new List<IGraphNode>();
            
            if (!quest.IsValid()) return nodes;
            
            var node = GetNode(quest);
            if (!node.IsValid()) return nodes;
            
            return GetDependentsNodes(node.NodeId);
        }
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (var node in _nodes)
            {
                if (node.Dependencies.Count == 0)
                {
                    _startNodes.Add(node);
                }
            }
        }

        private bool IsCyclicUtil(IGraphNode node, Dictionary<IGraphNode, bool> visited, Dictionary<IGraphNode, bool> recStack)
        {
            if (recStack[node]) return true;
            if (visited[node]) return false;

            visited[node] = true;
            recStack[node] = true;

            var dependents = GetDependentsNodes(node.NodeId);

            foreach (var dependent in dependents)
            {
                if (IsCyclicUtil(dependent, visited, recStack)) return true;
            }

            recStack[node] = false;

            return false;
        }
    }
}