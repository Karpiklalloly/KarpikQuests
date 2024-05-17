using Karpik.Quests.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Extensions;
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

            arr[0].Tasks.First().Start();
        }

        public void Setup(ITaskBundle bundle)
        {
            if (bundle.Count == 0) return;

            foreach (var task in bundle)
            {
                task.Setup();
                task.Completed += (t) => OnTaskCompleted(bundle, t);
            }

            bundle.Tasks.First().Start();
        }

        private void OnTaskCompleted(ITaskBundle bundle, ITask task)
        {
            var index = bundle.Tasks.ToList().IndexOf(task);
            if (index == -1) return;
            
            do
            {
                if (bundle.Tasks.Count() == index + 1)
                {
                    return;
                }
                
                var task2 = bundle.Tasks.ElementAt(index + 1);
                if (task2.IsUnStarted())
                {
                    task2.Start();
                    break;
                }

                index++;
            } while (true);
        }

        private void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            var arr = bundles as ITaskBundle[] ?? bundles.ToArray();
            var index = arr.ToList().IndexOf(bundle);
            if (index == -1) return;

            do
            {
                if (arr.Length == index + 1)
                {
                    return;
                }
                
                var bundle2 = arr[index + 1];
                if (bundle2.IsUnStarted())
                {
                    bundle.Setup();
                }

                index++;
            } while (true);
        }
    }
}
