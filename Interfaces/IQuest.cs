using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuest : IEquatable<IQuest>, IDisposable, ICloneable
    {
        public string Key { get; }

        public string Name { get; }
        public string Description { get; }

        public IEnumerable<IQuestTask> Tasks { get; }

        public IQuestStatus Status { get; }

        public event Action<IQuest> Started;
        public event Action<IQuest, IQuestTask> Updated;
        public event Action<IQuest> Completed;

        internal void Init(string key, string name, string description);
        internal void Start();
        internal void SetKey(string key);
        internal void AddTask(IQuestTask task);
        internal void RemoveTask(IQuestTask task);
        internal void OnTaskComplete(IQuestTask task);
    }
}