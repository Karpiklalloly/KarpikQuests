using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestCompletionType
    {
        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles);

        public bool CheckCompletion(ITaskBundle bundle);
    }
}
