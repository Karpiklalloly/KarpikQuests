﻿using System;
using Karpik.Quests.Serialization;

namespace Karpik.Quests
{
    [Serializable]
    public class Connection : IEquatable<Connection>
    {
        [DoNotSerializeThis]
        public Id QuestId => _questId;
        [DoNotSerializeThis]
        public IDependencyType DependencyType => _dependencyType;
            
        [SerializeThis("Id")]
        private Id _questId;
        [SerializeThis("DependencyType", IsReference = true)]
        private IDependencyType _dependencyType;
        
        public Connection(Id questId, IDependencyType dependencyType)
        {
            _questId = questId;
            _dependencyType = dependencyType;
        }

        public bool Equals(Connection other)
        {
            if (other is null) return false;
            
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
            if (left is null) return right is null;
            
            return left.Equals(right);
        }

        public static bool operator !=(Connection left, Connection right)
        {
            return !(left == right);
        }
    }
}