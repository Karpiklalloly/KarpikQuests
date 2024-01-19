using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.Interfaces
{
    public interface IReadOnlyTaskBundleCollection : ICollection<ITaskBundle>, ICloneable
    {
        public bool ContainsTask(IQuestTask task);
    }
}
