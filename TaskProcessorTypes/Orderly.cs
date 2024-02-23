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
                bundle.Completed += (b) => OnBundleCompleted(bundles, b);
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
                task.Completed += (t) => OnTaskCompleted(bundle, t);
            }
            bundle.First()?.Reset(true);
        }

        private void OnTaskCompleted(ITaskBundle bundle, ITask task)
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

        private void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
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
