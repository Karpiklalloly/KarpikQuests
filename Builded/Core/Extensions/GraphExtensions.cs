using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static class GraphExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemove(this IGraph graph, Quest quest)
        {
            return graph.TryRemove(quest.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has(this IGraph graph, Quest quest)
        {
            return graph.Has(quest.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddDependency(this IGraph graph, Quest quest, Quest dependencyQuest, IDependencyType dependencyType)
        {
            return graph.TryAddDependency(quest.Id, dependencyQuest.Id, dependencyType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddDependency(this IGraph graph, Quest quest, Quest dependencyQuest, DependencyType dependencyType)
        {
            return graph.TryAddDependency(quest.Id, dependencyQuest.Id, dependencyType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveDependencies(this IGraph graph, Quest quest)
        {
            return graph.TryRemoveDependencies(quest.Id);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveDependents(this IGraph graph, Quest quest)
        {
            return graph.TryRemoveDependents(quest.Id);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveDependency(this IGraph graph, Quest quest, Quest dependencyQuest)
        {
            return graph.TryRemoveDependency(quest.Id, dependencyQuest.Id);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<QuestConnection> GetDependenciesQuests(this IGraph graph, Quest quest)
        {
            return graph.GetDependenciesQuests(quest.Id);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<QuestConnection> GetDependentsQuests(this IGraph graph, Quest quest)
        {
            return graph.GetDependentsQuests(quest.Id);
        }
    }
}