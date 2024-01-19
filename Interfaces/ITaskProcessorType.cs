using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface ITaskProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles);

        public void Setup(ITaskBundle bundle);

        public void OnTaskCompleted(ITaskBundle bundle, IQuestTask task);

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle);
    }
}