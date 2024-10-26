using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Saving;

namespace Karpik.Quests
{
    public class QuestConnection : IEquatable<QuestConnection>
    {
        [Property]
[CreateProperty][JsonIgnore]        public Quest DependencyQuest => _dependencyQuest;
        [Property]
[CreateProperty][JsonIgnore]        public Quest DependentQuest => _dependentQuest;
        [Property]
[CreateProperty][JsonIgnore]        public IDependencyType Dependency
        {
            get => _dependency;
            private set => _dependency = value;
        }
    
        [SerializeThis("DependencyQuest")]
[SerializeField][JsonProperty(PropertyName = "DependencyQuest")]        private Quest _dependencyQuest;
        [SerializeThis("DependentQuest")]
[SerializeField][JsonProperty(PropertyName = "DependentQuest")]        private Quest _dependentQuest;
        [SerializeThis("Dependency")]
[SerializeField][JsonProperty(PropertyName = "Dependency")]        private IDependencyType _dependency;

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