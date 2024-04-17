using System;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Unneccesary : IDependencyType
    {
        public bool IsOk(IQuest from)
        {
            return true;
        }
    }
}