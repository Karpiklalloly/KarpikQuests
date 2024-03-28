using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
using Karpik.Quests.ID;
using Karpik.Quests.QuestSample;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Factories
{
    public class QuestFactory : IFactory<IQuest>, ISingleton<QuestFactory>
    {
        private static QuestFactory _instance;
        public static QuestFactory Instance => _instance ??= new QuestFactory();
        
        private readonly IFactory<ITaskBundleCollection> _bundleFactory;

        private QuestFactory(IFactory<ITaskBundleCollection>? bundleFactory = null)
        {
            _bundleFactory = bundleFactory ?? TaskBundleCollectionFactory.Instance;
        }

        public IQuest Create()
        {
            return Create(
                "Name", 
                "Description", 
                _bundleFactory.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name)
        {
            return Create(
                name,
                "Description",
                _bundleFactory.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name, string description)
        {
            return Create(
                name,
                description,
                _bundleFactory.Create());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name, string description, ITaskBundleCollection? bundles)
        {
            return Create(
                name,
                description,
                bundles,
                CompletionTypesPool.Instance.Pull<And>(),
                ProcessorTypesPool.Instance.Pull<Orderly>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name, string description, ICompletionType completionType)
        {
            return Create(
                name, 
                description, 
                _bundleFactory.Create(), 
                completionType, 
                ProcessorTypesPool.Instance.Pull<Orderly>());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name, string description, IProcessorType processorType)
        {
            return Create(
                name, 
                description, 
                _bundleFactory.Create(), 
                CompletionTypesPool.Instance.Pull<And>(), 
                processorType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name,
            string description,
            ITaskBundleCollection? bundles,
            ICompletionType completionType,
            IProcessorType processorType)
        {
            var quest = new Quest(Id.NewId());

            bundles ??= TaskBundleCollectionFactory.Instance.Create();
            completionType ??= CompletionTypesPool.Instance.Pull<And>();
            processorType ??= ProcessorTypesPool.Instance.Pull<Orderly>();

            quest.Init(name, description, bundles, completionType, processorType);

            return quest;
        }
    }
}