using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles);
        
        public void OnTaskCompleted(IEnumerable<ITaskBundle> bundles, IQuestTask task);

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle);
    }
}