using System;
using System.Collections.Generic;
using Karpik.Quests.Interfaces;

namespace Karpik.Quests.TaskProcessorTypes
{
    public class ProcessorTypesPool : ISingleton<ProcessorTypesPool>, IPool<IProcessorType>
    {
        private readonly Dictionary<Type, IProcessorType> _processorTypes = new Dictionary<Type, IProcessorType>();
        
        private static ProcessorTypesPool _instance;
        public static ProcessorTypesPool Instance => _instance ??= new ProcessorTypesPool();

        private ProcessorTypesPool()
        {
            
        }
        
        public TGet Pull<TGet>() where TGet : IProcessorType
        {
            if (!_processorTypes.ContainsKey(typeof(TGet)))
            {
                _processorTypes.Add(typeof(TGet), Activator.CreateInstance<TGet>());
            }

            return (TGet)_processorTypes[typeof(TGet)];
        }

        public void Push(IProcessorType instance)
        {
            if (_processorTypes.ContainsKey(instance.GetType()))
            {
                return;
            }
            
            _processorTypes.Add(instance.GetType(), instance);
        }
    }
}