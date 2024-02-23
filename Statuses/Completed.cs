using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public struct Completed : IStatus
    {
        public readonly string Status => nameof(Completed);

        public readonly bool Equals(IStatus? other)
        {
            return other is Completed;
        }

        public readonly override bool Equals(object? obj)
        {
            if (!(obj is IStatus status)) return false;

            return Equals(status);
        }

        public readonly override int GetHashCode()
        {
            return Status.GetHashCode();
        }

        public readonly override string ToString()
        {
            return Status;
        }

        public static bool operator ==(Completed left, IStatus right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Completed left, IStatus right)
        {
            return !(left == right);
        }
    }
}