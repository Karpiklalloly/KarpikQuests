using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public abstract class QuestBase : IQuest
    {
        private bool disposedValue;

        public abstract string Key { get; protected set; }

        public abstract string Name { get; protected set; }

        public abstract string Description { get; protected set; }

        public abstract IEnumerable<IQuestTask> Tasks { get; }

        public abstract IQuestStatus Status { get; protected set; }

        public abstract event Action<IQuest> Started;
        public abstract event Action<IQuest, IQuestTask> Updated;
        public abstract event Action<IQuest> Completed;

        public virtual bool Equals(IQuest other)
        {
            return Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            if (obj is IQuest quest)
            {
                return Equals(quest);
            }
            return false;
        }

        void IQuest.AddTask(IQuestTask task)
        {
            AddTask(task);
        }
        protected abstract void AddTask(IQuestTask task);

        void IQuest.RemoveTask(IQuestTask task)
        {
            RemoveTask(task);
        }
        protected abstract void RemoveTask(IQuestTask task);

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты)
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей

                disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        ~QuestBase()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
