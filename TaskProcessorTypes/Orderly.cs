using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.TaskProcessorTypes
{
    [Serializable]
    public class Orderly : IProcessorType
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

        public void Setup(ITaskBundle bundle)
        {
            foreach (var task in bundle)
            {
                task.Reset(false);
            }
            bundle.First()?.Reset(true);
        }

        public void OnTaskCompleted(ITaskBundle bundle, IQuestTask task)
        {
            var index = bundle.QuestTasks.ToList().IndexOf(task);
            if (index != -1)
            {
                if (bundle.QuestTasks.Count == index + 1)
                {
                    return;
                }
                bundle.QuestTasks.ElementAt(index + 1).Reset(true);
            }
        }

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            var index = bundles.ToList().IndexOf(bundle);
            if (index != -1)
            {
                if (bundles.Count() == index + 1)
                {
                    return;
                }
                bundles.ElementAt(index + 1).ResetFirst(true);
            }
        }


    }
}
