using System.Runtime.CompilerServices;
using Karpik.Quests.ID;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests
{
    public static class QuestCreator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create()
        {
            return Create(Id.NewId());
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(Id id)
        {
            return Create(id, string.Empty);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(string? name)
        {
            return Create(Id.NewId(), name);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(Id id, string? name)
        {
            return Create(id, name, string.Empty);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(string? name, string? description)
        {
            return Create(Id.NewId(), name, description);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(Id id, string? name, string? description)
        {
            return Create(id, name, description, null, null);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(string? name, string? description, ICompletionType? completionType, IProcessorType? processor)
        {
            return Create(Id.NewId(), name, description, completionType, processor);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(Id id, string? name, string? description, ICompletionType? completionType, IProcessorType? processor)
        {
            return Create(id, name, description, completionType, processor, Array.Empty<QuestAndRequirement>());
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(string? name, string? description, ICompletionType? completionType, IProcessorType? processor, params QuestAndRequirement[] subQuests)
        {
            return Create(Id.NewId(), name, description, completionType, processor, subQuests);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quest Create(Id id, string? name, string? description, ICompletionType? completionType, IProcessorType? processor, params QuestAndRequirement[] subQuests)
        {
            return new Quest(id, name, description, completionType, processor, subQuests);
        }
    }
}