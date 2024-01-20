namespace KarpikQuests.Interfaces.AbstractBases
{
    public abstract class StatusBase : IStatus
    {
        public virtual string Status => GetType().Name;

        public abstract bool Equals(IStatus? other);
    }
}
