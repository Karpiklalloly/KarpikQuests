using Karpik.Quests.Interfaces;
using Karpik.Quests.TaskProcessorTypes;

namespace Karpik.Quests.Factories
{
    public class ProcessorFactory : IFactory<IProcessorType>, ISingleton<ProcessorFactory>
    {
        private static ProcessorFactory _instance;
        public static ProcessorFactory Instance => _instance ??= new ProcessorFactory();

        private ProcessorFactory()
        {
            
        }
        
        public IProcessorType Create()
        {
            return new Disorderly();
        }

        public Disorderly Disorderly()
        {
            return new Disorderly();
        }

        public Orderly Orderly()
        {
            return new Orderly();
        }
    }
}