using System.Collections.Generic;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Enumerators
{
    public class TaskCollectionEnumerator : Enumerator<ITask>
    {
        public TaskCollectionEnumerator(ITaskCollection collection) : base(collection)
        {
        }
    }
}