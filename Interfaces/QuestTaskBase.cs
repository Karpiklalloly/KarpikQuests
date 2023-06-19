using System;

namespace KarpikQuests.Interfaces
{
    public abstract class QuestTaskBase : IQuestTask
    {
        public abstract string Name { get; }

        public abstract IQuestTask.TaskStatus Status { get; }
        bool IQuestTask.CanBeCompleted
        {
            get => CanBeCompleted;
            set => CanBeCompleted = value;
        }
        public bool CanBeCompleted { get; protected set; }

        public abstract event Action<IQuestTask> Completed;

        public abstract void Init(string name);

        void IQuestTask.Complete()
        {
            Complete();
        }
        protected abstract void Complete();

        void IQuestTask.ForceCanBeCompleted()
        {
            ForceBeCompleted();
        }
        protected abstract void ForceBeCompleted();
    }
}
