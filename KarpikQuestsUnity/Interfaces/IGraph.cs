using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface IGraph : IDisposable
    {
        public event Action<Quest> QuestUnlocked; 
        public event Action<Quest> QuestCompleted;
        public event Action<Quest> QuestFailed;
    
        public IEnumerable<Quest> Quests { get; }
        public IEnumerable<Quest> StartQuests { get; }

        public IEnumerable<Quest> StatusQuests(Status status);
    
        public bool TryAdd(Quest quest);

        public bool TryRemove(Id quest);
    
        public bool TryReplace(Quest from, Quest to);

        public void Setup();
        
        public void Clear();
    
        public bool Has(Id questId);
        
        public Quest GetQuest(Id questId);
        public Quest GetQuestDeep(Id questId);
    
        public bool TryAddDependency(Id questId, Id dependencyQuestId, IDependencyType dependencyType);
        public bool TryAddDependency(Id questId, Id dependencyQuestId, DependencyType dependencyType);
    
        public bool TryRemoveDependencies(Id questId);
        public bool TryRemoveDependents(Id questId);
    
        public bool TryRemoveDependency(Id questId, Id dependencyQuestId);
        
        public IEnumerable<QuestConnection> GetDependenciesQuests(Id questId);
        public IEnumerable<QuestConnection> GetDependentsQuests(Id questId);
    
        public bool IsCyclic();

        internal void InternalUpdate(Quest quest, bool inGraph);
    }
}