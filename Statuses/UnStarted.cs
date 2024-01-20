using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public class UnStarted : StatusBase
    {
        public override bool Equals(IStatus? other)
        {
            if (other is null) return false;

            if (other is UnStarted)
            {
                return true;
            }
            return false;
        }
    }
}