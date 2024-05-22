using System;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Extensions;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Fail : IDependencyType
    {
        public bool IsOk(IQuest from)
        {
            return from.IsFailed();
        }
    }
}
