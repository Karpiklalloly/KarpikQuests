﻿using System;
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
            _dependency = dependency;
        }

        public bool Equals(QuestConnection other)
        {
            if (other is null) return false;
            
            return Equals(_dependencyQuest, other._dependencyQuest) && Equals(_dependentQuest, other._dependentQuest) && _dependency.GetType() == other._dependency.GetType();
        }

        public override bool Equals(object obj)
        {
            return obj is QuestConnection other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_dependencyQuest, _dependentQuest, _dependency);
        }
        
        public static bool operator ==(QuestConnection left, QuestConnection right)
        {
            if (left is null) return right is null;
            
            return left.Equals(right);
        }

        public static bool operator !=(QuestConnection left, QuestConnection right)
        {
            return !(left == right);
        }
    }
}