using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestCollection : ICollection<IQuest>, IReadOnlyCollection<IQuest>, IEnumerable<IQuest>, IList<IQuest>
    {
        public bool Has(IQuest item);
    }
}