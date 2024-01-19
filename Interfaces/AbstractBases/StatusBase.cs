namespace KarpikQuests.Interfaces.AbstractBases
{
    public abstract class StatusBase : IStatus
    {
        public abstract bool Equals(IStatus other);

        public virtual string GetStatus()
        {
            return GetType().Name;
        }
    }
}
