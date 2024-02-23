using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public struct UnStarted : IStatus
    {
        public readonly string Status => nameof(UnStarted);

        public readonly bool Equals(IStatus? other)
        {
            return other is UnStarted;
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

        public static bool operator ==(UnStarted left, IStatus right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnStarted left, IStatus right)
        {
            return !(left == right);
        }
    }
}