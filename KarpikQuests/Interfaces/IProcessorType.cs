namespace Karpik.Quests
{
    public interface IProcessorType
    {
        public void Setup(QuestCollection quests);
        public void Update(QuestCollection quests);
    }
}