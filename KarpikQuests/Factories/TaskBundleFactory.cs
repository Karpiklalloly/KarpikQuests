using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Factories
{
    public class TaskBundleFactory : IFactory<ITaskBundle>, ISingleton<TaskBundleFactory>
    {
        private static TaskBundleFactory _instance;
        public static TaskBundleFactory Instance => _instance ??= new TaskBundleFactory();

        private TaskBundleFactory()
        {
            
        }
        
        public ITaskBundle Create()
        {
            return Create(And.Instance, ProcessorTypesPool.Instance.Pull<Disorderly>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(ICompletionType? completionType)
        {
            return Create(completionType, ProcessorTypesPool.Instance.Pull<Disorderly>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(IProcessorType? processor)
        {
            return Create(And.Instance, processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundle Create(ICompletionType? completionType, IProcessorType? processor)
        {
            completionType ??= And.Instance;
            processor      ??= ProcessorTypesPool.Instance.Pull<Disorderly>();

            TaskBundle collection = new TaskBundle(completionType, processor);

            return collection;
        }
    }
}