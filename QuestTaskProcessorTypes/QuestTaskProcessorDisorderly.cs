using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;

namespace KarpikQuests.QuestTaskProcessorTypes
{
    [Serializable]
    public class QuestTaskProcessorDisorderly : IQuestTaskProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles)
        {
            foreach (var bundle in bundles)
            {
                bundle.ResetAll(true);
            }
        }

        public void OnTaskCompleted(IEnumerable<ITaskBundle> bundles, IQuestTask task)
        {
            
        }

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            
        }
    }
}
