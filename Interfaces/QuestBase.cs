using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public abstract class QuestBase : IQuest
    {
        public abstract string Key { get; protected set; }

        public abstract string Name { get; protected set; }

        public abstract string Description { get; protected set; }

        public abstract IEnumerable<IQuestTask> Tasks { get; }

        public abstract IQuestStatus Status { get; protected set; }

        public abstract event Action<IQuest> Started;
        public abstract event Action<IQuest, IQuestTask> Updated;
        public abstract event Action<IQuest> Completed;

        public abstract bool Equals(IQuest other);

        void IQuest.AddTask(IQuestTask task)
        {
            AddTask(task);
        }
        protected abstract void AddTask(IQuestTask task);

        void IQuest.Init(string key, string name, string description)
        {
            Init(key, name, description);
        }
        protected abstract void Init(string key, string name, string description);

        void IQuest.OnTaskComplete(IQuestTask task)
        {
            OnTaskComplete(task);
        }
        protected abstract void OnTaskComplete(IQuestTask task);

        void IQuest.SetKey(string key)
        {
            SetKey(key);
        }
        protected abstract void SetKey(string key);

        void IQuest.Start()
        {
            Start();
        }
        protected abstract void Start();
    }
}
