using System;
using System.Collections.Generic;
using System.Linq;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.TaskProcessorTypes
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
            foreach (ITask task in bundle)
            {
                task.Setup();
                task.Start();
            }
        }
    }
}
