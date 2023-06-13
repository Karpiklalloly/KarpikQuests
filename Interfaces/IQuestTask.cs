using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KarpikQuestsTest")]
namespace KarpikQuests.Interfaces
{
    public interface IQuestTask
    {
        public string Name { get; }
        public TaskStatus Status { get; }
        internal bool CanBeCompleted { get; set; }

        public void Init(string name);

        public event Action<IQuestTask> Completed;

        public enum TaskStatus
        {
            Completed,
            UnCompleted
        }

        internal void Complete();

        internal void ForceBeCompleted();
    }
}