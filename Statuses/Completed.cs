using KarpikQuests.Interfaces;
using KarpikQuests.Interfaces.AbstractBases;
using System;

namespace KarpikQuests.Statuses
{
    [Serializable]
    public class Completed : StatusBase
    {
        public override bool Equals(IStatus other)
        {
            if (other is Completed)
            {
                return true;
            }
            return false;
        }
    }
}