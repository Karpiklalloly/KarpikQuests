using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface ICompletionType
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles);

        public bool CheckCompletion(ITaskBundle bundle);
    }
}
