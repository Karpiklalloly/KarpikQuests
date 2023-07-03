namespace KarpikQuests.Interfaces.AbstractBases
{
    public abstract class QuestStatusBase : IQuestStatus
    {
        public abstract bool Equals(IQuestStatus other);

        public virtual string GetStatus()
        {
            return GetType().Name;
        }
    }
}
