using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ITask : IInitable, IEqualityComparer<ITask>, ICloneable
    {
        public event Action<ITask> Completed;

        public string Key { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskStatus Status { get; }
        public bool CanBeCompleted { get; }

        public void Init(string key, string name, string description = "");
        public void Reset(bool canBeCompleted = false);
        public bool TryToComplete();

        public enum TaskStatus
        {
            Completed = 1,
            UnCompleted = 2
        }
    }
}