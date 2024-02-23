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
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            foreach (var bundle in arr)
            {
                bundle.ResetAll(false);
                bundle.Completed += (b) => OnBundleCompleted(arr, b);
            }

            if (!arr.Any()) return;

            arr[0].ResetFirst();
        }

        public void Setup(ITaskBundle bundle)
        {
            foreach (var task in bundle)
            {
                task.Reset(false);
                task.Completed += (t) => OnTaskCompleted(bundle, t);
            }

            bundle.FirstOrDefault()?.Reset(true);
        }

        private void OnTaskCompleted(ITaskBundle bundle, ITask task)
        {
            var index = bundle.QuestTasks.ToList().IndexOf(task);
            if (index == -1) return;

            if (bundle.QuestTasks.Count == index + 1)
            {
                return;
            }
            bundle.QuestTasks[index + 1].Reset(true);
        }

        private void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            var index = arr.ToList().IndexOf(bundle);
            if (index == -1) return;

            if (arr.Length == index + 1)
            {
                return;
            }

            arr[index + 1].ResetFirst(true);
        }


    }
}
