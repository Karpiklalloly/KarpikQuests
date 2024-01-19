using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KarpikQuestsTest")]
namespace KarpikQuests.Interfaces
{
    public interface IQuestTask : IEquatable<IQuestTask>, ICloneable
    {
        public string Key { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskStatus Status { get; }
        public bool CanBeCompleted { get; }

        public void Init(string key, string name, string description = "");
        public void Reset(bool canBeCompleted = false);
        public void Clear();
        public bool TryToComplete();

        public event Action<IQuestTask> Completed;

        protected internal void ForceCanBeCompleted();

        public enum TaskStatus
        {
            Completed = 1,
            UnCompleted = 2
        }
    }
}