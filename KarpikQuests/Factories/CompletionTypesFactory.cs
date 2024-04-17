using Karpik.Quests.CompletionTypes;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.Factories
{
    public class CompletionTypesFactory : IFactory<ICompletionType>, ISingleton<CompletionTypesFactory>
    {
        private static CompletionTypesFactory _instance;
        public static CompletionTypesFactory Instance => _instance ??= new CompletionTypesFactory();

        private CompletionTypesFactory()
        {
            
        }
    
        public ICompletionType Create()
        {
            return new And();
        }

        public And And()
        {
            return new And();
        }

        public Or Or()
        {
            return new Or();
        }

        public NeededCount NeededCount(int count)
        {
            return new NeededCount(count);
        }
    }
}