using System;
using System.Collections.Generic;
using System.Text;

namespace KarpikQuests.Interfaces
{
    public interface ITaskBundleCollection : ICollection<ITaskBundle>, ICloneable
    {
        public ICollection<ITaskBundle> Bundles { get; }

        public bool ContainsTask(IQuestTask task);
    }
}
