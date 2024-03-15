using KarpikQuests.Interfaces;

namespace KarpikQuests.Statuses
{
    public sealed class Failed : IStatus
    {
        public string Status => nameof(Failed);

        public bool Equals(IStatus? other)
        {
            return other is Started;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is IStatus status)) return false;

            return Equals(status);
        }

        public override int GetHashCode()
        {
            return Status.GetHashCode();
        }

        public override string ToString()
        {
            return Status;
        }

        public static bool operator ==(Failed left, IStatus right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Failed left, IStatus right)
        {
            return !(left == right);
        }
    }
}
