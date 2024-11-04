using Karpik.Quests.Sample;

namespace Karpik.Quests.Interfaces
{
    public interface IProcessorType
    {
        public void Setup(QuestCollection quests);
        public void Update(QuestCollection quests);
    }
}