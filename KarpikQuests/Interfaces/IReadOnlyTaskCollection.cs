using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskCollection : IEquatable<IReadOnlyTaskCollection>, IEnumerable<ITask>
    {
        public bool Has(ITask task);
    }
}
