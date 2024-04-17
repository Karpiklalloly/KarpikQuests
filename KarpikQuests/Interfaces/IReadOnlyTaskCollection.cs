using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IReadOnlyTaskCollection : IList<ITask>, ICloneable, IEquatable<IReadOnlyTaskCollection>
    {
        public bool Has(ITask task);
    }
}
