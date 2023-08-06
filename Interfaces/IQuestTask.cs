using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KarpikQuestsTest")]
namespace KarpikQuests.Interfaces
{
    public interface IQuestTask : IEquatable<IQuestTask>, ICloneable
    {
        public string Key { get; }
        public string Name { get; }
        public TaskStatus Status { get; }
        public bool CanBeCompleted { get; internal set; }

        public void Init(string key, string name);
        public void Reset(bool canBeCompleted = false);

        public event Action<IQuestTask> Completed;

        internal void ForceCanBeCompleted();
        internal bool TryToComplete();

        public enum TaskStatus
        {
            Completed = 1,
            UnCompleted = 2
        }
    }
}