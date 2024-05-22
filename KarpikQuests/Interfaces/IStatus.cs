using System;

namespace Karpik.Quests.Interfaces
{
    public interface IStatus : IEquatable<IStatus>
    {
        public string Status { get; }
    }
}