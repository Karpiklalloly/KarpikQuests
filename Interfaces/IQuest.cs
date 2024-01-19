using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuest : IEquatable<IQuest>, IDisposable, ICloneable
    {
        public string Key { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyTaskBundleCollection TaskBundles { get; }
        public ICompletionType CompletionType { get; }
        public ITaskProcessorType TaskProcessor { get; }

        public IStatus Status { get; }

        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;

        public void Reset();
        public void Start();
        public void Clear();

        protected internal void Init(string key, string name, string description);
        protected internal void SetKey(string key);
        protected internal void AddTask(IQuestTask task);
        protected internal void RemoveTask(IQuestTask task);
        protected internal void AddBundle(ITaskBundle bundle);
        protected internal void RemoveBundle(ITaskBundle bundle);
        protected internal void OnBundleComplete(ITaskBundle bundle);
        protected internal void SetCompletionType(ICompletionType completionType);
        protected internal void SetTaskProcessorType(ITaskProcessorType processor);
    }
}