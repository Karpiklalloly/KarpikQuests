using System.Collections.Generic;

namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskProcessorType
    {
        public void Setup(IEnumerable<IQuestTask> tasks);
        
        public void OnTaskCompleted(IEnumerable<IQuestTask> tasks, IQuestTask task);
    }
}