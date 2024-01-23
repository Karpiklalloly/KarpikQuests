using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyQuestTaskCollection : ICollection<IQuestTask>, ICloneable, IEquatable<IReadOnlyQuestTaskCollection>
    {
        public bool Has(IQuestTask task);
    }
}
