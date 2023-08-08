using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestTaskProcessorTypes
{
    [Serializable]
    public class QuestTaskProcessorOrderly : IQuestTaskProcessorType
    {
        public void OnTaskCompleted(IEnumerable<IQuestTask> tasks, IQuestTask task)
        {
            var list = tasks.ToList();
            var index = list.IndexOf(task);
            if (tasks.Count() == index + 1)
            {
                return;
            }
            tasks.ElementAt(index + 1).Reset(true);
        }

        public void Setup(IEnumerable<IQuestTask> tasks)
        {
            foreach (var task in tasks)
            {
                task.Reset(false);
            }
            tasks?.First()?.Reset(true);
        }
    }
}
