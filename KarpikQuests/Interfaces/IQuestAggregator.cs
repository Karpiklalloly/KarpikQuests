using System;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface IQuestAggregator : IEquatable<IQuestAggregator>
    {
        public IReadOnlyQuestCollection Quests { get; }
        
        public bool TryAddGraph(IGraph graph); 
        public bool TryAddQuest(IGraph graph, IQuest quest);
        public bool TryRemoveQuest(IGraph graph, IQuest quest);
        public bool TryReplace(IGraph graph, IQuest from, IQuest to);

        public bool TryAddDependence(IGraph graph, IQuest quest, IQuest dependence, IDependencyType dependencyType);
        public bool TryRemoveDependence(IGraph graph, IQuest quest, IQuest dependence);
        public bool TryRemoveDependencies(IGraph graph, IQuest quest);
        public bool TryRemoveDependents(IGraph graph, IQuest quest);
        public IQuestCollection GetDependencies(IGraph graph, IQuest quest);
        public IQuestCollection GetDependents(IGraph graph, IQuest quest);

        public bool Has(IGraph graph);
        public bool Has(IQuest quest);
        public IQuest Get(Id id);

        /// <summary>
        /// Call after quests added
        /// </summary>
        public void Start();
        public void ResetQuests();
        public void Clear();
        public bool HasCollisions();
    }
}