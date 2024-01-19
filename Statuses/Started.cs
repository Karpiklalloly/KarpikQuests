using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public class Started : StatusBase
    {
        public override bool Equals(IStatus other)
        {
            if (other is Started)
            {
                return true;
            }
            return false;
        }
    }
}