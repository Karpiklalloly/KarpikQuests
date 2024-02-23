using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuestAggregator : IEquatable<IQuestAggregator>
    {
        public IReadOnlyQuestCollection Quests { get; }

        public bool TryAddQuest(IQuest quest);
        public bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true);
        public bool TryReplaceQuest(IQuest quest1, IQuest quest2, bool keysAreEquel);

        public bool TryAddDependence(IQuest quest, IQuest dependence);
        public bool TryRemoveDependence(IQuest quest, IQuest dependence);
        public bool TryRemoveDependencies(IQuest quest);
        public bool TryRemoveDependents(IQuest quest);
        public IQuestCollection GetDependencies(IQuest quest);
        public IQuestCollection GetDependents(IQuest quest);

        public bool Has(IQuest quest);

        /// <summary>
        /// Call after quests added
        /// </summary>
        public void Start();
        public void ResetQuests();
        public void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if no collisions</returns>
        public bool CheckKeyCollisions();
    }
}