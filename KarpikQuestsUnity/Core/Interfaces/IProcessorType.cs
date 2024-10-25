using NewKarpikQuests.Sample;

namespace NewKarpikQuests.Interfaces
{
    public interface IProcessorType
    {
        public void Setup(QuestCollection quests);
        public void Update(QuestCollection quests);
    }
}