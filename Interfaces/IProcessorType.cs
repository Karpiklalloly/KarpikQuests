using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles);

        public void Setup(ITaskBundle bundle);
    }
}