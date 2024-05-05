using System;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface IQuest : IInitable, IEquatable<IQuest>, IDisposable, ICloneable
    {
        public event Action<IQuest> Started;
        public event Action<IQuest, ITaskBundle> Updated;
        public event Action<IQuest> Completed;
        public event Action<IQuest> Failed;

        public Id Id { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyTaskBundleCollection TaskBundles { get; }
        public ICompletionType CompletionType { get; }
        public IProcessorType Processor { get; }

        public IStatus Status { get; }

        public void SetStatus(IStatus status, Action<IQuest, IStatus> moreStatusesSetting = null);
        
        public void Clear();
        public void Reset();

        public void Init(string name, string description,
            ITaskBundleCollection bundles, ICompletionType completionType, IProcessorType processorType);
        public void AddBundle(ITaskBundle bundle);
        public void RemoveBundle(ITaskBundle bundle);
    }
}