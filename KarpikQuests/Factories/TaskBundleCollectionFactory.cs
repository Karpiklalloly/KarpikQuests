using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Factories
{
    public class TaskBundleCollectionFactory : IFactory<ITaskBundleCollection>, ISingleton<TaskBundleCollectionFactory>
    {
        private static TaskBundleCollectionFactory _instance;
        public static TaskBundleCollectionFactory Instance => _instance ??= new TaskBundleCollectionFactory();

        private TaskBundleCollectionFactory()
        {
            
        }
        
        public ITaskBundleCollection Create()
        {
            return Create(And.Instance, ProcessorTypesPool.Instance.Pull<Disorderly>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(ICompletionType? completionType)
        {
            return Create(completionType, ProcessorTypesPool.Instance.Pull<Disorderly>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(IProcessorType? processor)
        {
            return Create(And.Instance, processor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITaskBundleCollection Create(ICompletionType? completionType, IProcessorType? processor)
        {
            completionType ??= And.Instance;
            processor      ??= ProcessorTypesPool.Instance.Pull<Disorderly>();

            TaskBundleCollection collection = new TaskBundleCollection(completionType, processor);
            
            return collection;
        }
    }
}