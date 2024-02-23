using KarpikQuests.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace KarpikQuests.CompletionTypes
{
    public class NeededCount : ICompletionType
    {
        public bool SuccessResult { get; }
        public readonly int Count;

        public NeededCount(int count, bool successResult = true)
        {
            Count = count;
            SuccessResult = successResult;
        }

        public bool CheckCompletion(IEnumerable<ITaskBundle> bundles)
        {
            int curCount = 0;
            if (!bundles.Any())
            {
                return !SuccessResult;
            }

            foreach (var bundle in bundles)
            {
                foreach (var task in bundle)
                {
                    if (task.Status == ITask.TaskStatus.Completed)
                    {
                        curCount++;
                    }
                }
            }

            if (curCount >= Count)
            {
                return SuccessResult;
            }

            return !SuccessResult;
        }

        public bool CheckCompletion(ITaskBundle bundle)
        {
            int curCount = 0;
            if (!bundle.Any())
            {
                return SuccessResult;
            }

            foreach (var task in bundle)
            {
                if (task.Status == ITask.TaskStatus.Completed)
                {
                    curCount++;
                }
            }

            if (curCount >= Count)
            {
                return SuccessResult;
            }

            return !SuccessResult;
        }
    }
}
