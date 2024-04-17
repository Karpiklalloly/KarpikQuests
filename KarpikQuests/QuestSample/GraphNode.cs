using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Extensions;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Newtonsoft.Json;

namespace Karpik.Quests.QuestSample
{
    [Serializable]
    public class GraphNode : IGraphNode
    {
        public event Action<string, string>? KeyChanged;
        public event Action<IGraphNode>? Started;
        public event Action<IGraphNode, ITaskBundle>? Updated;
        public event Action<IGraphNode>? Completed;
        public event Action<IGraphNode>? Failed;
        
        [JsonIgnore] public Id NodeId => _nodeId;
        [JsonIgnore] public IQuest Quest => _quest;
        [JsonIgnore] public IReadOnlyList<IGraphNode.Connection> Dependencies => _dependencies;

        [JsonProperty("ID")]
        private readonly Id _nodeId;
        [JsonProperty("Quest")]
        private IQuest _quest;
        [JsonProperty("Dependencies")]
        private List<IGraphNode.Connection> _dependencies = new List<IGraphNode.Connection>();

        private bool _disposed = false;
        private readonly string _toString;

        public GraphNode(IQuest quest) : this(Id.NewId(), quest)
        {
            
        }
        
        private GraphNode(Id id, IQuest quest)
        {
            _nodeId = id;
            _quest = quest;

            _toString = $"Node: {_nodeId}\nQuest: {_quest.Id}";
        }

        public bool TryAddDependency(IGraphNode.Connection connection)
        {
            if (Has(connection)) return false;
            
            _dependencies.Add(connection);
            return true;
        }

        public bool TryRemoveDependency(IGraphNode.Connection connection)
        {
            if (connection.IsEmpty()) return false;
            
            var i = 0;
            foreach (var dependency in _dependencies)
            {
                if (dependency.Equals(connection))
                {
                    return TryRemoveDependency(i);
                }

                i++;
            }

            return false;
        }

        public bool TryRemoveDependency(int index)
        {
            if (index < 0 || index > _dependencies.Count)
            {
                return false;
            }
            
            _dependencies.RemoveAt(index);
            
            return true;
        }

        public bool TryRemoveDependency(string nodeId)
        {
            for (var i = 0; i < Dependencies.Count; i++)
            {
                if (Dependencies[i].NodeId == nodeId)
                {
                    TryRemoveDependency(i);
                    return true;
                }
            }

            return false;
        }

        public bool TryRemoveDependency(Id nodeId)
        {
            if (nodeId.IsEmpty()) return false;

            return TryRemoveDependency(nodeId.Value);
        }

        public bool Has(IGraphNode.Connection connection)
        {
            return _dependencies.Any(dependency => dependency.Equals(connection));
        }

        public bool Equals(IGraphNode? other)
        {
            if (other is null) return false;

            return NodeId.Equals(other.NodeId);
        }

        public void SetQuest(IQuest quest)
        {
            _quest = quest;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _dependencies = null;
            }
            
            _disposed = true;
        }

        public override bool Equals(object? obj)
        {
            return obj is GraphNode node && Equals(this, node);
        }

        public override int GetHashCode()
        {
            return _nodeId.GetHashCode();
        }

        public override string ToString()
        {
            return _toString;
        }

        ~GraphNode()
        {
            Dispose(false);
        }
    }
}