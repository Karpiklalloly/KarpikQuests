using Karpik.Quests.Interfaces;
using Karpik.Quests.Extensions;
using Karpik.Quests.Sample;

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
