using System;

namespace KarpikQuests.Interfaces.AbstractBases
{
    public abstract class QuestTaskBase : IQuestTask
    {
        public abstract string Key { get; protected set; }

        public abstract string Name { get; protected set; }

        public abstract IQuestTask.TaskStatus Status { get; protected set; }

        bool IQuestTask.CanBeCompleted
        {
            get => CanBeCompleted;
            set => CanBeCompleted = value;
        }
        public abstract bool CanBeCompleted { get; protected set; }


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

        public bool Equals(IQuestTask other)
        {
            if (other == null) return false;
            if (Key == null) return false;
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (obj is IQuestTask task)
            {
                return Equals(task);
            }
            return false;
        }
    }
}
