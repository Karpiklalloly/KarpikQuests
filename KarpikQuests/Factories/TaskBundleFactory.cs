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
            return Create(
                new And(),
                new Disorderly());
        }

        public ITaskBundle Create(ICompletionType completionType)
        {
            return Create(
                completionType,
                new Disorderly());
        }

        public ITaskBundle Create(IProcessorType processor)
        {
            return Create(
                new And(),
                processor);
        }

        public ITaskBundle Create(ICompletionType completionType, IProcessorType processor)
        {
            return new TaskBundle(completionType, processor);
        }
    }
}