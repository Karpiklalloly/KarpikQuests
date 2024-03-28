using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskBundleCollection : ICollection<ITaskBundle>, ICloneable, IEquatable<IReadOnlyTaskBundleCollection>
    {
        public bool Has(ITask task);
        public bool Has(ITaskBundle bundle);

        public void Setup(IProcessorType processor);

        public void ResetAll();
        public void ResetFirst();
    }
}
