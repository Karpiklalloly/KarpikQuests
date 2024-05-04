using System;
using System.Collections.Generic;
using Karpik.Quests.ID;
using Newtonsoft.Json;

namespace Karpik.Quests.Interfaces
{
    public interface IGraphNode : IEquatable<IGraphNode>, IDisposable
    {
        public event Action<string, string> KeyChanged;
        public event Action<IGraphNode> Started;
        public event Action<IGraphNode, ITaskBundle> Updated;
        public event Action<IGraphNode> Completed;
        public event Action<IGraphNode> Failed;
        
        public Id NodeId { get; }
        public IQuest Quest { get; }
        public IReadOnlyList<Connection> Dependencies { get; }

        public bool TryAddDependency(Connection connection);

        public bool TryRemoveDependency(Connection connection);

        public bool TryRemoveDependency(int index);

        public bool TryRemoveDependency(string nodeId);
        
        public bool TryRemoveDependency(Id nodeId);

        public bool Has(Connection connection);
    
        [Serializable]
        public readonly struct Connection : IEquatable<Connection>
        {
            [JsonProperty("ID")]
            public Id NodeId { get; } = Id.Empty;
            [JsonProperty("DependencyType")]
            public IDependencyType DependencyType { get; }
        
            public static readonly Connection Empty = new Connection(Id.Empty, null);

            public Connection(string id, IDependencyType dependencyType) :
                this(id == Id.Empty.Value ? Id.Empty : new Id(id), dependencyType)
            {
            
            }
        
            public Connection(Id nodeId, IDependencyType dependencyType)
            {
                NodeId = nodeId;
                DependencyType = dependencyType;
            }

            public bool Equals(Connection other)
            {
                return NodeId.Equals(other.NodeId);
            }

            public override bool Equals(object obj)
            {
                return obj is Connection other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(NodeId, DependencyType);
            }

            public static bool operator ==(Connection left, Connection right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Connection left, Connection right)
            {
                return !(left == right);
            }
        }
    }
}