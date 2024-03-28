using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions;

public static class QuestNodeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid(this IGraphNode node)
    {
        if (node is null) return false;
        if (!node.NodeId.IsValid()) return false;
        if (!node.Quest.IsValid()) return false;

        return true;
    }
}