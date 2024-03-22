using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskBundleCollection : IList<ITaskBundle>, ICloneable, IEqualityComparer<IReadOnlyTaskBundleCollection>
    {
        public event Action<IReadOnlyTaskBundleCollection, ITaskBundle>? Updated;
        public event Action<IReadOnlyTaskBundleCollection>? Completed;

        public ICompletionType CompletionType { get; }
        public IProcessorType Processor { get; }

        public bool Has(ITask task);
        public bool Has(ITaskBundle bundle);

        public void Setup();
        public void StartFirst();

        public void ResetAll();
        public void ResetFirst();

        public bool IsCompleted();
    }
}
