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

        public void Setup(ITaskBundle bundle)
        {
            foreach (var task in bundle)
            {
                task.Reset(true);
            }
        }

        public void OnTaskCompleted(ITaskBundle bundle, IQuestTask task)
        {
            
        }

        public void OnBundleCompleted(IEnumerable<ITaskBundle> bundles, ITaskBundle bundle)
        {
            
        }
    }
}
