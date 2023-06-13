using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskCollection : ICollection<IQuestTask>, IReadOnlyCollection<IQuestTask>
    {

    }
}