using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyQuestCollection : IList<IQuest>, ICloneable, IEqualityComparer<IReadOnlyQuestCollection>
    {
        public bool Has(IQuest item);
    }
}
