using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests
{
    public readonly struct QuestConnection : IEquatable<QuestConnection>
    {
        public Quest DependencyQuest => _graph.GetQuest(_dependencyId);
        public Quest DependentQuest => _graph.GetQuest(_dependentId);
        public readonly IDependencyType Dependency;
    
        private readonly Id _dependencyId;
        private readonly Id _dependentId;
        private readonly IGraph _graph;

        public QuestConnection(Id dependencyId, Id dependentId, IDependencyType dependency, IGraph graph)
    {
        _dependencyId = dependencyId;
        _dependentId = dependentId;
        Dependency = dependency;
        _graph = graph;
    }

        public bool Equals(QuestConnection other)
    {
        return Equals(DependencyQuest, other.DependencyQuest) && Equals(DependentQuest, other.DependentQuest) && Dependency.GetType() == other.Dependency.GetType();
    }

        public override bool Equals(object obj)
    {
        return obj is QuestConnection other && Equals(other);
    }

        public override int GetHashCode()
    {
        return HashCode.Combine(DependencyQuest, DependentQuest, Dependency);
    }
        
        public static bool operator ==(QuestConnection left, QuestConnection right)
    {
        return left.Equals(right);
    }

        public static bool operator !=(QuestConnection left, QuestConnection right)
    {
        return !(left == right);
    }
    }
}