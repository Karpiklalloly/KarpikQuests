using Karpik.Quests.Interfaces;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
    [Serializable]
    public class QuestConnection : IEquatable<QuestConnection>
    {
        [DoNotSerializeThis][Property]
        public Quest DependencyQuest => _dependencyQuest;
        [DoNotSerializeThis][Property]
        public Quest DependentQuest => _dependentQuest;
        [DoNotSerializeThis][Property]
        public IDependencyType Dependency
        {
            get => _dependency;
            private set => _dependency = value;
        }
    
        [SerializeThis("DependencyQuest")]
        private Quest _dependencyQuest;
        [SerializeThis("DependentQuest")]
        private Quest _dependentQuest;
        [SerializeThis("Dependency")]
        private IDependencyType _dependency;

        public QuestConnection(Quest dependencyQuest, Quest dependentQuest, IDependencyType dependency)
        {
            _dependencyQuest = dependencyQuest;
            _dependentQuest = dependentQuest;
            Dependency = dependency;
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