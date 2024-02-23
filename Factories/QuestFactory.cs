using System.Runtime.CompilerServices;
using KarpikQuests.Interfaces;
using KarpikQuests.Keys;
using KarpikQuests.QuestSample;

namespace KarpikQuests.Factories
{
    public readonly struct QuestFactory : IFactory<IQuest>
    {
        private readonly IFactory<ITaskBundleCollection> _bundleFactory;

        public QuestFactory(IFactory<ITaskBundleCollection>? bundleFactory = null)
        {
            _bundleFactory = bundleFactory ?? new TaskBundleCollectionFactory();
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

            bundles ??= new TaskBundleCollectionFactory().Create();

            quest.Init(KeyGenerator.GenerateKey(), name, description, bundles);

            return quest;
        }
    }
}