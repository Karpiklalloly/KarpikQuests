using System;
using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyTaskCollection : IList<ITask>, ICloneable, IEqualityComparer<IReadOnlyTaskCollection>
    {
        public bool Has(ITask task);
    }
}
