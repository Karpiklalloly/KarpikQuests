using System;
using System.Collections.Generic;
using Karpik.Quests.ID;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskCollection : IEquatable<IReadOnlyTaskCollection>, IEnumerable<ITask>
    {
        public bool Has(ITask task);
        public bool Has(Id task);
    }
}
