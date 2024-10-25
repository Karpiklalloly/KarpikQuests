using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;

namespace Karpik.Quests
{
    [Serializable]
    public struct Connection : IEquatable<Connection>
    {
        [DoNotSerializeThis]
        public Id QuestId => _questId;
        [DoNotSerializeThis]
        public IDependencyType DependencyType => _dependencyType;
            
        [SerializeThis("Id")]
        private Id _questId;
        [SerializeThis("DependencyType", IsReference = true)]
        private IDependencyType _dependencyType;

        public Connection(string id, IDependencyType dependencyType) :
            this(id == Id.Empty.Value ? Id.Empty : new Id(id), dependencyType)
    {
            
    }
        
        public Connection(Id questId, IDependencyType dependencyType)
    {
        _questId = questId;
        _dependencyType = dependencyType;
    }

        public bool Equals(Connection other)
    {
        return _questId.Equals(other._questId);
    }

        public override bool Equals(object obj)
    {
        return obj is Connection other && Equals(other);
    }

        public override int GetHashCode()
    {
        return HashCode.Combine(_questId, _dependencyType);
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