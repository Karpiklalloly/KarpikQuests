using NewKarpikQuests.Interfaces;

namespace NewKarpikQuests.CompletionTypes
{
    [Serializable]
    public class NeededCount : ICompletionType
    {
        public int Count { get; set; }

        public NeededCount() : this(0)
        {
            
        }

        public NeededCount(int count)
        {
            Count = count;
        }

        public Status Check(IEnumerable<Quest> quests)
        {
            if (Count == 0) return Status.Completed;
            if (!quests.Any()) return Status.Unlocked;

            var completedCount = quests.Count(Predicates.QuestIsCompleted);
            var failedCount = quests.Count(Predicates.QuestIsFailed);
            var unlockedCount = quests.Count(Predicates.QuestIsUnlocked);
            
            if (completedCount >= Count) return Status.Completed;
            if (quests.Count() - failedCount < Count) return Status.Failed;
            if (completedCount > 0 || failedCount > 0 || unlockedCount > 0) return Status.Unlocked;
            
            return Status.Locked;
        }
    }
}
