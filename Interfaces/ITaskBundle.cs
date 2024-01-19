using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ITaskBundle : ICollection<IQuestTask>, IEnumerable<IQuestTask>, ICloneable, IEquatable<ITaskBundle>
    {
        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;

        public bool IsCompleted { get; }
        public IQuestTaskCollection QuestTasks { get; }
        public ICompletionType CompletionType { get; }
        public ITaskProcessorType TaskProcessor { get; }

        public bool ContainsTask(IQuestTask task);
        public bool ContainsTask(string taskKey);
        public void ResetAll(bool canBeCompleted = false);
        public void ResetFirst(bool canBeCompleted = false);
        public void ClearTasks();

        internal protected void OnTaskCompleted(IQuestTask task);
    }
}
