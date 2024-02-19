using System.Runtime.CompilerServices;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;

namespace KarpikQuests.Factories
{
    public struct TaskBundleCollectionFactory : IFactory<ITaskBundleCollection>
    {
        public ITaskBundleCollection Create()
        {
            return Create(new AND(), new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(ICompletionType? completionType)
        {
            return Create(completionType, new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(IProcessorType? processor)
        {
            return Create(new AND(), processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(ICompletionType? completionType, IProcessorType? processor)
        {
            completionType ??= new AND();
            processor      ??= new Disorderly();

            TaskBundleCollection collection = new TaskBundleCollection(completionType, processor);
            
            return collection;
        }
    }
}