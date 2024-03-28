using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskCollection : ICollection<ITask>, ICloneable, IEquatable<IReadOnlyTaskCollection>
    {
        public bool Has(ITask task);
    }
}
