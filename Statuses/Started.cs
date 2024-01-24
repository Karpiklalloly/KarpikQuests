using KarpikQuests.Interfaces;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public struct Started : IStatus
    {
        public readonly string Status => GetType().Name;

        public readonly bool Equals(IStatus? other)
        {
            if (other is null) return false;

            if (other is Started)
            {
                return true;
            }
            return false;
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is null || !(obj is IStatus status)) return false;

            return Equals(status);
        }

        public override readonly int GetHashCode()
        {
            return Status.GetHashCode();
        }

        public override readonly string ToString()
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