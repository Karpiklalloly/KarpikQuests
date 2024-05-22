using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface ICompletionType
    {
        public IStatus Check(IEnumerable<ITaskBundle> bundles);

        public IStatus Check(ITaskBundle bundle);
    }
}
