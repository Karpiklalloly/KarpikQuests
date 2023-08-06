using KarpikQuests.Interfaces;
using System.Collections.Generic;

namespace KarpikQuests.QuestTaskProcessorTypes
{
    public class QuestTaskProcessorDisorderly : IQuestTaskProcessorType
    {
        public void Setup(IEnumerable<IQuestTask> tasks)
        {
            foreach (var task in tasks)
            {
                task.Reset(true);
            }
        }

        public void OnTaskCompleted(IEnumerable<IQuestTask> tasks, IQuestTask task)
        {
            
        }
    }
}
