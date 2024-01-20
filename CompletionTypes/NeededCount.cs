using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarpikQuests.CompletionTypes
{
    public class NeededCount : ICompletionType
    {
        public readonly int Count;

        public NeededCount(int count)
        {
            Count = count;
        }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            int curCount = 0;
            if (!bundles.Any())
            {
                return false;
            }

            foreach (var bundle in bundles)
            {
                foreach (var task in bundle)
                {
                    if (task.Status == IQuestTask.TaskStatus.Completed)
                    {
                        curCount++;
                    }
                }
            }

            if (curCount >= Count)
            {
                return true;
            }

            return false;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            int curCount = 0;
            if (!bundle.Any())
            {
                return true;
            }

            foreach (var task in bundle)
            {
                if (task.Status == IQuestTask.TaskStatus.Completed)
                {
                    curCount++;
                }
            }

            if (curCount >= Count)
            {
                return true;
            }

            return false;
        }
    }
}
