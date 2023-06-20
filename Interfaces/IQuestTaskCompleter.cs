using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskCompleter<T>
    where T : IEquatable<T>
    {
        public void Subscribe(IQuestTask task, params T[] requiredValues);
        public bool Unsubscribe(IQuestTask task);

        public void Update(T value, params T[] values);
    }
}