using System;

namespace KarpikQuests.Interfaces
{
    public abstract class QuestTaskBase : IQuestTask
    {
        public abstract string Key { get; }

        public abstract string Name { get; }

        public abstract IQuestTask.TaskStatus Status { get; }
        bool IQuestTask.CanBeCompleted
        {
            get => CanBeCompleted;
            set => CanBeCompleted = value;
        }
        public bool CanBeCompleted { get; protected set; }

        public abstract event Action<IQuestTask> Completed;

        public abstract void Init(string key, string name);

        bool IQuestTask.TryToComplete()
        {
            return TryToComplete();
        }
        protected abstract bool TryToComplete();

        void IQuestTask.ForceCanBeCompleted()
        {
            ForceBeCompleted();
        }
        protected abstract void ForceBeCompleted();
    }
}
