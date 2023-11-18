using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.QuestTaskProcessorTypes
{
    [Serializable]
    public class QuestTaskProcessorOrderly : IQuestTaskProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles)
        {
            foreach (var bundle in bundles)
            {
                bundle.ResetAll(false);
            }

            if (bundles.Any())
            {
                var bundle = bundles.ElementAt(0);
                bundle?.ResetFirst();
            }
        }

        public void OnTaskCompleted(IEnumerable<ITaskBundle> bundles, IQuestTask task)
        {
            foreach (var item in bundles)
            {
                var index = item.QuestTasks.ToList().IndexOf(task);
                if (index != -1)
                {
                    if (item.QuestTasks.Count == index + 1)
                    {
                        return;
                    }
                    item.QuestTasks.ElementAt(index + 1).Reset(true);
                    break;
                }
            }
        }

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            
        }
    }
}
