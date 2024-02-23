using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public struct Started : IStatus
    {
        public readonly string Status => nameof(Started);

        public readonly bool Equals(IStatus? other)
        {
            return other is Started;
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

        public static bool operator ==(Started left, IStatus right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Started left, IStatus right)
        {
            return !(left == right);
        }
    }
}