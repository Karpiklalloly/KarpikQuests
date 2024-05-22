using System;
using System.Collections.Generic;
using Karpik.Quests.ID;
using Karpik.Quests.QuestSample;
using Newtonsoft.Json;

namespace Karpik.Quests.Interfaces
{
    public interface IGraph
    {
        public IReadOnlyQuestCollection Quests { get; }
        public IReadOnlyQuestCollection StartQuests { get; }
        
        public bool TryAdd(IQuest quest);

        public bool TryRemove(IQuest quest);
        public bool TryRemove(Id questId);

        public bool TryReplace(IQuest from, IQuest to);
        public void Clear();

        public bool TryAddDependency(Id questId, Id dependencyQuestId, IDependencyType dependencyType);
        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, IDependencyType dependencyType);
        public bool TryAddDependency(Id questId, Id dependencyQuestId, DependencyType dependencyType);
        public bool TryAddDependency(IQuest quest, IQuest dependencyQuest, DependencyType dependencyType);
        
        public bool TryRemoveDependencies(Id questId);
        public bool TryRemoveDependencies(IQuest quest);
        public bool TryRemoveDependents(Id questId);
        public bool TryRemoveDependents(IQuest quest);
        
        public bool TryRemoveDependency(Id questId, Id dependencyQuestId);
        public bool TryRemoveDependency(IQuest quest, IQuest dependencyQuest);
        
        public IEnumerable<ConnectionWithQuest> GetDependenciesQuests(Id questId);
        public IEnumerable<ConnectionWithQuest> GetDependenciesQuests(IQuest quest);
        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(Id questId);
        public IEnumerable<ConnectionWithQuest> GetDependentsQuests(IQuest quest);

        public bool IsCyclic();

        public bool Has(Id questId);
        public bool Has(IQuest quest);
        
        public IQuest GetQuest(Id questId);
        
        public enum DependencyType
        {
            Completion,
            Fail,
            Start
        }
        
        [Serializable]
        public readonly struct Connection : IEquatable<Connection>
        {
            [JsonProperty("ID")]
            public readonly Id QuestId;
            [JsonProperty("DependencyType")]
            public readonly IDependencyType DependencyType;

            public Connection(string id, IDependencyType dependencyType) :
                this(id == Id.Empty.Value ? Id.Empty : new Id(id), dependencyType)
            {
            
            }
        
            public Connection(Id questId, IDependencyType dependencyType)
            {
                QuestId = questId;
                DependencyType = dependencyType;
            }

            public bool Equals(Connection other)
            {
                return QuestId.Equals(other.QuestId);
            }

            public override bool Equals(object obj)
            {
                return obj is Connection other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(QuestId, DependencyType);
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