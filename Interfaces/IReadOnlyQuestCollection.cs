using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyQuestCollection : IList<IQuest>, ICloneable, IEquatable<IReadOnlyQuestCollection>
    {
        public bool Has(IQuest item);
    }
}
