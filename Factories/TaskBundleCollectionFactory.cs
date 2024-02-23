using System.Runtime.CompilerServices;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;

namespace KarpikQuests.Factories
{
    public readonly struct TaskBundleCollectionFactory : IFactory<ITaskBundleCollection>
    {
        public readonly ITaskBundleCollection Create()
        {
            return Create(new AND(), new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundleCollection Create(ICompletionType? completionType)
        {
            return Create(completionType, new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundleCollection Create(IProcessorType? processor)
        {
            return Create(new AND(), processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundleCollection Create(ICompletionType? completionType, IProcessorType? processor)
        {
            completionType ??= new AND();
            processor      ??= new Disorderly();

            TaskBundleCollection collection = new TaskBundleCollection(completionType, processor);
            
            return collection;
        }
    }
}