using UnityEngine;
using Karpik.UIExtension;
using Unity.Properties;
using Newtonsoft.Json;
using System;

namespace Karpik.Quests.Processors
{
    [Serializable]
    public class Disorderly : IProcessorType
    {
        public void Setup(QuestCollection quests)
        {
            foreach (var subQuest in quests)
            {
                subQuest.Setup();
                subQuest.TryUnlock();
            }
        }

        public void Update(QuestCollection quests)
        {
            
        }
    }
}
