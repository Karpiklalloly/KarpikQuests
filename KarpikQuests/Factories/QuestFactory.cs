using System.Runtime.CompilerServices;
using Karpik.Quests.Interfaces;
using Karpik.Quests.Keys;
using Karpik.Quests.QuestSample;

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
            return Create("Name", "Description", _bundleFactory.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name)
        {
            return Create(name, "Description", _bundleFactory.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name, string description)
        {
            return Create(name, description, _bundleFactory.Create());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQuest Create(string name,
            string description,
            ITaskBundleCollection? bundles)
        {
            var quest = new Quest();

            bundles ??= TaskBundleCollectionFactory.Instance.Create();

            quest.Init(KeyGenerator.GenerateKey(), name, description, bundles);

            return quest;
        }
    }
}