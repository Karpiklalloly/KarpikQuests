using System.Collections.Generic;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Enumerators
{
    public class TaskBundleEnumerator : Enumerator<ITask>
    {
        public TaskBundleEnumerator(ITaskBundle collection) : base(collection)
        {
        }
    }
}