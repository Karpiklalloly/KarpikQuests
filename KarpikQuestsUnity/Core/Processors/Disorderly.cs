using Karpik.Quests.Interfaces;
using Karpik.Quests.Sample;

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
