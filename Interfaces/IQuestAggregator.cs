﻿using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestAggregator
    {
        public IReadOnlyCollection<IQuest> Quests { get; }

        public bool TryAddQuest(IQuest quest);
        public bool TryRemoveQuest(IQuest quest);
        public bool TryAddDependence(IQuest quest, IQuest dependence);
        public bool TryRemoveDependence(IQuest quest, IQuest dependence);
        public IQuestCollection GetDependencies(IQuest quest);
        public IQuestCollection GetDependents(IQuest quest);

        /// <summary>
        /// Call after quests added
        /// </summary>
        public void Start();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if no collisions</returns>
        public bool CheckKeyCollisions();
    }
}