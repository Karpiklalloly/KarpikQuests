using System;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.QuestSample
{
    public readonly struct ConnectionWithQuest : IEquatable<ConnectionWithQuest>
    {
        public readonly IQuest DependencyQuest;
        public readonly IQuest DependentQuest;
        public readonly IDependencyType Dependency;

        public ConnectionWithQuest(IQuest dependencyQuest, IQuest dependentQuest, IDependencyType dependency)
        {
            DependencyQuest = dependencyQuest;
            DependentQuest = dependentQuest;
            Dependency = dependency;
        }

        public bool Equals(ConnectionWithQuest other)
        {
            return Equals(DependencyQuest, other.DependencyQuest) && Equals(DependentQuest, other.DependentQuest) && Equals(Dependency, other.Dependency);
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionWithQuest other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DependencyQuest, DependentQuest, Dependency);
        }
        
        public static bool operator ==(ConnectionWithQuest left, ConnectionWithQuest right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConnectionWithQuest left, ConnectionWithQuest right)
        {
            return !(left == right);
        }
    }
}