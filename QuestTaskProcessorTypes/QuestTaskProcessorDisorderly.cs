using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;

namespace KarpikQuests.QuestTaskProcessorTypes
{
    [Serializable]
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
