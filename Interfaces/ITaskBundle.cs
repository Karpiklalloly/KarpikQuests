using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ITaskBundle : ICollection<IQuestTask>, IEnumerable<IQuestTask>, ICloneable, IEquatable<ITaskBundle>
    {
        public IQuestTaskCollection QuestTasks { get; }
        public IQuestCompletionType CompletionType { get; }
        public IQuestTaskProcessorType QuestTaskProcessor { get; }

        public bool ContainsTask(IQuestTask task);
        public bool ContainsTask(string taskKey);
        public void ResetAll(bool canBeCompleted = false);
        public void ResetFirst(bool canBeCompleted = false);
    }
}
