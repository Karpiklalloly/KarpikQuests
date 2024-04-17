using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Extensions
{
    public static partial class ConnectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this IGraphNode.Connection connection)
        {
            return connection == IGraphNode.Connection.Empty;
        }
    }
}