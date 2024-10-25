using NewKarpikQuests.Extensions;
using NewKarpikQuests.Interfaces;
using NewKarpikQuests.Sample;

namespace NewKarpikQuests.DependencyTypes
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
