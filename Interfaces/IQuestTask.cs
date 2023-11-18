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

        public event Action<IQuestTask> Completed;

        protected internal void ForceCanBeCompleted();
        protected internal bool TryToComplete();

        public enum TaskStatus
        {
            Completed = 1,
            UnCompleted = 2
        }
    }
}