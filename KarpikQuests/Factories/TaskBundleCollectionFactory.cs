using Karpik.Quests.Interfaces;
using Karpik.Quests.QuestSample;

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
            return new TaskBundleCollection();
        }
    }
}