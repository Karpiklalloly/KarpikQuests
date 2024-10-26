using Karpik.Quests.Interfaces;

namespace Karpik.Quests.CompletionTypes
{
    [Serializable]
    public class And : ICompletionType
    {
        public Status Check(IEnumerable<Quest> quests)
        {
            if (!quests.Any()) return Status.Completed;
            if (quests.Any(Predicates.QuestIsFailed)) return Status.Failed;
            if (quests.All(Predicates.QuestIsCompleted)) return Status.Completed;
            if (quests.Any(Predicates.QuestIsCompleted)
                || quests.Any(Predicates.QuestIsUnlocked)) return Status.Unlocked;

            return Status.Locked;
        }
    }
}
