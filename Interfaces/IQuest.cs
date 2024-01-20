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

        public IStatus Status { get; }

        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;

        public void Reset();
        public void Start();
        public void Clear();

        public void Init(string key, string name, string description);
        public void AddTask(IQuestTask task);
        public void AddBundle(ITaskBundle bundle);
        public void RemoveBundle(ITaskBundle bundle);
        public void CheckCompleteion() => CompletionType.CheckCompletion(TaskBundles);

        public void SetCompletionType(ICompletionType completionType);
        public void SetTaskProcessorType(IProcessorType processor);
    }
}