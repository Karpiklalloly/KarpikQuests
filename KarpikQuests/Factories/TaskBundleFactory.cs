using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

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
            return Create(
                CompletionTypesFactory.Instance.Create(),
                ProcessorFactory.Instance.Create());
        }

        public ITaskBundle Create(ICompletionType completionType)
        {
            return Create(
                completionType,
                ProcessorFactory.Instance.Create());
        }

        public ITaskBundle Create(IProcessorType processor)
        {
            return Create(
                CompletionTypesFactory.Instance.Create(),
                processor);
        }

        public ITaskBundle Create(ICompletionType completionType, IProcessorType processor)
        {
            completionType ??= CompletionTypesFactory.Instance.Create();
            processor ??= ProcessorFactory.Instance.Create();
            
            return new TaskBundle(completionType, processor);
        }
    }
}