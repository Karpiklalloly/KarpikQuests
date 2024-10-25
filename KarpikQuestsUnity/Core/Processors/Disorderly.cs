using NewKarpikQuests.Interfaces;
using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Processors
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
