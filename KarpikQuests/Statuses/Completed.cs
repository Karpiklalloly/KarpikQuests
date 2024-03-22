using System;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Statuses
{
    [Serializable]
    public sealed class Completed : IStatus
    {
        public string Status => nameof(Completed);

        public bool Equals(IStatus? other)
        {
            return other is Completed;
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