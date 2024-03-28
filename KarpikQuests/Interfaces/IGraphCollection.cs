using System;
using System.Collections.Generic;

namespace Karpik.Quests.Interfaces
{
    public interface IGraphCollection : IList<IGraph>, ICloneable, IEquatable<IGraphCollection>
    {
        public bool Has(IGraph item);
        public bool Has(IQuest item);
    }
}