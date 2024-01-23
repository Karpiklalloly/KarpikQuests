using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyTaskBundleCollection : IList<ITaskBundle>, ICloneable, IEquatable<IReadOnlyTaskBundleCollection>
    {
        public ICompletionType CompletionType { get; }

        public IProcessorType Processor { get; }

        public bool Has(IQuestTask task);

        public bool Has(ITaskBundle bundle);

        public bool CheckCompletion();
    }
}
