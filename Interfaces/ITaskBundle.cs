using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ITaskBundle : ICollection<IQuestTask>, ICloneable, IEquatable<ITaskBundle>
    {
        public event Action<ITaskBundle> Updated;
        public event Action<ITaskBundle> Completed;

        public bool IsCompleted { get; }
        public IReadOnlyQuestTaskCollection QuestTasks { get; }

        public bool Has(IQuestTask task);
        public bool Has(string taskKey);
        public void ResetAll(bool canBeCompleted = false);
        public void ResetFirst(bool canBeCompleted = false);
        public void ClearTasks();
        public bool CheckCompletion();
    }
}
