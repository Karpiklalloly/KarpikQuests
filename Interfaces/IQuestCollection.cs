using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestCollection : ICollection<IQuest>, IReadOnlyCollection<IQuest>, IEnumerable<IQuest>
    {

    }
}