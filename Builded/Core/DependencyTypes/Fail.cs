using Newtonsoft.Json;
using System;

namespace Karpik.Quests.DependencyTypes
{
    [Serializable]
    public class Fail : IDependencyType
    {
        public bool IsOk(Quest from)
        {
            return from.IsFailed();
        }
    }
}
