using System;

namespace KarpikQuests.Interfaces
{
    public interface IQuest : IEquatable<IQuest>, IDisposable, ICloneable
    {
        public event Action<string, string> KeyChanged;
        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;

        public string Key { get; set; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyTaskBundleCollection TaskBundles { get; }

        public IStatus Status { get; }

        public void Start();
        public void Clear();
        public void Reset();

        public void Init(string key, string name, string description, IProcessorType? processor, ICompletionType? completionType);
        public void AddBundle(ITaskBundle bundle);
        public void RemoveBundle(ITaskBundle bundle);
        public bool CheckCompletion();
    }
}