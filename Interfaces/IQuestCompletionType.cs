using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestCompletionType
    {
        public bool CheckCompletion(IEnumerable<IQuestTask> tasks);
    }
}
