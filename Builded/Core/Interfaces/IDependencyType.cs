using Newtonsoft.Json;
namespace Karpik.Quests
{
    public interface IDependencyType
    {
        public bool IsOk(Quest from);
    }
}