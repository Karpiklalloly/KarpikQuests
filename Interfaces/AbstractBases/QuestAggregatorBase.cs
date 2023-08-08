using System.Collections.Generic;

namespace KarpikQuests.Interfaces.AbstractBases
{
    public abstract class QuestAggregatorBase : IQuestAggregator
    {
        public abstract IReadOnlyCollection<IQuest> Quests { get; }

        public abstract bool CheckKeyCollisions();

        public abstract IQuestCollection GetDependencies(IQuest quest);

        public abstract IQuestCollection GetDependents(IQuest quest);

        public abstract void Start();

        public abstract bool TryAddDependence(IQuest quest, IQuest dependence);

        public abstract bool TryAddQuest(IQuest quest);

        public abstract bool TryRemoveDependence(IQuest quest, IQuest dependence);

        public abstract bool TryRemoveQuest(IQuest quest, bool autoChangeDependencies = true);

        public abstract bool TryToReplace(IQuest quest1, IQuest quest2, bool keysMayBeEquel);

        public abstract bool TryRemoveDependencies(IQuest quest);

        public abstract bool TryRemoveDependents(IQuest quest);

        public abstract bool Contains(IQuest quest);

        public abstract IQuest GetQuest(string questKey);

        public abstract void ResetAll();

        void IQuestAggregator.Start(IQuest quest)
        {
            quest.Start();
        }
        protected void Start(IQuest quest)
        {
            (this as IQuestAggregator).Start(quest);
        }
    }
}
