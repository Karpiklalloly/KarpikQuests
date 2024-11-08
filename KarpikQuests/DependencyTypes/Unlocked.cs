using System;
using Karpik.Quests.Extensions;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Unlocked : IDependencyType
    {
        public bool IsOk(Quest from)
        {
            return from.IsUnlocked();
        }
    }
}
