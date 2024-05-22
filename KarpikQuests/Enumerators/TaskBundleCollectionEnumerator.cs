using System.Collections.Generic;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

namespace Karpik.Quests.Enumerators
{
    public class TaskBundleCollectionEnumerator : Enumerator<ITaskBundle>
    {
        public TaskBundleCollectionEnumerator(ITaskBundleCollection collection) : base(collection)
        {
        }
    }
}