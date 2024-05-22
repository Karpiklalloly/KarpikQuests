using System;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Extensions;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Completion : IDependencyType
    {
        public bool IsOk(IQuest from)
        {
            return from.IsCompleted();
        }
    }
}
