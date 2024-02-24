using System.Runtime.CompilerServices;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;

namespace KarpikQuests.Factories
{
    public readonly struct TaskBundleFactory : IFactory<ITaskBundle>
    {
        public readonly ITaskBundle Create()
        {
            return Create(new AND(), new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundle Create(ICompletionType? completionType)
        {
            return Create(completionType, new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundle Create(IProcessorType? processor)
        {
            return Create(new AND(), processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ITaskBundle Create(ICompletionType? completionType, IProcessorType? processor)
        {
            completionType ??= new AND();
            processor      ??= new Disorderly();

            TaskBundle collection = new TaskBundle(completionType, processor);

            return collection;
        }
    }
}