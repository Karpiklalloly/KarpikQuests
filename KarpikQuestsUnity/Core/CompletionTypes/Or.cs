using NewKarpikQuests.Interfaces;

namespace NewKarpikQuests.CompletionTypes
{
    [Serializable]
    public class Or : ICompletionType
    {
        public Status Check(IEnumerable<Quest> quests)
        {
            if (!quests.Any()) return Status.Completed;
            if (quests.Any(Predicates.QuestIsCompleted)) return Status.Completed;
            if (quests.All(Predicates.QuestIsFailed)) return Status.Failed;
            if (quests.Any(Predicates.QuestIsFailed)
                || quests.Any(Predicates.QuestIsUnlocked)) return Status.Unlocked;
            
            return Status.Locked;
        }
    }
}
