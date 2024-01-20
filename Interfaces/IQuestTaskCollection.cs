using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskCollection : ICollection<IQuestTask>, ICloneable
    {
        public bool Has(IQuestTask task);
    }
}