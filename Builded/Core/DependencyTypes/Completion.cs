using Newtonsoft.Json;
using System;
using Karpik.Quests.Extensions;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Completion : IDependencyType
    {
        public bool IsOk(Quest from)
        {
            return from.IsCompleted();
        }
    }
}
