using System.Runtime.CompilerServices;
using KarpikQuests.CompletionTypes;
using KarpikQuests.Interfaces;
using KarpikQuests.QuestSample;
using KarpikQuests.TaskProcessorTypes;

namespace KarpikQuests.Factories
{
    public struct TaskBundleFactory : IFactory<ITaskBundle>
    {
        public ITaskBundle Create()
        {
            return Create(new AND(), new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(ICompletionType completionType)
        {
            return Create(completionType, new Disorderly());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(IProcessorType processor)
        {
            return Create(new AND(), processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(ICompletionType completionType, IProcessorType processor)
        {
            completionType ??= new AND();
            processor      ??= new Disorderly();

            TaskBundle collection = new TaskBundle(completionType, processor);

            return collection;
        }
    }
}