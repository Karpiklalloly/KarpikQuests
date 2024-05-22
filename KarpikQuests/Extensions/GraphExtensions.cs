using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static partial class GraphExtensions
    {
        public static bool IsValid(this IGraph graph)
        {
            if (graph is null) return false;

            return true;
        }
    }
}