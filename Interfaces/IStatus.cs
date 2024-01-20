using System;

namespace KarpikQuests.Interfaces
{
    public interface IStatus : IEquatable<IStatus>
    {
        public string Status { get; }
    }
}