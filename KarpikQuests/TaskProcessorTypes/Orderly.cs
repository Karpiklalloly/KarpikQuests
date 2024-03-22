using Karpik.Quests.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.TaskProcessorTypes
{
    [Serializable]
    public class Orderly : IProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            foreach (var bundle in arr)
            {
                bundle.Setup();
                bundle.Completed += (b) => OnBundleCompleted(arr, b);
            }

            if (!arr.Any()) return;

            arr[0].StartFirst();
        }

        public void Setup(ITaskBundle bundle)
        {
            if (bundle.Count == 0) return;

            foreach (var task in bundle)
            {
                task.Setup();
                task.Completed += (t) => OnTaskCompleted(bundle, t);
            }

            bundle.StartFirst();
        }

        private void OnTaskCompleted(ITaskBundle bundle, ITask task)
        {
            var index = bundle.QuestTasks.ToList().IndexOf(task);
            if (index == -1) return;

            if (bundle.QuestTasks.Count == index + 1)
            {
                return;
            }
            bundle.QuestTasks[index + 1].Start();
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

            arr[index + 1].StartFirst();
        }


    }
}
