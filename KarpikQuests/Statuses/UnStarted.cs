using System;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Statuses
{
    [Serializable]
    public sealed class UnStarted : IStatus
    {
        public string Status => nameof(UnStarted);

        public bool Equals(IStatus? other)
        {
            return other is UnStarted;
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