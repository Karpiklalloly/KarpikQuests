using System.Runtime.CompilerServices;
using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;
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

        public IQuest Create(string name, string description)
        {
            return Create(
                name,
                description,
                _bundleFactory.Create());
        }
        
        public IQuest Create(string name, string description, ITaskBundleCollection? bundles)
        {
            return Create(
                name,
                description,
                bundles,
                new And(),
                new Orderly());
        }

        public IQuest Create(string name, string description, ICompletionType completionType)
        {
            return Create(
                name, 
                description, 
                _bundleFactory.Create(), 
                completionType, 
                new Orderly());
        }
        
        public IQuest Create(string name, string description, IProcessorType processorType)
        {
            return Create(
                name, 
                description, 
                _bundleFactory.Create(), 
                new And(), 
                processorType);
        }

        public IQuest Create(string name,
            string description,
            ITaskBundleCollection? bundles,
            ICompletionType completionType,
            IProcessorType processorType)
        {
            var quest = new Quest();

            bundles ??= TaskBundleCollectionFactory.Instance.Create();

            quest.Init(name, description, bundles, completionType, processorType);

            return quest;
        }
    }
}