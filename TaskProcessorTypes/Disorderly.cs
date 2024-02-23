using KarpikQuests.Interfaces;
using System;
using System.Collections.Generic;

namespace KarpikQuests.TaskProcessorTypes
{
    [Serializable]
    public class Disorderly : IProcessorType
    {
        public void Setup(IEnumerable<ITaskBundle> bundles)
        {
            foreach (var bundle in bundles)
            {
                bundle.Setup();
            }
        }

        public void Setup(ITaskBundle bundle)
        {
            foreach (var task in bundle)
            {
                task.Setup();
            }
        }
    }
}
