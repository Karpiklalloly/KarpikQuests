namespace KarpikQuests.Interfaces
{
    public interface IQuestTaskOwner
    {
        public IQuestTask Task { get; }

        public void Update();
    }
}
